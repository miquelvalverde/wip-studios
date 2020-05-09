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

    public struct AnimationParameters
    {
        public float speed;
        public bool isGrounded;
        public bool IsJumping;
    }

    public void UpdateAnimation()
    {
        if (!anim)
            return;

        anim.SetFloat("Speed", PlayerController.instance.stats.speed, .1f, Time.deltaTime);
        anim.SetBool("IsGrounded", PlayerController.instance.stats.isGrounded);
        anim.SetBool("IsJumping", PlayerController.instance.stats.isJumping);
        anim.SetBool("IsGliding", PlayerController.instance.stats.isGliding);

        PlayerController.instance.stats.isJumping = false;
    }

}
