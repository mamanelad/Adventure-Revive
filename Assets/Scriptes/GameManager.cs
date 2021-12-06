using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    
    private static GameManager _shared;

    public Camera[] cameras;
    private int _curCam;

    public UnityEvent yellowSwordRoom;
    public UnityEvent openGateYellow;

    public UnityEvent blackTrophyRoom;
    // public UnityEvent openGateBlack;
    // Start is called before the first frame update

    public void Awake()
    {
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].enabled = false;
        }
        
    }

    void Start()
    {
        _shared = this;
    }

  
    public static void SwitchCamara(int num)
    {
        if (num == 1)
        {
            _shared.yellowSwordRoom.Invoke(); 
            _shared.openGateYellow.Invoke();
        }

        if (num == 12)
        {
            _shared.blackTrophyRoom.Invoke();
        }
        
        _shared.cameras[_shared._curCam].enabled = false;
        _shared._curCam = num;
        _shared.cameras[_shared._curCam].enabled = true;
    }
}