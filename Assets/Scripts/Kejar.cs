using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kejar : MonoBehaviour
{
    [SerializeField] private NavMeshAgent me;
    public Transform Target;
    public SHanpe Focus;
    public Rigidbody rijidbodi;

    public ItemEffects VisionEyecast;
    public ItemEffects AttackHandcast;

    public float Damaging = 5f;
    public float DelayIn = 3f;
    public float AttackDelayTimer = 0f;
    public bool hasBeenAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        AttackDelayTimer = DelayIn;
        hasBeenAttacked = false;
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
                Debug.Log("Player" + Target.name + "found!");
                if (Target && me.isActiveAndEnabled) me.SetDestination(Target.position);
            }
            else
            {

            }
        }
        if (AttackHandcast)
        {
            if (AttackHandcast.isAttackHandcastHit)
            {
                Focus = Target.GetComponent<SHanpe>();
                if (Focus && me.isActiveAndEnabled)
                {
                    
                    Debug.Log("Attacked");
                    if (AttackDelayTimer >= 0f)
                    {
                        AttackDelayTimer -= Time.deltaTime;
                        hasBeenAttacked = false;
                    }
                    else if (AttackDelayTimer < 0f)
                    {
                        if (!hasBeenAttacked)
                        {
                            Focus.DamageMe(Damaging);
                            hasBeenAttacked = true;
                        }
                        AttackDelayTimer = DelayIn;
                    }
                }
            } else
            {
                AttackDelayTimer = DelayIn;
                hasBeenAttacked = false;
            }
        } else
        {
            AttackDelayTimer = DelayIn;
            hasBeenAttacked = false;
        }
    }

    public void ScronchDiriSendiri()
    {
        me.enabled = false;
        rijidbodi.isKinematic = false;
    }

    //idle or wander
}
