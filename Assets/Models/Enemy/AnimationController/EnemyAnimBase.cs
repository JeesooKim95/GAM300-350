/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 11/16/2021
    Desc    : Base class for enemy animation
*/
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimBase : MonoBehaviour
{
    protected Animator animator = null;
    protected virtual void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        
        if(gameObject.GetComponent<EnemyBase>() != null)
            animator.SetBool("IsWalking", true);
    }

    virtual public void OnWalk(bool is_walking)
    {
        if(animator)
            animator.SetBool("IsWalking", is_walking);
    }

    virtual public void SetWalkingSpeed(float speed)
    {
        if (animator)
            animator.SetFloat("WalkingSpeed", speed);
    }

    virtual public void OnAttack()
    {
        if (animator)
            animator.SetTrigger("OnAttack");
    }

    virtual public void SetAttackSpeed(float speed)
    {
        if (animator)
            animator.SetFloat("AttackSpeed", speed);
    }
}
