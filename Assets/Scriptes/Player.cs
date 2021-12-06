using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject[] mazeTriggers;
    public Sprite[] sprites;
    public SpriteRenderer SpriteRenderer;
    private int numSprite;

    public Rigidbody2D rigidbody2D;

    public GameObject passToSwordRoom;
    public GameObject passToCastleRoom;

    private bool _withKey;
    public GameObject key;

    private bool _withKeyBlack;
    public GameObject keyBlack;
    
    private bool _inSwordRoom = false;

    private float _speed = 6;
    private bool _leftPressed;
    private bool _rightPressed;
    private bool _upPressed;
    private bool _downPressed;
    private int _color = 0;
    
    
    private bool _moveAfterTransfer = true;
    


    private void Awake()
    {
        numSprite = 0;
        SpriteRenderer.sprite = sprites[numSprite];
    }

    private void FixedUpdate()
    {
        if (_leftPressed)
        {
            rigidbody2D.velocity = new Vector2(-_speed, 0);
        }


        if (_rightPressed)
        {
            rigidbody2D.velocity = new Vector2(_speed, 0);
        }

        if (_upPressed)
        {
            rigidbody2D.velocity = new Vector2(0, _speed);
        }


        if (_downPressed)
        {
            rigidbody2D.velocity = new Vector2(0, -_speed);
        }
    }

    private void Update()
    {
        _leftPressed = Input.GetKey(KeyCode.LeftArrow);
        _rightPressed = Input.GetKey(KeyCode.RightArrow);
        _upPressed = Input.GetKey(KeyCode.UpArrow);
        _downPressed = Input.GetKey(KeyCode.DownArrow);

        if (Input.GetKey(KeyCode.Space) & _withKey)
        {
            _withKey = false;
        }

        if ( !_moveAfterTransfer)
        {
            StartCoroutine(WaitAfterTransfer());

        }
    }


    private IEnumerator WaitAfterTransfer()
    {
        yield return new WaitForSeconds(0.1f);
        _moveAfterTransfer = true;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "KeyYellow" )
        {
            _withKey = true;
        }

        if (other.gameObject.name == "KeyBlack")
        {
            _withKeyBlack = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_moveAfterTransfer)
        {
            bool flag = false;
            GameObject otherGameObject = other.gameObject;
            var otherPosition = otherGameObject.transform.position;
            var otherPositionY = otherPosition.y;
            var otherPositionX = otherPosition.x;

            var myPosition = transform.position;
            var myPositionY = myPosition.y;
            var myPositionX = myPosition.x;

            int camNum = 0;

            if (otherGameObject.name == "triggerYellowCasttleRoom")
            {
                flag = true;
                camNum = 1;
                _color = 0;
                TransferPlayer(0, false, false);
                other.enabled = true;
            }
            
            if (otherGameObject.name == "triggerBlackCasttleRoom")
            {
                flag = true;
                camNum = 12;
                _color = 4;
                TransferPlayer(17, false, false);
                other.enabled = true;
            }
            
            
            
            if (otherGameObject.name == "triggerSwordRoom")
            {
                flag = true;
                camNum = 0;
                _color = 0;
                TransferPlayer(16, false, false);
            }
            
            if (otherGameObject.name == "triggerTrophyRoom")
            {
                flag = true;
                camNum = 11;
                _color = 4;
                TransferPlayer(18, false, false);
            }

            if (otherGameObject.name == "triggerYellowGreen")
            {
                flag = true;
                if (myPositionY <= otherPositionY)
                {
                    camNum = 0;
                    _color = 0;
                }

                else
                {
                    camNum = 2;
                    _color = 1;
                }
            }

            if (otherGameObject.name == "triggerGreenGreen1 ")
            {
                flag = true;
                if (myPositionX <= otherPositionX)
                {
                    camNum = 3;
                }

                else
                {
                    camNum = 2;
                }

                _color = 1;
            }


            if (otherGameObject.name == "triggerGreenRed")
            {
                flag = true;
                if (myPositionY <= otherPositionY)
                {
                    camNum = 3;
                    _color = 1;
                }
                else
                {
                    camNum = 4;
                    _color = 2;
                }
            }

            if (otherGameObject.name == "triggerGreenGreen2")
            {
                flag = true;
                if (myPositionX >= otherPositionX)
                {
                    camNum = 5;
                }

                else
                {
                    camNum = 2;
                }
            }

            if (otherGameObject.name == "triggerGreenBlue")
            {
                flag = true;
                TransferPlayer(3, false, false);
                camNum = 6;
                _color = 3;
            }

            if (otherGameObject.name == "triggerBlueGreen")
            {
                flag = true;
                TransferPlayer(2, false, false);
                camNum = 5;
                _color = 1;
            }

            if (otherGameObject.name == "triggerBlue1")
            {
                flag = true;
                TransferPlayer(5, false, true);
                camNum = 7;
            }

            if (otherGameObject.name == "triggerBlue2")
            {
                flag = true;
                TransferPlayer(4, false, true);
                camNum = 6;
            }

            if (otherGameObject.name == "triggerBlue3")
            {
                flag = true;
                if (myPositionX >= otherPositionX)
                {
                    camNum = 6;
                }

                else
                {
                    camNum = 7;
                }
            }

            if (otherGameObject.name == "triggerBlue4")
            {
                flag = true;
                camNum = 10;
                TransferPlayer(7, true, false);

            }
            
            
            if (otherGameObject.name == "triggerBlue5")
            {
                flag = true;
                camNum = 6;
                TransferPlayer(6, true, false);

            }
            
            if (otherGameObject.name == "triggerBlue6")
            {
                flag = true;
                if (myPositionX >= otherPositionX)
                {
                    camNum = 10;
                }

                else
                {
                    camNum = 9;
                }
            }
            
            
            
            if (otherGameObject.name == "triggerBlue7")
            {
                flag = true;
                if (myPositionY >= otherPositionY)
                {
                    camNum = 9;
                }

                else
                {
                    camNum = 7;
                }
            }
            
            if (otherGameObject.name == "triggerBlue8")
            {
                flag = true;
                if (myPositionY >= otherPositionY)
                {
                    camNum = 7;
                }

                else
                {
                    camNum = 8;
                }
            }
            
            if (otherGameObject.name == "triggerBlue9")
            {
                flag = true;
                TransferPlayer(9, false, false);
                camNum = 8;
            }
            
            if (otherGameObject.name == "triggerBlue10")
            {
                flag = true;
                TransferPlayer(8, false, false);
                camNum = 9;
            }
            
            
            
            if (otherGameObject.name == "triggerBlue11")
            {
                flag = true;
                TransferPlayer(11, false, false);
                camNum = 8;
            }
            
            
            if (otherGameObject.name == "triggerBlue12")
            {
                flag = true;
                TransferPlayer(10, false, false);
                camNum = 9;
            }
            
            
                  
            if (otherGameObject.name == "triggerBlue13")
            {
                flag = true;
                TransferPlayer(13, false, false);
                camNum = 8;
            }
            
            if (otherGameObject.name == "triggerBlue14")
            {
                flag = true;
                TransferPlayer(12, false, false);
                camNum = 10;
            }
            
                          
            if (otherGameObject.name == "triggerBlue15")
            {
                flag = true;
                TransferPlayer(14, false, false);
                camNum = 8;
            }
            
            if (otherGameObject.name == "triggerBlue16")
            {
                flag = true;
                TransferPlayer(15, false, false);
                camNum = 10;
            }
            
            
            if (otherGameObject.name == "triggerBlue17")
            {
                flag = true;
                if (myPositionY >= otherPositionY)
                {
                    camNum = 8;
                    _color = 3; 
                }

                else
                {
                    camNum = 11;
                    _color = 4;
                }
            }
            

            

            if (flag)
            {
                _moveAfterTransfer = false;
                GameManager.SwitchCamara(camNum);
                SpriteRenderer.sprite = sprites[_color];
            }
        }
    }


    /**
     * Transferring the player to the sword room and back.
     * using unity event after collision with the gate, if the gate is open.
     */
    public void TransferPlayer(int num, bool stayX, bool stayY)
    {
        var newPosition = mazeTriggers[num].transform.position;

        var position = transform.position;
        float oldX = position.x;
        float oldY = position.y;


        float newY = newPosition.y;
        float newX = newPosition.x;
        float newKy = newY;
        float newKx = newX;
        // int numCamara = 0;


        if (num == 0 & !_inSwordRoom)
        {
           
            newX = oldX;
            newY += 0.5f;
            _inSwordRoom = true;
        }

        if (num == 1)
        {
            newX = oldX;
            if (_inSwordRoom)
            {
                newX -= 0.2f; 
              _inSwordRoom = false;  
            }
            
            // numCamara = 0;
        }

        if (num == 3)
        {
            newY += 0.3f;
        }

        if (num == 2)
        {
            newY -= 0.5f;
        }

        


        if (stayX)
        {
            newX = oldX;
        }

        if (stayY)
        {
            newY = oldY;
        }

        transform.position = new Vector2(newX, newY);
        // GameManager.SwitchCamara(numCamara);

        if (_withKey)
        {
            changeItemPosition(key, oldX, oldY, newX, newY);
            // key.transform.position = new Vector2(newX, newKy);
        }
        
        if (_withKeyBlack)
        {
            changeItemPosition(keyBlack, oldX, oldY, newX, newY);
            // key.transform.position = new Vector2(newX, newKy);
        }
        
    }


    private void changeItemPosition(GameObject other, float x, float y, float newX, float newY)
    {
        var position = other.transform.position;
        float distanceX = x - position.x;
        float distanceY = y - position.y;

        position = new Vector2(newX - distanceX, newY - distanceY);
        other.transform.position = position;
    }
    
    
}