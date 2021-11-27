using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;
    public HingeJoint2D hingeJoint2D;
    public Rigidbody2D rigidbody2D;


    private bool _joint = false;

    private bool collisionHappen = false;

    private float _spaceToPlayerAfterCollision = 0.095f;

    // Start is called before the first frame update
    void Start()
    {
        // Set the joint between the key and the player to false
        hingeJoint2D.enabled = false;
    }


    private void Update()
    {
        if (!_joint & collisionHappen)
        {
            StartCoroutine(PlayerCollisionHelper());
            collisionHappen = false;
        }

        if (Input.GetKey(KeyCode.Space) & _joint == true)
        {
            hingeJoint2D.enabled = false;
            boxCollider2D.enabled = true; //that the key will not crash with the walls.  
            _joint = false;
            rigidbody2D.velocity = Vector2.zero;
            
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        collisionHappen = true;

        // Set the joint between the key and the player to true.
        //And disable the key box collider
    }


    private IEnumerator PlayerCollisionHelper()
    {
        //Wait for a while until setting the joint between the player to the key.
        //Letting the key to move from the player - to have a distance between them when Space is press.
        yield return new WaitForSeconds(_spaceToPlayerAfterCollision);

        hingeJoint2D.enabled = true;
        boxCollider2D.enabled = false; //that the key will not crash with the walls.  
        _joint = true;
    }


    // Update is called once per frame
}