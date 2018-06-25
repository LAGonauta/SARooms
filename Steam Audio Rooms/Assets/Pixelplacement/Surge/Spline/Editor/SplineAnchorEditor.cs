/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Forces pivot mode to center so an anchor's pivot is always correct while adjusting a spline.
/// 
/// </summary>

using UnityEditor;
using UnityEngine;

namespace Pixelplacement
{
	[CustomEditor(typeof(SplineAnchor))]
	public class SplineAnchorEditor : Editor
	{
		#region Scene GUI
		void OnSceneGUI ()
		{
			//ensure pivot is used so anchor selection has a proper transform origin:
			if (Tools.pivotMode == PivotMode.Center)
			{
				Tools.pivotMode = PivotMode.Pivot;
			}
		}
		#endregion
	}
}