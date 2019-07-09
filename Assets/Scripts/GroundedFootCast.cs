using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental;

public class GroundedFootCast : MonoBehaviour
{
    [SerializeField] private Collider TheColliding;

    // Start is called before the first frame update
    void Start()
    {
        TheColliding = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool footIsGrounded = false;
    private void OnTriggerEnter(Collider collision)
    {
        footIsGrounded = true;
    }
    private void OnTriggerExit(Collider collision)
    {
        footIsGrounded = false;
    }
}
