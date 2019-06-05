using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

//[RequireComponent(typeof (GUITexture))] //GUITexture has been removed this Alpha of 3 today, 5 June 2019. Obsolesence is fine but removal will Thanosifies everything!
[RequireComponent(typeof(Image))]
public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // if we have forced a reset ...
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            //... reload the scene
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
}
