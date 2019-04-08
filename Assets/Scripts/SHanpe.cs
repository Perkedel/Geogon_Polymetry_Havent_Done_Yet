using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SHanpe : MonoBehaviour
{
    public HexEngineProto CoreLevelManager;
    public newPersonCamera CameraRig;

    public ShapeList[] shapeLists;
    public int shapeIndex = 0;
    public int prevShapeIndex = 0;
    public Camera selectCamera;

    public float Torqueing = 0f;
    public float Forcing = 0f;
    public float Jumping = 500f;

    //Deadzoning. Many 3rd party controllers are badly programmed! not all people has luxury!
    public float DeadzoneUp = 0.01f;
    public float DeadzoneDown = -0.01f;
    public float DeadzoneLeft = -0.01f;
    public float DeadzoneRight = 0.01f;

    //Move based on camera look at
    public Vector3 front;
    public Vector3 side;

    //Statusing
    [Range(0f,100f)] public float HP = 100f;
    [Range(0f, 100f)] public float Armor = 10f;
    [Range(0,2)] public int JumpToken = 2;
    public bool hasJumped = false;

    private void Awake()
    {
        GameObject[] SHanpeMyselves = GameObject.FindGameObjectsWithTag("Player");
        if (SHanpeMyselves.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!selectCamera) selectCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (selectCamera)
        {
            front = new Vector3(0, selectCamera.transform.rotation.y, 0);
        } else
        {
            front = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            moveShape(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            moveShape(-1);
        }
        correctIndex();

        /*if(Input.GetAxis("Horizontal") >= DeadzoneRight || Input.GetAxis("Horizontal") <= DeadzoneLeft)
        {
            //GetComponent<Rigidbody>().AddForce(Vector3.right * Forcing * Input.GetAxis("Horizontal"));
            GetComponent<Rigidbody>().AddTorque(-Vector3.forward * Torqueing * Input.GetAxis("Horizontal")); //Rotation of axis relative. if you X, it will rotate in x axis!
        }
        if (Input.GetAxis("Vertical") >= DeadzoneUp || Input.GetAxis("Vertical") <= DeadzoneDown)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.forward * Forcing * Input.GetAxis("Vertical"));
            GetComponent<Rigidbody>().AddTorque(Vector3.right * Torqueing * Input.GetAxis("Vertical"));
        }
        */

        if(Input.GetAxis("Jump") > .5f)
        {
            if (JumpToken > 0)
            {
                if (!hasJumped)
                {
                    GetComponent<Rigidbody>().AddForce(Vector3.up * Jumping);
                    JumpToken--;
                    hasJumped = true;
                }
            }
        } else
        {
            hasJumped = false;
        }

        //Debug.Log(Input.GetAxis("Horizontal")); //result = -1, 0, 1, float value
    }

    private void FixedUpdate()
    {
        // https://forum.unity.com/threads/moving-character-relative-to-camera.383086/ Andrey Kubyshkin
        //reading the input:
        float horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");

        //assuming we only using the single camera:
        var camera = Camera.main;

        //camera forward and right vectors:
        var forward = camera.transform.forward;
        var right = camera.transform.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //this is the direction in the world space we want to move:
        var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;

        GetComponent<Rigidbody>().AddForce(desiredMoveDirection * Forcing);
        GetComponent<Rigidbody>().AddTorque(new Vector3(desiredMoveDirection.z * Torqueing, 0, desiredMoveDirection.x * -Torqueing));
    }

    void correctIndex()
    {
        if (shapeIndex >= shapeLists.Length)
        {
            shapeIndex = 0;
        }
        if (shapeIndex < 0)
        {
            shapeIndex = shapeLists.Length-1;
        }
    }

    void setShape(int whichIndex)
    {
        shapeLists[prevShapeIndex].gameObject.SetActive(false);
        shapeLists[whichIndex].gameObject.SetActive(true);
        Torqueing = shapeLists[whichIndex].Torqueing;
        Forcing = shapeLists[whichIndex].Forcing;
    }

    void moveShape(int howMuch)
    {
        prevShapeIndex = shapeIndex;
        shapeIndex += howMuch;
        correctIndex();
        setShape(shapeIndex);
    }

    private void OnCollisionEnter(Collision collision)
    {
        JumpToken = 2;
        //To be added: Anti-tag. Jump Token won't reset if collided with the specific gameobject tag. use script to mark instead of tag Unity.
        //It also has special insteading function to do with jumptoken.
        //Item Effect Collider
    }
}
