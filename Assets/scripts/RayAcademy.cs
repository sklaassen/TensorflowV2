using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RayAcademy : Academy
{
    public int maxBalls = 5;
    public int balls = 1;
    public int steps = 0;
    public override void AcademyReset()
    {

    }

    public override void AcademyStep()
    {
        steps++;
        balls = Mathf.Min((int)(steps / 100000f) + 1, maxBalls);

    }
}
