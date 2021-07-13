using System;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UniRx;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private Transform ring;
    [SerializeField] private Transform number;
    
    [SerializeField] private ParticleSystem sparkle;
    [SerializeField] private GameObject fireTrail;

    [SerializeField] private MMFeedbacks correctFeedback;
    [SerializeField] private MMFeedbacks wrongFeedback;
    
    void Start()
    {
        Observable
            .Interval(TimeSpan.FromSeconds(1.0f))
            .Subscribe(x =>
            {
                MoveRing((int) x);
                MoveNumber();
            }).AddTo(this);
    }

    private void MoveRing(int num)
    {
        DOTween.Sequence()
            .Append(ring.DORotate(new Vector3(0, 0, num * 360), 0.3f))
            .Append(ring.DOScale(Vector3.one * 1.3f, 0.2f))
            .Append(ring.DOScale(Vector3.one * 0.9f, 0.2f))
            .Append(ring.DOScale(Vector3.one * 1.0f, 0.2f))
            .Play();
    }

    private void MoveNumber()
    {
        DOTween.Sequence()
            .Append(number.DOLocalRotate(new Vector3(0, 90, 0), 0.1f))
            .Append(number.DOLocalRotate(new Vector3(0, 180, 0), 0.1f))
            .Append(number.DOLocalRotate(new Vector3(0, 270, 0), 0.1f))
            .Append(number.DOLocalRotate(new Vector3(0, 360, 0), 0.1f))
            .InsertCallback(0.0f, () => sparkle.Play())
            .Play();
    }

    public void OnCorrect()
    {
        correctFeedback.PlayFeedbacks();
        var temp = Instantiate(fireTrail, transform);
        temp.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject, 2);
    }

    public void OnWrong()
    {
        wrongFeedback.PlayFeedbacks();
        Destroy(gameObject, 1);
    }
}