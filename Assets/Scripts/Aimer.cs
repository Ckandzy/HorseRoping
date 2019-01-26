﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    public GameObject playerObj;
    private Player player;
    public GameObject arrowL;
    public GameObject arrowR;

    public float R;
    private readonly float Rmin = 0.3f;
    private readonly float Rmax = 1.1f;
    private readonly float spd = 5;
    private readonly float step = 0.8f; // curSpeed of charging


    // Start is called before the first frame update
    void Start()
    {
        player = playerObj.GetComponent<Player>();
        arrowR.transform.localScale = new Vector2(-1, 1);
        ResetAimer();
    }

    // Update is called once per frame
    void Update()
    {
        float LR, UD;
        if((int)player.playerStatus <= 1) // not roping
        {
            // PC test
            /***********************************************
            float LR = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
            float UD = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
            ***********************************************/
            // move aimer
            if (player.playerID == 1)
            {
                LR = -Input.GetAxis("Horizontal_P1R");
                UD = Input.GetAxis("Vertical_P1R");
            }
            else // player.playerID == 2
            {
                LR = -Input.GetAxis("Horizontal_P2R");
                UD = Input.GetAxis("Vertical_P2R");
            }
            transform.position += new Vector3(LR * spd * Time.deltaTime, UD * spd * Time.deltaTime, 0);
            
            // limit aimer distance
            Vector3 rope = transform.position - player.transform.position;
            float mag = rope.magnitude;
            if(mag > player.lenRope)
            {
                rope = rope * (player.lenRope / rope.magnitude);
                transform.position = player.transform.position + rope;
            }
            arrowL.transform.position = transform.position - new Vector3(R, 0, 0);
            arrowR.transform.position = transform.position + new Vector3(R, 0, 0);
        }
    }

    public void ResetAimer()
    {
        Debug.Log("Aimer reset");
        R = Rmin;
        Vector3 temp = transform.position - new Vector3(R, 0, 0);
        arrowL.transform.position = transform.position - new Vector3(R, 0, 0);
        arrowR.transform.position = transform.position + new Vector3(R, 0, 0);
        GetComponent<SpriteRenderer>().enabled = true;
        arrowL.GetComponent<SpriteRenderer>().enabled = true;
        arrowR.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void HideAimer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        arrowL.GetComponent<SpriteRenderer>().enabled = false;
        arrowR.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void AddR()
    {
        R += step*Time.deltaTime;
        if (R > Rmax)
            R = Rmax;
    }

    public float CalDelay()
    {
        // cal rope flying time by R
        return 0.8f - 0.2f * (R - Rmin) / step;
    }
}
