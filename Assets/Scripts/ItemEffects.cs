using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental;
//using UnityEngine.Experimental.PlayerLoop;

public class ItemEffects : MonoBehaviour
{
    public GameObject WhoIsHitting;
    public GameObject ParentOfHitting;
    public SHanpe player;
    public SHanpe targetPlayer;

    public bool[] MustBeTheseShapeIndexes = { true, true, true };
    public bool Deserves = false;
    

    // Start is called before the first frame update
    void Start()
    {
        //MustBeTheseShapeIndexes = new int[] { 0, 1, 2 };
        WhoIsHitting = null;
        ParentOfHitting = null;
        player = null;
        //targetPlayer = FindObjectOfType<SHanpe>().CompareTag("Player");
        if (!targetPlayer)
        {
            // Search for object with Player tag
            var go = GameObject.FindWithTag("Player");
            // Check we found an object with the player tag
            if (go)
                // Set the target to the object we found
                targetPlayer = go.transform.GetComponent<SHanpe>();
        }
        if (targetPlayer)
        {
            SHanpeShapeIndex = targetPlayer.shapeIndex;

            for (int i = 0; i < MustBeTheseShapeIndexes.Length; i++)
            {
                if (MustBeTheseShapeIndexes[i] && SHanpeShapeIndex == i)
                {
                    Deserves = true;
                }
            }
        }

        if (!hexEngineProtoTargetMe)
        {
            var Go = GameObject.FindGameObjectWithTag("HexagonEngineCore");
            if (Go)
            {
                hexEngineProtoTargetMe = Go.transform.GetComponent<HexEngineProto>();
            }
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (targetPlayer)
        {
            SHanpeShapeIndex = targetPlayer.shapeIndex;

            for (int i = 0; i < MustBeTheseShapeIndexes.Length; i++)
            {
                if (MustBeTheseShapeIndexes[i] && SHanpeShapeIndex == i)
                {
                    Deserves = true;
                } else if(!MustBeTheseShapeIndexes[i] && SHanpeShapeIndex == i)
                {
                    Deserves = false;
                }
            }


        }
        //if (!hexEngineProtoTargetMe)
        //{
        //    var Go = GameObject.FindGameObjectWithTag("HexagonEngineCore");
        //    if (Go)
        //    {
        //        hexEngineProtoTargetMe = Go.transform.GetComponent<HexEngineProto>();
        //    }
        //}
    }

    [SerializeField] private int SHanpeShapeIndex = 0;
    private void LateUpdate()
    {
        
    }

    [Header("Single Use")]
    public bool doDestroyItself = false;
    public void letsDestroyItself()
    {
        Destroy(this.gameObject);
    }

    [Header("Functionality")]
    public bool doDisableItself = false; //Single Use
    public void letsDisableItself()
    {
        gameObject.SetActive(false);
    }

    [Header("Pseudo Single Use")]
    public bool doDisableItselfTrigger = false;
    public void letsDisableItselfTrigger()
    {
        GetComponent<Collider>().enabled = false;
    }

    [Header("Disable Game Objects")]
    public bool doDisableGameObjects = false;
    public GameObject [] disableGameObjectsList;
    public void letsDisableGameObjects()
    {
        for(int i = 0; i < disableGameObjectsList.Length; i++)
        {
            if (disableGameObjectsList[i])
            {
                disableGameObjectsList[i].SetActive(false);
            }
        }
    }
    [Header("Disable Colliders")]
    public bool doDisableColliders = false;
    public Collider[] disableCollidersList;
    public void letsDisableColliders()
    {
        for (int i = 0; i < disableCollidersList.Length; i++)
        {
            if (disableCollidersList[i])
            {
                disableCollidersList[i].enabled = false;
            }
        }
    }
    [Header("Disable NavMeshAgents")]
    public bool doDisableNavMeshAgents = false;
    public NavMeshAgent[] disableNavMeshAgentsList;
    public void letsDisableNavMeshAgents()
    {
        for (int i = 0; i < disableNavMeshAgentsList.Length; i++)
        {
            if (disableNavMeshAgentsList[i])
            {
                disableNavMeshAgentsList[i].enabled = false;
            }
        }
    }

    [Header("Finish Level, Level Complete / Fail")]
    public bool doFinishLevel = false;
    public HexEngineProto hexEngineProtoTargetMe;
    public enum FinishChoice { Completed, Failed};
    public FinishChoice SelectFinishType;
    public enum FinishAction { NextLevel, RestartLevel, MainMenu, ExitGame}
    public void letsFinishLevel()
    {
        hexEngineProtoTargetMe.FinishLevel(SelectFinishType);
    }

    [Header("Play These Sounds")]
    public bool doPlayTheseSounds = false;
    [Range(0f,100f)] public float Volumeting = 100f;
    public AudioSource AudioSourceTarget;
    public AudioClip[] SoundLists;
    public void letsPlayTheseSounds()
    {
        if (AudioSourceTarget)
        {
            for(int i = 0; i < SoundLists.Length; i++)
            {
                if(SoundLists[i])
                AudioSourceTarget.PlayOneShot(SoundLists[i],Volumeting);
            }
        } else if (!AudioSourceTarget)
        {
            for (int i = 0; i < SoundLists.Length; i++)
            {
                if (SoundLists[i])
                    GetComponent<AudioSource>().PlayOneShot(SoundLists[i],Volumeting);
            }
        }
    }

    [Header("Play one of these sounds randomly selected")]
    public bool doPlayOneRandomSounds = false;
    [Range(0f, 100f)] public float Volumetong = 100f;
    public AudioClip[] SoundListsRand;
    public void letsPlayOneRandomSounds()
    {
        if (AudioSourceTarget)
        {
            for (int i = 0; i < SoundListsRand.Length; i++)
            {
                if (SoundListsRand[i])
                    AudioSourceTarget.PlayOneShot(SoundListsRand[i],Volumetong);
            }
        }
        else if (!AudioSourceTarget)
        {
            for (int i = 0; i < SoundListsRand.Length; i++)
            {
                if (SoundListsRand[i])
                    GetComponent<AudioSource>().PlayOneShot(SoundListsRand[i],Volumetong);
            }
        }
    }


    [Header("Scronch Musuh")]
    public bool doScronchMusuh = false;
    public Kejar MusuhEngine;
    public void letsScronchMusuh()
    {
        if (MusuhEngine)
        {
            MusuhEngine.ScronchDiriSendiri();
        }
    }

    [Header("Player hit chase range EyeCast")]
    public bool doVisionEyecast = false;
    public bool isVisionEyecastHit = false;
    public void letsVisionEyeCast()
    {
        isVisionEyecastHit = true;
        if (MusuhEngine)
        {
            MusuhEngine.Target = player.gameObject.transform;
        }
    }
    public void stopVisionEyeCast()
    {
        isVisionEyecastHit = false;
    }

    [Header("Player hit attack range Handcast")]
    public bool doAttackHandcast = false;
    public bool isAttackHandcastHit = false;
    public void letsAttackHandcast()
    {
        isAttackHandcastHit = true;
        if (MusuhEngine)
        {
            MusuhEngine.Focus = player;
        }
    }
    public void stopAttackHandcasst()
    {
        isAttackHandcastHit = false;
    }

    [Header("Spawn GameObjects")]
    public bool doEmitGameObjects = false;
    public GameObject[] GameObjectPrefabs;
    public void letsEmitGameObjects()
    {
        for (int i = 0; i < GameObjectPrefabs.Length; i++)
        {
            if (GameObjectPrefabs[i])
            {
                Instantiate(GameObjectPrefabs[i].gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    [Header("Spawn Particles")]
    public bool doEmitParticles = false;
    public ParticleSystem[] ParticlePrefabs;
    public void letsEmitParticles()
    {
        for (int i = 0; i < ParticlePrefabs.Length; i++)
        {
            if (ParticlePrefabs[i])
            {
                Instantiate(ParticlePrefabs[i].gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    [Header("Kaerlev wave")]
    public bool doEmitKaerlevWave = false; //Hat kid has tendency to taunt :P whenever she saw Mafia, e.g.
    public bool PersistentKaerleEmission = false;
    [Range(-100f,100f)] public float howMuchKaerlevWavePowerItHas = 0f;
    public void letsEmitKaerlevWave()
    {
        targetPlayer.ReceiveKaerlevWave(howMuchKaerlevWavePowerItHas);
    }

    [Header("Core of the Core Intinya Inti, Pak Ndul")]
    public bool CoreOfInti = true;
    private void DoTriggerEnter()
    {
        if (doDisableItselfTrigger)
        {
            letsDisableItselfTrigger();
        }
        if (doDisableGameObjects)
        {
            letsDisableGameObjects();
        }
        if (doDisableColliders)
        {
            letsDisableColliders();
        }
        if (doDisableNavMeshAgents)
        {
            letsDisableNavMeshAgents();
        }
        if (doVisionEyecast)
        {
            letsVisionEyeCast();
        }
        if (doScronchMusuh)
        {
            letsScronchMusuh();
        }
        if (doEmitParticles)
        {
            letsEmitParticles();
        }
        if (doEmitGameObjects)
        {
            letsEmitGameObjects();
        }
        if (doPlayTheseSounds)
        {
            letsPlayTheseSounds();
        }
        if (doPlayOneRandomSounds)
        {
            letsPlayOneRandomSounds();
        }
        if (doEmitKaerlevWave)
        {
            letsEmitKaerlevWave();
        }
        if (doAttackHandcast)
        {
            letsAttackHandcast();
        }
        if (doFinishLevel)
        {
            letsFinishLevel();
        }

        //Destroys and Disables
        if (doDisableItself)
        {
            letsDisableItself();
        }
        if (doDestroyItself)
        {
            letsDestroyItself();
        }
    }
    private void DoTriggerExit()
    {
        stopVisionEyeCast();
        stopAttackHandcasst();
    }


    bool ActualDeserves = false;
    public bool beingHitByPlayer = false;
    private void OnTriggerEnter(Collider other)
    {

        // https://docs.unity3d.com/ScriptReference/Transform.IsChildOf.html
        // Ignore trigger events if between this collider and colliders in children
        // Eg. when you have a complex character with multiple triggers colliders.
        //if (other.transform.IsChildOf(transform))
        //{
        //    //ActualDeserves = false;
        //    //return;
        //}
        Debug.Log("Touched By " + other.name + " ColliderObject");
        WhoIsHitting = other.gameObject;
        if(WhoIsHitting) ParentOfHitting = WhoIsHitting.transform.parent.transform.parent.gameObject; 
        //we have made collection of shapes GameObjects in ShapeList GameObject
        // that this ShapeList contains them 3.
        //therefore now the ItemEffect must reffer
        //GRANDPARENT of this shape Who Is Hitting. Because
        // A.   SHanpe <- and parent again!
        //      1. ShapeLists: <- go to parent
        //          - Sphere        -
        //          - Cube           |<- These were hit
        //          - Tetrahedron   -
        if (ParentOfHitting) player = ParentOfHitting.GetComponent<SHanpe>();
        if (player)
        {
            if (player.gameObject == ParentOfHitting)
            {
                if (Deserves)
                {
                    beingHitByPlayer = true;
                    Debug.Log("DoTrigger");
                    DoTriggerEnter();
                }
            }
        }

        //if (other && other.transform.parent.gameObject.CompareTag("Player"))
        //{
        //    WhoIsHitting = other.gameObject;
        //    if (WhoIsHitting && WhoIsHitting.transform.parent.gameObject.CompareTag("Player"))
        //    {
        //        if(WhoIsHitting.transform.parent.gameObject && WhoIsHitting.transform.parent.gameObject.CompareTag("Player")) ParentOfHitting = WhoIsHitting.transform.parent.gameObject;
        //        if (ParentOfHitting && ParentOfHitting.GetComponent<SHanpe>())
        //        {
        //            player = ParentOfHitting.GetComponent<SHanpe>();
        //        }
        //    }
        //}

        //if (WhoIsHitting.transform.parent && WhoIsHitting.transform.parent.gameObject.CompareTag("Player"))
        //{
        //    ParentOfHitting = WhoIsHitting.transform.parent.gameObject;
        //}
        //if (ParentOfHitting)
        //{
        //    player = ParentOfHitting.GetComponent<SHanpe>();
        //}
        //if (player)
        //{
        //    if(player.gameObject == ParentOfHitting)
        //    {
        //        if (Deserves) ActualDeserves = true;
        //        else if(!Deserves) ActualDeserves = false;
        //        if (Deserves)
        //        {
        //            beingHitByPlayer = true;
        //            Debug.Log("DoTrigger");
        //            DoTriggerEnter();
        //        }
        //    }
        //}

        //if((ParentOfHitting && player) && ParentOfHitting == player.gameObject)
        //{
        //    DoTriggerEnter();
        //}
    }
    private void OnTriggerStay(Collider other)
    {
        if (player)
        {
            if (doEmitKaerlevWave)
            {
                if(PersistentKaerleEmission) letsEmitKaerlevWave();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (player)
        {
            DoTriggerExit();
            beingHitByPlayer = false;
            Deserves = false;
            WhoIsHitting = null;
            ParentOfHitting = null;
            player = null;
        }

        player = null;
    }

    
}
