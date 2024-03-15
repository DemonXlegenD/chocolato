using Nova;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public const uint MousePointerControlID = 1;
    public const uint ScrollWheelControlID = 2;

    public bool InvertScrolling = true; // Inverser le défilement si vrai

    private void Update()
    {
        // Obtenir le rayon du monde actuel de la caméra
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Obtenir l'entrée de défilement de la souris
        Vector2 mouseScrollDelta = Input.mouseScrollDelta;

        // Vérifier s'il y a un défilement avec la souris cette frame
        if (mouseScrollDelta != Vector2.zero)
        {
            // Inverser le défilement si nécessaire
            if (InvertScrolling)
            {
                mouseScrollDelta.y *= -1f;
            }

            // Créer une nouvelle mise à jour d'interaction pour le défilement
            Interaction.Update scrollInteraction = new Interaction.Update(cameraRay, ScrollWheelControlID);

            // Envoyer la mise à jour de défilement et l'entrée de défilement aux APIs d'interaction de Nova
            Interaction.Scroll(scrollInteraction, mouseScrollDelta);
        }

        // Obtenir l'état du bouton principal de la souris
        bool leftMouseButtonDown = Input.GetMouseButton(0);

        // Créer une nouvelle mise à jour d'interaction pour le pointeur de la souris
        Interaction.Update mousePointerInteraction = new Interaction.Update(cameraRay, MousePointerControlID);

        // Envoyer la mise à jour du pointeur et l'état du bouton de la souris aux APIs d'interaction de Nova
        Interaction.Point(mousePointerInteraction, leftMouseButtonDown);

        /* // Vérifier si une manette est branchée
         if (Input.GetJoystickNames().Length > 0)
         {
             // Obtenir l'entrée d'axe vertical de la manette
             float joystickVerticalInput = Input.GetAxis("JoystickVertical");

             // Vérifier s'il y a un défilement avec la manette cette frame
             if (!Mathf.Approximately(joystickVerticalInput, 0))
             {
                 // Inverser le défilement si nécessaire
                 if (InvertScrolling)
                 {
                     joystickVerticalInput *= -1f;
                 }

                 // Créer une nouvelle mise à jour d'interaction pour le défilement avec la manette
                 Interaction.Update joystickScrollInteraction = new Interaction.Update(cameraRay, ScrollWheelControlID);

                 // Envoyer la mise à jour de défilement avec la manette et l'entrée de défilement aux APIs d'interaction de Nova
                 Interaction.Scroll(joystickScrollInteraction, new Vector2(0, joystickVerticalInput));
             }

             // Obtenir l'état du bouton principal de la manette
             bool joystickPrimaryButtonDown = Input.GetButton("JoystickButton1");

             // Créer une nouvelle mise à jour d'interaction pour le pointeur de la manette
             Interaction.Update joystickPointerInteraction = new Interaction.Update(cameraRay, MousePointerControlID);

             // Envoyer la mise à jour du pointeur avec la manette et l'état du bouton de la manette aux APIs d'interaction de Nova
             Interaction.Point(joystickPointerInteraction, joystickPrimaryButtonDown);
         }*/
    }
}