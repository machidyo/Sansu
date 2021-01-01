using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class GameViewer : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    private DeDebug deDebug;
    
    void Start()
    {
        deDebug = FindObjectOfType<DeDebug>();
    }


    void Update()
    {
        
    }
}
