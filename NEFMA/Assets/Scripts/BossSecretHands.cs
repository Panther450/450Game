﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSecretHands : MonoBehaviour {

    //private Rigidbody2D myBody;
    private BossController myController;
    [HideInInspector] public GameObject myHand = null;
    public bool leftHand = true;

    // Use this for initialization
    void Start () {
        //myBody = this.GetComponent<Rigidbody2D>();
        myController = GameObject.FindWithTag("Boss Controller").GetComponent<BossController>();
        if (leftHand)
        {
            myController.registerSecretHand(gameObject, -1);
        }
        else
        {
            myController.registerSecretHand(gameObject, 1);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void linkHand(GameObject hand)
    {
        myHand = hand;
    }
}
