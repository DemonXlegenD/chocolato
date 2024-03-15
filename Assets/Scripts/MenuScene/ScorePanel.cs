using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private Collection<string> Scores = new Collection<string>();

    #region Add

    public void AddScore(string _namePlayer, string _score)
    {
        if (Scores.HasItem(_namePlayer))
        {
            Debug.Log("Est-ce vous : " + _namePlayer + " ? ");
        }
        else
        {
            Debug.Log("Player enregistré " + _namePlayer + " ? ");
            Scores.AddItem(_namePlayer, _score);
        }
    }

    #endregion


}
