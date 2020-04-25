using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : PlayerSpecificController
{

    [SerializeField] private float glideSpeed = 2f;
    [SerializeField] private float climbSpeed = 10f;
    [SerializeField] private float climbRadius = .5f;

    private TreeInteractable currentTree = null;
    private Vector3 currentClimbPosition = Vector3.zero;
    private Vector3 nextClimbPosition = Vector3.zero;
    private bool endedTree = false;

    public override void Initializate(InputSystem controls)
    {
        controls.Player.Glide.performed += _ => StartGlide();
        controls.Player.Glide.canceled += _ => StopGlide();

        //controls.Player.Climb.performed += _ => Climb();
    }

    public override void UpdateSpecificAction()
    {
        if (this.playerController.stats.isGliding && this.playerController.stats.isGrounded)
            StopGlide();

        /*if (currentTree && this.playerController.transform.position != currentClimbPosition)
        {
            this.playerController.transform.position = Vector3.Lerp(this.playerController.transform.position, currentClimbPosition, climbSpeed * Time.deltaTime);
        }
        
        if (endedTree && Vector3.Distance(this.playerController.transform.position, currentClimbPosition) < .1f)
        {
            endedTree = false;
            currentTree = null;
            playerController.canNormalMove = true;
        }*/
    }

    private void StartGlide()
    {
        if (this.playerController.stats.isGrounded && this.playerController.stats.velocity.y >= 0)
            return;

        this.playerController.doNormalMovement = false;
        this.playerController.stats.isGliding = true;
        this.playerController.ChangeMaxVerticalSpeed(glideSpeed);
    }

    private void StopGlide()
    {
        if (!this.playerController.stats.isGliding)
            return;

        this.playerController.doNormalMovement = true;
        this.playerController.stats.isGliding = false;
        this.playerController.ResetMaxVerticalSpeed();
    }

    /*private void Climb()
    {
        if (!currentTree)
        {
            Collider[] colliders = Physics.OverlapSphere(playerController.transform.position + (playerController.transform.forward * .6f), climbRadius);
            foreach(Collider c in colliders)
            {
                if (c.GetComponent<TreeInteractable>())
                {
                    currentTree = c.GetComponent<TreeInteractable>();
                    nextClimbPosition = currentTree.GetNextPosition(playerController.transform);

                    if(nextClimbPosition.y < this.playerController.transform.position.y)
                    {
                        currentTree.ResetTree();
                        currentTree = null;
                        return;
                    }

                    this.playerController.transform.forward = currentTree.GetForward(this.playerController.transform);

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
            catch (CannotClimbException)
            {
                endedTree = true;
            }

        }
    }*/

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(((playerController) ? playerController.transform.position : transform.position) + (((playerController) ? playerController.transform.forward : transform.forward) * .6f), climbRadius);

    }*/

}
