using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int numKey;
 
    public HingeJoint2D hingeJoint2D;
    // Start is called before the first frame update
    
    void Start()
    {
        // Set the joint between the key and the player to false
        hingeJoint2D.enabled = false;
    }

    

}