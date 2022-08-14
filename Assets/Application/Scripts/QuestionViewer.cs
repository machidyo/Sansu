using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class QuestionViewer : MonoBehaviour
{
    [SerializeField] private GameObject questionSet;

    [SerializeField] private List<GameObject> numbers;
    [SerializeField] private List<GameObject> operators;

    private QuestionController questionController;

    private GameObject set;
    private CancellationTokenSource throughCheckerCancellationTokenSource;

    void Start()
    {
        questionController = FindObjectOfType<QuestionController>();

        questionController.HasQuestion
            .DistinctUntilChanged()
            .Where(has => has)
            .Subscribe(_ =>
            {
                ViewQuestion();
                SetColliderAction();
            })
            .AddTo(this);
    }

    void OnDestroy()
    {
        Destroy(set);
    }

    private void ViewQuestion()
    {
        var pos = transform.position + Vector3.forward * 150;
        set = Instantiate(questionSet, pos, Quaternion.identity);
        set.transform.parent = FindObjectOfType<Background>().NextStage.transform;
        throughCheckerCancellationTokenSource = new CancellationTokenSource();
        CheckThatPlayerThroughQuestion(throughCheckerCancellationTokenSource).Forget();

        var xTran = set.transform.FindChildRecursive("X");
        ShowNumber(questionController.X, xTran);

        var opeTran = set.transform.FindChildRecursive("Operation");
        ShowOperator(questionController.Operation, opeTran);

        var yTran = set.transform.FindChildRecursive("Y");
        ShowNumber(questionController.Y, yTran);

        var leftAnswer = set.transform.FindChildRecursive("LeftAnswerNumber");
        var rightAnswer = set.transform.FindChildRecursive("RightAnswerNumber");
        if (ConvertIndexToSide(questionController.CorrectIndex) == "Left")
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
                ? 0.4f * (center - i) - 0.2f // 偶数個の場合センターが文字の区切りになるので -0.2f している
                : 0.4f * (center - i); // 奇数個の場合
            num.transform.localPosition = new Vector3(posX, 0.0f, 0.0f);
            num.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void ShowOperator(QuestionController.OperatorKind kind, Transform parent)
    {
        var ope = Instantiate(operators[(int) kind], parent);
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

    private void SetColliderAction()
    {
        SetOnTriggerEnterAction("Left");
        SetOnTriggerEnterAction("Right");
    }

    private void SetOnTriggerEnterAction(string side)
    {
        var answer = set.transform.FindChildRecursive(side + "Answer");
        var box = answer.GetComponent<BoxCollider>();
        box.OnTriggerEnterAsObservable()
            .Where(_ => _.gameObject.name == "Plane")
            .Subscribe(_ =>
            {
                throughCheckerCancellationTokenSource.Cancel();
                
                var isCorrect = questionController.CheckAnswer(ConvertSideToIndex(side));
                if (isCorrect)
                {
                    answer.FindChildRecursive("Ring").GetComponent<Ring>().OnCorrect();
                }
                else
                {
                    answer.FindChildRecursive("Ring").GetComponent<Ring>().OnWrong();
                }

                // ここで Destory したほうがいいが、CheckAnswer の後、非同期で問題生成、表示までやると、
                // ここの Destory より早く ↑ の処理が終わってしまう問題があり、いったんステイした。
                // todo: Destroy(set);
            });
    }

    private async UniTask CheckThatPlayerThroughQuestion(CancellationTokenSource token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay(1000, cancellationToken: token.Token);

            Debug.Log(set.transform.position.z);
            if (set.transform.position.z < -99) // camera.pos.z = -90 + buffer -9
            {
                token.Cancel();
                Destroy(set);
                questionController.ThroughToAnswer();
                Debug.Log("プレイヤーが答えをスルーしてしまったので消去します。");
            }
        }   
    }

    private string ConvertIndexToSide(int index)
    {
        return index == 0 ? "Left" : "Right";
    }

    private int ConvertSideToIndex(string side)
    {
        return side == "Left" ? 0 : 1;
    }
}