using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kejar : MonoBehaviour
{
    [SerializeField] private NavMeshAgent me;
    [SerializeField] private Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<NavMeshAgent>();


    }

    // Update is called once per frame
    void Update()
    {
        if(Target) me.SetDestination(Target.position);
    }
}
