using System.Collections;
using UnityEngine;

public class EntitiyAnimator : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Rigidbody2D RB;

    [SerializeField]
    private EnemyBase enemyBase;

    private bool moving;


    private void Start()
    {
        enemyBase.EnemyAttackBaseInstance.OnEnemyAttackStart += AttackAnimation;
        enemyBase.OnBeingAttacked += Hurt;
    }

    private void OnDisable()
    {
        enemyBase.EnemyAttackBaseInstance.OnEnemyAttackStart -= AttackAnimation;
        enemyBase.OnBeingAttacked -= Hurt;
    }

    private void Update()
    {
        Animate();
    }


    private void Animate()
    {
        moving = RB.linearVelocity.magnitude > 0.1 || RB.linearVelocity.magnitude < -0.1;

        if (moving)
        {

            animator.SetFloat("X", RB.linearVelocityX);
            animator.SetFloat("Y", RB.linearVelocityY);
        }

        animator.SetBool("Moving", moving);

    }

    private void AttackAnimation()
    {
      
        if ((RB.rotation >= -45f && RB.rotation < 0) || (RB.rotation >= 0 && RB.rotation < 45f)) {
            animator.SetFloat("X", 0);
            animator.SetFloat("Y", 1);
        }
        else if (RB.rotation >= 45f && RB.rotation < 135f)
        {
            animator.SetFloat("X", 1);
            animator.SetFloat("Y", 0);
  
        }
        else if (RB.rotation >= 135f && RB.rotation <=180 || RB.rotation > -180 && RB.rotation < -135f)
        {
            animator.SetFloat("X", 0);
            animator.SetFloat("Y", -1);
            
 
        }
        else if (RB.rotation >= -135f && RB.rotation < -45f)
        {
            animator.SetFloat("X", -1);
            animator.SetFloat("Y", 0);
        }

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

