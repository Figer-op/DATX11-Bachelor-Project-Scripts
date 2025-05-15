using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Rigidbody2D RB;

    [SerializeField]
    private PlayerBase playerBase;

    [SerializeField]
    private PlayerAttack playerAttack;

    private bool moving;

    private void Start()
    {
        playerAttack.OnPlayerAttackStart += AttackAnimation;
        playerBase.OnBeingAttacked += Hurt;
    }

    private void OnDisable()
    {
        playerAttack.OnPlayerAttackStart -= AttackAnimation;
        playerBase.OnBeingAttacked -= Hurt;
    }

    private void Update()
    {
        Animate();
    }


    private void Animate()
    {
        if ((RB.rotation >= -45f && RB.rotation < 0) || (RB.rotation >= 0 && RB.rotation < 45f))
        {
            animator.SetFloat("X", 1);
            animator.SetFloat("Y", 0);
        }
        else if (RB.rotation >= 45f && RB.rotation < 135f)
        {
            animator.SetFloat("X", 0);
            animator.SetFloat("Y", 1);
        }
        else if (RB.rotation >= 135f && RB.rotation <= 180 || RB.rotation > -180 && RB.rotation < -135f)
        {
            animator.SetFloat("X", -1);
            animator.SetFloat("Y", 0);
        }
        else if (RB.rotation >= -135f && RB.rotation < -45f)
        {
            animator.SetFloat("X", 0);
            animator.SetFloat("Y", -1);
        }

        moving = RB.linearVelocity.magnitude > 0.1 || RB.linearVelocity.magnitude < -0.1;

        animator.SetBool("Moving", moving);
    }

    private void AttackAnimation()
    {
        animator.SetBool("Attacking", true);

        StartCoroutine(DisableAnimationAfterDelay(0.3f, "Attacking"));
    }

    private void Hurt(float _)
    {
        animator.SetBool("Hurt", true);
        StartCoroutine(DisableAnimationAfterDelay(0.3f, "Hurt"));
    }

    IEnumerator DisableAnimationAfterDelay(float delay, string animation)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(animation, false);
    }
}
