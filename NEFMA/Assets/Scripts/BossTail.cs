﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTail : MonoBehaviour {

    private Rigidbody2D myBody;
    private BossController myController;
    public float maxX = 45;
    public float minX = -45;
    public float maxY = 56;
    public float minY = 56;
    private float startTime = 0;
    public float direction = 1;
    private float oldval = 0;
    public float needle = 0;
    public float numberOfNeedles = 0;
    public GameObject needePrefab;
    public float needleSpeed = 0;
    public float needleDelayTime = 0;
    private float internalNeedleDelayTime = 0;
    public float needleAngleSpread = 0;
    private Color debugColor = Color.red;
    public bool movingY = false;
    public bool yDown = true;
    private float cycler = 0;
    private bool shooting = false;
    public GameObject tailArt;
    [HideInInspector] public bool attacking = false;
    [HideInInspector] public bool ready = false;
    public Animator tailAnimation;
    //private int tempCount = 0;

    // Use this for initialization
    void Start () {
        myBody = this.GetComponent<Rigidbody2D>();
        //myBody.MovePosition(new Vector2(minX, maxY));
        myController = GameObject.FindWithTag("Boss Controller").GetComponent<BossController>();
        myController.registerTail(gameObject);
        startTime = Time.time;
        cycler = 0;
        internalNeedleDelayTime = needleDelayTime;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ((cycler - Time.time <= 3.14f) && (attacking || ready) && !shooting)
        {
            tailArt.GetComponent<SpriteRenderer>().color = new Color(Mathf.Sin(2 * Mathf.PI * 5 * Mathf.Abs(cycler - Time.time) + 0), Mathf.Sin(2 * Mathf.PI * 5 * Mathf.Abs(cycler - Time.time) + 0), Mathf.Sin(2 * Mathf.PI * 5 * Mathf.Abs(cycler - Time.time) + 0), 1f);
        }
        if (Time.time >= cycler && !shooting && ready)
        {
            movingY = true;
        }
        if (attacking && !ready)
        {
            if ((myBody.position.x >= maxX) || (myBody.position.x <= minX))
            {
                ready = true;
            }
        }
    }

    void FixedUpdate()
    {
        //tempCount++;
        //Debug.Log("--------------------------------------------------");
        //Debug.Log("Frame: " + tempCount);
        if (!movingY)
        {
            sway();
        }
        if (movingY && ready)
        {
            if (yDown)
            {
                moveDown();
            }
            else
            {
                moveUp();
            }
        }
        if (tailArt != null)
        {
            tailArt.transform.position = transform.position;
        }
    }

    public void moveUp()
    {
        //Debug.Log("Moving Up");
        myBody.MovePosition(myBody.position + (Vector2.up * 10 * Time.fixedDeltaTime));
        if (myBody.position.y >= maxY)
        {
            if (myBody.position.x > 0)
            {
                tailAnimation.CrossFadeInFixedTime("TailAnimation", 0.25f, -1, 3.14f);
                myBody.MovePosition(new Vector2(maxX, maxY));
            }
            else
            {
                tailAnimation.CrossFadeInFixedTime("TailAnimation", 0.25f, -1, 0f);
                myBody.MovePosition(new Vector2(minX, maxY));
            }
            movingY = false;
            yDown = true;
            startTime = Time.time;
            cycler = Time.time + (2 * 3.14f);
            tailArt.GetComponent<SpriteRenderer>().color = Color.white;
            shooting = false;
            // headed left
            if (myBody.position.x > 0)
            {
                direction = -1;
            }
            // headed right
            else
            {
                direction = 1;
            }
        }
    }

    public void moveDown()
    {
        //Debug.Log("Moving Down");
        myBody.MovePosition(myBody.position - (Vector2.up * 10 * Time.fixedDeltaTime));
        if (myBody.position.y <= minY)
        {
            if (myBody.position.x > 0)
            {
                tailAnimation.CrossFadeInFixedTime("TailAnimation", 0.25f, -1, 3.14f);
                myBody.MovePosition(new Vector2(maxX, minY));
            }
            else
            {
                tailAnimation.CrossFadeInFixedTime("TailAnimation", 0.25f, -1, 0f);
                myBody.MovePosition(new Vector2(minX, minY));
            }
            movingY = false;
            yDown = false;
            startTime = Time.time;
            shooting = true;
            tailArt.GetComponent<SpriteRenderer>().color = Color.white;
            spawnNeedles();
            // headed left
            if (myBody.position.x > 0)
            {
                direction = -1;
            }
            // headed right
            else
            {
                direction = 1;
            }
        }
    }

    // maybe change this to just use increasing and decreasing velocities like BossHand?
    public void sway()
    {
        if (startTime > Time.time)
        {
            return;
        }
        float val = Mathf.Sin(Time.time - startTime) * (Mathf.Abs(maxX - minX) / 100f);
        if ((val > 0 && oldval < 0) || (val < 0 && oldval > 0))
        {
            //Debug.Log("val: " + val + " | oldval: " + oldval + " | Highest X: " + myBody.position.x);
            // at right side
            if (direction >= 0)
            {
                myBody.MovePosition(new Vector2(maxX, myBody.position.y));
            }
            // at left side
            else
            {
                myBody.MovePosition(new Vector2(minX, myBody.position.y));
            }
            startTime = Time.time;
            direction = -direction;
            oldval = 0;
            return;
        }
        // headed right
        if (direction >= 0)
        {
            myBody.MovePosition(myBody.position + (Vector2.right * val));
        }
        // headed left
        else
        {
            myBody.MovePosition(myBody.position - (Vector2.right * val));
        }
        oldval = val;
    }

    public void spawnNeedles()
    {
        if (needle < numberOfNeedles)
        {
            if (needle == 0 || needle == 3 || needle == 6) 
            {
                debugColor = Color.green;
                internalNeedleDelayTime = needleDelayTime;
            }
            else if (needle == 4 || needle == 5)
            {
                debugColor = Color.blue;
            }
            else
            {
                debugColor = Color.red;
            }
            needle++;
            StartCoroutine(attack());
        }
        if (needle == numberOfNeedles)
        {
            needle++;
            movingY = true;
            return;
        }
        if (needle > numberOfNeedles)
        {
            needle = 0;
        }
    }

    IEnumerator attack()
    {
        float y = 3f;
        float x = 1f;
        GameObject newNeedle1 = Instantiate(needePrefab, (transform.position - (transform.right * x) - (transform.up * y)), Quaternion.Euler(0, 0, -needleAngleSpread));
        GameObject newNeedle2 = Instantiate(needePrefab, (transform.position + (transform.right * 0) - (transform.up * y)), Quaternion.identity);
        GameObject newNeedle3 = Instantiate(needePrefab, (transform.position + (transform.right * x) - (transform.up * y)), Quaternion.Euler(0, 0, needleAngleSpread));

        ///*
        Debug.DrawLine((transform.position - (transform.right * x) - (transform.up * y)), (transform.position - ((transform.right * x) * 40.25454f) - ((transform.up * y) * 50)), debugColor, 300);
        Debug.DrawLine((transform.position + (transform.right * 0) - (transform.up * y)), (transform.position + ((transform.right * 0) * 50) - ((transform.up * y) * 50)), debugColor, 300);
        Debug.DrawLine((transform.position + (transform.right * x) - (transform.up * y)), (transform.position + ((transform.right * x) * 40.25454f) - ((transform.up * y) * 50)), debugColor, 300);
        //*/

        float xcomp, ycomp;
        //float angle = Mathf.Atan(45f * Mathf.Deg2Rad);
        xcomp = Mathf.Cos((90 - needleAngleSpread) * Mathf.Deg2Rad) * -needleSpeed;
        ycomp = Mathf.Sin((90 - needleAngleSpread) * Mathf.Deg2Rad) * -needleSpeed;
        newNeedle1.GetComponent<Rigidbody2D>().velocity = new Vector2(xcomp, ycomp);

        newNeedle2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -needleSpeed);

        xcomp = Mathf.Cos((90 - needleAngleSpread) * Mathf.Deg2Rad) * needleSpeed;
        ycomp = Mathf.Sin((90 - needleAngleSpread) * Mathf.Deg2Rad) * -needleSpeed;
        newNeedle3.GetComponent<Rigidbody2D>().velocity = new Vector2(xcomp, ycomp);

        //Debug.Log(newNeedle1 + " | position: " + newNeedle1.transform.position + " | Velocity: " + newNeedle1.GetComponent<Rigidbody2D>().velocity);
        //Debug.Log(newNeedle2 + " | position: " + newNeedle2.transform.position + " | Velocity: " + newNeedle2.GetComponent<Rigidbody2D>().velocity);
        //Debug.Log(newNeedle3 + " | position: " + newNeedle3.transform.position + " | Velocity: " + newNeedle3.GetComponent<Rigidbody2D>().velocity);
        //Debug.Log("Waiting");
        float waitTime;
        float rand = Random.value;
        float value = (Random.value / 8);
        if (rand > 0.5f)
        {
            waitTime = internalNeedleDelayTime + value;
            internalNeedleDelayTime = internalNeedleDelayTime - value;
        }
        else
        {
            waitTime = internalNeedleDelayTime - value;
            internalNeedleDelayTime = internalNeedleDelayTime + value;
        }

        yield return new WaitForSeconds(waitTime);
        spawnNeedles();
    }
}
