using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This Class and its instance is MainMenu and PauseMenu hybrid

public class MainMenuing : MonoBehaviour
{
    [Header("Acts In")]
    public Button PlayButton;
    public Button[] ExtraButtons = new Button[5];

    [Header("Text Mesh Pros / Just Texts")]
    [SerializeField] TextMeshProUGUI PlayTMP;
    [SerializeField] Text[] ExtraTMP = new Text[5];

    public LevelSelector SelectTheLevelMenu;
    

    public enum MenuingType { MainMenu, PauseMenu};
    public MenuingType SelectMenuingType;

    private void Awake()
    {
        if(PlayButton && !PlayTMP) PlayTMP = PlayButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        for(int i = 0; i < 5; i++)
        {
            if (ExtraButtons[i] && !ExtraTMP[i]) ExtraTMP[i] = ExtraButtons[i].gameObject.GetComponentInChildren<Text>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /**/ if(SelectMenuingType == MenuingType.MainMenu) //The Bisquit equalises the placings
        {
            PlayTMP.text = "Play";
            ExtraTMP[4].text = "Quit";
        }
        else if(SelectMenuingType == MenuingType.PauseMenu)
        {
            PlayTMP.text = "Resume";
            ExtraTMP[4].text = "Main Menu";
        }
    }
}
