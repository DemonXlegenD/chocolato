using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType{
    None,
    Main,
    Score,
    Settings,
}
public class MenuController : MonoBehaviour
{
    [SerializeField] private Collection<PanelType, MenuPanel> panelsCollection = new Collection<PanelType, MenuPanel>();
    private MenuPanel currentPanel;

    private GameManager gameManager;
    // Start is called before the first frame update
    private AudioSource audioSource;

    public AudioClip mainMenuMusic;
    public AudioClip scoreMusic;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
        OpenPanel(PanelType.Main);

    }

    // Update is called once per frame
    void Update()
    {
        
    } 
    public void StartScene(string _scene)
    {
        gameManager.ChangeScene(_scene);
    }

    public void OpenPanel(PanelType _panelType)
    {
        foreach (MenuPanel _panel in panelsCollection.GetItemsList())
        {
            _panel.ChangeState(false);
        }
        currentPanel = panelsCollection.GetItemBykey(_panelType);
        currentPanel.ChangeState(true);
        ChangeMusic();
    }

    public void ChangeMusic()
    {

        if (currentPanel.GetPanelType() == PanelType.Main)
        {
            audioSource.clip = mainMenuMusic;
            if (audioSource.clip != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (currentPanel.GetPanelType() == PanelType.Score)
        {
            audioSource.clip = scoreMusic;
            if (audioSource.clip != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }


    }

    public void Quit()
    {
        gameManager.Quit();
    }
}
