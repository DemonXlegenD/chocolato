using UnityEngine;

public class WeaponsDisplay : MonoBehaviour
{

    [SerializeField] private bool isActive;

    [SerializeField] private LoadSkill white;
    [SerializeField] private LoadSkill dark;
    // Start is called before the first frame update
    void Start()
    {
        SetVisible(isActive);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetVisible(bool state)
    {
        gameObject.SetActive(state);
    }

    public void ChangeDisplay()
    {
        isActive = !isActive;
        SetVisible(isActive);
        if (isActive)
        {
            if (!white.currentChocolate)
            {
                white.Reset();
            } 
            else if (!dark.currentChocolate)
            {
                dark.Reset();
            }
        }

    }
}
