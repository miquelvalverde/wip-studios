using UnityEngine;

public abstract class AMonoBehaivourWithInputs : MonoBehaviour
{

    protected InputSystem controls;

    protected bool canDisableInputs = true;

    protected void Awake()
    {
        controls = new InputSystem();
        canDisableInputs = true; //Make sure is true by default
        SetControls();

        if (!canDisableInputs)
            controls.Enable();

    }

    protected virtual void SetControls() { }

    protected virtual void OnEnable()
    {
        this.EnableControls();
    }

    protected virtual void OnDisable()
    {
        this.DisableControls();
    }

    public void EnableControls()
    {
        this.controls.Enable();
    }

    public void DisableControls()
    {
        if (canDisableInputs)
            this.controls.Disable();
    }

}
