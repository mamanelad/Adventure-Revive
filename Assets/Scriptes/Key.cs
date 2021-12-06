using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;
    public HingeJoint2D hingeJoint2D;
    public Rigidbody2D rigidbody2D;


    private bool _jointPlayer = false;
    private bool collisionHappenPlayer = false;

    private float _spaceToPlayerAfterCollision = 0.05f;


    // Start is called before the first frame update


    void Start()
    {
        // Set the joint between the key and the player to false
        hingeJoint2D.enabled = false;
    }


    private void Update()
    {
        if (!_jointPlayer & collisionHappenPlayer)
        {
            StartCoroutine(PlayerCollisionHelper());
            collisionHappenPlayer = false;
        }


        //check if the player want to rellese the key 
        if (Input.GetKey(KeyCode.Space) & _jointPlayer == true)
        {
            hingeJoint2D.enabled = false;
            boxCollider2D.enabled = true; //that the key will not crash with the walls.  
            _jointPlayer = false;
            rigidbody2D.velocity = Vector2.zero;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            collisionHappenPlayer = true;
        }
    }


    private IEnumerator PlayerCollisionHelper()
    {
        //Wait for a while until setting the joint between the player to the key.
        //Letting the key to move from the player - to have a distance between them when Space is press.
        yield return new WaitForSeconds(_spaceToPlayerAfterCollision);

        hingeJoint2D.enabled = true;
        boxCollider2D.enabled = false; //that the key will not crash with the walls.  
        _jointPlayer = true;
    }
}