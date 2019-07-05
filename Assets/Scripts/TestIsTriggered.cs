using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIsTriggered : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("I am triggered by " + collider.name + "!!!");
    }
}
