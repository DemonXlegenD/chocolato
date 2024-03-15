using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private Collection<PanelType, MenuPanel> panelsCollection = new Collection<PanelType, MenuPanel>();
    private MenuPanel currentPanel;

    private GameManager gameManager;
    // Start is called before the first frame update

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UIBlock menuBlock;

    private bool IsPause = false;

    private void Start()
    {
        gameManager = GameManager.Instance;
        ClosePanel();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPause) PauseGame();
            else ResumeGame();
        }

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
        playerInput.enabled = false;
        menuBlock.gameObject.SetActive(false);
        OpenPanel(PanelType.Main);
        Time.timeScale = 0f;

    }

    public void ResumeGame()
    {
        IsPause = false;
        playerInput.enabled = true;
        menuBlock.gameObject.SetActive(true);
        ClosePanel();
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        playerInput.enabled = true;
        menuBlock.gameObject.SetActive(true);
        gameManager.ChangeScene("MenuScene");
    }

    public void Quit()
    {
        gameManager.Quit();
    }
}
