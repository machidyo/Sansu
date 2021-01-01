using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class PlaneHandle : MonoBehaviour
{
    [SerializeField] private Transform direction;

    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Material on;
    [SerializeField] private Material off;

    private bool isOn = false;

    private float updateInterval = 0.01f; // s
    private float speed = 0.01f;

    void Start()
    {
        OnTriggerEnter(null);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction.transform.localPosition += new Vector3(-speed, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction.transform.localPosition += new Vector3(speed, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction.transform.localPosition += new Vector3(0, speed, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction.transform.localPosition += new Vector3(0, -speed, 0);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        renderer.materials = new[] {on};
        isOn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        renderer.materials = new[] {off};
        isOn = false;
    }
}
