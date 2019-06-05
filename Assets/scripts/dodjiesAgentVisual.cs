using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class dodjiesAgentVisual : Agent
{
    public int team;
    public int maxHealth = 1;
    public int health;
    public GameObject player;
    public GameObject spawnPoint;
    private Rigidbody rb;
    public GameObject barrel;
    public GameObject rotateSec;
    public float moveSpeed = 5;
    public float shotSpeed = 5;


    public GameObject enemy;
    private dojiesAcademy myAcademy;
    public dodjiesMap gridSystem;
    public GameObject goodSpawners;
    public GameObject badSpawners;

    public override void InitializeAgent()
    {
        rb = GetComponent<Rigidbody>();
        myAcademy = GameObject.Find("Academy").GetComponent<dojiesAcademy>();

        gridSystem.players[team] = this.gameObject;
    }

    public override void CollectObservations()
    {
        AddVectorObs(team);

    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        AddReward(0.01f);
        Debug.DrawRay(transform.position,new Vector3(vectorAction[1], 0, vectorAction[0]));
        rb.AddForce (new Vector3(vectorAction[1], 0, vectorAction[0]).normalized*moveSpeed,ForceMode.Acceleration);// vectorAction[1])*moveSpeed);

    }

    private void resetgoal()
    {

    }

    public override void AgentReset()
    {
        health = maxHealth;
        this.gameObject.transform.position = spawnPoint.transform.position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ball")
        {
            dodjiesBall ball = collision.gameObject.GetComponent<dodjiesBall>();
            if(ball.team == team)
            {
                //shoot
                AddReward(0.2f);
                collision.transform.position = barrel.transform.position;
                collision.gameObject.GetComponent<Rigidbody>().velocity = (enemy.transform.position-this.transform.position).normalized*shotSpeed;
                
            }
            else
            {
                Destroy(collision.gameObject);
                //get hit
                getHit();
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "wall" || collision.gameObject.tag == "Player")
        {
            AddReward(-0.05f);
        }
    }
    public void getHit()
    {
        health--;
        if (health <= 0)
        {
            gridSystem.Reset(team);
            AddReward(-2);
            Done();
        }
        else
        {
            AddReward(-1f);
        }

    }
    public void win()
    {
        //AddReward(1);
        Done();
    }
    void Update()
    {
        Vector3 tmp = (enemy.transform.position - this.transform.position).normalized;
        
        rotateSec.transform.rotation=Quaternion.Euler(0,Mathf.Atan2(-tmp.z,tmp.x)*Mathf.Rad2Deg,0);
        Debug.DrawRay(transform.position, tmp, Color.red);
    }

}
