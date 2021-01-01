using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class DeDebug : MonoBehaviour
{
    public ReactiveProperty<bool> IsDebug = new ReactiveProperty<bool>();
    
    [SerializeField] private GameStatusController status;
    [SerializeField] private TextMeshProUGUI statusText;

    void Start()
    {
        IsDebug.DistinctUntilChanged().Subscribe(isDebug => statusText.gameObject.SetActive(isDebug));
        status.CurrentStatus.DistinctUntilChanged().Subscribe(s => statusText.text = s.ToString());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            IsDebug.Value = !IsDebug.Value;
        }
    }
}
