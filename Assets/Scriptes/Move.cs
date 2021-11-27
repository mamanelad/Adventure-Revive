using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    
    private bool _leftPressed;
    private bool _rightPressed;
    private bool _upPressed;
    private bool _downPressed;


    private void FixedUpdate()
    {
        if (_leftPressed)
            rigidbody2D.velocity = new Vector2(-3, 0);
        
        if (_rightPressed)
            rigidbody2D.velocity = new Vector2(3, 0);
        
        if (_upPressed)
            rigidbody2D.velocity = new Vector2(0, 3);
        
        if (_downPressed)
            rigidbody2D.velocity = new Vector2(0, -3);
        
    }

    private void Update()
    {
        _leftPressed = Input.GetKey(KeyCode.LeftArrow);
        _rightPressed = Input.GetKey(KeyCode.RightArrow);
        _upPressed = Input.GetKey(KeyCode.UpArrow);
        _downPressed = Input.GetKey(KeyCode.DownArrow);
        
    }
}
