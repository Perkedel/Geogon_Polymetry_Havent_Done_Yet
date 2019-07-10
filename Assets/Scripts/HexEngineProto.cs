using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mono.Data.Sqlite;

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

        GameplayHUD.SetActive(false);
        LevelSelectMenu.SetActive(false);
        MainMenuItself.SetActive(true);
        yesNoDialog.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        initDatabaseLoading();
        initMainMenu();

    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go)
            {
                player = go.GetComponent<SHanpe>();
            }
        }

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
                if (OneOfMoreMenuButton) goToMoreMenu(OneOfMoreMenuButton);
            }
            if (Input.GetAxis("Vertical") > .5f)
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
                if (NextActionTimer < 0)
                {
                    DoLevelFinishCountDownTimeout();
                }
            }


        }

        if (isLeveling)
        {
            //Debug.Log("IsLeveling");
            //if (Input.GetAxisRaw("Cancel")>.5)
            //{
            //    if (!PausePress)
            //    {
            //        PressPauseButton();
            //        PausePress = true;
            //    }
            //} else if (Input.GetAxisRaw("Cancel") < .5)
            //{
            //    PausePress = false;
            //}
            if (SimpleInput.GetKeyDown(KeyCode.Escape)) //Escape won't work in editor
            {
                Debug.Log("Escape Straused");
                if (!IsGamePaused)
                {
                    PressPauseButton();
                } else
                {
                    ResumeThisGame();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) Debug.Log("EscapeButton");
    }

    [Header("Core Buttons")]
    public bool Melling;
    public void PressPlayButton()
    {
        if (MainMenuo.SelectMenuingType == MainMenuing.MenuingType.MainMenu)
        {
            toLevelSelect();
        } else
        if (MainMenuo.SelectMenuingType == MainMenuing.MenuingType.PauseMenu)
        {
            ResumeThisGame();
        }
    }
    public void PressQuitButton()
    {
        switch (MainMenuo.SelectMenuingType)
        {
            case MainMenuing.MenuingType.PauseMenu:
                QuitNounSaying = "Level & back to menu";
                break;
            case MainMenuing.MenuingType.MainMenu:
                QuitNounSaying = "Game";
                break;
        }
        WriteYesNo("Are you sure to Quit this" + QuitNounSaying + "?");
        ActionYesNo = YesNoAction.Quito;
        InvokeYesNoDialog(YesNoAction.Quito);
    }
    [SerializeField] string QuitNounSaying = "Game";
    public void ActualQuitButton()
    {
        switch (MainMenuo.SelectMenuingType)
        {
            case MainMenuing.MenuingType.PauseMenu:
                leaveTheLevel();
                break;
            case MainMenuing.MenuingType.MainMenu:
                QuitGame();
                break;
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
        isLeveling = false;
        UnfreezeGame();
        GameHUDManager.EndChangeShape();
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
        isLeveling = true;
        ResetLevelFinishActionCountdown();
        LevelFinished = false;
        CurrentMenuLocation = GameplayHUD;
        MainMenuo.SelectMenuingType = MainMenuing.MenuingType.PauseMenu;
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
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go)
        {
            player = go.GetComponent<SHanpe>();
        }
        setCharacter();
        
    }
    public void PlayThisLevel(string LevelName)
    {
        isLeveling = true;
        ResetLevelFinishActionCountdown();
        LevelFinished = false;
        CurrentMenuLocation = GameplayHUD;
        MainMenuo.SelectMenuingType = MainMenuing.MenuingType.PauseMenu;
        if (player)
        {
            player.IamControllable = true;
            player.CheckPointres = player.transform.position;
            spawnLocation = player.transform.position;
        }
        if (cameraRig) cameraRig.IamControlable = true;
        if (GameplayHUD) GameplayHUD.SetActive(true);
        if (LevelSelectMenu) LevelSelectMenu.SetActive(false);
        CurrentLevelName = LevelName;
        LoadLevel(LevelName, LoadSceneMode.Additive);
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go)
        {
            player = go.GetComponent<SHanpe>();
        }
        setCharacter();
        
    }

    //Pause Resumes
    [Header("Pause Resume")]
    [SerializeField] float previousTimeScale = 1f;
    [SerializeField] bool PausePress = false;
    [SerializeField] bool IsGamePaused = false;
    public void PressPauseButton()
    {
        PauseThisGame();
    }
    public void FreezeGame()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }
    public void UnfreezeGame()
    {
        //Time.timeScale = previousTimeScale;
        Time.timeScale = 1f;
    }
    public void PauseThisGame()
    {
        IsGamePaused = true;
        GameHUDManager.EndChangeShape();
        FreezeGame();
        if (player) player.IamControllable = false;
        if (cameraRig) cameraRig.IamControlable = false;
        
        if (GameplayHUD) GameplayHUD.SetActive(false);
        
        MainMenuo.SelectMenuingType = MainMenuing.MenuingType.PauseMenu;
        isOnMainMenu = true;
        MainMenuItself.SetActive(true);
    }
    public void ResumeThisGame()
    {
        IsGamePaused = false;
        UnfreezeGame();
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
        string WorkLevelName = CurrentLevelName;
        UnloadLevel(WorkLevelName);
        PlayThisLevel(LevelName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit cannot done in editor!");
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
    [Header("Finish Level")]
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
        GameHUDManager.EndChangeShape();
        LevelFinished = true;
        player.IamControllable = false;

        StartLevelFinishActionCountDown();

        ChooseAction = Actioning;
        ChooseFinish = Choosing;
    }

    [SerializeField] ItemEffects.FinishChoice ChooseFinish;
    [SerializeField] ItemEffects.FinishAction ChooseAction;
    [SerializeField] string NextLevelName;
    public string NextLevelName1 { get => NextLevelName; set => NextLevelName = value; }
    void DoLevelFinishCountDownTimeout()
    {
        if (ChooseFinish == ItemEffects.FinishChoice.Completed)
        {

        }
        else
        {

        }
        switch (ChooseAction)
        {
            case ItemEffects.FinishAction.NextLevel:
                NextLevel(NextLevelName);
                break;
            case ItemEffects.FinishAction.RestartLevel:
                RestartLevel();
                break;
            case ItemEffects.FinishAction.MainMenu:
                leaveTheLevel();
                break;
            case ItemEffects.FinishAction.ExitGame:
                QuitGame();
                break;
            default:
                break;
        }
        ResetLevelFinishActionCountdown();
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
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go)
        {
            player = go.GetComponent<SHanpe>();
        }
        if (cameraRig) cameraRig.setTarget();
    }

    [Header("Dialoging")]
    public YesNoDialog yesNoDialog;
    [SerializeField] string YesNoSay;
    public void WriteYesNo(string say)
    {
        YesNoSay = say;
        yesNoDialog.DialogTitle.text = YesNoSay;
    }
    public void invokeYesNoDialog()
    {
        yesNoDialog.gameObject.SetActive(true);
    }
    public void InvokeYesNoDialog(YesNoAction selectthat)
    {
        
        invokeYesNoDialog();
        ActionYesNo = selectthat;
    }
    public enum YesNoAction { Quito=0, Playo=1, Customizeo=2}
    public string AboutToPlayName = "SampleScene";
    public YesNoAction ActionYesNo;
    public void PressYesButton()
    {
        Debug.Log("YES button pressed ");
        switch (ActionYesNo)
        {
            case YesNoAction.Quito:
                Debug.Log("Go quit this game");
                ActualQuitButton();
                break;
            case YesNoAction.Playo:
                Debug.Log("Go play a level");
                PlayThisLevel(AboutToPlayName);
                break;
            case YesNoAction.Customizeo:
                break;
        }
        devokeYesNoDialog();
    }
    public void PressNoButton()
    {
        Debug.Log("NO button pressed");
        devokeYesNoDialog();
    }
    public void devokeYesNoDialog()
    {
        yesNoDialog.gameObject.SetActive(false);
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

    //Databasings
    void FirstTimeDatabasing()
    {
        string conn = "URI=file:" + Application.dataPath + "/SQL/GameSave.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "CREATE TABLE IF NOT EXISTS [Level_status] (ID INTEGER, Name TEXT, Status INTEGER, PRIMARY KEY(ID));"; //TheDean Stack Overflow
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();


        while (reader.Read())
        {

        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    void initDatabaseLoading()
    {
        FirstTimeDatabasing();
        string conn = "URI=file:" + Application.dataPath + "/SQL/GameSave.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT ID, Name, Status " + "FROM Level_Status;";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();


        while (reader.Read())
        {
            int IDs = reader.GetInt32(0);
            string Names = reader.GetString(1);
            int Statuses = reader.GetInt32(2);

            Debug.Log("ID: " + IDs + ", Name: " +Names+ ", CompleteStatus: " + Statuses);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
