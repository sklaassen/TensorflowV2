using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodjiesMap : MonoBehaviour
{
    public int[] gridSize = { 15, 10 };
    public List<GameObject> balls;
    public GameObject[] players = new GameObject[2];

    public float[] getGrid(int team)
    {
        int totSize = gridSize[0] * gridSize[1];
        float[] outarr = new float[2* totSize];
        foreach (GameObject ball in balls)
        {
            if(ball!= null)
            {
                //Debug.Log((ball.GetComponent<dodjiesBall>().team == team ? 0 : 1) * totSize + (int)(ball.transform.position.x - transform.position.x) * gridSize[1] + (int)(ball.transform.position.z - transform.position.z));
                outarr[(ball.GetComponent<dodjiesBall>().team==team?0:1)*totSize+ (int)(transform.position.x - transform.position.x) *gridSize[1]+ (int)(transform.position.z-transform.position.z)] +=1;
            }
        }
        if (team == -1)
        {
            string s = "";
            for(int y = 0; y < gridSize[1]; y++)
            {
                for(int x= 0; x < gridSize[0]; x++)
                {
                    Debug.DrawRay(new Vector3(x+0.45f, 0, y+0.5f)+transform.position, Vector3.up * outarr[(x * gridSize[1] + y)], Color.red);
                    Debug.DrawRay(new Vector3(x+0.55f, 0, y+0.5f)+transform.position, Vector3.up * outarr[(totSize + x * gridSize[1] + y)], Color.blue);
                    s += outarr[(x * gridSize[1] + y)];
                    s += ":";
                    s += outarr[(1 * totSize + x * gridSize[1] + y)];
                    s += "|";
                }
                s += "\n";
            
            }
            //Debug.Log(s);
        }

        return outarr;
    }

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> diagonalPoints = new List<Vector3>();
        for (float i = 0; i < 359; i += wiskersSubdivisions)
        {

            diagonalPoints.Add(new Vector3(Mathf.Sin(i * Mathf.Deg2Rad), 0, Mathf.Cos(i * Mathf.Deg2Rad)).normalized* WiskerLength);
        }
        diagonalPointsArr = diagonalPoints.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        //drawGrid();
        drawWiskers();
    }
    private void drawGrid()
    {
        for(int x = 0; x<= gridSize[0]; x++)
        {
            Debug.DrawLine(new Vector3(x, 0, 0)+transform.position, new Vector3(x, 0, gridSize[1]) + transform.position);
        }
        for(int y = 0; y <= gridSize[1]; y++)
        {
            Debug.DrawLine(new Vector3(0, 0, y) + transform.position, new Vector3(gridSize[0], 0, y) + transform.position);
        }
    }
    float wiskersSubdivisions = 360 / 16f;
    Vector3[] diagonalPointsArr;
    float WiskerLength = 5;

    public float[] drawWiskers(int team = 0)
    {
        float[] distanceArr = new float[diagonalPointsArr.Length*2];

        for (int i = 0; i < distanceArr.Length; i++)
        {
            distanceArr[i] = WiskerLength;
        }
        foreach (GameObject ball in balls)
        {
            if (ball != null)
            {
                float angle = Mathf.Abs(Vector3.SignedAngle(Vector3.back , (ball.transform.position-players[team].transform.position).normalized, Vector3.up)+179.9f);
                if (ball.GetComponent<dodjiesBall>().team == team)
                {
                    
                    distanceArr[(int)(angle / wiskersSubdivisions)] = Mathf.Min(distanceArr[(int)(angle / wiskersSubdivisions)], Vector3.Distance(ball.transform.position, players[team].transform.position));
                }
                else
                {
                    try
                    {
                        distanceArr[(int)(angle / wiskersSubdivisions)+ diagonalPointsArr.Length] = Mathf.Min(distanceArr[(int)(angle / wiskersSubdivisions) + diagonalPointsArr.Length], Vector3.Distance(ball.transform.position, players[team].transform.position));
                    }
                    catch
                    {
                        Debug.Log("angle: " + angle + " wiskers: " + wiskersSubdivisions + " diagonal: " + diagonalPointsArr.Length);
                    }
                   
                }
            }
        }
        if(team == -1)
        {
            for (int i = 0; i < diagonalPointsArr.Length; i++)
            {
                Debug.DrawRay(players[team].transform.position, diagonalPointsArr[i], Color.blue);
                Color c = distanceArr[i]==WiskerLength?Color.blue:Color.green;
                Color c2 = distanceArr[i + diagonalPointsArr.Length] == WiskerLength ? Color.blue : Color.red;

                Debug.DrawLine(Vector3.Lerp(players[team].transform.position, players[team].transform.position + diagonalPointsArr[i], distanceArr[i] / WiskerLength), Vector3.Lerp(players[team].transform.position, players[team].transform.position + diagonalPointsArr[(i + 1) % diagonalPointsArr.Length], distanceArr[i] / WiskerLength), c);
                Debug.DrawLine(Vector3.Lerp(players[team].transform.position, players[team].transform.position + diagonalPointsArr[i], distanceArr[i + diagonalPointsArr.Length] / WiskerLength), Vector3.Lerp(players[team].transform.position, players[team].transform.position + diagonalPointsArr[(i + 1) % diagonalPointsArr.Length], distanceArr[i + diagonalPointsArr.Length] / WiskerLength), c2);
            }
        }
        return distanceArr;

    }

    public void Reset(int teamLost)
    {
        for(int i = 0; i < players.Length; i++)
        {
            if (i != teamLost)
            {
                players[i].GetComponent<dodjiesAgent>().win();
            }
        }
        foreach(var ball in balls)
        {
            if(ball != null)
                Destroy(ball);
        }
        balls = new List<GameObject>();
    }
}
