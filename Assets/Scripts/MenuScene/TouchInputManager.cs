using Nova;
using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    private void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            // Utilise le système d'entrée hérité de Unity pour obtenir le premier point de contact.
            Touch touch = Input.GetTouch(i);

            // Convertit le point de contact en un rayon dans l'espace du monde.
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            // Crée une nouvelle Interaction à partir du rayon et de l'ID du doigt.
            Interaction.Update interaction = new Interaction.Update(ray, (uint)touch.fingerId);

            // Obtient la phase de contact tactile actuelle.
            TouchPhase touchPhase = touch.phase;

            // Si la phase de contact tactile n'est ni terminée ni annulée, alors pointerDown == true.
            bool pointerDown = touchPhase != TouchPhase.Canceled && touchPhase != TouchPhase.Ended;

            // Alimente la mise à jour et l'état enfoncé aux APIs d'interaction de Nova.
            Interaction.Point(interaction, pointerDown);
        }
    }
}