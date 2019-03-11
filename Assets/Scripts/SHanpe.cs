using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHanpe : MonoBehaviour
{

    public ShapeList[] shapeLists;
    public int shapeIndex = 0;
    public int prevShapeIndex = 0;

    public float Torqueing = 0f;
    public float Forcing = 0f;

    //Deadzoning. Many 3rd party controllers are badly programmed! not all people has luxury!
    public float DeadzoneUp = 0.01f;
    public float DeadzoneDown = -0.01f;
    public float DeadzoneLeft = -0.01f;
    public float DeadzoneRight = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            prevShapeIndex = shapeIndex;
            shapeIndex++;
            if (shapeIndex > 2)
            {
                shapeIndex = 0;
            }
            if (shapeIndex < 0)
            {
                shapeIndex = 2;
            }
            setShape(shapeIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            prevShapeIndex = shapeIndex;
            shapeIndex--;
            if (shapeIndex > 2)
            {
                shapeIndex = 0;
            }
            if (shapeIndex < 0)
            {
                shapeIndex = 2;
            }
            setShape(shapeIndex);
        }
        if (shapeIndex > 2)
        {
            shapeIndex = 0;
        }
        if (shapeIndex < 0)
        {
            shapeIndex = 2;
        }

        if(Input.GetAxis("Horizontal") >= DeadzoneRight || Input.GetAxis("Horizontal") <= DeadzoneLeft)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.right * Forcing * Input.GetAxis("Horizontal"));
            GetComponent<Rigidbody>().AddTorque(Vector3.right * Torqueing * Input.GetAxis("Horizontal"));
        }
        if (Input.GetAxis("Vertical") >= DeadzoneUp || Input.GetAxis("Vertical") <= DeadzoneDown)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.forward * Forcing * Input.GetAxis("Vertical"));
            GetComponent<Rigidbody>().AddTorque(Vector3.forward * Torqueing * Input.GetAxis("Vertical"));
        }

        //Debug.Log(Input.GetAxis("Horizontal")); //result = -1, 0, 1, float value
    }
    void setShape(int whichIndex)
    {
        shapeLists[prevShapeIndex].gameObject.SetActive(false);
        shapeLists[whichIndex].gameObject.SetActive(true);
        Torqueing = shapeLists[whichIndex].Torqueing;
        Forcing = shapeLists[whichIndex].Forcing;
    }
}
