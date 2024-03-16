using UnityEngine;
using UnityEngine.SceneManagement;
   public enum GameState
    {
        IsPlaying, IsLoading
    }
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

    public GameState _state = GameState.IsPlaying;

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

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
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