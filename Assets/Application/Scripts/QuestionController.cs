using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
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

    public int X { get; private set; } = -1;
    public int Y { get; private set; } = -1;
    public OperatorKind Operation { get; private set; }
    public int Answer { get; private set; } = -1;
    // todo: 間違い二つ、三つと増やす
    public int WrongAnswer { get; private set; } = -1;

    void Start()
    {
        SetQuestion();
    }

    private void Reset()
    {
        X = -1;
        Y = -1;
        Answer = -1;
        WrongAnswer = -1;
    }
    
    public void SetQuestion()
    {
        Reset();
        
        // todo: 繰り上げ、繰り下げモードを追加する
        while (Answer < 0)
        {
            X = Random.Range(0, 19);
            Y = Random.Range(0, 19);
            Operation = Random.Range(0, 2) % 2 == 0 ? OperatorKind.Plus : OperatorKind.Minus;
            Answer = Calculate();

            while (WrongAnswer < 0 || WrongAnswer == Answer)
            {
                WrongAnswer = Random.Range(0, 2) % 2 == 0 
                    ? Answer + Random.Range(1, 3) 
                    : Answer - Random.Range(1, 3);
            }
        }
    }

    public int Calculate()
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
}
