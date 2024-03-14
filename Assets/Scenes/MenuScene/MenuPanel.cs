using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIBlock2D))]
public class MenuPanel : MonoBehaviour
{
    [SerializeField] private PanelType type;
    private bool state;

    private UIBlock2D canvas;

    private void Awake()
    {
        canvas = GetComponent<UIBlock2D>(); 
    }

    private void UpdateState()
    {
        canvas.enabled = state;
    }

    private void ChangeState()
    {
        state = !state;
        UpdateState();
    }

    private void ChangeState(bool _state)
    {
        state = _state;
        UpdateState();
    }

    #region Getter

    public PanelType GetPanelType() { return type; }    
    public bool GetState() { return state; }    

    #endregion

}
