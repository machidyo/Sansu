using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestionController : MonoBehaviour
{
    public enum OperatorKind
    {
        Plus = 0,
        Minus = 1,
        Multiply = 2,
        Divide = 3
    }
    
    public ReactiveProperty<bool> HasQuestion = new ReactiveProperty<bool>();
    public Action OnCorrect;

    public int X { get; private set; } = -1;
    public int Y { get; private set; } = -1;
    public OperatorKind Operation { get; private set; }
    public int Answer { get; private set; } = -1;
    public int WrongAnswer { get; private set; } = -1;
    public int CorrectIndex { get; private set; } = -1;

    private bool canQuestion = false;

    void Start()
    {
        ResetQuestion();

        HasQuestion
            .DistinctUntilChanged()
            .Where(_ => canQuestion)
            .Where(has => !has)
            .Subscribe(_ =>
            {
                Debug.Log("HasQuestion call SetQuestion");
                SetQuestion();
            })
            .AddTo(this);
    }

    public void StartQuestion()
    {
        canQuestion = true;
        SetQuestion();
    }

    public void StopQuestion()
    {
        ResetQuestion();
        canQuestion = false;
    }

    private void ResetQuestion()
    {
        X = -1;
        Y = -1;
        Answer = -1;
        WrongAnswer = -1;
        CorrectIndex = -1;
    }

    private void SetQuestion()
    {
        ResetQuestion();

        // todo: 繰り上げ、繰り下げモードを追加する
        while (Answer < 0)
        {
            X = Random.Range(0, 20);
            Y = Random.Range(0, 20);
            Operation = Random.Range(0, 2) % 2 == 0 ? OperatorKind.Plus : OperatorKind.Minus;
            Answer = Calculate();
        }

        while (WrongAnswer < 0 || WrongAnswer == Answer)
        {
            WrongAnswer = Random.Range(0, 2) % 2 == 0
                ? Answer + Random.Range(1, 3)
                : Answer - Random.Range(1, 3);
        }

        CorrectIndex = Random.Range(0, 2);
        HasQuestion.Value = true;
    }

    private int Calculate()
    {
        switch (Operation)
        {
            case OperatorKind.Plus:
                return X + Y;
            case OperatorKind.Minus:
                return X - Y;
            case OperatorKind.Multiply:
                return X * Y;
            case OperatorKind.Divide:
                return X / Y;
            default:
                return X + Y;
        }
    }

    public bool CheckAnswer(int index)
    {
        var isCorrect = CorrectIndex == index;
        if (isCorrect)
        {
            OnCorrect?.Invoke();
        }

        HasQuestion.Value = false;
        return isCorrect;
    }
}
