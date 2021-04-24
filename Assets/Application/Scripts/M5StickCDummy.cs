using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class M5StickCDummy : MonoBehaviour
{
    private bool isUpArrowDowned = false;
    private bool isDownArrowDowned = false;
    private bool isLeftArrowDowned = false;
    private bool isRightArrowDowned = false;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isUpArrowDowned = true;
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            isUpArrowDowned = false;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isDownArrowDowned = true;
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            isDownArrowDowned = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isLeftArrowDowned = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isLeftArrowDowned = false;
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isRightArrowDowned = true;
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            isRightArrowDowned = false;
        }
        
        Rotate();
    }

    private void Rotate()
    {
        if (isUpArrowDowned)
        {
            transform.Rotate(Vector3.right, 1);
        }
        if (isDownArrowDowned)
        {
            transform.Rotate(Vector3.right, -1);
        }
        if (isLeftArrowDowned)
        {
            transform.Rotate(Vector3.down, 1);
        }
        if (isRightArrowDowned)
        {
            transform.Rotate(Vector3.down, -1);
        }
    }
}
