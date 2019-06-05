using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RayAgent : Agent
{
    public GameObject ball;
    private Rigidbody rb;
    public GameObject target;
    public float movespeed = 10;
    private int curBallsTouch = 0;
    private RayAcademy academy;

    public override void InitializeAgent()
    {
        rb = ball.GetComponent<Rigidbody>();
        academy = FindObjectOfType<RayAcademy>();
    }

    public override void CollectObservations()
    {

        AddVectorObs(ball.transform.localPosition);
        AddVectorObs(rb.velocity);
        AddVectorObs(rb.angularVelocity);
        AddVectorObs(target.transform.localPosition);
        AddVectorObs(curBallsTouch);
        AddVectorObs(academy.balls);


    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {

        rb.AddTorque(new Vector3(vectorAction[1], 0, vectorAction[0])*movespeed, ForceMode.Acceleration);
        AddReward(-0.001f);
    }
    
    

    public override void AgentReset()
    {
        // ballsTouch = (int)academy.resetParameters["many_balls"];
        curBallsTouch = -1;
        moveBall();

        ball.transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    public void moveBall()
    {

        target.transform.localPosition = new Vector3(Random.Range(-1f, 1f), 0, (Random.Range(-1f, 1f))).normalized * Random.Range(1, 4);

        curBallsTouch++;
        AddReward(1f);
        if (curBallsTouch == academy.balls)
        {
            Done();
        }
    }


    private void Update()
    {
        //rb.AddTorque(new Vector3(1, 0, -1).normalized * movespeed, ForceMode.Acceleration);
    }
}
