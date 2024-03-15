using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private string previousLoadedScene = null;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        previousLoadedScene = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "MenuScene") Quit();
            else ChangeScene("MenuScene");
        }

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().name == "Game")
        {
            WaveSpawner.GetInstance().Init();
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;   
    }
    public void StartGame()
    {
        Debug.Log("Le jeu commence !");
    }

    public void ChangeScene(string _sceneName)
    {
        previousLoadedScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(_sceneName);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(previousLoadedScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}