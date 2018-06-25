using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script written by ZimonD (http://answers.unity3d.com/users/31993/zimond.html)
 * Distributed under License GNU 1.3
 * This script is published without warranty.
 * If you modify this script, document it in this small description.
 * Modified by: LAGonauta
*/

[RequireComponent(typeof(AudioSource))]
public class PlaybackPlaylist : MonoBehaviour
{

  [SerializeField] private AudioClip[] m_audioClips;           // Array for the audio clips
  [SerializeField] private bool m_loop = true;                 // Indicates whether the play list should loop when finished (start from beginning)
  [SerializeField] private float m_init_delay = 0;             // Initialization delay (seconds)
  [SerializeField] private float m_delay_between_clips = 0;    // Delay between clips (seconds)
  private AudioSource m_controlledAudioSource;                 // The audio source that will play the audio clips
  private float m_timer;                                       // Timer that keeps track of how long the audio clip has been playing
  private float m_currentAudioClipLength;                      // Length of the active audio clip in seconds
  private int m_iterator = 0;                                  // The index of the active audio clip
  private bool m_playlistEnded = false;                        // Whether or not the playlist has ended

	// Use this for initialization
	void Start ()
  {
    m_controlledAudioSource = GetComponent<AudioSource>();
    m_timer = -m_init_delay;
    // If atleast one audio clip exists, start playing:
    if (m_audioClips.Length > 0)
    {
      PlayCurrentClip();
    }
  }
	
	// Update is called once per frame
	void Update ()
  {
    // If atleast one audio clip exists and the play list has ended, update:
    if (m_audioClips.Length > 0 && !m_playlistEnded)
    {
      // Increase timer with the time difference between this and the previous frame:
      m_timer += Time.deltaTime;

      // Check whether the timer has exceeded the length of the audio clip:
      if (m_timer > m_currentAudioClipLength)
      {
        // Either start from the beginning if the last clip is played
        // or go to next audio clip:
        if (m_iterator + 1 == m_audioClips.Length)
        {
          if (m_loop)
          {
            // Set it to the first audio clip:
            m_iterator = 0;
          }
          else
          {
            // Stop the active audio clip:
            m_controlledAudioSource.Stop();

            // Set the playlist as ended:
            m_playlistEnded = true;

            // No more playing, so return:
            return;
          }

        }
        else
        {
          m_iterator++;
        }
        // Play the next audio clip:
        PlayCurrentClip();
      }
    }
  }

  /*
  * This function plays the current clip. It does not take
  * any parameters, as it accesses the global variables.
 */
  void PlayCurrentClip()
  {
    // Stop the active clip:
    m_controlledAudioSource.Stop();

    // Set the current clip as active audio clip:
    m_controlledAudioSource.clip = m_audioClips[m_iterator];

    // Set the length (in seconds) of the audio clip:
    m_currentAudioClipLength = m_audioClips[m_iterator].length;
    
    // Reset timer:
    m_timer = -m_delay_between_clips;

    // Play the clip:
    m_controlledAudioSource.PlayDelayed(m_delay_between_clips);
  }

  /*
   * This function start the playlist from an specific index.
  */
  void PlayFromIndex(int index)
  {
    if (m_iterator + 1 <= m_audioClips.Length)
    {
      // Set the new start iterator:
      m_iterator = index;

      // Play the audio clip:
      PlayCurrentClip();

      // Start the playlist again:
      m_playlistEnded = false;

    }
    else
    {
      // This is not allowed:
      Debug.Log("Index " + index + " is out of the audio clip range.");
    }
  }
}
