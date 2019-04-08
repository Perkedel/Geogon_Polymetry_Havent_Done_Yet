using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundScript : MonoBehaviour
{
    public AudioSource componentAudio;
    public bool DestroyAfterFinishPlaying = true;

    // Start is called before the first frame update
    void Start()
    {
        componentAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DestroyAfterFinishPlaying)
        {
            if (componentAudio)
            {
                if (!componentAudio.isPlaying)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    
}
