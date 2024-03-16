using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingLine : MonoBehaviour
{
    // Start is called before the first frame update
    private UIBlock2D[] uiblock2ds;
    void Start()
    {
        uiblock2ds = GetComponentsInChildren<UIBlock2D>();
        for (int i = 0; i < uiblock2ds.Length; i++)
        {
            uiblock2ds[i].transform.localScale = Vector3.zero;
        }
    }

    public void StartAnimation()
    {
        StartCoroutine(TraitementDonnees());
    }
    IEnumerator TraitementDonnees()
    {
        // Parcours de tous les éléments du tableau
        for (int i = 0; i < uiblock2ds.Length; i++)
        {
            // Traitement de chaque donnée
            StartCoroutine(Animation(uiblock2ds[i]));

            // Attendre 1 seconde avant de traiter la prochaine donnée
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator EndTHis()
    {
        // Parcours de tous les éléments du tableau
        for (int i = 0; i < uiblock2ds.Length; i++)
        {
            // Traitement de chaque donnée
            StartCoroutine(EndAnimationFinal(uiblock2ds[i]));

            // Attendre 1 seconde avant de traiter la prochaine donnée
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator Animation(UIBlock2D uiblock2d)
    {
        Vector3 scale = uiblock2d.transform.localScale;
        Quaternion rotation = uiblock2d.transform.rotation;
        for (int i = 0; i < 100; i++)
        {
            scale -= new Vector3(0.01f, 0.01f, 0.01f);
            rotation *= Quaternion.Euler(0, 0, 3.6f);
            // Traitement de chaque donnée
            uiblock2d.transform.localScale = scale;
            uiblock2d.transform.rotation = rotation;

            // Attendre 1 seconde avant de traiter la prochaine donnée
            yield return new WaitForSeconds(0.01f);
        }
    }
    private IEnumerator EndAnimationFinal(UIBlock2D uiblock2d)
    {
        Vector3 scale = uiblock2d.transform.localScale;
        Quaternion rotation = uiblock2d.transform.rotation;
        for (int i = 0; i < 100; i++)
        {
            scale += new Vector3(0.01f, 0.01f, 0.01f);
            rotation *=  Quaternion.Euler(0, 0, -3.6f);
            uiblock2d.transform.localScale = scale;
            uiblock2d.transform.rotation = rotation;

            // Attendre 1 seconde avant de traiter la prochaine donnée
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void EndAnimation()
    {
        StartCoroutine(EndTHis());
    }
}
