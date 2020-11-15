using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class Counter : MonoBehaviour
{
    [SerializeField] private AudioSource audio;

    [SerializeField] private GameObject three;
    [SerializeField] private GameObject two;
    [SerializeField] private GameObject one;
    [SerializeField] private ParticleSystem zero;

    private Vector3 countPos;
    private GameObject current;
    
    void Start()
    {
        countPos = transform.position;
        CountDown();
    }

    private void CountDown()
    {
        Observable.Interval(TimeSpan.FromSeconds(1.0f)).Take(4).Subscribe(x =>
        {
            Debug.Log(3-x);
            switch (3 - x)
            {
                case 3:
                    audio.Play();
                    SetNumber(three);
                    MoveNumber(current);
                    break;
                case 2:
                    SetNumber(two);
                    MoveNumber(current);
                    break;
                case 1:
                    SetNumber(one);
                    MoveNumber(current);
                    break;
                case 0:
                    Destroy(current);
                    zero.Play();
                    break;
            }
        });
    }

    private void SetNumber(GameObject num)
    {
        if (current != null)
        {
            Destroy(current);
        }
        current = Instantiate(num, countPos, Quaternion.identity, transform);
    }

    private void MoveNumber(GameObject num)
    {
        var tran = num.transform;
        var origin = tran.position;
        var top = origin + new Vector3(0, 1.0f, 0);
        DOTween.Sequence()
            .Append(tran.DORotate(new Vector3(0, 540, 0), 0.3f, RotateMode.FastBeyond360))
            .Append(tran.DOMove(top, 0.2f))
            .Append(tran.DOMove(origin, 0.2f))
            .Append(tran.DOScale(Vector3.one * 3f, 0.1f))
            .Append(tran.DOScale(Vector3.one * 0.7f, 0.1f))
            .Append(tran.DOScale(Vector3.one * 1.0f, 0.1f))
            .Play();
    }
}
