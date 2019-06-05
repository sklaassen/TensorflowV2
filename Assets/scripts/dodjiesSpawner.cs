using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodjiesSpawner : MonoBehaviour
{
    public GameObject[] path;
    public float moveSpeed = 0.1f;
    public GameObject ball;
    public float fireRate = 1;
    private float currentPosition = 0.5f;
    public float nextShot = 0;
    public dodjiesMap map;
    public GameObject barrel;
    public float shotSpeed = 2;

    public void ResetObject()
    {
        currentPosition = 0.5f;
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    private void FixedUpdate()
    {
        currentPosition += moveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(path[0].transform.position, path[1].transform.position, Mathf.PingPong(currentPosition,1));
        if(nextShot < Time.time)
        {
            GameObject obj = Instantiate(ball, barrel.transform.position,new Quaternion(),map.gameObject.transform);
// Debug.DrawLine(barrel.transform.position, (barrel.transform.position+(barrel.transform.position-transform.position)), Color.red,5);
            obj.GetComponent<Rigidbody>().velocity = (barrel.transform.position-transform.position).normalized * shotSpeed + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            map.balls.Add(obj);
            nextShot = Time.time + fireRate;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
