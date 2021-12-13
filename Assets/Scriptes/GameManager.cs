using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public UnityEvent dragonBackToNormal;

    public UnityEvent eatPlayer;
    // public UnityEvent openGateBlack;
    // Start is called before the first frame update

    public Dragon[] dragons;
    public Player player;

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

    public static void EatPlayer()
    {
        _shared.eatPlayer.Invoke();
    }

    public static void DragonReturnNormal()
    {
        _shared.dragonBackToNormal.Invoke();
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
        DragonManageCamara(num, 0);
    }


    private static void DragonManageCamara(int num, int dragonNum)
    {
        var curDragon = _shared.dragons[dragonNum];
        if (curDragon.curCamara == num)
        {
            curDragon.startWithPatrol = false;
            curDragon.metPlayer = true;
            curDragon._move = true;
        }


        else
        {
            if (curDragon.CompareTag("Dragon") & curDragon.metPlayer & !curDragon._deadDragon)
            {
                if (!_shared.player._withItem & curDragon._move)
                {
                    curDragon._move = false;
                    curDragon.rigidB.position = _shared.cameras[curDragon.curCamara].transform.position;
                }

                else
                {
                    if (curDragon._move)
                    {
                        curDragon.curCamara = num;
                    }
                }
            }
        }
    }


    private void Update()
    {
        var target = dragons[1];
        if (target.metPlayer) return;
        for (int i = 0; i < cameras.Length; i++)
        {
            var cam = cameras[i];
            var targetRander = target.GetComponent<Renderer>();
            if (IsVisible(cam, target))
            {
                target.curCamara = i;
                if (target.curCamara == _shared.player.curCamara)
                {   
                    DragonManageCamara(i, 1);
                    break;
                }
            }
        }
    }

    private bool IsVisible(Camera c, Dragon target)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = target.transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }

        return true;
    }
}