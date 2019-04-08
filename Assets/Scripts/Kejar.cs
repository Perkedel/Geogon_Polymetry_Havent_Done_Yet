using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kejar : MonoBehaviour
{
    [SerializeField] private NavMeshAgent me;
    public Transform Target;
    public Rigidbody rijidbodi;

    public ItemEffects VisionEyecast;
    public ItemEffects AttackEyecast;
    // Start is called before the first frame update
    void Start()
    {
        //Please add if trigger colider range hit, it chases
        //if trigger collider attack area hit, it attacks
        me = GetComponent<NavMeshAgent>();
        rijidbodi = GetComponent<Rigidbody>();
        if (!VisionEyecast)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (VisionEyecast)
        {
            if (VisionEyecast.isVisionEyecastHit)
            {

                if (Target && me.isActiveAndEnabled) me.SetDestination(Target.position);
            }
            else
            {

            }
        }
    }

    public void ScronchDiriSendiri()
    {
        me.enabled = false;
        rijidbodi.isKinematic = false;
    }

    //idle or wander
}
