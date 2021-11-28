using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _shared;

    public Camera[] cameras;
    private int _camAmount;
    private int _curCam;

    // Start is called before the first frame update

    public void Awake()
    {
        cameras[1].enabled = false;
    }

    void Start()
    {
        _shared = this;
        _camAmount = cameras.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SwitchCamara(int num)
    {
        _shared.cameras[_shared._curCam].enabled = false;
        _shared._curCam = num;
        _shared.cameras[_shared._curCam].enabled = true;
    }
}
