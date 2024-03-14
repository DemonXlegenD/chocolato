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

    private GameManager gameManager;
    // Start is called before the first frame update

    private void Start()
    {
        gameManager = GameManager.Instance;  
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

    }

    public void Quit()
    {
        gameManager.Quit();
    }
}
