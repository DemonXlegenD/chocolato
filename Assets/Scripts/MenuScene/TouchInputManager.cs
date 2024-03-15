using Nova;
using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    private void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            // Utilise le syst�me d'entr�e h�rit� de Unity pour obtenir le premier point de contact.
            Touch touch = Input.GetTouch(i);

            // Convertit le point de contact en un rayon dans l'espace du monde.
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            // Cr�e une nouvelle Interaction � partir du rayon et de l'ID du doigt.
            Interaction.Update interaction = new Interaction.Update(ray, (uint)touch.fingerId);

            // Obtient la phase de contact tactile actuelle.
            TouchPhase touchPhase = touch.phase;

            // Si la phase de contact tactile n'est ni termin�e ni annul�e, alors pointerDown == true.
            bool pointerDown = touchPhase != TouchPhase.Canceled && touchPhase != TouchPhase.Ended;

            // Alimente la mise � jour et l'�tat enfonc� aux APIs d'interaction de Nova.
            Interaction.Point(interaction, pointerDown);
        }
    }
}