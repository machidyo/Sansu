using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameStatusController : MonoBehaviour
{
    public enum Status
    {
        Ready = 0,
        CountDown = 1,
        Playing = 2,
        GaveOver = 3,
    }

    public ReactiveProperty<Status> CurrentStatus = new ReactiveProperty<Status>(Status.Ready);

    void Start()
    {
        CurrentStatus.DistinctUntilChanged().Subscribe(s =>
        {
            switch (s)
            {
                case Status.Ready:
                    break;
                case Status.CountDown:
                    Observable.Timer(TimeSpan.FromSeconds(4)).Subscribe(_ => CurrentStatus.Value = Status.Playing);
                    break;
                case Status.Playing:
                    break;
                case Status.GaveOver:
                    Observable.Timer(TimeSpan.FromSeconds(15)).Subscribe(_ => CurrentStatus.Value = Status.Ready);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, null);
            }
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CurrentStatus.Value += 1;
        }
    }
}
