﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : MonoBehaviour {

    //private Rigidbody2D myBody;
    private BossController myController;
    public GameObject fireballPrefab;

    // Use this for initialization
    void Start () {
        //myBody = this.GetComponent<Rigidbody2D>();
        myController = GameObject.FindWithTag("Boss Controller").GetComponent<BossController>();
        myController.registerBodyPart(gameObject, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void fireballs()
    {
        GameObject newBullet = Instantiate(fireballPrefab, (transform.position + (transform.up / 20)), Quaternion.identity) as GameObject;
        newBullet.tag = "EnemyAttack";
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -30);
    }
}
