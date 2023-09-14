using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip gameOverEffect;
    [SerializeField] private AudioClip gameWonEffect;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void GameOverMusic()
    {
        audioSource.clip = gameOverEffect;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void GameWonMusic()
    {
        audioSource.clip = gameWonEffect;
        audioSource.loop = false;
        audioSource.Play();
    }
}
