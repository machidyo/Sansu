using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class PlayerPlane : MonoBehaviour
{
    private const int MAX_ROTATE_SPEED = 5;
    
    [SerializeField] private GameObject propeller;
    
    void Start()
    {
        Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x =>
        {
            var speed = Mathf.Min(MAX_ROTATE_SPEED, x);
            var rotationSpeed = speed * 360;
            propeller
                .transform
                .DORotate(new Vector3(0, 0, rotationSpeed), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear);
        });
    }

    void Update()
    {
        
    }
}
