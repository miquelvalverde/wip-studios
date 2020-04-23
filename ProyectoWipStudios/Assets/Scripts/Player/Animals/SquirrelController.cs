using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : PlayerSpecificController
{

    private bool glideInput;

    [SerializeField] private float maxVerticalSpeedGlide = 2f;
    [SerializeField] private float climbSpeed = 10f;
    [SerializeField] private float climbRadius = .5f;

    private TreeInteractable currentTree = null;
    private Vector3 currentClimbPosition = Vector3.zero;
    private Vector3 nextClimbPosition = Vector3.zero;
    private bool endedTree = false;

    public override void Initializate(InputSystem controls)
    {
        controls.Player.Glide.performed += _ => glideInput = true;
        controls.Player.Glide.canceled += _ => glideInput = false;

        controls.Player.Climb.performed += _ => Climb();
    }

    public override void UpdateSpecificAction()
    {
        if (glideInput && this.playerController.groundDistance > 1f && this.playerController.movementVector.y < 0)
        {
            this.playerController.ChangeMaxVerticalSpeed(maxVerticalSpeedGlide);
        }
        else if (!glideInput)
            this.playerController.ResetMaxVerticalSpeed();

        if (currentTree && this.playerController.transform.position != currentClimbPosition)
        {
            this.playerController.transform.position = Vector3.Lerp(this.playerController.transform.position, currentClimbPosition, climbSpeed * Time.deltaTime);
        }
        
        if (endedTree && Vector3.Distance(this.playerController.transform.position, currentClimbPosition) < .1f)
        {
            endedTree = false;
            currentTree = null;
            playerController.canNormalMove = true;
        }
    }

    private void Climb()
    {
        if (!currentTree)
        {
            Collider[] colliders = Physics.OverlapSphere(playerController.transform.position + (playerController.transform.forward * .6f), climbRadius);
            foreach(Collider c in colliders)
            {
                if (c.GetComponent<TreeInteractable>())
                {
                    currentTree = c.GetComponent<TreeInteractable>();
                    this.playerController.transform.forward = currentTree.GetForward(this.playerController.transform);
                    nextClimbPosition = currentTree.GetNextPosition(playerController.transform);

                    if(nextClimbPosition.y < this.playerController.transform.position.y)
                    {
                        currentTree.ResetTree();
                        currentTree = null;
                        return;
                    }

                    continue;
                }
            }
        }

        if (currentTree)
        {
            playerController.canNormalMove = false;

            try
            {
                currentClimbPosition = nextClimbPosition;
                nextClimbPosition = currentTree.GetNextPosition(playerController.transform);
            }
            catch
            {
                endedTree = true;
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(((playerController) ? playerController.transform.position : transform.position) + (((playerController) ? playerController.transform.forward : transform.forward) * .6f), climbRadius);

    }

}
