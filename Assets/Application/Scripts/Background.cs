using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Background : MonoBehaviour
{
    private const float UPDATE_INTERVAL = 0.1f; // s
    private const float MAX_SPEED_MOVEMENT = 1f;

    [SerializeField] private GameObject stageGameObject;
    [SerializeField] private GameObject currentStage;
    [SerializeField] private GameObject nextStage;

    [SerializeField] private Transform m5StickC;

    public GameObject NextStage => nextStage;

    private bool canMove = false;
    private float moveSpeed = 0.1f;

    void Start()
    {
        Time.timeScale = 5;
        
        Reset();
        Move().Forget();
        CheckDistance().Forget();
    }

    private async UniTask CheckDistance()
    {
        await UniTask.WaitUntil(() => canMove);
        while (canMove)
        {
            if (currentStage.transform.position.z <= -500)
            {
                Destroy(currentStage);
                currentStage = nextStage;
                var pos = new Vector3(0, 0, 1100);
                nextStage = Instantiate(stageGameObject.transform, pos, Quaternion.identity).gameObject;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public async UniTask StartToMove()
    {
        canMove = true;
        moveSpeed = 0;
        
        var duration = 3; // s
        var loop = duration / UPDATE_INTERVAL;
        for (var i = 0; i < loop; i++)
        {
            moveSpeed += MAX_SPEED_MOVEMENT / loop;
            await UniTask.Delay(TimeSpan.FromSeconds(UPDATE_INTERVAL));
        }
    }
    
    public async UniTask StopToMove()
    {
        var duration = 3; // s
        var loop = duration / UPDATE_INTERVAL;
        for (var i = 0; i < loop; i++)
        {
            moveSpeed -= MAX_SPEED_MOVEMENT / loop;
            await UniTask.Delay(TimeSpan.FromSeconds(UPDATE_INTERVAL));
        }

        moveSpeed = 0;
        canMove = false;
    }
    
    private void Reset()
    {
        canMove = false;
        moveSpeed = 0.0f;
    }

    private async UniTask Move()
    {
        await UniTask.WaitUntil(() => canMove);
        while (canMove)
        {
            var forward = -m5StickC.forward * moveSpeed;
            currentStage.transform.DOMove(currentStage.transform.position + forward, UPDATE_INTERVAL);
            nextStage.transform.DOMove(nextStage.transform.position + forward, UPDATE_INTERVAL);
            await UniTask.Delay(TimeSpan.FromSeconds(UPDATE_INTERVAL));
        }
    }
}
