﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class AudioFader : MonoBehaviour
{
    private bool fade = false;
    private AudioSource audioSource;  
    [Range(0f, 10f)]
    public float musicFadeSpeed;
    public bool DestroyAfterFade;
    public UnityEvent FadedEvent;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (DestroyAfterFade)
            DontDestroyOnLoad(gameObject);
    }

    public void Begin()
    {
        if (fade) return;
        fade = true;
    }

    void Update()
    {        
        if (fade == true)
        {
            float oldvolume = audioSource.volume;
            audioSource.volume = Mathf.Max(0, audioSource.volume - musicFadeSpeed * Time.deltaTime);
            if (audioSource.volume <= 0)
            {
                fade = false;
                FadedEvent?.Invoke();
                if (DestroyAfterFade)
                    Destroy(gameObject);
            }
        }
    }   
}