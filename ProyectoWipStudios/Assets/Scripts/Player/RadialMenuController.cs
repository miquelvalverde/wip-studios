using UnityEngine;
using UnityEngine.Events;

public class RadialMenuController : AMonoBehaivourWithInputs
{
    [System.Serializable]
    public struct Portion
    {
        public RadialMenuPortionScriptable portion;
        public UnityEvent callback;
    }

    [SerializeField] private Portion[] portions;
    [SerializeField] private RadialMenuPortion portionPrefabRef;
    [SerializeField] private float iconDistanceFromCenter;
    [SerializeField] private Color unselectedPortionBackground;
    private RadialMenuPortion[] radialMenuPortions;
    private int portionCount;
    private float portionSize360;
    private float portionSize01;
    private int currentSelection;

    private bool IsHoldingToChange;
    private Vector2 pixelsMouse;
    private Vector2 deltaMouse;
    private float tolerance = 2.0F;
    private int previousSelection;

    protected override void SetControls()
    {
        canDisableInputs = false; // On this case its need to be always enabled.
        controls.Player.Change.performed += _ => EnableRadialMenu();
        controls.Player.Change.canceled += _ => DisableRadialMenu();
        controls.UI.MousePosition.performed += ctx => pixelsMouse = ctx.ReadValue<Vector2>();
        controls.UI.MouseDelta.performed += ctx => deltaMouse = ctx.ReadValue<Vector2>();
        controls.UI.MouseDelta.canceled += _ => deltaMouse = Vector2.zero;
        DisableRadialMenu();
    }

    public void Initializate()
    {
        portionCount = portions.Length;
        portionSize360 = 360F / portionCount;
        portionSize01 = portionSize360 / 360F;
        currentSelection = 0;
        radialMenuPortions = CreateWheel();
    }

    private void SubmitSelection()
    {
        if (IsHoldingToChange && currentSelection != -1 && currentSelection != previousSelection)
        {
            SelectPortion(currentSelection);
        }
    }

    private RadialMenuPortion[] CreateWheel()
    {
        var portionInstances = new RadialMenuPortion[portionCount];
        for (int portionIndex = 0; portionIndex < portionCount; portionIndex++)
        {
            var portion = CreatePortion(portionIndex, portions[portionIndex]);
            portionInstances[portionIndex] = portion;
        }
        return portionInstances;
    }

    private RadialMenuPortion CreatePortion(int index,  Portion settings)
    {   
        var portion = Instantiate(portionPrefabRef, this.transform) as RadialMenuPortion;
        portion.icon.sprite = settings.portion.icon;
        portion.iconRect.localPosition = new Vector3(0, iconDistanceFromCenter, 0);
        portion.iconPivot.rotation = Quaternion.Euler(0,0, -portionSize360 / 2);
        portion.background.color = settings.portion.background;
        portion.background.fillAmount = portionSize01;
        portion.backgroundRect.rotation = Quaternion.Euler(0,0, -index * portionSize360);
        portion.callback = settings.callback;
        return portion;
    }

    public void UpdateRadialMenu()
    {
        if (IsHoldingToChange && !PlayerController.instance.IsDoingSomething())
        {
            Time.timeScale = .1f;
            var mouseAngle = GetAngleFromMouseInput(pixelsMouse);

            if (deltaMouse.magnitude > tolerance)
            {
                currentSelection = (int)(mouseAngle / portionSize360);
            }

            HoverPortion(currentSelection);
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void EnableRadialMenu()
    {
        if (PlayerController.instance.IsDoingSomething())
            return;

        PlayerController.instance.DisableInputs();

        previousSelection = currentSelection;
        this.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        IsHoldingToChange = true;
    }

    private void DisableRadialMenu()
    {
        SubmitSelection();

        PlayerController.instance.EnableInputs();

        this.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        IsHoldingToChange = false;
    }

    private void SelectPortion(int selectedPortionIndex)
    {
        radialMenuPortions[selectedPortionIndex].callback.Invoke();
    }

    private void HoverPortion(int selectedPortionIndex)
    {
        for (int portionIndex = 0; portionIndex < portionCount; portionIndex++)
        {
            if(portionIndex == selectedPortionIndex)
            {
                radialMenuPortions[portionIndex].background.color = portions[portionIndex].portion.background;
            }
            else
            {
                radialMenuPortions[portionIndex].background.color = unselectedPortionBackground;
            }
        }
    }

    private float GetAngleFromMouseInput(Vector3 mousePosition)
    {
        var screenHalf = new Vector3(Screen.width / 2, Screen.height / 2);
        var nonNormalizedAngle = Vector3.SignedAngle(Vector3.up, mousePosition - screenHalf, -Vector3.forward);
        var normalizedAngle = (nonNormalizedAngle + 360F) % 360F;
        return normalizedAngle;
    }
}
