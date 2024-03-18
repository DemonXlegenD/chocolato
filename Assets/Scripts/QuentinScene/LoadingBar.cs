using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingBar : MonoBehaviour
{

    GameManager manager;
    [SerializeField] private List<LoadingLine> lines = new List<LoadingLine>();
    public LoadingCoockies coockies;

    void Awake()
    {
        manager = GameManager.Instance;
    }

    private void Start()
    {
        coockies.SetVisible(false);
    }

    public void StartAsClose()
    {
        manager._state = GameState.IsPlaying;
        coockies.SetVisible(false);
        foreach (var line in lines)
        {
            line.StartAsClose();
        }
    }
    public void StartAsOpen()
    {
        manager._state = GameState.IsLoading;
        coockies.SetVisible(true);
        foreach (var line in lines)
        {
            line.StartAsOpen();
        }
    }

    public void LoadLevel(string scene)
    {
        coockies.SetVisible(false);
        foreach (var line in lines)
        {
            line.StartAsClose();
        }
        StartCoroutine(LoadAsync(scene));   
    }

    IEnumerator LoadAsync(string scene)
    {
        manager._state = GameState.IsLoading;

        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].BeginLoading();
            yield return new WaitForSeconds(0.1f);
        }

        coockies.SetVisible(true);

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadSceneAsync(scene);
        
    }

    public void StopLoading()
    {
        StartCoroutine(StopLoadingCoroutine());
    }

    IEnumerator StopLoadingCoroutine()
    {
        yield return new WaitForSeconds(2.5f);

        coockies.SetVisible(false);
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].EndLoading();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        manager._state = GameState.IsPlaying;

    }

}
