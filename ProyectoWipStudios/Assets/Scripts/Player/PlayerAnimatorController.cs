using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviourPlayerGettable
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

        anim.SetFloat("Speed", player.stats.speed, .1f, Time.deltaTime);
        anim.SetBool("IsGrounded", player.stats.isGrounded);
        anim.SetBool("IsJumping", player.stats.isJumping);
        anim.SetBool("IsGliding", player.stats.isGliding);
        anim.SetBool("IsClimbing", player.stats.isClimbing);
        anim.SetBool("IsTongue", player.stats.isTongue);

        player.stats.isJumping = false;
    }

}
