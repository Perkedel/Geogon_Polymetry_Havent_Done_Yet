using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class behaviourpeluru : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward * 1000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
