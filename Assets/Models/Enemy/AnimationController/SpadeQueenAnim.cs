using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeQueenAnim : EnemyAnimBase
{
    protected override void Start()
    {
        base.Start();
    }

    public void OnMeleeAttack()
    {
        base.animator.SetTrigger("OnMeleeAttack");
    }

    public void OnRangeAttack()
    {
        base.animator.SetTrigger("OnRangeAttack");
    }

    public void OnSummon()
    {
        base.animator.SetTrigger("OnSummon");
    }
}
