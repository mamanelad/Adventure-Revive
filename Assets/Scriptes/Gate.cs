using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject swordRoomTrigger;
    public GameObject gateTrigger;
    public GameObject key;
    public Animator gateAnimator;

    public int numGate;

    private bool _gateIsOpen = false;
    private bool _animationFinish = true;
    private bool gateIsBlack = false;


    private static readonly int CloseGate = Animator.StringToHash("closeGate");

    private static readonly int OpenGate = Animator.StringToHash("openGate");

    private void Awake()
    {
        if (gameObject.name == "GateBlack")
        {
            gateIsBlack = true;
        }
    }

    void Update()
    {
        if (KeyImpactGateTrigger() & _animationFinish)
        {
            _animationFinish = false;
            GateAnimation();
        }
    }

    public void SetGateOpen()
    {
        _gateIsOpen = true;
        _animationFinish = true;
    }

    public void SetGateClose()
    {
        _gateIsOpen = false;
        _animationFinish = true;
    }


    private bool KeyImpactGateTrigger()
    {
        var swordRoomPosition = this.swordRoomTrigger.transform.position;
        float posSy = swordRoomPosition.y;
        float space = 0.3f;
        var keyPosition = key.transform.position;
        float posKx = keyPosition.x;
        float posKy = keyPosition.y;

        var triggerPosition = gateTrigger.transform.position;
        float posTx = triggerPosition.x;
        float posTy = triggerPosition.y;
        return (posTx - space <= posKx & posKx <= posTx + space
                                       & posTy <= posKy
                                       & posKy < posSy);
    }

    private void GateAnimation()
    {
        gateAnimator.SetTrigger(_gateIsOpen ? CloseGate : OpenGate);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player" & _gateIsOpen)
        {
            GameManager.SwitchCamara(numGate == 1 ? 12 : 1);
        }
    }

    
    public void OpenGateFromOutSide()
    {
        var position = gateTrigger.transform.position;
            float temp = position.y;
            gateAnimator.SetTrigger(OpenGate);
            _gateIsOpen = true;
       
    }
}