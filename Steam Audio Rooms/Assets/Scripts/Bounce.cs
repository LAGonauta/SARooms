using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {

  [SerializeField] private Vector3 m_speed;
  [SerializeField] private AudioClip m_bounce_clip;
  private AudioSource m_audiosource;
  private ContactPoint m_contact;
  private Vector3 m_calculated_velocity;
  private Vector3 m_velocity;

	// Use this for initialization
	void Start ()
  {
    m_velocity = m_speed;
    m_audiosource = GetComponent<AudioSource>();
	}

  void Update()
  {
    transform.Translate(m_velocity * Time.deltaTime);
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.collider.tag != "Player")
    {
      m_contact = collision.contacts[0];
      m_calculated_velocity = Vector3.Reflect(m_velocity, m_contact.normal);
      if (m_calculated_velocity.y != 0.0f)
      {
        m_calculated_velocity.y = 0;
        m_calculated_velocity = m_calculated_velocity.normalized * m_velocity.magnitude;
      }
      m_velocity = m_calculated_velocity;
      if (m_audiosource != null && m_bounce_clip != null)
      {
        m_audiosource.PlayOneShot(m_bounce_clip);
      }
    }

  }

}
