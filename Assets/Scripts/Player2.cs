using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public Vector3 BarPosition;
    public GameObject Player;
    public GameObject GameCenter;
    
    void Start()
    {
        Player = GameObject.Find("Player2Target"); 
        GameCenter = GameObject.Find("GameCenter");
    }

    // Update is called once per frame
    void Update()
    {

        float x = Player.transform.position.x;
        if (x < 1f && x > -1f)
        {
            BarPosition = new Vector3(x, 0, 2.3f);
        }
        else
        {
            if (x > 1f)
            {
                BarPosition = new Vector3(1, 0, 2.3f);
            }
            else
            {
                BarPosition = new Vector3(-1, 0, 2.3f); 
            }
        }
        transform.position = BarPosition;
    }
}
