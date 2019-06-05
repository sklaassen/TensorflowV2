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
    public GameObject[] Walls;

    public override void InitializeAgent()
    {
        rb = ball.GetComponent<Rigidbody>();
        academy = FindObjectOfType<RayAcademy>();
    }
    private Vector3[] directions = new Vector3[]{
        Vector3.forward
        ,Vector3.forward+ Vector3.left
        , Vector3.left
        ,Vector3.left+Vector3.back
        , Vector3.back
        ,Vector3.back+Vector3.right
        ,Vector3.right
    ,Vector3.right+Vector3.forward};
    public override void CollectObservations()
    {

        AddVectorObs(ball.transform.localPosition);
        AddVectorObs(rb.velocity);
        AddVectorObs(rb.angularVelocity);
        AddVectorObs(target.transform.localPosition);
        AddVectorObs(curBallsTouch);
        AddVectorObs(academy.balls);
        RaycastHit hit;
        for(int i=0; i < directions.Length; i++)
        {
            Ray ray = new Ray(ball.transform.position, directions[i]);
        
            if (Physics.Raycast(ray, out hit, 10f))
            {
                //Debug.DrawLine(ball.transform.position, hit.point, Color.yellow);
                AddVectorObs(Vector3.Distance(ball.transform.position,hit.point));
            }
            else
            {
                //Debug.DrawLine(ball.transform.position, ball.transform.position+directions[i]*10, Color.yellow);
                AddVectorObs(10);
            }

        }


    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {

        rb.AddTorque(new Vector3(vectorAction[1], 0, vectorAction[0])*movespeed, ForceMode.Acceleration);
        AddReward(-0.001f);
    }
    
    

    public override void AgentReset()
    {
        for (int i = 0; i < Walls.Length; i++) Walls[i].SetActive(false);
        Walls[Random.Range(0, Walls.Length)].SetActive(true);

        ball.transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;    
        
        curBallsTouch = -1;
        moveBall();


    }
    private float platformSize = 5f;
    public void moveBall()
    {
        Vector3 Location = Vector3.zero;
        RaycastHit hit;
        do
        {
            Location = new Vector3(Random.Range(-platformSize, platformSize), 10, (Random.Range(-platformSize, platformSize)));
            Location += this.transform.position;
            Ray ray = new Ray(Location, Vector3.down);
            
            if (Physics.Raycast(ray, out hit, 20f))
            {
                //Debug.DrawLine(Location, hit.point, Color.yellow,2f);
                Location = hit.point;
            }
        } while (Vector3.Distance(Location, ball.transform.position) < 1 || hit.collider.tag =="wall");
        Location = new Vector3(Location.x, Location.y + 0.5f, Location.z);
        target.transform.position = Location;
        if (curBallsTouch != 0) { AddReward(1f); }

        curBallsTouch++;
        
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
