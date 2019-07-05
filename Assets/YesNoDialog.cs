using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YesNoDialog : MonoBehaviour
{
    public Image PanelImage;
    public TextMeshProUGUI DialogTitle;
    public Button YesButton, NoButton;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
