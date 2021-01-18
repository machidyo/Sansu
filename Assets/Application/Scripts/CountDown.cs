using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class CountDown : MonoBehaviour
{
    [SerializeField] private AudioSource audio;

    [SerializeField] private GameObject three;
    [SerializeField] private GameObject two;
    [SerializeField] private GameObject one;
    [SerializeField] private ParticleSystem zero;

    private GameObject current;
    
    public async void StartCountDown()
    {
        audio.Play();
        SwitchNumber(three);
        MoveNumber(current);
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        SwitchNumber(two);
        MoveNumber(current);
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        SwitchNumber(one);
        MoveNumber(current);
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        Destroy(current);
        Instantiate(zero, transform).Play();
        await UniTask.Delay(TimeSpan.FromSeconds(1));
    }

    private void SwitchNumber(GameObject num)
    {
        if (current != null)
        {
            Destroy(current);
        }
        current = Instantiate(num, transform);
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
