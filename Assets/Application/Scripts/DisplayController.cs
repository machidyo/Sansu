using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class DisplayController : MonoBehaviour
{
    [SerializeField] private bool isDebug;
    
    [SerializeField] private TextMeshProUGUI statusText;
    
    [SerializeField] private GameStatusManager status;

    void Start()
    {
        if (isDebug)
        {
            status.CurrentStatus.DistinctUntilChanged().Subscribe(s => statusText.text = "" + s);
        }
        
        
    }


    void Update()
    {
        
    }
}
