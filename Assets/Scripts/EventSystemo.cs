using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemo : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] EventSmos = GameObject.FindGameObjectsWithTag("EventSystem");
        if (EventSmos.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
