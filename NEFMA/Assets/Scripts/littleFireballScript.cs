﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script so far just makes the fireballs disappear when they go off screen, more will probably be added later
public class littleFireballScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
 
    }

    private void Update()
    {
        if (!gameObject.GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.GetComponent<AttributeController>())
        {
            if (collision.gameObject.GetComponent<AttributeController>().myTag == "Enemy")
            {
                if (gameObject.tag == "LittleAttack")
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
