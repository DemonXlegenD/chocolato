using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadingBar : MonoBehaviour
{

    GameManager manager;
    [SerializeField] private List<LoadingLine> lines = new List<LoadingLine>();
    public LoadingCoockies coockies;
    public bool IsDone;

    public float LoadingTimer = 10;
    // Start is called before the first frame update
    void Awake()
    {
        manager = GameManager.Instance;
        
    }

    private void Start()
    {
        StartLoading();
    }

    public void StartLoading()
    {
        manager._state = GameState.IsLoading;
        StartCoroutine(TraitementDonnees());
        coockies.SetVisible(false);
    }

    IEnumerator TraitementDonnees()
    {

       for (int i = 0; i < lines.Count; i++)
        {
            lines[i].EndAnimation();
            yield return new WaitForSeconds(0.5f);
            if (i == lines.Count - 1)
            {
                yield return new WaitForSeconds(2f);
                coockies.SetVisible(true);
            }
        }
        
        yield return new WaitForSeconds(LoadingTimer);

        coockies.SetVisible(false);
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].StartAnimation();
            yield return new WaitForSeconds(0.5f);
            if (i == lines.Count - 1)
            {
                yield return new WaitForSeconds(3f);
                manager._state = GameState.IsPlaying;
            }
        }

    }
}
