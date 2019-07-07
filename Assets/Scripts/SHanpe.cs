using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Experimental;

public class SHanpe : MonoBehaviour
{

    public HexEngineProto CoreLevelManager;
    public newPersonCamera CameraRig; 
    
    //Spare parts
    public ShapeList[] shapeLists;
    public ShapeListMaster shapeListMaster;
    public ParticleSystem ExplodosDuar;
    public AudioClip[] ExplodoSounds;
    public ShapeList EikSerkatShape;
    public int shapeIndex = 0;
    public int prevShapeIndex = 0;
    public Camera selectCamera;
    //public GroundedFootCast GroundedFootCasting;

    //Parametrics
    public float Torqueing = 0f;
    public float Forcing = 0f;
    public float Jumping = 500f;
    public float FootGroundLength = 0.5f;
    public float respawnInTime = 5f;
    public float respawnTimer;
    public Vector3 CheckPointres;

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
    public bool isAlive= true;
    public bool hasBooted = true;
    [Range(0f, 100f)] public float Armor = 10f;
    [Range(0,2)] public int JumpToken = 2;
    public bool hasJumped = false;
    public bool IamControllable = true;
    public bool RayGrounded = false;

    //Inventorial
    [SerializeField] float coine = 0f;
    public float Coine { get => coine; set => coine = value; }

    //Debugging
    public bool ScronchSelfButton = false;

    private void Awake()
    {
        GameObject[] SHanpeMyselves = GameObject.FindGameObjectsWithTag("Player");
        if (SHanpeMyselves.Length > 1)
        {
            Destroy(this.gameObject);
        }
        GetComponent<Rigidbody>().maxAngularVelocity = 50f;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!selectCamera) selectCamera = Camera.main;
        correctIndex();
        CheckPointres = transform.position;
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

        
        RaycastHit Hit; //Raycasting from Official Unity Tutorial
        Ray GroundingRay = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down * FootGroundLength);
        RayGrounded = Physics.Raycast(GroundingRay, out Hit, FootGroundLength);
        float horizontalAxis = 0f;
        float verticalAxis = 0f;
        if (isAlive)
        {
            if ((MeIsGrounded && RayGrounded) && JumpToken < 2)
            {
                JumpToken = 2;
            }
            if (IamControllable)
            {
                // https://forum.unity.com/threads/moving-character-relative-to-camera.383086/ Andrey Kubyshkin
                //reading the input:
                
                if (IamControllable)
                {
                    //horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");
                    //verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");
                    horizontalAxis = Input.GetAxisRaw("Horizontal");
                    verticalAxis = Input.GetAxisRaw("Vertical");
                    //IT'S RAW!!! RECOOK AGAIN!
                    //DO NOT CRASH MY GAME AGAIN!
                    //sorry Jonas, I was slipped a thought about your game. about mods that needs recook.
                }
                else
                {
                    horizontalAxis = 0f;
                    verticalAxis = 0f;
                }
                if (Input.GetAxisRaw("Jump") > .5f)
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
                } else if(Input.GetAxisRaw("Jump") < .5f)
                {
                    hasJumped = false;
                }
            }
            else
            {
                hasJumped = false;
            }
        } else
        {
            hasJumped = false;
            JumpToken = 0;
        }
        joyHori = horizontalAxis;
        joyVert = verticalAxis;
        //GetComponent<Rigidbody>().AddForce(desiredMoveDirection2 * Forcing); //This keeps adding more velocity to until one point character moves too fast,
        //the slippery object will be benefitable or some case player could use this to jump really high somehow. that's cheating!
        //Therefore, we have to use set velocity rather than adding it so its speed are kept constant to desired velocity to move.
        //{Moved to that keypress check}
        //GetComponent<Rigidbody>().AddTorque(new Vector3(desiredMoveDirection2.z * Torqueing, 0, desiredMoveDirection2.x * -Torqueing)); //This adds torque and it's too fast!
        //if(desiredMoveDirection2.z != 0f|| desiredMoveDirection2.x !=0f)

        //Problem is, Unity's Key2Joy has quite sticky and resistive recoil which foolishly recoil the analog stick much slower than most irl elastic recoil on most gamepads and joystick.
        //therefore, the angular velocity will still set if the Joystick is not exactly zero. Hence, slowing the roll velocity on Keyboard player if player releases the axis and let it recoil.
        //We suggest to use direct Keycode check to see if the key is actually released and by the next tick the keycode release, the system no longer set angular velocity until the keycode is pressed by another tick one day /tick.

        //if(Input.GetKey("Horizontal")||Input.GetKey("Vertical")) //not working! get key is about direct system, not by the axis list on project setting!

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) // with keypress check to make sure it doesn't set velocity when all axes are not pressed
        { //OH! maybe if I use raw, this method won't apply the irrealistic slow recoil! I guess...
            if (Forcing > 0)
            {
                //GetComponent<Rigidbody>().velocity = new Vector3(desiredMoveDirection2.x * Forcing, Physics.gravity.y, desiredMoveDirection2.z * Forcing); //this doesn't work really well and badly unrealistic operation. Apply with gravity means y minus gravity level which seems unexpectedly too fast and floor slamming. requires complex heurestic which is foolishly inefficient and over-competitive.
                //just use addforce then.
                GetComponent<Rigidbody>().AddForce(desiredMoveDirection2 * Forcing);
            }
            if (Torqueing > 0)
            {
                GetComponent<Rigidbody>().angularVelocity = new Vector3(desiredMoveDirection2.z * Torqueing, 0, desiredMoveDirection2.x * -Torqueing);
            }
        } //It works! it doesn't apply any sticky recoil!
        //Well, if you'd ask "why don't we use recoil?", is that because Geogon's character are meant to be truly Physics based unlike any other platformer which commonly a kinematic that has illusion inertia (deceleration thingy)
        //So you'll see that there is a caveat, where your character will have no natural brakes caused by Unity's slow recoil of Key2Joy, Hence your character will have less stability compared to Kinematic pre-scripted physics (that deceleration thingy)
        //But it should resolvable using counteract movement (turn around to move opposite to brake), usually using Forcing if you are on air, and/or Torque if on floor or so.
        

        //Debug.Log(Input.GetAxis("Horizontal")); //result = -1, 0, 1, float value
        if (ScronchSelfButton)
        {
            HP = 0;
            ScronchSelfButton = false;
        }

        if(HP > 0)
        {
            isAlive = true;
        } else
        {
            isAlive = false;
        }
        if (isAlive)
        {
            if (!hasBooted)
            {
                aliveShape();
                hasBooted = true;
            }
        } else
        {
            if (hasBooted)
            {
                deddShape();
                hasBooted = false;
            }
        }
        if (hasBooted)
        {

        }
        else
        {
            if (!respawnTimerHasStarted)
            {
                startRespawn();
                respawnTimerHasStarted = true;
            }
        }
        if (respawnTimerHasStarted)
        {
            respawnTimer -= Time.deltaTime;
            if(respawnTimer < 0)
            {
                respawnSHanpe();
                respawnTimer = 0;
                respawnTimerHasStarted = false;
            }
        }
    }

    [SerializeField] Vector3 desiredMoveDirection2;
    [SerializeField] float joyVert, joyHori;
    private void FixedUpdate()
    {
        
        

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
        var desiredMoveDirection = forward * joyVert + right * joyHori;

        desiredMoveDirection2 = desiredMoveDirection;
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
        Torqueing = shapeLists[shapeIndex].Torqueing;
        Forcing = shapeLists[shapeIndex].Forcing;
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

    void refreshShape()
    {
        isAlive = true;
        correctIndex();
        setShape(shapeIndex);
    }

    void deddShape()
    {
        correctIndex();
        shapeListMaster.gameObject.SetActive(false);
        Instantiate(ExplodosDuar, transform.position, Quaternion.identity);
        EikSerkatShape.gameObject.SetActive(true);
        Torqueing = EikSerkatShape.Torqueing;
        Forcing = EikSerkatShape.Forcing;
    }
    void aliveShape()
    {
        correctIndex();
        shapeListMaster.gameObject.SetActive(true);
        EikSerkatShape.gameObject.SetActive(false);
        Torqueing = shapeLists[shapeIndex].Torqueing;
        Forcing = shapeLists[shapeIndex].Forcing;
    }

    [SerializeField] bool respawnTimerHasStarted = false;
    void startRespawn()
    {
        respawnTimer = respawnInTime;
        //respawnTimerHasStarted = true;
    }

    public void teleportSHanpe(Vector3 position)
    {
        transform.position = position;
    }

    public void respawnSHanpe()
    {
        HP = 100f;
        refreshShape();
        teleportSHanpe(CheckPointres);
    }

    public void ReceiveKaerlevWave(float KaerlevPower)
    {
        //Do something if the item has Kaerlev Wave
        if(KaerlevPower > 0f)
        {
            Debug.Log("SHanpe: Cool and Good!!!"); //SHanpe likes
        }
        else if(KaerlevPower == 0f)
        {
            Debug.Log("SHanpe: meh!"); //SHanpe mehs
        }
        else if (KaerlevPower < 0f)
        {
            Debug.Log("SHanpe: Wuek!!!"); //SHape hates
        }
    }

    public void DamageMe(float HowMuch)
    {
        HP -= (HowMuch)/Armor;
        Armor -= HowMuch;
    }

    public void HealMe(float HowMuch)
    {
        HP += HowMuch;
    }
    public void HealMe(float HowMuch, float withArmor)
    {
        HP += HowMuch;
        Armor += withArmor;
    }

    [Header("Grounded")]
    [SerializeField] bool MeIsGrounded = false;
    [SerializeField] bool footIsGrounded = false;

    

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Me is colliding");
        MeIsGrounded = true;
        //JumpToken = 2;
        //To be added: Anti-tag. Jump Token won't reset if collided with the specific gameobject tag. use script to mark instead of tag Unity.
        //It also has special insteading function to do with jumptoken.
        //Item Effect Collider
    }

    private void OnCollisionExit(Collision collision)
    {
        MeIsGrounded = false;
    }
}
