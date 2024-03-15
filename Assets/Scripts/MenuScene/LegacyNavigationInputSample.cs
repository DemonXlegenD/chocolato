using Nova;
using UnityEngine;

public class LegacyNavigationInputSample : MonoBehaviour
{
    public const uint NavigationID = 3;

    [Tooltip("The starting point from which to begin navigating.")]
    public GestureRecognizer StartFrom = null;
    [Tooltip("The UIBlock to move around as navigation focus changes. Will be created if null.")]
    public UIBlock FocusIndicator = null;

    [Header("Direction Keys")]
    public KeyCode UpKey = KeyCode.UpArrow;
    public KeyCode DownKey = KeyCode.DownArrow;
    public KeyCode LeftKey = KeyCode.LeftArrow;
    public KeyCode RightKey = KeyCode.RightArrow;

    [Header("Action Keys")]
    public KeyCode SelectKey = KeyCode.Return;
    public KeyCode DeselectKey = KeyCode.Escape;

    private void OnEnable()
    {
        if (StartFrom == null)
        {
            // No place to start. Can't begin navigating.
            return;
        }

        EnsureFocusIndicator();

        // Show the focus indicator 
        FocusIndicator.gameObject.SetActive(true);

        // Subscribe to focus change events
        Navigation.OnNavigationFocusChanged += HandleFocusChanged;

        // Begin navigating
        Navigation.Focus(StartFrom.UIBlock, NavigationID);
    }

    private void OnDisable()
    {
        // Unsubscribe from focus change events
        Navigation.OnNavigationFocusChanged -= HandleFocusChanged;

        if (FocusIndicator != null)
        {
            // Hide the focus indicator
            FocusIndicator.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!Navigation.TryGetFocusedUIBlock(NavigationID, out UIBlock focused))
        {
            // Nothing has navigation focus. Unable to navigate.
            return;
        }

        if (Input.GetKeyUp(DownKey))
        {
            Navigation.Move(Vector3.down, NavigationID);
        }
        else if (Input.GetKeyUp(UpKey))
        {
            Navigation.Move(Vector3.up, NavigationID);
        }
        else if (Input.GetKeyUp(LeftKey))
        {
            Navigation.Move(Vector3.left, NavigationID);
        }
        else if (Input.GetKeyUp(RightKey))
        {
            Navigation.Move(Vector3.right, NavigationID);
        }
        else if (Input.GetKeyUp(DeselectKey))
        {
            Navigation.Deselect(NavigationID);
        }
        else if (Input.GetKeyUp(SelectKey))
        {
            Navigation.Select(NavigationID);
        }
    }

    private void HandleFocusChanged(uint controlID, UIBlock focused)
    {
        if (FocusIndicator == null)
        {
            // Don't have an indicator. Nothing to do.
            return;
        }

        // Hide indicator if nothing is focused
        FocusIndicator.gameObject.SetActive(focused != null);

        if (focused == null)
        {
            // Don't have a size/position to move
            return;
        }

        // Match the world scale of the focused element
        Vector3 parentScale = FocusIndicator.transform.parent == null ? Vector3.one : FocusIndicator.transform.parent.lossyScale;
        Vector3 focusedScale = focused.transform.lossyScale;
        FocusIndicator.transform.localScale = new Vector3(focusedScale.x / parentScale.x,
                                                          focusedScale.y / parentScale.y,
                                                          focusedScale.z / parentScale.z);

        // Update size and position to match whatever's focused
        FocusIndicator.Size = focused.CalculatedSize.Value + FocusIndicator.CalculatedPadding.Size;
        FocusIndicator.TrySetWorldPosition(focused.transform.position);
        FocusIndicator.transform.rotation = focused.transform.rotation;
    }

    private void EnsureFocusIndicator()
    {
        if (FocusIndicator != null)
        {
            return;
        }

        // Create a new game object to be our focus indicator
        UIBlock2D indicator = new GameObject("Focus Indicator").AddComponent<UIBlock2D>();

        // Hide the body and enable the border
        indicator.BodyEnabled = false;
        indicator.Border.Enabled = true;
        indicator.Border.Direction = BorderDirection.Out;
        indicator.Border.Width = 5;

        // Add some padding
        indicator.Padding.XY = 2;

        // Make sure the indicator will render over the other content
        SortGroup sortGroup = indicator.gameObject.AddComponent<SortGroup>();
        sortGroup.RenderQueue = 4001;
        sortGroup.RenderOverOpaqueGeometry = true;

        // assign the focus indicator
        FocusIndicator = indicator;
    }
}