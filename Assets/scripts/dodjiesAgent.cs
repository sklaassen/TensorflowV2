using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class dodjiesAgent : Agent
{
    //immitation: https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Training-Imitation-Learning.md
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

    public enum DodgiesAITypes
    {
        visual,
        grid,
        wiskers,
        student

    }
    public DodgiesAITypes AIType;

    public override void InitializeAgent()
    {
        rb = GetComponent<Rigidbody>();
        myAcademy = GameObject.Find("Academy").GetComponent<dojiesAcademy>();

        gridSystem.players[team] = this.gameObject;
    }

    public override void CollectObservations()
    {
        switch (AIType)
        {
            case DodgiesAITypes.visual:
                AddVectorObs(team);
                break;
            case DodgiesAITypes.grid:
                AddVectorObs(player.transform.localPosition);
                AddVectorObs(enemy.transform.localPosition);
                AddVectorObs(goodSpawners.transform.localPosition);
                AddVectorObs(badSpawners.transform.localPosition);

                AddVectorObs(gridSystem.getGrid(team));
                break;
            case DodgiesAITypes.wiskers:
                AddVectorObs(player.transform.localPosition);
                AddVectorObs(enemy.transform.localPosition);
                AddVectorObs(goodSpawners.transform.localPosition);
                AddVectorObs(badSpawners.transform.localPosition);

                AddVectorObs(gridSystem.drawWiskers(team));
                break;
            case DodgiesAITypes.student:
                break;
        }

    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        AddReward(0.1f);
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
                AddReward(1f);
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
            AddReward(-0.15f);
        }
    }
    public void getHit()
    {
        health--;
        if (health <= 0)
        {
            gridSystem.Reset(team);
            AddReward(-10);
            Done();
        }
        else
        {
            AddReward(-5f);
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
