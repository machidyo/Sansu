using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] private GameStatusController status;
    
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
                case GameStatusController.Status.Ready:
                    audio.volume = 0.5f;
                    Play(ready);
                    break;
                case GameStatusController.Status.CountDown:
                    audio.volume = 0.1f;
                    Play(playing);
                    break;
                case GameStatusController.Status.Playing:
                    audio.volume = 0.5f;
                    break;
                case GameStatusController.Status.GaveOver:
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
