using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class FollowPath : MonoBehaviour {
  [SerializeField] private Spline m_spline;
  [SerializeField] private Transform m_object;
  [SerializeField] private Tween.LoopType m_looptype;
  [SerializeField] private bool m_face_direction = false;
  [SerializeField] private float m_duration = 1;
  [SerializeField] private float m_start_percentage = 0;
  [SerializeField] private float m_end_percentage = 1;
  void Awake ()
  {
    Tween.Spline(m_spline, m_object, m_start_percentage, m_end_percentage, m_face_direction, m_duration, 0, Tween.EaseLinear, m_looptype);
	}
}
