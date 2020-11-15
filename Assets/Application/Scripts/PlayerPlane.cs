using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class PlayerPlane : MonoBehaviour
{
    [SerializeField] private GameObject porp;
    
    void Start()
    {
        Observable.Interval(TimeSpan.FromMilliseconds(1000)).Subscribe(x =>
        {
            porp.transform.DORotate(new Vector3(0, 0, x * 360), 1f, RotateMode.FastBeyond360);
        });
    }

    void Update()
    {
        
    }
}
