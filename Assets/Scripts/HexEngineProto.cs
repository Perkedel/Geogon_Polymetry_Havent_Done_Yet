using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HexEngineProto : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public EventSystem eventSystem;
    public SHanpe player;
    public newPersonCamera cameraRig;
    public HajiyevMusicManager MusicPlayer;
    public Vector3 spawnLocation;

    

    public string WarkSceneName;
    public string CoreSceneName;

    private void Awake()
    {
        if (MainMenuItself) MainMenuo = MainMenuItself.GetComponent<MainMenuing>();
        if (GameplayHUD) GameHUDManager = GameplayHUD.GetComponent<GamePlayHUDManager>();

        GameObject[] HexEngineCores = GameObject.FindGameObjectsWithTag("HexagonEngineCore");

        if (HexEngineCores.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        initMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        //https://youtu.be/FRbRQFpVFxg
        if (eventSystem.currentSelectedGameObject != StoreSelected)
        {
            if (eventSystem.currentSelectedGameObject == null)
            {
                eventSystem.SetSelectedGameObject(StoreSelected);
            }
            else
            {
                StoreSelected = eventSystem.currentSelectedGameObject;
            }
        }

        if (slider)
        {
            if (slider.value >= 1f)
            {
                loadingScreen.SetActive(false);
            }
        }

        if (isOnMainMenu)
        {
            if (Input.GetAxis("Vertical") < -.5f)
            {
                if(OneOfMoreMenuButton) goToMoreMenu(OneOfMoreMenuButton);
            }
            if(Input.GetAxis("Vertical") > .5f)
            {
                if (PlayButton) AwayFromMoreMenu(PlayButton);
            }
        }

        if (player)
        {
            CharacterControlling = player.IamControllable;
        }

        if (cameraRig)
        {
            CameraControlling = cameraRig.IamControlable;
        }

        if (LevelFinished)
        {
            if (!CountDownActionNextStarted)
            {
                ResetLevelFinishActionCountdown();
                StartLevelFinishActionCountDown();
            }
            if (CountDownActionNextStarted)
            {
                NextActionTimer -= Time.deltaTime;
            }

        }
    }

    [Header("Core Buttons")]
    public bool Melling;
    public void PressPlayButton()
    {
        if(MainMenuo.SelectMenuingType == MainMenuing.MenuingType.MainMenu)
        {
            toLevelSelect();
        } else
        if(MainMenuo.SelectMenuingType == MainMenuing.MenuingType.PauseMenu)
        {
            ResumeThisGame();
        }
    }
    public void PressQuitButton()
    {
        if (MainMenuo.SelectMenuingType == MainMenuing.MenuingType.MainMenu)
        {
            QuitGame();
        }
        else
        if (MainMenuo.SelectMenuingType == MainMenuing.MenuingType.PauseMenu)
        {
            leaveTheLevel();
        }
    }

    [Header("More Menu Main Menu")]
    [SerializeField] bool isOnMainMenu = true;
    [SerializeField] bool isOnMoreMenuArea = false;
    [SerializeField] GameObject CurrentMenuLocation;
    public GameObject MainMenuItself;
    public MainMenuing MainMenuo;
    public GameObject PlayButton;
    public GameObject OneOfMoreMenuButton;
    public Animator FocusBarDrawer;
    public void initMainMenu()
    {
        if (player) player.IamControllable = false;
        if (cameraRig) cameraRig.IamControlable = false;
        if (cameraRig) cameraRig.zeroPositionCamera();
    }
    public void goToMoreMenu()
    {
        isOnMoreMenuArea = true;
        FocusBarDrawer.SetBool("isOnMoreMenu", isOnMoreMenuArea);
    }
    public void goToMoreMenu(GameObject chooseWhichMenu)
    {
        isOnMoreMenuArea = true;
        FocusBarDrawer.SetBool("isOnMoreMenu", isOnMoreMenuArea);
        SetStoreSelected(chooseWhichMenu);
    }
    public void AwayFromMoreMenu()
    {
        isOnMoreMenuArea = false;
        FocusBarDrawer.SetBool("isOnMoreMenu", isOnMoreMenuArea);
    }
    public void AwayFromMoreMenu(GameObject insertPlayButtonHere)
    {
        
        isOnMoreMenuArea = false;
        FocusBarDrawer.SetBool("isOnMoreMenu", isOnMoreMenuArea);
        SetStoreSelected(insertPlayButtonHere);
    }
    public void backToMainMenu()
    {
        initMainMenu();
        MainMenuo.SelectMenuingType = MainMenuing.MenuingType.MainMenu;
        isOnMainMenu = true;
        if (CurrentMenuLocation) CurrentMenuLocation.SetActive(false);
        MainMenuItself.SetActive(true);
    }
    public void backToMainMenu(GameObject fromWhere)
    {
        initMainMenu();
        MainMenuo.SelectMenuingType = MainMenuing.MenuingType.MainMenu;
        isOnMainMenu = true;
        fromWhere.SetActive(false);
        MainMenuItself.SetActive(true);
    }
    public void leaveTheLevel()
    {
        if (GameplayHUD) GameplayHUD.SetActive(false);
        UnloadLevel(CurrentLevelName);
        backToMainMenu(GameplayHUD);
    }
    public void awayFromMainMenu()
    {
        MainMenuo.SelectMenuingType = MainMenuing.MenuingType.PauseMenu;
        isOnMainMenu = false;
    }

    [Header("Level Select")]
    public GameObject LevelSelectMenu;
    public void toLevelSelect() // PressPlayButton
    {
        isOnMainMenu = false;
        CurrentMenuLocation = LevelSelectMenu;
        CurrentMenuLocation.SetActive(true);
        MainMenuItself.SetActive(false);
    }

    [Header("Set Store Selected")]
    [SerializeField] GameObject StoreSelected; //https://youtu.be/FRbRQFpVFxg Store button selection
    public void SetStoreSelected(GameObject newThing)
    {
        StoreSelected = newThing;
        eventSystem.SetSelectedGameObject(StoreSelected);
    }

    [Header("Play the Level")]
    public bool isLeveling;
    public string CurrentLevelName;
    public GameObject GameplayHUD;
    [SerializeField] string TestoingLevelName = "SampleScene";
    public void PlayThisLevel()
    {
        LevelFinished = false;
        CurrentMenuLocation = GameplayHUD;
        if (player)
        {
            player.IamControllable = true;
            player.CheckPointres = player.transform.position;
            spawnLocation = player.transform.position;
        }
        if (cameraRig) cameraRig.IamControlable = true;
        if (GameplayHUD) GameplayHUD.SetActive(true);
        if (LevelSelectMenu) LevelSelectMenu.SetActive(false);
        CurrentLevelName = TestoingLevelName;
        LoadLevel(TestoingLevelName, LoadSceneMode.Additive);

        setCharacter();
    }
    public void PlayThisLevel(string LevelName)
    {
        LevelFinished = false;
        CurrentMenuLocation = GameplayHUD;
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go)
        {
            GameObject
        }
        if (player)
        {
            player.IamControllable = true;
            player.CheckPointres = player.transform.position;
            spawnLocation = player.transform.position;
        }
        if (cameraRig) cameraRig.IamControlable = true;
        if (GameplayHUD) GameplayHUD.SetActive(true);
        if(LevelSelectMenu) LevelSelectMenu.SetActive(false);
        CurrentLevelName = LevelName;
        LoadLevel(LevelName, LoadSceneMode.Additive);

        setCharacter();
    }

    public void PauseThisGame()
    {
        if (player) player.IamControllable = false;
        if (cameraRig) cameraRig.IamControlable = false;
        if (GameplayHUD) GameplayHUD.SetActive(false);
        MainMenuo.SelectMenuingType = MainMenuing.MenuingType.PauseMenu;
        isOnMainMenu = true;
        MainMenuItself.SetActive(true);
    }
    public void ResumeThisGame()
    {
        CurrentMenuLocation = GameplayHUD;
        if (player) player.IamControllable = true;
        if (cameraRig) cameraRig.IamControlable = true;
        if (GameplayHUD) GameplayHUD.SetActive(true);
        //MainMenuo.SelectMenuingType = MainMenuing.MenuingType.MainMenu;
        isOnMainMenu = false;
        MainMenuItself.SetActive(false);
    }

    public void RestartLevel()
    {
        string WorkLevelName = CurrentLevelName;
        UnloadLevel(WorkLevelName);
        PlayThisLevel(WorkLevelName);
    }
    public void NextLevel(string LevelName)
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void WarkTheScene() //please refference
    {
        //SceneManager.LoadSceneAsync(WarkSceneName, LoadSceneMode.Additive);
        LoadLevel(WarkSceneName, LoadSceneMode.Additive);
        setCharacter();
    }

    public void UnWarkTheScene()
    {
        UnloadLevel(WarkSceneName);
        //loadingScreen.SetActive(false);
    }

    public void CarkTheScene()
    {
        LoadLevel(WarkSceneName);
        UnloadLevel(CoreSceneName); // why core scene cannot unload? is it that there is gameobjects have been do not destroy on load?!
        setCharacter();
    }

    public void UnCarkTheScene()
    {
        LoadLevel(CoreSceneName);
    }

    //Finish Level
    [SerializeField] bool CountDownActionNextStarted = false;
    public float DoNextActionIn = 5f;
    [SerializeField] float NextActionTimer = 5f;
    [SerializeField] ItemEffects.FinishChoice MemFinishChoice;
    [SerializeField] ItemEffects.FinishAction MemFinishAction;
    [SerializeField] string NextSceneName;
    void StartLevelFinishActionCountDown()
    {
        CountDownActionNextStarted = true;
    }
    void StopLevelFinishActionCountDown()
    {
        CountDownActionNextStarted = false;
    }
    void ResetLevelFinishActionCountdown()
    {
        StopLevelFinishActionCountDown();
        NextActionTimer = DoNextActionIn;
    }
    public void FinishLevel()
    {
        FinishLevel(ItemEffects.FinishChoice.Completed, ItemEffects.FinishAction.MainMenu);
    }
    public void FinishLevel(ItemEffects.FinishChoice Choosing, ItemEffects.FinishAction Actioning)
    {
        LevelFinished = true;
        player.IamControllable = false;
        if (Choosing == ItemEffects.FinishChoice.Completed)
        {
            
        } else
        {

        }
        switch (Actioning)
        {
            case ItemEffects.FinishAction.NextLevel:

                break;
            case ItemEffects.FinishAction.RestartLevel:

                break;
            case ItemEffects.FinishAction.MainMenu:
                backToMainMenu();
                break;
            case ItemEffects.FinishAction.ExitGame:

                break;
            default:
                break;
        }
    }

    [Header("Statusing Controlling")]
    public bool CharacterControlling = false;
    public bool CameraControlling = false;
    public GamePlayHUDManager GameHUDManager;
    public bool LevelFinished = false;

    

    //GetSHanpe
    //public void setCharacter()
    //{
    //    GameObject findShanpe = GameObject.FindGameObjectWithTag("Player");
    //    if (!player)
    //    {
            
    //        if (findShanpe)
    //        {
    //            player = findShanpe.GetComponent<SHanpe>();
    //        }
    //    }
    //    if (cameraRig)
    //    {
    //        cameraRig.setTarget(player.gameObject);
    //    }
    //}
    public void setCharacter()
    {
        if(cameraRig) cameraRig.setTarget();
    }

    //Loading Level
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex,LoadSceneMode.Single));
    }

    public void LoadLevel(string sceneName) //This is JOELwindows7's mod. sometimes you will reffer scene by its name in the build.
    {
        StartCoroutine(LoadAsynchronously(sceneName, LoadSceneMode.Single));
    }

    public void LoadLevel(int sceneIndex, LoadSceneMode modus)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex, modus));
    }

    public void LoadLevel(string sceneName, LoadSceneMode modus) //This is JOELwindows7's mod. sometimes you will reffer scene by its name in the build.
    {
        StartCoroutine(LoadAsynchronously(sceneName, modus));
    }

    public void UnloadLevel(int sceneIndex)
    {
        StartCoroutine(UnLoadAsynchronously(sceneIndex));
    }

    public void UnloadLevel(string sceneName)
    {
        StartCoroutine(UnLoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(int sceneIndex, LoadSceneMode loadSceneModing) //Brackeys Async loading
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneModing);

        if(loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log(progress);

            if(slider) slider.value = progress;
            if(progressText) progressText.text = progress * 100f + "%";

            if (slider)
            {
                if (slider.value >= 50)
                {
                    //HajiyevMusicManager.instance.ForcePause();
                    //MusicPlayer.ForcePause();
                }
                
            }

            yield return null;
        }
    }

    IEnumerator LoadAsynchronously(string sceneName, LoadSceneMode loadSceneModing) //JOELwindows7
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, loadSceneModing);

        if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log(progress);

            if (slider) slider.value = progress;
            if (progressText) progressText.text = progress * 100f + "%";

            if (slider)
            {
                if (slider.value >= 50)
                {
                    //HajiyevMusicManager.instance.ForcePause();
                    //MusicPlayer.ForcePause();
                }

            }

            yield return null;
        }
    }

    IEnumerator UnLoadAsynchronously(int sceneIndex) //based on previous but this unloads
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneIndex);

        if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log(progress);

            if (slider) slider.value = progress;
            if (progressText) progressText.text = progress * 100f + "%";

            if (slider)
            {
                if (slider.value >= 50)
                {
                    //HajiyevMusicManager.instance.ForcePause();
                    //MusicPlayer.ForcePause();
                }

            }

            yield return null;
        }
    }

    IEnumerator UnLoadAsynchronously(string sceneName) //JOELwindows7
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);

        if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log(progress);

            if (slider) slider.value = progress;
            if (progressText) progressText.text = progress * 100f + "%";

            if (slider)
            {
                if (slider.value >= 50)
                {
                    //HajiyevMusicManager.instance.ForcePause();
                    //MusicPlayer.ForcePause();
                }

            }

            yield return null;
        }
    }
}
