
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Basketball : MonoBehaviour
{
    public float Speed;
    AudioSource audioData, audioData2;
    Rigidbody rb;
    // limite: 2.1 , 1.2
    float WIDTH = 1.36f, LENGTH = 2.06f, Angle;
    int Direction = 1, LeftRight = 1, x, z;
    bool Moving = false, GameHasStarted = false, Reset = false, EndGame = true;
    GameObject Count, Player1, Player2, ScoreR, ScoreB;
    float BallOnBar;
    // Start is called before the first frame update

    void Start()
    {
        Speed = 1F;
        EndGame = true;
        rb = GetComponent<Rigidbody>();
        Count = GameObject.Find("CountDown");
        Player1 = GameObject.Find("BarPlayer1");
        Player2 = GameObject.Find("BarPlayer2");
        audioData = Count.GetComponent<AudioSource>();
        audioData2 = Player1.GetComponent<AudioSource>();
        ScoreB = GameObject.Find("ScoreBlue");
        ScoreR = GameObject.Find("ScoreRed");
        rb.angularVelocity = new Vector3(5, 3, 1);
        //ii da viteza mingii
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Speed > 4)
            Speed = 4;
        
        if (GameHasStarted)
        {
            // initial startGame
            Speed = Mathf.Min(Speed, 6);
            if (!Moving)
            {
                Angle = 0.2f;
                Moving = true;
                rb.velocity = new Vector3(LeftRight* Speed * Angle, 0f, Direction * (1 - Mathf.Abs(Angle)) * Speed);
            }


            //daca ajunge pe margini, isi schimba directia
            if (transform.position.x > WIDTH || transform.position.x < -WIDTH)
            {
                Speed += 0.2f;
                if (transform.position.x > WIDTH && LeftRight * Angle > 0)
                    LeftRight *= (-1);
                if (transform.position.x < WIDTH && LeftRight * Angle < 0)
                    LeftRight *= (-1);
                rb.velocity = new Vector3(LeftRight * Speed * Angle, 0f, Direction * (1 - Mathf.Abs(Angle)) * Speed);
            }

            
            //a ajuns la capat player 1
            if (transform.position.z < -LENGTH && Direction == -1)
            {// player 1
                BallOnBar = Player1.transform.position.x - transform.position.x;
                BallOnBar *= -LeftRight;
                if (BallOnBar< 0.5 && BallOnBar > -0.5)
                {//e bun si ricoseaza
                    Speed+= 0.2f;
                    Angle = BallOnBar * 7 / 5;
                    Direction = Direction * (-1);
                    if (BallOnBar < 0.1 && BallOnBar > -0.1)
                    {
                        audioData.Play(0);
                        Speed *= 2f;
                        Angle = Random.Range(-0.2f, 0.2f);
                    }
                    rb.velocity = new Vector3(LeftRight * Speed * Angle, 0f, Direction * (1 - Mathf.Abs(Angle)) * Speed);
                    if (BallOnBar < 0.1 && BallOnBar > -0.1)
                        Speed /= 2f;
                }
                else
                {
                    if(transform.position.z < -LENGTH - 0.2 && Direction == -1)
                        EndGame = true;
                }
            }

            //a ajuns la un capat player 2
            if (transform.position.z > LENGTH && Direction == 1)
            {
                BallOnBar = Player2.transform.position.x - transform.position.x;
                BallOnBar *= -LeftRight;
                if (BallOnBar < 0.5 && BallOnBar > -0.5)
                {//e bun si ricoseaza
                    Speed += 0.2f;
                    Angle = BallOnBar * 7 / 5;
                    Direction = Direction * (-1);
                    if (BallOnBar < 0.1 && BallOnBar > -0.1)
                    {
                        audioData.Play(0);
                        Speed *= 2f;
                        Angle = Random.Range(-0.2f, 0.2f);
                    }
                    rb.velocity = new Vector3(LeftRight * Speed * Angle, 0f, Direction * (1 - Mathf.Abs(Angle)) * Speed);
                    if (BallOnBar < 0.1 && BallOnBar > -0.1)
                        Speed /= 2f;
                }
                else
                {
                    if (transform.position.z > LENGTH + 0.2 && Direction == 1)
                        EndGame = true;
                }
            }
        }


        if (EndGame)
        {
            if(Direction == -1)
            {//a castigat blue
                ScoreB.transform.position = new Vector3(1.6f, 0f, ScoreB.transform.position.z + 0.1f);
                ScoreB.transform.localScale = new Vector3(0.2f, ScoreB.transform.localScale.y + 0.1f, 0.2f);
            }
            else
            {
                ScoreR.transform.position = new Vector3(1.6f, 0f, ScoreR.transform.position.z - 0.1f);
                ScoreR.transform.localScale = new Vector3(0.2f, ScoreR.transform.localScale.y + 0.1f, 0.2f);
            }
            audioData2.Play(1);
            Speed = 1.5f;
            GameHasStarted = false;
            EndGame = false;
            StartCoroutine(StartNewGame());
        }
    }

    IEnumerator StartNewGame()
    {
        transform.position = new Vector3(0, 0, 0);
        rb.velocity = new Vector3(0, 0, 0);
        Moving = false;
        for (int i = 5; i>=0; i--)
        { 
            Count.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);
        GameHasStarted = true; // de sters; SAU...
    }
}
