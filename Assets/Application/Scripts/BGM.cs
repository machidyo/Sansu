using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public enum Bgm
    {
        Ready,
        Play,
        GameOver,
        GameClear
    }
    
    [SerializeField] private List<AudioClip> audios;

    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void Play(Bgm bgm, float volume = 0.5f)
    {
        audio.clip = audios[(int)bgm];
        audio.volume = volume;
        audio.Play();
    }

    public void SetVolume(float volume)
    {
        audio.volume = volume;
    }
}
