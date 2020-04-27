using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : PlayerSpecificController
{

    [Header("Glide")]
    [SerializeField] private float glideSpeed = 2f;

    [Header("Climb")]
    [SerializeField] private float climbCheckerVerticalOffset = .5f;
    [SerializeField] private float climbCheckerHorizontalOffset = .5f;
    [SerializeField] private float climbCheckerRadius = .5f;
    [SerializeField] private LayerMask whatIsTree = 0;
    private Vector3 checkerPosition
    {
        get
        {
            return transform.position + (transform.forward * climbCheckerHorizontalOffset) + (transform.up * climbCheckerVerticalOffset);
        }

        set { }
    }

    private TreeInteractable currentTree = null;
    private bool endedTree = false;
    private Vector3 nextClimbPosition = Vector3.zero;

    private bool inputClimb;

    public override void Initializate(InputSystem controls)
    {
        controls.Player.Glide.performed += _ => StartGlide();
        controls.Player.Glide.canceled += _ => StopGlide();

        controls.Player.Climb.performed += _ => inputClimb = true;
        controls.Player.Climb.canceled += _ => inputClimb = false;

        controls.Enable();
    }

    public override void UpdateSpecificAction()
    {
        if (this.playerController.stats.isGliding && this.playerController.groundDistance < 1)
            StopGlide();

        if (!this.playerController.stats.isClimbing && !this.playerController.stats.isGliding && !this.playerController.stats.isGrounded && inputClimb && IsTreeClose())
        {
            GetTree();

            if (!currentTree)
                return;

            this.playerController.stats.isClimbing = true;

            this.playerController.doNormalMovement = false;
            this.playerController.lockRotation = true;
            this.playerController.doGravity = false;
            currentTree.ResetTree();
            GetNextClimbPoint();
            this.playerController.transform.forward = currentTree.GetForward(this.playerController.transform);
        }

        if (this.playerController.stats.isClimbing && Vector3.Distance(transform.position, nextClimbPosition) < .5f)
        {
            try
            {
                if (inputClimb)
                    endedTree = GetNextClimbPoint();
                else if (endedTree)
                    EndClimb();
            }
            catch (CannotClimbException) { EndClimb(); }
        }
    }

    #region Gliding
    private void StartGlide()
    {
        if (this.playerController.stats.isGrounded && this.playerController.stats.velocity.y >= 0 || this.playerController.stats.isClimbing)
            return;

        this.playerController.useMovementInputs = false;
        this.playerController.stats.isGliding = true;
        this.playerController.ChangeMaxVerticalSpeed(glideSpeed);
    }

    private void StopGlide()
    {
        if (!this.playerController.stats.isGliding)
            return;

        this.playerController.useMovementInputs = true;
        this.playerController.stats.isGliding = false;
        this.playerController.ResetMaxVerticalSpeed();
    }
    #endregion

    private bool IsTreeClose()
    {
        return Physics.CheckSphere(checkerPosition, climbCheckerRadius, whatIsTree);
    }

    private void GetTree()
    {
        Collider[] colliders = Physics.OverlapSphere(checkerPosition, climbCheckerRadius, whatIsTree);

        currentTree = colliders[0].GetComponent<TreeInteractable>();

        if (currentTree.GetFirstPoint().y < this.playerController.transform.position.y)
            currentTree = null;

    }

    private bool GetNextClimbPoint()
    {
        nextClimbPosition = currentTree.GetNextPosition(transform);
        this.playerController.alternativeMoveDestination = nextClimbPosition;

        return currentTree.IsLastPoint;
    }

    private void EndClimb()
    {
        endedTree = false;

        this.playerController.stats.isClimbing = false;

        this.playerController.doNormalMovement = true;
        this.playerController.doGravity = true;
        this.playerController.lockRotation = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(checkerPosition, climbCheckerRadius);
    }

}
