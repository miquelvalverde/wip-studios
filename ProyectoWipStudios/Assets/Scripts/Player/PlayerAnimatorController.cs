using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator anim;

    public void SetAnimator(Animator anim)
    {
        this.anim = anim;
    }

    public void UpdateAnimation(PlayerMovementController.State state)
    {
        Vector3 velocity = state.velocity;
        velocity.y = 0;
        velocity.Normalize();
        anim.SetFloat("Speed", velocity.magnitude);
        anim.SetBool("OnGrounded", state.onGrounded);
        anim.SetBool("OnJump", state.onJump);
    }

}
