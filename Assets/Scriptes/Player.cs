using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    private float _gotInSideMouthTimer = 0;
    private float _continuumTimeInMouthTimer;
    private bool _gotInSideMouth = false;
    private bool _inSideMouth = false;
    public float deadTime;
    public GameObject[] dragonBallys;

    public GameObject[] mazeTriggers;
    public Sprite[] sprites;
    public SpriteRenderer SpriteRenderer;
    private int _color = 0;

    //movement
    public float timeToWaitAfterDragonAttack;
    private bool _canMove = true;
    private Rigidbody2D _rigidbody2D;
    public float size;
    private bool _leftPressed;
    private bool _rightPressed;
    private bool _upPressed;
    private bool _downPressed;
    private bool _moveAfterTransfer = true;
    private bool _inSwordRoom = false;


    //item hendelling 
    public GameObject empty;
    public GameObject _currentItem;
    public bool _withItem = false;
    private bool collisionHappenPlayer = false;
    private float _waitTimeAfterCollision = 0.05f;


    //movethrow
    private bool _withMoveThrow = false;

    public int curCamara;


    private void Awake()
    {
        Application.targetFrameRate = 70;
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        size = _rigidbody2D.transform.localScale.x / 2;
        SpriteRenderer.sprite = sprites[_color];
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;
        var ultimate = Vector2.zero;

        if (_leftPressed)
            ultimate += Vector2.left;
        if (_rightPressed)
            ultimate += Vector2.right;
        if (_upPressed)
            ultimate += Vector2.up;
        if (_downPressed)
            ultimate += Vector2.down;

        _rigidbody2D.MovePosition(_rigidbody2D.position + size * ultimate);
    }

    private void Update()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _leftPressed = Input.GetKey(KeyCode.LeftArrow);
        _rightPressed = Input.GetKey(KeyCode.RightArrow);
        _upPressed = Input.GetKey(KeyCode.UpArrow);
        _downPressed = Input.GetKey(KeyCode.DownArrow);

        if (Input.GetKey(KeyCode.Space) & _withItem)
        {
            ReleaseItem();
            _currentItem = empty;
        }

        if (collisionHappenPlayer)
        {
            StartCoroutine(ItemCatching(_currentItem));
            collisionHappenPlayer = false;
        }

        if (!_moveAfterTransfer)
            StartCoroutine(WaitAfterTransfer());


        if (_gotInSideMouth)
        {
            _gotInSideMouthTimer += Time.deltaTime;
            _continuumTimeInMouthTimer += Time.deltaTime;
        }

        if (_gotInSideMouthTimer >= deadTime)
        {
            if (_inSideMouth & _continuumTimeInMouthTimer >= deadTime)
            {
                GameManager.EatPlayer();
                TransferToBally();
            }

            else
            {
                GameManager.DragonReturnNormal();
            }

            _gotInSideMouthTimer = 0;
            _gotInSideMouth = false;
        }

        if (!_canMove)
            StartCoroutine(Wait(timeToWaitAfterDragonAttack));
    }


    private void ReleaseItem()
    {
        _withMoveThrow = false;
        _currentItem.GetComponent<HingeJoint2D>().enabled = false;
        _currentItem.GetComponent<PolygonCollider2D>().enabled = true;
        _currentItem.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _withItem = false;
      
    }

    private IEnumerator ItemCatching(GameObject other)
    {
        other.GetComponent<HingeJoint2D>().enabled = true;
        other.GetComponent<PolygonCollider2D>().enabled = false;
        yield return new WaitForSeconds(_waitTimeAfterCollision);
    }


    private IEnumerator WaitAfterTransfer()
    {
        yield return new WaitForSeconds(0.1f);
        _moveAfterTransfer = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Dragon") & _canMove)
            _canMove = false;


        if (other.gameObject.CompareTag("Item") & !collisionHappenPlayer)
        {
            if (_withItem)
            {
                if (other.gameObject.name == _currentItem.name)
                {
                    return;
                }

                ReleaseItem();
                _currentItem = empty;
            }

            _currentItem = other.gameObject;
            collisionHappenPlayer = true;
            _withItem = true;
        }
    }

    private IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        if (!_canMove)
            _canMove = true;
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

            curCamara = 0;

            if (otherGameObject.name == "triggerYellowCasttleRoom")
            {
                flag = true;
                curCamara = 1;
                _color = 0;
                TransferPlayer(0, false, false);
                other.enabled = true;
            }

            if (otherGameObject.name == "triggerBlackCasttleRoom")
            {
                flag = true;
                curCamara = 12;
                _color = 2;
                TransferPlayer(17, false, false);
                other.enabled = true;
            }


            if (otherGameObject.name == "triggerSwordRoom")
            {
                flag = true;
                curCamara = 0;
                _color = 0;
                TransferPlayer(16, false, false);
            }

            if (otherGameObject.name == "triggerMagnetaRoom")
            {
                flag = true;
                curCamara = 11;
                _color = 4;
                TransferPlayer(18, false, false);
            }
            
            
            

            if (otherGameObject.name == "triggerYellowGreen")
            {
                flag = true;
                if (myPositionY <= otherPositionY)
                {
                    curCamara = 0;
                    _color = 0;
                }

                else
                {
                    curCamara = 2;
                    _color = 1;
                }
            }

            if (otherGameObject.name == "triggerGreenGreen1")
            {
                flag = true;
                if (myPositionX <= otherPositionX)
                {
                    curCamara = 3;
                }

                else
                {
                    curCamara = 2;
                }

                _color = 1;
            }


            if (otherGameObject.name == "triggerGreenRed")
            {
                flag = true;
                if (myPositionY <= otherPositionY)
                {
                    curCamara = 3;
                    _color = 1;
                }
                else
                {
                    curCamara = 4;
                    _color = 2;
                }
            }

            if (otherGameObject.name == "triggerGreenGreen2")
            {
                flag = true;
                if (myPositionX >= otherPositionX)
                {
                    curCamara = 5;
                }

                else
                {
                    curCamara = 2;
                }
            }

            if (otherGameObject.name == "triggerGreenBlue")
            {
                flag = true;
                TransferPlayer(3, false, false);
                curCamara = 6;
                _color = 3;
            }

            if (otherGameObject.name == "triggerBlueGreen")
            {
                flag = true;
                TransferPlayer(2, false, false);
                curCamara = 5;
                _color = 1;
            }

            if (otherGameObject.name == "triggerBlue1")
            {
                flag = true;
                TransferPlayer(5, false, true);
                curCamara = 7;
            }

            if (otherGameObject.name == "triggerBlue2")
            {
                flag = true;
                TransferPlayer(4, false, true);
                curCamara = 6;
            }

            if (otherGameObject.name == "triggerBlue3")
            {
                flag = true;
                if (myPositionX >= otherPositionX)
                {
                    curCamara = 6;
                }

                else
                {
                    curCamara = 7;
                }
            }

            if (otherGameObject.name == "triggerBlue4")
            {
                flag = true;
                curCamara = 10;
                TransferPlayer(7, true, false);
            }


            if (otherGameObject.name == "triggerBlue5")
            {
                flag = true;
                curCamara = 6;
                TransferPlayer(6, true, false);
            }

            if (otherGameObject.name == "triggerBlue6")
            {
                flag = true;
                if (myPositionX >= otherPositionX)
                {
                    curCamara = 10;
                }

                else
                {
                    curCamara = 9;
                }
            }


            if (otherGameObject.name == "triggerBlue7")
            {
                flag = true;
                if (myPositionY >= otherPositionY)
                {
                    curCamara = 9;
                }

                else
                {
                    curCamara = 7;
                }
            }

            if (otherGameObject.name == "triggerBlue8")
            {
                flag = true;
                if (myPositionY >= otherPositionY)
                {
                    curCamara = 7;
                }

                else
                {
                    curCamara = 8;
                }
            }

            if (otherGameObject.name == "triggerBlue9")
            {
                flag = true;
                TransferPlayer(9, false, false);
                curCamara = 8;
            }

            if (otherGameObject.name == "triggerBlue10")
            {
                flag = true;
                TransferPlayer(8, false, false);
                curCamara = 9;
            }


            if (otherGameObject.name == "triggerBlue11")
            {
                flag = true;
                TransferPlayer(11, false, false);
                curCamara = 8;
            }


            if (otherGameObject.name == "triggerBlue12")
            {
                flag = true;
                TransferPlayer(10, false, false);
                curCamara = 9;
            }


            if (otherGameObject.name == "triggerBlue13")
            {
                flag = true;
                TransferPlayer(13, false, false);
                curCamara = 8;
            }

            if (otherGameObject.name == "triggerBlue14")
            {
                flag = true;
                TransferPlayer(12, false, false);
                curCamara = 10;
            }


            if (otherGameObject.name == "triggerBlue15")
            {
                flag = true;
                TransferPlayer(14, false, false);
                curCamara = 8;
            }

            if (otherGameObject.name == "triggerBlue16")
            {
                flag = true;
                TransferPlayer(15, false, false);
                curCamara = 10;
            }


            if (otherGameObject.name == "triggerBlue17")
            {
                flag = true;
                if (myPositionY >= otherPositionY)
                {
                    curCamara = 8;
                    _color = 3;
                }

                else
                {
                    curCamara = 11;
                    _color = 4;
                }
            }
            
            
            
            
            if (otherGameObject.name == "triggerTrophyRoom")
            {
                flag = true;
                if (myPositionY >= otherPositionY)
                {
                    curCamara = 12;
                    _color = 2;
                }

                else
                {
                    curCamara = 13;
                    _color = 5;
                }
            }


            if (other.gameObject.tag == "Item" & !collisionHappenPlayer)
            {
                _withMoveThrow = true;
                if (_withItem)
                {
                    if (other.gameObject.name == _currentItem.name)
                    {
                        return;
                    }

                    ReleaseItem();
                    _currentItem = empty;
                }

                _currentItem = other.gameObject;
                collisionHappenPlayer = true;
                _withItem = true;
            }


            if (other.gameObject.CompareTag("moveThrow"))
            {
                if (!_withMoveThrow)
                {
                    _rigidbody2D.constraints |= RigidbodyConstraints2D.FreezePositionX;
                    _withMoveThrow = true;
                    _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                }
            }


            if (flag)
            {
                _moveAfterTransfer = false;
                GameManager.SwitchCamara(curCamara);
                SpriteRenderer.sprite = sprites[_color];
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        // print("got inside mouth ");
        if (other.gameObject.name == "mouthTrigger")
        {
            _inSideMouth = true;
            _gotInSideMouth = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "mouthTrigger")
        {
            _continuumTimeInMouthTimer = 0f;
            _inSideMouth = false;
        }


        if (other.gameObject.CompareTag("moveThrow"))
        {
            if (_withMoveThrow)
            {
                _rigidbody2D.constraints &= RigidbodyConstraints2D.FreezeRotation;
                _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                _withMoveThrow = false;
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


        switch (num == 0 & !_inSwordRoom)
        {
            case true:
                newX = oldX;
                newY += 0.5f;
                _inSwordRoom = true;
                break;
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

        _rigidbody2D.position = new Vector2(newX, newY);
        // transform.position = new Vector2(newX, newY);
        // GameManager.SwitchCamara(numCamara);

        if (_withItem)
        {
            changeItemPosition(_currentItem, oldX, oldY, newX, newY);
            // key.transform.position = new Vector2(newX, newKy);
        }
    }


    private void changeItemPosition(GameObject other, float x, float y, float newX, float newY)
    {
        var position = other.transform.position;
        float distanceX = x - position.x;
        float distanceY = y - position.y;
        position = new Vector2(newX - distanceX, newY - distanceY);
        // other.GetComponent<Rigidbody2D>().position = position;
        other.transform.position = position;
    }


    private void TransferToBally()
    {
        var dragonBallyPosition0 = dragonBallys[0].transform.position;
        var dragonBallyPosition1 = dragonBallys[1].transform.position;
        var playerNextPosition = dragonBallyPosition0;
        var d0 = Vector2.Distance(this.transform.position, dragonBallyPosition0);
        var d1 = Vector2.Distance(this.transform.position, dragonBallyPosition1);
        print(d0);
        print(d1);
        if (d1 < d0)
            playerNextPosition = dragonBallyPosition1;
        var oldPosition = _rigidbody2D.position;
        if (_withItem)
        {
            changeItemPosition(_currentItem, oldPosition.x, oldPosition.y, playerNextPosition.x,
                playerNextPosition.y);
            _currentItem.isStatic = true;
        }

        _rigidbody2D.position = playerNextPosition;
    }

  
}