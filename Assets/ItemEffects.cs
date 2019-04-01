using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemEffects : MonoBehaviour
{
    public GameObject WhoIsHitting;
    public GameObject ParentOfHitting;
    public SHanpe player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Header("Functionality")]
    [Header("Single Use")]
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

    [Header("Player hit chase range EyeCast")]
    public bool doVisionEyecast = false;
    public bool isVisionEyecastHit = false;
    public void letsVisionEyeCast()
    {
        isVisionEyecastHit = true;
    }
    public void stopVisionEyeCast()
    {
        isVisionEyecastHit = false;
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

    [Header("Particle")]
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

    [Header("Core of the Core Intinya Inti, Pak Ndul")]
    public bool CoreOfInti = true;
    private void DoTriggerEnter()
    {
        if (doDisableItself)
        {
            letsDisableItself();
        }
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
    }
    private void DoTriggerExit()
    {
        stopVisionEyeCast();
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            WhoIsHitting = other.gameObject;
            if (WhoIsHitting)
            {
                if(WhoIsHitting.transform.parent.gameObject) ParentOfHitting = WhoIsHitting.transform.parent.gameObject;
                if (ParentOfHitting && ParentOfHitting.GetComponent<SHanpe>())
                {
                    player = ParentOfHitting.GetComponent<SHanpe>();
                }
            }
        }
        if((ParentOfHitting && player) && ParentOfHitting == player.gameObject)
        {
            DoTriggerEnter();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        DoTriggerExit();
    }
}
