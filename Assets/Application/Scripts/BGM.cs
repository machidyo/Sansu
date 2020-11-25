using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] private GameStatusManager status;
    
    [SerializeField] private AudioClip ready;
    [SerializeField] private AudioClip playing;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip winner;

    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        
        status.CurrentStatus.DistinctUntilChanged().Subscribe(s =>
        {
            switch (s)
            {
                case GameStatusManager.Status.Ready:
                    audio.volume = 0.75f;
                    Play(ready);
                    break;
                case GameStatusManager.Status.CountDown:
                    audio.Stop();
                    break;
                case GameStatusManager.Status.Playing:
                    audio.volume = 0.5f;
                    Play(playing);
                    break;
                case GameStatusManager.Status.GaveOver:
                    // todo: 結果次第で gameover へ
                    Play(winner);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, null);
            }
        }).AddTo(this);
    }

    private void Play(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }
}
