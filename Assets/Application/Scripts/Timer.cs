using System;
using Cysharp.Threading.Tasks;
using UniRx;

public class Timer
{
    public static DateTime StartTime { get; private set; }
    
    public static ReactiveProperty<int> ElapsedSecond = new ReactiveProperty<int>();
    public static ReactiveProperty<int> RemainingSecond = new ReactiveProperty<int>();

    private static double elapsedSecond  => (DateTime.Now - StartTime).TotalSeconds;
    private static int playTime = 0; // s
    private static bool isRunning;
    
    public static void Start(int playTime)
    {
        Restart(DateTime.Now, playTime);
    }

    /// <summary>
    /// メモリ上の StartTime が破棄されていて、ロードデータから Restart することを前提としている。
    /// </summary>
    public static void Restart(DateTime startTime, int playTime)
    {
        StartTime = startTime;
        Timer.playTime = playTime;
        UpdateRemainingTime().Forget();
    }

    private static async UniTask UpdateRemainingTime()
    {
        isRunning = true;
        while (isRunning)
        {
            ElapsedSecond.Value = (int)elapsedSecond;
            RemainingSecond.Value = playTime - (int)elapsedSecond;
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public static void Reset()
    {
        isRunning = false;
        playTime = 0;
        StartTime = DateTime.MinValue;
        RemainingSecond.Value = 0;
        ElapsedSecond.Value = 0;
    }

    public static bool IsEnd()
    {
        return elapsedSecond >= playTime;
    }
}