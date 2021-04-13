using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class SamplePlane : MonoBehaviour
{
    private const float FLY_UPDATE_INTERVAL = 0.1f; // s
    
    [SerializeField] private Transform m5StickC;

    private bool canFly = false;
    private float flySpeed = 0.1f;

    // memo
    //   sample の動きは上々。
    //   Game シーンだとがくがくしたりするのでその解明から。
    
    void Start()
    {
        canFly = true;
        Fly().Forget();
    }
    
    private async UniTask Fly()
    {
        while (canFly)
        {
            var forward = m5StickC.forward * flySpeed;
            transform.DOMove(transform.position + forward, FLY_UPDATE_INTERVAL);
            transform.rotation = m5StickC.rotation;

            await UniTask.Delay(TimeSpan.FromSeconds(FLY_UPDATE_INTERVAL));
        }
    }
}
