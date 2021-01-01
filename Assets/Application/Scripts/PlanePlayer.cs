using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class PlanePlayer : MonoBehaviour
{
    private const int MAX_ROTATE_SPEED = 5;

    private const float FLY_UPDATE_INTERVAL = 0.01f; // s
    private const float FLY_SPEED = 0.01f;
    
    [SerializeField] private GameObject propeller;
    [SerializeField] private Transform direction;

    private bool isReady = false;

    void Start()
    {
        Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x =>
        {
            var speed = Mathf.Min(MAX_ROTATE_SPEED, x);
            var rotationSpeed = speed * 360;
            propeller
                .transform
                .DOLocalRotate(new Vector3(0, 0, rotationSpeed), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear);

            if (x > 4)
            {
                isReady = true;
            }
        });
        
        Observable.Interval(TimeSpan.FromSeconds(FLY_UPDATE_INTERVAL))
            .Where(_ => isReady)
            .Subscribe(_ =>
            {
                Fly();
            });
    }

    void Update()
    {
        
    }
    
    private void Fly()
    {
        var forward = transform.rotation * new Vector3(0, 0, FLY_SPEED);
        transform.DOMove(transform.position + forward, FLY_UPDATE_INTERVAL);
        transform.DOLookAt(direction.position, FLY_UPDATE_INTERVAL);
    }

}
