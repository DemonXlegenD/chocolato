using Nova;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private Collection<PanelType, MenuPanel> panelsCollection = new Collection<PanelType, MenuPanel>();
    private MenuPanel currentPanel;

    private GameManager gameManager;
    // Start is called before the first frame update

    private LoadingBar loadingBar;

    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;
    private InputActionMap _actionMap;
    [SerializeField] private UIBlock menuBlock;

    [SerializeField] private AudioSource audioSource;

    public bool IsPause = false;

    private void Start()
    {
        loadingBar = FindAnyObjectByType<LoadingBar>();
        loadingBar.StartAsOpen();
        loadingBar.StopLoading();
        gameManager = GameManager.Instance;
        gameManager._state = GameState.IsLoading;
        _playerInput = FindAnyObjectByType<PlayerInput>();
        _inputActions = _playerInput.actions;
        _actionMap = _inputActions.FindActionMap("Menu");
        ClosePanel();
    }


    // Update is called once per frame
    private void Update()
    {
    
    }
    public void StartScene(string _scene)
    {
        gameManager.ChangeScene(_scene);
    }

    public void OpenPanel(PanelType _panelType)
    {
        Debug.Log(_panelType);
        ClosePanel();
        currentPanel = panelsCollection.GetItemBykey(_panelType);
        currentPanel.ChangeState(true);
    }

    public void ClosePanel()
    {
        foreach (MenuPanel _panel in panelsCollection.GetItemsList())
        {
            _panel.ChangeState(false);
        }
    }

    public void PauseGame()
    {
        IsPause = true;
        audioSource.Pause();
        menuBlock.gameObject.SetActive(false);
        OpenPanel(PanelType.Main);
        Time.timeScale = 0f;

    }

    public void ResumeGame()
    {
        IsPause = false;
        audioSource.UnPause();
        menuBlock.gameObject.SetActive(true);
        ClosePanel();
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        gameManager._state = GameState.IsPlaying;
        menuBlock.gameObject.SetActive(true);
        gameManager.ChangeScene("MenuScene");
    }

    public void Quit()
    {
        gameManager.Quit();
    }
}