using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OpenPanel : MonoBehaviour
{
    [SerializeField] private PanelType type;

    private MenuController menuController;  


    private void Start()
    {
        menuController = FindObjectOfType<MenuController>();
    }

    public void OnClick()
    {
        if (menuController != null) { menuController.OpenPanel(type); }
    }
}
