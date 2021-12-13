using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class Dragon : MonoBehaviour
{
    public Transform[] patrolPoints;
    private Transform _currentPatrolPoint;
    private int _currentPatrolIndex;
    
    public Rigidbody2D rigidB;
    public GameObject player;
    public float toDragonMouthX;
    public float toDragonMouthY;
    public bool _deadDragon;
    private bool _deadPlayer = false;
    public bool _move = false;
    private bool _returnMovement = false;
    public int curCamara;


    private Animator _animator;
    public float rotateFrom;

    public Sprite[] sprites;


    public PolygonCollider2D[] polygonCollider2Ds;
    public GameObject mouthCollider2D;

    private int _currentStage;
    private int _nextStage;
    private bool _mouthTriggerMode = false;
    private bool _changeState = false;
    private bool _openMouth = false;
    public float timeToWait;

    public GameObject spriteGameObjectDragon;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _movement;

    public bool startWithPatrol;
    public bool metPlayer = false;
    public float moveSpeed;


    private void Awake()
    {
        if (startWithPatrol)
        {
            _currentPatrolIndex = 0;
            _currentPatrolPoint = patrolPoints[_currentPatrolIndex];
        }
     
        _spriteRenderer = spriteGameObjectDragon.GetComponent<SpriteRenderer>();
        _currentStage = 0;
        foreach (var t in polygonCollider2Ds)
        {
            t.enabled = false;
        }

        MouthTriggerChangeMode(_mouthTriggerMode);
        polygonCollider2Ds[0].enabled = true;
        _spriteRenderer.sprite = sprites[0];
        _animator = spriteGameObjectDragon.GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        
        if (_deadDragon) return;
        Rotate();
        if (startWithPatrol & !metPlayer)
        { 
            Movement(_movement);
            rigidB.velocity = Vector2.zero;
            Vector2 direction = patrolPoints[_currentPatrolIndex].position - transform.position;
            direction.Normalize();
            _movement = direction;

            if (Vector2.Distance(transform.position, _currentPatrolPoint.position) < 1.0f)
            {
                _currentPatrolIndex = ((_currentPatrolIndex + 1) % patrolPoints.Length);
                _currentPatrolPoint = patrolPoints[_currentPatrolIndex];

            }
        }
        
        
        if (_move)
        {
            Movement(_movement);
            rigidB.velocity = Vector2.zero;
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            _movement = direction;
        }

        if (_changeState)
            StartCoroutine(WaitToStateChange(timeToWait));
        
    }



    private void Rotate()
    {
        var dicY = Math.Abs(_movement.y);
        if (_move & (dicY <= rotateFrom) & (dicY >= -rotateFrom))
        {
            _animator.SetBool("rotate", true);
        }

        else
        {
            _animator.SetBool("rotate", false);
        }
    }


    private void Movement(Vector2 direction)
    {
        rigidB.MovePosition((Vector2) gameObject.transform.position + (direction * moveSpeed * Time.deltaTime));
        Vector2 newP = rigidB.position;
        
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        var nameO = other.gameObject.name;
        print(nameO);
        CollisionAndTrigger(nameO);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger")) return;
        var nameO = other.gameObject.name;
        CollisionAndTrigger(nameO);
    }


    private void CollisionAndTrigger(String nameO)
    {
        if (_currentStage == 0 & !_deadPlayer)
        {
            _move = false;
            _changeState = true;
            switch (nameO)
            {
                case "Player":
                    _nextStage = 1;
                    _mouthTriggerMode = true;
                    _openMouth = true;
                    break;
                case "KillDragon":
                    _nextStage = 2;
                    break;
            }
        }
    }


    private IEnumerator WaitToStateChange(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        ChangeDragonState();
    }


    public void EatPlayer()
    {
        print("eatplayer");
        _currentStage = 1;
        _nextStage = 0;
        _mouthTriggerMode = false;
        _changeState = true;
        _deadPlayer = true;
        _move = false;
    }


    public void ReturnNormal()
    {
        print("returnnormal");
        _currentStage = 1;
        _nextStage = 0;
        _mouthTriggerMode = false;
        _changeState = true;
        _returnMovement = true;
    }


    private void ChangeDragonState()
    {
        _changeState = false;
        MouthTriggerChangeMode(_mouthTriggerMode);

        var toRemove = polygonCollider2Ds[_currentStage];
        toRemove.enabled = false;
        _currentStage = _nextStage;
        var changeTo = polygonCollider2Ds[_currentStage];
        changeTo.enabled = true;
        _spriteRenderer.sprite = sprites[_currentStage];


        if (_returnMovement)
        {
            _move = true;
            _returnMovement = false;
        }


        var newPosition = player.GetComponent<Rigidbody2D>().position;
        if (_openMouth)
        {
            newPosition.x += toDragonMouthX;
            newPosition.y -= toDragonMouthY;
            rigidB.position = newPosition;
            _openMouth = false;
        }

        if (_currentStage == 2)
            _deadDragon = true;
    }


    private void MouthTriggerChangeMode(bool mode)
    {
        var mouthCollider = mouthCollider2D.GetComponent<PolygonCollider2D>();
        mouthCollider.enabled = mode;
    }
}