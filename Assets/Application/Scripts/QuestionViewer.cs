using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestionViewer : MonoBehaviour
{
    [SerializeField] private GameObject questionSet;
    
    [SerializeField] private List<GameObject> numbers;
    [SerializeField] private List<GameObject> operators;

    private QuestionController questionController;

    private void Awake()
    {
    }

    void Start()
    {
        questionController = FindObjectOfType<QuestionController>();
        ViewQuestion();
    }
    
    public async void ViewQuestion()
    {
        await UniTask.WaitUntil(() => questionController.Answer > 0);
        
        var pos = transform.position + Vector3.forward * 30;
        var set = Instantiate(questionSet, pos, Quaternion.identity);

        var xTran = set.transform.FindChildRecursive("X");
        ShowNumber(questionController.X, xTran);

        var opeTran = set.transform.FindChildRecursive("Operation");
        ShowOperator(questionController.Operation, opeTran);
        
        var yTran = set.transform.FindChildRecursive("Y");
        ShowNumber(questionController.Y, yTran);

        // todo: 複数の間違いに対応する
        var leftAnswer = set.transform.FindChildRecursive("LeftAnswerNumber");
        var rightAnswer = set.transform.FindChildRecursive("RightAnswerNumber");
        var isEven = Random.Range(0, 2) % 2 == 0;
        if (isEven)
        {
            ShowNumber(questionController.Answer, leftAnswer);
            ShowNumber(questionController.WrongAnswer, rightAnswer);
        }
        else
        {
            ShowNumber(questionController.WrongAnswer, leftAnswer);
            ShowNumber(questionController.Answer, rightAnswer);
        }
    }

    /// <summary>
    /// parent の位置をセンターにして、 number を GameObject で表示する
    /// </summary>
    private void ShowNumber(int number, Transform parent)
    {
        var indexes = new List<int>();
        do
        {
            indexes.Add(number % 10);
            number /= 10;
        } while (number > 0);

        var center = indexes.Count / 2;
        for (var i = 0; i < indexes.Count; i++)
        {
            var num = Instantiate(numbers[indexes[i]], parent);
            // 0.4f は画面上で見て良い値をマジックナンバーとして設定
            var posX = indexes.Count % 2 == 0 
                ? 0.4f * (center - i) - 0.2f  // 偶数個の場合センターが文字の区切りになるので -0.2f している
                : 0.4f * (center - i);        // 奇数個の場合
            num.transform.localPosition = new Vector3(posX, 0.0f, 0.0f);
            num.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void ShowOperator(QuestionController.OperatorKind kind, Transform parent)
    {
        var ope = Instantiate(operators[(int)kind], parent);
        switch (kind)
        {
            case QuestionController.OperatorKind.Plus:
                break;
            case QuestionController.OperatorKind.Minus:
                ope.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                ope.transform.localScale = new Vector3(1.0f, 0.8f, 1.0f);
                break;
            case QuestionController.OperatorKind.Multiply:
            case QuestionController.OperatorKind.Divide:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }
    }
}