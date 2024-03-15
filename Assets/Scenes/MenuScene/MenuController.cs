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

    private void Start()
    {
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
    }

    public void Quit()
    {
        gameManager.Quit();
    }
}
