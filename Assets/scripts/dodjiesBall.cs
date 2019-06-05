using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodjiesBall : MonoBehaviour
{
    public int team;
    private Rigidbody rb;
    public float speed =2;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall" || collision.gameObject.tag == "ball")
        {
            if(rb!=null)
            rb.velocity = Vector3.Reflect(-collision.relativeVelocity,collision.contacts[0].normal).normalized*speed;
        }
    }
}
