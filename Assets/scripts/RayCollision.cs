using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RayCollision : MonoBehaviour
{
    public RayAgent agent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            //Debug.Log("score!!");
            agent.moveBall();
        }
        else if (other.gameObject.tag == "pit")
        {
            //Debug.Log("out");
            agent.AddReward(-10);
            agent.Done();
        }
    }
}
