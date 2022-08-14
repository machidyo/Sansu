using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class PlanePlayer : MonoBehaviour
{
    private const int MAX_ROTATE_SPEED = 5;

    private const float FLY_UPDATE_INTERVAL = 0.1f; // s
    private const float FLY_MAX_SPEED = 1f;
    
    [SerializeField] private GameObject propeller;
    [SerializeField] private Transform m5StickC;

    [SerializeField] private ParticleSystem wind;

    private bool canFly = false;
    private float flySpeed = 0.1f;

    void Start()
    {
        Reset();
        Fly().Forget();
    }

    void OnDestroy()
    {
        Destroy(propeller);
    }
    
    public async UniTask RotatePropeller()
    {
        for (var i = 0;; i++)
        {
            var speed = Mathf.Min(MAX_ROTATE_SPEED, i);
            var rotationSpeed = speed * 360;
            var duration = 1f;
            propeller
                .transform
                .DOLocalRotate(new Vector3(0, 0, rotationSpeed), duration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
    
    public async UniTask StartToFly()
    {
        canFly = true;
        flySpeed = 0;
        
        var duration = 3; // s
        var loop = duration / FLY_UPDATE_INTERVAL;
        for (var i = 0; i < loop; i++)
        {
            flySpeed += FLY_MAX_SPEED / loop;
            await UniTask.Delay(TimeSpan.FromSeconds(FLY_UPDATE_INTERVAL));
        }
    }
    
    public async UniTask StopToFly()
    {
        var duration = 3; // s
        var loop = duration / FLY_UPDATE_INTERVAL;
        for (var i = 0; i < loop; i++)
        {
            flySpeed -= FLY_MAX_SPEED / loop;
            await UniTask.Delay(TimeSpan.FromSeconds(FLY_UPDATE_INTERVAL));
        }

        flySpeed = 0;
        canFly = false;
    }
    
    private void Reset()
    {
        canFly = false;
        flySpeed = 0.0f;
        wind.Pause();
    }
    
    private async UniTask Fly()
    {
        await UniTask.WaitUntil(() => canFly);

        wind.Play();
        while (canFly)
        {
            transform.rotation = m5StickC.rotation;
            
            var em = wind.emission;
            em.rateOverTime = 100 * flySpeed / FLY_MAX_SPEED;

            await UniTask.Delay(TimeSpan.FromSeconds(FLY_UPDATE_INTERVAL));
        }
    }
}
