using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanelPause : MonoBehaviour
{
    [SerializeField] private PanelType type;

    private MenuPause menuPause;


    private void Start()
    {
        menuPause = FindObjectOfType<MenuPause>();
    }

    public void OnClick()
    {
        if (menuPause != null) { menuPause.OpenPanel(type); }
    }


}
