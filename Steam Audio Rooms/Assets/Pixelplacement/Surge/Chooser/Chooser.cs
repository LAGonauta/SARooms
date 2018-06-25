/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Simplify the act of selecting and interacting with things.
/// 
/// </summary>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pixelplacement
{
	public class Chooser : MonoBehaviour
	{
		#region Public Events
		public GameObjectEvent OnSelected;
		public GameObjectEvent OnDeselected;
		public GameObjectEvent OnPressed;
		public GameObjectEvent OnReleased;
		#endregion

		#region Public Enums
		public enum Method { Raycast, RaycastAll };
		#endregion

		#region Public Variables
		public bool _cursorPropertiesFolded;
		public bool _unityEventsFolded;
		public Transform source;
		public float raycastDistance = 3;
		public LayerMask layermask = -1;
		public KeyCode[] pressedInput;
		public Transform cursor;
		public float surfaceOffset;
		public float idleDistance = 3f;
		public float stabilityDelta = 0.0127f;
		public float snapDelta = 1;
		public float stableSpeed = 2;
		public float unstableSpeed = 20;
		public bool flipForward;
		public bool matchSurfaceNormal = true;
		public bool autoHide;
		public bool flipCastDirection;
		public LineRenderer lineRenderer;
		#endregion

		#region Public Properties
		public Transform[] Current
		{
			get;
			private set;
		}
		#endregion

		#region Private Variables
		[SerializeField] Method _method;
		Transform _previousCursor;
		List<Transform> _current = new List<Transform>();
		List<Transform> _previous = new List<Transform>();
		Transform _currentRaycast;
		Transform _previousRaycast;
		Vector3 _targetPosition;
		bool _hidden;
		[SerializeField] bool _debugView;
		#endregion

		#region Init
		private void Reset()
		{
			source = transform;
			pressedInput = new KeyCode[] { KeyCode.Mouse0 };
		}
		#endregion

		#region Gizmos
		private void OnDrawGizmosSelected()
		{
			if (Application.isPlaying) return;

			Vector3 castDirection = source.forward;
			if (flipCastDirection) castDirection *= -1;
			Gizmos.color = Color.green;
			Gizmos.DrawRay(source.position, castDirection * raycastDistance);

			if (cursor != null)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(source.position, cursor.position);
			}
		}
		#endregion

		#region Public Methods
		public void Pressed()
		{
			switch (_method)
			{
				case Method.Raycast:
					if (_currentRaycast != null)
					{
						_currentRaycast.SendMessage("Pressed", SendMessageOptions.DontRequireReceiver);
						if (OnPressed != null) OnPressed.Invoke(_currentRaycast.gameObject);
					}
					break;

				case Method.RaycastAll:
					if (_current.Count > 0)
					{
						foreach (var item in _current)
						{
							item.SendMessage("Pressed", SendMessageOptions.DontRequireReceiver);
							if (OnPressed != null) OnPressed.Invoke(item.gameObject);
						}
					}
					break;
			}
		}

		public void Released()
		{
			switch (_method)
			{
				case Method.Raycast:
					if (_currentRaycast != null)
					{
						_currentRaycast.SendMessage("Released", SendMessageOptions.DontRequireReceiver);
						if (OnReleased != null) OnReleased.Invoke(_currentRaycast.gameObject);
					}
					break;

				case Method.RaycastAll:
					if (_current.Count > 0)
					{
						foreach (var item in _current)
						{
							item.SendMessage("Released", SendMessageOptions.DontRequireReceiver);
							if (OnReleased != null) OnReleased.Invoke(item.gameObject);
						}
					}
					break;
			}
		}
		#endregion

		#region Loops
		private void Update()
		{
			//cursor setup:
			if (cursor != _previousCursor)
			{
				_previousCursor = cursor;
				if (cursor == null) return;

				foreach (var item in cursor.GetComponentsInChildren<Collider>())
				{
					Debug.Log("Cursor can not contain colliders. Disabling colliders on: " + item.name);
					item.enabled = false;
				}
			}

			//process input:
			if (pressedInput != null)
			{
				foreach (var item in pressedInput)
				{
					if (Input.GetKeyDown(item))
					{
						Pressed();
					}

					if (Input.GetKeyUp(item))
					{
						Released();
					}
				}
			}

			//clear out:
			_current.Clear();

			//raycast:
			RaycastHit hit;
			Vector3 castDirection = source.forward;
			if (flipCastDirection) castDirection *= -1;
			Physics.Raycast(source.position, castDirection, out hit, raycastDistance, layermask);
			_currentRaycast = hit.transform;

			//debug info:
			if (_debugView)
			{
				if (hit.transform != null)
				{
					Debug.DrawLine(source.position, hit.point, Color.green);
				}
				else
				{
					Debug.DrawRay(source.position, castDirection * raycastDistance, Color.red);
				}
			}

			//cursor visibility:
			if (cursor != null)
			{
				if (autoHide)
				{
					cursor.gameObject.SetActive(hit.transform != null);
				}
				else
				{
					cursor.gameObject.SetActive(true);
				}
			}

			//cursor management:
			if (cursor != null)
			{
				if (hit.transform != null)
				{
					//get position:
					_targetPosition = hit.point + hit.normal * surfaceOffset;

					//get position speed:
					float posSpeed = unstableSpeed;
					float delta = Vector3.Distance(_targetPosition, cursor.position);
					if (delta <= stabilityDelta)
					{
						posSpeed = stableSpeed;
					}

					if (delta >= snapDelta)
					{
						cursor.position = _targetPosition;
					}
					else
					{
						cursor.position = Vector3.Lerp(cursor.position, _targetPosition, Time.deltaTime * posSpeed);
					}

					//set rotation:
					if (matchSurfaceNormal)
					{
						cursor.rotation = Quaternion.LookRotation(hit.normal);
					}
					else
					{
						cursor.LookAt(source);
					}

					//adjust:
					if (flipForward)
					{
						cursor.Rotate(Vector3.up * 180);
					}
				}
				else
				{
					//put out in front and face source (flip if needed):
					Vector3 inFront = source.position + castDirection * idleDistance;
					float delta = Vector3.Distance(inFront, cursor.position);
					float posSpeed = unstableSpeed;

					if (delta <= stabilityDelta)
					{
						posSpeed = stableSpeed;
					}

					if (delta >= snapDelta)
					{
						cursor.position = inFront;
					}
					else
					{
						cursor.position = Vector3.Lerp(cursor.position, inFront, Time.deltaTime * posSpeed);
					}

					cursor.LookAt(source.position);
					if (flipForward)
					{
						cursor.Rotate(Vector3.up * 180);
					}
				}
			}

			//handle raycast messages:
			if (_method == Method.Raycast)
			{
				//select:
				if (_previousRaycast == null && hit.transform != null)
				{
					hit.transform.SendMessage("Selected", SendMessageOptions.DontRequireReceiver);
					if (OnSelected != null) OnSelected.Invoke(hit.transform.gameObject);
				}

				//updated select:
				if (hit.transform != null && _previousRaycast != null && _previousRaycast != hit.transform)
				{
					_previousRaycast.SendMessage("Deselected", SendMessageOptions.DontRequireReceiver);
					if (OnDeselected != null) OnDeselected.Invoke(_previousRaycast.gameObject);
					hit.transform.SendMessage("Selected", SendMessageOptions.DontRequireReceiver);
					if (OnSelected != null) OnSelected.Invoke(hit.transform.gameObject);
				}

				//deselect:
				if (_previousRaycast != null && hit.transform == null)
				{
					_previousRaycast.SendMessage("Deselected", SendMessageOptions.DontRequireReceiver);
					if (OnDeselected != null) OnDeselected.Invoke(_previousRaycast.gameObject);
				}

				//cache:
				_previousRaycast = hit.transform;
			}

			//raycast all:
			if (_method == Method.RaycastAll)
			{
				//catalog:
				foreach (var item in Physics.RaycastAll(source.position, castDirection, raycastDistance, layermask))
				{
					_current.Add(item.transform);
				}

				//handle selects:
				if (_current.Count > 0)
				{
					foreach (var item in _current)
					{
						if (_previous.Count == 0 || !_previous.Contains(item))
						{
							item.SendMessage("Selected", SendMessageOptions.DontRequireReceiver);
							if (OnSelected != null) OnSelected.Invoke(item.gameObject);
						}
					}
				}

				//handle deselects:
				if (_previous.Count > 0)
				{
					foreach (var item in _previous)
					{
						if (_current.Count == 0 || !_current.Contains(item))
						{
							item.SendMessage("Deselected", SendMessageOptions.DontRequireReceiver);
							if (OnDeselected != null) OnDeselected.Invoke(item.gameObject);
						}
					}
				}

				//cache:
				_previous.Clear();
				_previous.AddRange(_current);
			}

			//line renderer:
			if (cursor != null && cursor.gameObject.activeSelf && lineRenderer != null )
			{
				if (lineRenderer.positionCount != 2) lineRenderer.positionCount = 2;
				lineRenderer.SetPosition(0, source.position);
				lineRenderer.SetPosition(1, cursor.position);
			}
		}
		#endregion
	}
}