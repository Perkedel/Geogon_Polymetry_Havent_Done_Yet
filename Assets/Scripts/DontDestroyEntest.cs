using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyEntest : MonoBehaviour
{
    //Inspired from SimpleInput's Don't Destroy

    private static DontDestroyEntest instance;

    public delegate void UpdateCallback();
    public static event UpdateCallback OnUpdate;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        // Initialize singleton instance
        instance = new GameObject("DontDestroyEntest").AddComponent<DontDestroyEntest>();
        DontDestroyOnLoad(instance.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (this != instance)
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
