using UniRx;
using UnityEngine;

public class DeDebug : MonoBehaviour
{
    public ReactiveProperty<bool> IsDebug = new ReactiveProperty<bool>();
    
    [SerializeField] private GameObject statusText;

    void Start()
    {
        IsDebug.DistinctUntilChanged().Subscribe(d =>
        {
            statusText.SetActive(d);
        });
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            IsDebug.Value = !IsDebug.Value;
        }
    }
}
