using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    private GameManager gameManager;
    public Slider volumeSlider; // R�f�rence � la barre de d�filement du volume
    public AudioSource audioSource; // R�f�rence � la source audio que vous souhaitez contr�ler

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    void Start()
    {
        // Assurez-vous que le volume de la source audio correspond � la valeur initiale de la barre de d�filement
        if(gameManager.volume < 0f)
        {
            gameManager.volume = volumeSlider.Value / 100f;
            audioSource.volume = volumeSlider.Value / 100f;
        } else
        {
            audioSource.volume = gameManager.volume;
            volumeSlider.Value = gameManager.volume * 100;
        }
        
    }

    public void OnVolumeChanged()
    {
        // Cette m�thode sera appel�e chaque fois que la valeur de la barre de d�filement change
        audioSource.volume = volumeSlider.Value / 100f;
        gameManager.volume = volumeSlider.Value / 100f;
    }

}
