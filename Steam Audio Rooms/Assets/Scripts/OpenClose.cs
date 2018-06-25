using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClose : MonoBehaviour
{

  [SerializeField] private float m_speed = 0;
  [SerializeField] private Vector3 m_open_target;
  [SerializeField] private AudioClip m_door_open;
  [SerializeField] private AudioClip m_door_open_stop;
  [SerializeField] private AudioClip m_door_close;
  [SerializeField] private AudioClip m_door_close_stop;
  private AudioSource m_audiosource;
  private Vector3 m_close_target;
  private Vector3 m_target;
  private bool m_closing = false;
  private bool m_opening = false;
  private float step = 0;

  // Use this for initialization
  void Start()
  {
    m_audiosource = gameObject.GetComponentInChildren<AudioSource>();
    // Dummy AudioSource if there is no available.
    if (m_audiosource == null)
      m_audiosource = new AudioSource();
    m_close_target = transform.GetChild(0).position;
    m_target = m_close_target;
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      m_audiosource.PlayOneShot(m_door_open);
      m_target = m_open_target;
      m_opening = true;
    }
  }

  void OnTriggerExit(Collider other)
  {
    
    if (other.CompareTag("Player"))
    {
      m_audiosource.PlayOneShot(m_door_close);
      m_target = m_close_target;
      m_closing = true;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (m_opening)
    {
      m_target = m_open_target;
    }
    else if (m_closing)
    {
      m_target = m_close_target;
    }

    if (m_target != transform.GetChild(0).position)
    {
      step = m_speed * Time.deltaTime;
      transform.GetChild(0).position = Vector3.MoveTowards(transform.GetChild(0).position, m_target, step);
    }
    else
    {
      if (m_opening)
      {
        m_audiosource.Stop();
        m_audiosource.PlayOneShot(m_door_open_stop);
        m_opening = false;
      }
      else if (m_closing)
      {
        m_audiosource.Stop();
        m_audiosource.PlayOneShot(m_door_close_stop);
        m_closing = false;
      }      
    }
  }
}
