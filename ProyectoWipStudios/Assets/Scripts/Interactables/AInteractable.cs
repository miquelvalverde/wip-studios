using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AInteractable : MonoBehaviour
{
    
    public virtual bool CanInteract()
    {
        return true;
    }

    public virtual void StartInteract()
    {
        Debug.LogWarning("Start Interact of " + this.name + " is not implemented.");
    }

    public virtual void EndInteract()
    {
        Debug.LogWarning("End Interact of " + this.name + " is not implemented.");
    }

}
