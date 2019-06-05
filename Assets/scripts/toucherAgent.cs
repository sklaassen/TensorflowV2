using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class toucherAgent : Agent
{
    public GameObject[] Arms;
    public GameObject goal;
    private toucherAcademy myAcademy;

    public override void InitializeAgent()
    {
        myAcademy = GameObject.Find("Academy").GetComponent<toucherAcademy>();
    }

    public override void CollectObservations()
    {
        for(int i = 0; i < Arms.Length; i++)
        {
            AddVectorObs(Arms[i].transform.localPosition);
            AddVectorObs(Arms[i].transform.rotation);
        }

        AddVectorObs(goal.transform.localPosition);
        AddVectorObs(goal.transform.rotation);


    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
      
        for(int i = 0; i < Arms.Length-1; i++)
        {
            //Quaternion tmp = Arms[i].transform.rotation;
            Arms[i].transform.Rotate(i==0?Vector3.up:Vector3.right, vectorAction[i],Space.Self);
            //Arms[i].transform.rotation = Quaternion.Lerp(tmp, Arms[i].transform.rotation,0.2f);

        }
        rewards();

    }

    public float prevDistance;

    public void rewards()
    {
        if(Vector3.Distance(goal.transform.position, Arms[Arms.Length - 1].transform.position) < 1)
        {
            Debug.Log("Touched");
            Done();
            AddReward(5);
            return;
        }

        float reward = -0.001f;
        if (prevDistance > Vector3.Distance(goal.transform.position,Arms[Arms.Length-1].transform.position))
        {
            reward += 0.02f;
        }
        AddReward(reward);
        prevDistance = Vector3.Distance(goal.transform.position, Arms[Arms.Length - 1].transform.position);
    }
    private void resetgoal()
    {
        goal.transform.position = new Vector3(Random.Range(-maxGoalDIstance, maxGoalDIstance), Random.Range(-maxGoalDIstance, maxGoalDIstance), Random.Range(-maxGoalDIstance, maxGoalDIstance)) + transform.position;

        //goal.transform.localPosition = new Vector3(-1f, 3, -10);
        prevDistance = Vector3.Distance(goal.transform.position, Arms[Arms.Length - 1].transform.position);
    }

    private float maxGoalDIstance = 10f;
    public override void AgentReset()
    {
        for (int i = 0; i < Arms.Length; i++)
        {
            Arms[i].transform.rotation = new Quaternion();
        }
        
        resetgoal();
    }
}
