using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private const int PLAY_TIME = 180;
    
    public enum Status
    {
        Ready = 0,
        CountDown = 1,
        Play = 2,
        Result = 3,
    }
    
    public ReactiveProperty<Status> CurrentStatus = new ReactiveProperty<Status>();
    public ReactiveProperty<int> Score = new ReactiveProperty<int>();

    public bool IsClear { get; private set; }

    private bool canStart = false;
    
    async void Start()
    {
        // ready to play
        Reset();
        CurrentStatus.Value = Status.Ready;
        await UniTask.WaitUntil(() => canStart);
        
        // count down
        CurrentStatus.Value = Status.CountDown;
        await UniTask.Delay(TimeSpan.FromSeconds(4));

        // game start
        CurrentStatus.Value = Status.Play;
        Timer.Start(PLAY_TIME);

        await WaitGameEnd();

        // show result
        CurrentStatus.Value = Status.Result;
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        
        SceneManager.LoadScene("Sansu");
    }

    public void StartGame()
    {
        canStart = true;
    }

    private void Reset()
    {
        IsClear = false;
        canStart = false;
        Score.Value = 0;
        Timer.Reset();
    }

    private async UniTask WaitGameEnd()
    {
        while (true)
        {
            if (Timer.IsEnd())
            {
                IsClear = true;
                break;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.1));
        }
    }
}
