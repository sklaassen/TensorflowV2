using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Rotate(axis, speed);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
