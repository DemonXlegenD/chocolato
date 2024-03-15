using Nova;
using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class LoadSkill : MonoBehaviour
{

    [SerializeField] private UIBlock2D fill;
    [SerializeField] private Slider Slider;

    [SerializeField] private bool currentChocolate;
    // Start is called before the first frame update
    void Start()
    {
        if (currentChocolate)
        {
            fill.RadialFill.FillAngle = 0;
        }
        else
        {
            fill.RadialFill.FillAngle = 360;
        }
        fill.RadialFill.Rotation = -90;
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentChocolate && fill.RadialFill.FillAngle != 0)
        {
            if (fill.RadialFill.FillAngle <= 0)
            {
                fill.RadialFill.FillAngle = 0;
            }
            if (fill.RadialFill.FillAngle > 0)
            {
                fill.RadialFill.FillAngle -= Time.deltaTime * 10f;
            }
        }
    }

    public void UpdateFillAngle()
    {
        float percentage = Slider.Value;
        percentage = Mathf.Clamp(percentage, 0f, 100f);

        fill.RadialFill.FillAngle = 360 - percentage * 3.6f;

    }
}
