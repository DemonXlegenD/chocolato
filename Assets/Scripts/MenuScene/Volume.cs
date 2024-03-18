using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    private GameManager gameManager;
    public Slider volumeSlider; // Référence à la barre de défilement du volume
    public AudioSource audioSource; // Référence à la source audio que vous souhaitez contrôler

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    void Start()
    {
        // Assurez-vous que le volume de la source audio correspond à la valeur initiale de la barre de défilement
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
        // Cette méthode sera appelée chaque fois que la valeur de la barre de défilement change
        audioSource.volume = volumeSlider.Value / 100f;
        gameManager.volume = volumeSlider.Value / 100f;
    }

}
