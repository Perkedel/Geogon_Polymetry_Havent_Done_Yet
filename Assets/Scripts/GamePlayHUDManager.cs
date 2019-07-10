using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlayHUDManager : MonoBehaviour
{
    public Scrollbar HPbar;
    [Range(0f,100f)] public float HPmeter;
    public newPersonCamera Cameraing;
    public GameObject playerFound;
    public SHanpe player;
    [SerializeField] float DisplayCoineValue = 0f;
    [SerializeField] TextMeshProUGUI CoineText;
    [SerializeField] GameObject DisplayController;
    [SerializeField] GameObject ChangeShapeButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            DisplayController.SetActive(false);
        }
        if (Input.touchCount >= 1)
        {
            DisplayController.SetActive(true);
        }
        //A hat in time http://hatintime.com/ 
        if (player.IamControllable)
        {
            if (!DisplayController.activeSelf)
            {
                if (SimpleInput.GetMouseButtonDown(1))
                {
                    ToggleChangeShape();
                }
            } else
            {
                if (ChangeButtonPress)
                {
                    ToggleChangeShape();
                    ChangeButtonPress = false;
                }
            }
        }
        findPlayer();
        //player = Cameraing.target.gameObject.GetComponent<SHanpe>();
        if (player)
        {
            HPmeter = player.HP;
            SetHPBar(player.HP);
            DisplayCoineValue = player.Coine;
            CoineText.text = DisplayCoineValue.ToString();
        }
    }

    public void SetHPBar(float value)
    {
        HPbar.size = value / 100f;
    }

    public void findPlayer()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go)
        {
            playerFound = go;
        }
        if (playerFound)
        {
            player = playerFound.GetComponent<SHanpe>();
        }
    }

    [SerializeField] GameObject GamingArea, ChangeShapeArea;
    [SerializeField] float previousTimeScale;
    public bool ChangingShape = false;
    public bool ChangeButtonPress = false;
    public void PressChangeShape()
    {
        ChangeButtonPress = true;
    }
    public void ReleaseChangeShape()
    {
        ChangeButtonPress = false;
    }
    public void ToggleChangeShape()
    {
        if (!ChangingShape)
        {
            StartChangeShape();
        }
        else
        {
            EndChangeShape();
        }
    }
    public void StartChangeShape()
    {
        ChangingShape = true;
        GamingArea.SetActive(false);
        ChangeShapeButton.SetActive(false);
        ChangeShapeArea.SetActive(true);
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0.1f;
    }
    public void EndChangeShape()
    {
        ChangingShape = false;
        GamingArea.SetActive(true);
        ChangeShapeButton.SetActive(true);
        ChangeShapeArea.SetActive(false);
        Time.timeScale = 1f;
    }
    public void SelectTheShape(int SelectNumber)
    {
        player.goToShape(SelectNumber);
        EndChangeShape();
    }
    //SelectShape
    public void SelectSphere()
    {
        player.goToShape(0);
        Debug.Log("Change Shape");
        EndChangeShape();
        Debug.Log("Close Shape");
    }
    public void SelectBox()
    {
        player.goToShape(1);
        Debug.Log("Change Shape");
        EndChangeShape();
        Debug.Log("Close Shape");
    }
    public void SelectTetrahedron()
    {
        player.goToShape(2);
        Debug.Log("Change Shape");
        EndChangeShape();
        Debug.Log("Close Shape");
    }

    private void OnEnable()
    {
        //findPlayer();
    }
}
