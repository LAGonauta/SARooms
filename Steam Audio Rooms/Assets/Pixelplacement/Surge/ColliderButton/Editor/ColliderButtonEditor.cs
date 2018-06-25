/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Custom inspector ColliderButton.
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pixelplacement
{
	[CustomEditor(typeof(ColliderButton), true)]
	[CanEditMultipleObjects]
	public class ColliderButtonEditor : Editor
	{
		#region Private Variables
		ColliderButton _target;
		#endregion

		#region Init
		void OnEnable()
		{
			_target = target as ColliderButton;
		}
		#endregion

		#region Inspector GUI
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawPropertiesExcluding(serializedObject, new string[] {
				"keyInput",
				"collisionLayerMask",
				"resizeGUIBoxCollider",
				"guiBoxColliderPadding",
				"Color Responses",
				"applyColor",
				"colorRendererTarget",
				"colorImageTarget",
				"colorImageTarget",
				"normalColor",
				"selectedColor",
				"pressedColor",
				"colorDuration",
				"Scale Responses",
				"applyScale",
				"scaleTarget",
				"normalScale",
				"selectedScale",
				"pressedScale",
				"scaleDuration",
				"scaleEaseType",
				"Unity Events",
				"OnSelected",
				"OnDeselected",
				"OnClick",
				"OnPressed",
				"OnReleased",
				"_unityEventsFolded",
				"_scaleResponseFolded",
				"_colorResponseFolded"
			});

			EditorGUILayout.PropertyField(serializedObject.FindProperty("keyInput"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("collisionLayerMask"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("resizeGUIBoxCollider"));
			GUI.enabled = _target.resizeGUIBoxCollider;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("guiBoxColliderPadding"));
			GUI.enabled = true;

			_target._colorResponseFolded = EditorGUILayout.Foldout(_target._colorResponseFolded, "Color Responses", true);
			if (_target._colorResponseFolded)
			{
				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField(serializedObject.FindProperty("applyColor"));
				GUI.enabled = _target.applyColor;
				EditorGUILayout.PropertyField(serializedObject.FindProperty("colorRendererTarget"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("colorImageTarget"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("normalColor"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("selectedColor"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("pressedColor"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("colorDuration"));
				EditorGUI.indentLevel = 0;
				GUI.enabled = true;
				EditorGUILayout.Space();
			}

			_target._scaleResponseFolded = EditorGUILayout.Foldout(_target._scaleResponseFolded, "Scale Responses", true);
			if (_target._scaleResponseFolded)
			{
				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField(serializedObject.FindProperty("applyScale"));
				GUI.enabled = _target.applyScale;
				EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleTarget"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("normalScale"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("selectedScale"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("pressedScale"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleDuration"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleEaseType"));
				EditorGUI.indentLevel = 0;
				GUI.enabled = true;
				EditorGUILayout.Space();
			}

			//fold events:
			_target._unityEventsFolded = EditorGUILayout.Foldout(_target._unityEventsFolded, "Unity Events", true);
			if (_target._unityEventsFolded)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("OnSelected"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("OnDeselected"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("OnClick"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("OnPressed"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("OnReleased"));
			}

			serializedObject.ApplyModifiedProperties();
		}
		#endregion
	}
}