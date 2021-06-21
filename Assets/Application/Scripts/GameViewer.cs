using System;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using TMPro;
using UniRx;
using UnityEngine;

public class GameViewer : MonoBehaviour
{
    [SerializeField] private CountDown countDown;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private MMFeedbacks timerFeedback;
    
    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI statusText;
    private DeDebug deDebug;

    private GameController gameController;
    private BGM bgm;

    [CanBeNull] private IDisposable scoreDisposable;
    [CanBeNull] private IDisposable timerDisposable;
    
    async void Start()
    {
        gameController = FindObjectOfType<GameController>();
        bgm = FindObjectOfType<BGM>();
        deDebug = FindObjectOfType<DeDebug>();

        await gameController.StartAsync();
        await bgm.StartAsync();
        await deDebug.StartAsync();

        gameController.CurrentStatus.Subscribe(status =>
        {
            switch (status)
            {
                case GameController.Status.Ready:
                    bgm.Play(BGM.Bgm.Ready, 0.5f);
                    scoreText.text = "0";
                    timerText.text = "0";
                    break;
                case GameController.Status.CountDown:
                    bgm.Play(BGM.Bgm.Play, 0.1f);
                    countDown.StartCountDown();
                    break;
                case GameController.Status.Play:
                    bgm.SetVolume(0.5f);
                    scoreDisposable = gameController.Score.Subscribe(score => scoreText.text = $"{score}");
                    timerDisposable = Timer.RemainingSecond.Subscribe(timer =>
                    {
                        timerText.text = $"{timer}";
                        timerFeedback.PlayFeedbacks();
                    });
                    break;
                case GameController.Status.Result:
                    if (gameController.IsClear)
                    {
                        bgm.Play(BGM.Bgm.GameClear, 0.5f);
                    }
                    else
                    {
                        bgm.Play(BGM.Bgm.GameOver, 0.5f);
                    }
                    scoreDisposable?.Dispose();
                    timerDisposable?.Dispose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        });
        
        deDebug.IsDebug
            .DistinctUntilChanged()
            .Subscribe(isDebug => statusText.gameObject.SetActive(isDebug))
            .AddTo(this);
    }

    void Update()
    {
        // 本当はプレイヤーの違う入力
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameController.StartGame();
        }
    }

    private void OnDestroy()
    {
        scoreDisposable?.Dispose();
        timerDisposable?.Dispose();
    }
}
