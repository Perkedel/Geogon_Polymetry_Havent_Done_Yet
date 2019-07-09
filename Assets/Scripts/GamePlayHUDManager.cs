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

    private void OnEnable()
    {
        //findPlayer();
    }
}
