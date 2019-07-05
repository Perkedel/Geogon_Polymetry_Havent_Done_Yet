using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinme : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float torquelevel = 100f;
    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody>().AddTorque(Vector3.up * torquelevel, ForceMode.VelocityChange); //this constantly adds it up!
        GetComponent<Rigidbody>().maxAngularVelocity = 99999999999999f; //deconstraint the max angular velocity spin record!!!
        GetComponent<Rigidbody>().angularVelocity = Vector3.up * torquelevel;
    }
}
