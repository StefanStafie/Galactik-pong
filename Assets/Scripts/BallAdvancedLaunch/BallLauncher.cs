using UnityEngine;
using System.Collections;

public class BallLauncher : MonoBehaviour
{
    public GameObject ballParent;

    public Rigidbody ball;
    public Transform target;

    public float h = 0.25f;
    public float gravity = -1;

    public bool debugPath;

    private float errorFactor = 0.5f;
    private Vector3 error;
    private Vector3 userInput;

    float startTime;

    void Start()
    {
        ball.useGravity = false;
        error = new Vector3(0, errorFactor, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            ResetBallVelocity();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startTime = Time.time;
            Debug.Log("Space Pressed");
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Debug.Log(Time.time - startTime);
            float userDelay = (Time.time - startTime) / 2;
            Launch(userDelay);
        }



    }

    void Launch(float userDelay)
    {
        Vector3 corection = new Vector3(0, userDelay, 0);
        Physics.gravity = Vector3.up * gravity;
        ball.useGravity = true;
        ball.velocity = CalculateLaunchData().initialVelocity - error + corection;
    }

    public void ResetBallVelocity()
    {

        ball.transform.position = ballParent.transform.position;
        ball.transform.rotation = new Quaternion(0, 0, 0, 0);
        Rigidbody rb = ball.transform.GetComponent<Rigidbody>();
        rb.useGravity = false;
       
    }

    LaunchData CalculateLaunchData()
    {
        float displacementY = target.position.y - ball.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - ball.position.x, 0, target.position.z - ball.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = ball.position;

        int resolution = 30;
        for (int i = 1; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = ball.position + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;
        }
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }
}