using Nova;
using NovaSamples.UIControls;
using UnityEngine;
using UnityEngine.Events;

public class LoadSkill : MonoBehaviour
{

    [SerializeField] PlayerController controller;

    [SerializeField] private UIBlock2D BackgroundUI;
    [SerializeField] private UIBlock2D Drag;
    [SerializeField] private UIBlock2D fill;
    [SerializeField] private Slider Slider;

    public bool currentChocolate;

    [SerializeField] private UnityEvent OnLoad;

    public float totalTime = 5f;
    private float currentTime = 0f;
    private float startAngle = 0f;
    private float endAngle = 360f;

    void Start()
    {
        if (currentChocolate)
        {
            fill.RadialFill.FillAngle = 0f;
            Background();
        }
        else
        {
            fill.RadialFill.FillAngle = 360;
            ForeGround();
        }
        fill.RadialFill.Rotation = -90;
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentChocolate)
        {
            if (currentTime < totalTime)
            {
                currentTime += Time.deltaTime;
                float fillAmount = currentTime / totalTime;
                float currentAngle = Mathf.Lerp(startAngle, endAngle, fillAmount);
                fill.RadialFill.FillAngle = 360f - currentAngle;
            }
            else
            {
                currentTime = totalTime;
                fill.RadialFill.FillAngle = 0f;
                OnLoad.Invoke();
            }
        }

    }

    public void Background()
    {
        BackgroundUI.ZIndex = 0;
        fill.ZIndex = 0;
        Drag.ZIndex = 0;
    }

    public void ForeGround()
    {
        BackgroundUI.ZIndex = 1;
        fill.ZIndex = 1;
        Drag.ZIndex = 1;

    }

    public void Reset()
    {
        ForeGround();
        fill.RadialFill.FillAngle = 360f;
        currentTime = 0f;
    }
}
