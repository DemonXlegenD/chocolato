using Nova;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public const uint MousePointerControlID = 1;
    public const uint ScrollWheelControlID = 2;

    public bool InvertScrolling = true; // Inverser le d�filement si vrai

    private void Update()
    {
        // Obtenir le rayon du monde actuel de la cam�ra
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Obtenir l'entr�e de d�filement de la souris
        Vector2 mouseScrollDelta = Input.mouseScrollDelta;

        // V�rifier s'il y a un d�filement avec la souris cette frame
        if (mouseScrollDelta != Vector2.zero)
        {
            // Inverser le d�filement si n�cessaire
            if (InvertScrolling)
            {
                mouseScrollDelta.y *= -1f;
            }

            // Cr�er une nouvelle mise � jour d'interaction pour le d�filement
            Interaction.Update scrollInteraction = new Interaction.Update(cameraRay, ScrollWheelControlID);

            // Envoyer la mise � jour de d�filement et l'entr�e de d�filement aux APIs d'interaction de Nova
            Interaction.Scroll(scrollInteraction, mouseScrollDelta);
        }

        // Obtenir l'�tat du bouton principal de la souris
        bool leftMouseButtonDown = Input.GetMouseButton(0);

        // Cr�er une nouvelle mise � jour d'interaction pour le pointeur de la souris
        Interaction.Update mousePointerInteraction = new Interaction.Update(cameraRay, MousePointerControlID);

        // Envoyer la mise � jour du pointeur et l'�tat du bouton de la souris aux APIs d'interaction de Nova
        Interaction.Point(mousePointerInteraction, leftMouseButtonDown);

        /* // V�rifier si une manette est branch�e
         if (Input.GetJoystickNames().Length > 0)
         {
             // Obtenir l'entr�e d'axe vertical de la manette
             float joystickVerticalInput = Input.GetAxis("JoystickVertical");

             // V�rifier s'il y a un d�filement avec la manette cette frame
             if (!Mathf.Approximately(joystickVerticalInput, 0))
             {
                 // Inverser le d�filement si n�cessaire
                 if (InvertScrolling)
                 {
                     joystickVerticalInput *= -1f;
                 }

                 // Cr�er une nouvelle mise � jour d'interaction pour le d�filement avec la manette
                 Interaction.Update joystickScrollInteraction = new Interaction.Update(cameraRay, ScrollWheelControlID);

                 // Envoyer la mise � jour de d�filement avec la manette et l'entr�e de d�filement aux APIs d'interaction de Nova
                 Interaction.Scroll(joystickScrollInteraction, new Vector2(0, joystickVerticalInput));
             }

             // Obtenir l'�tat du bouton principal de la manette
             bool joystickPrimaryButtonDown = Input.GetButton("JoystickButton1");

             // Cr�er une nouvelle mise � jour d'interaction pour le pointeur de la manette
             Interaction.Update joystickPointerInteraction = new Interaction.Update(cameraRay, MousePointerControlID);

             // Envoyer la mise � jour du pointeur avec la manette et l'�tat du bouton de la manette aux APIs d'interaction de Nova
             Interaction.Point(joystickPointerInteraction, joystickPrimaryButtonDown);
         }*/
    }
}