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

    public string WarkSceneName;
    public string CoreSceneName;

    private void Awake()
    {

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
    }

    [Header("More Menu Main Menu")]
    [SerializeField] bool isOnMainMenu = true;
    [SerializeField] bool isOnMoreMenuArea = false;
    [SerializeField] GameObject CurrentMenuLocation;
    public GameObject MainMenuItself;
    public GameObject PlayButton;
    public GameObject OneOfMoreMenuButton;
    public Animator FocusBarDrawer;
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
        isOnMainMenu = true;
        if (CurrentMenuLocation) CurrentMenuLocation.SetActive(false);
    }
    public void backToMainMenu(GameObject fromWhere)
    {
        isOnMainMenu = true;
        fromWhere.SetActive(false);
        MainMenuItself.SetActive(true);
    }
    public void awayFromMainMenu()
    {
        isOnMainMenu = false;
    }

    [Header("Level Select")]
    public GameObject LevelSelectMenu;
    public void toLevelSelect()
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
    public GameObject GameplayHUD;
    public void PlayThisLevel()
    {

    }
    public void PlayThisLevel(string LevelName)
    {
        if (GameplayHUD) GameplayHUD.SetActive(true);
        if(LevelSelectMenu) LevelSelectMenu.SetActive(false);
        LoadLevel(LevelName, LoadSceneMode.Additive);
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
