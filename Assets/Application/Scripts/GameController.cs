using System;
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

    [SerializeField] private QuestionController questionController;
    [SerializeField] private PlanePlayer planePlayer;
    [SerializeField] private Background background;
    
    public ReactiveProperty<Status> CurrentStatus = new ReactiveProperty<Status>();
    public ReactiveProperty<int> Score = new ReactiveProperty<int>();

    public bool IsClear { get; private set; }

    private bool canPlay = false;
    
    async void Start()
    {
        // ready to play
        Reset();
        CurrentStatus.Value = Status.Ready;
        await UniTask.WaitUntil(() => canPlay);
        
        // count down
        CurrentStatus.Value = Status.CountDown;
        planePlayer.RotatePropeller().Forget();
        await UniTask.Delay(TimeSpan.FromSeconds(4));

        // game start
        CurrentStatus.Value = Status.Play;
        Timer.Start(PLAY_TIME);
        planePlayer.StartToFly().Forget();
        background.StartToMove().Forget();
        questionController.OnCorrect = () => Score.Value++;
        questionController.StartQuestion();
        
        await WaitGameEnd();

        // game stop
        questionController.StopQuestion();
        questionController.OnCorrect = null;
        planePlayer.StopToFly().Forget();
        background.StopToMove().Forget();
        
        // show result
        CurrentStatus.Value = Status.Result;
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        
        SceneManager.LoadScene("Sansu");
    }

    public void StartGame()
    {
        canPlay = true;
    }

    private void Reset()
    {
        IsClear = false;
        canPlay = false;
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
