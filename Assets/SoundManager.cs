using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private static AudioSource m_audioSource;

    void Start () {
        m_audioSource = GetComponent<AudioSource>();
    }

    public static void PlayHitSound()
    {
        if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }
        m_audioSource.Play();
    }
}
