using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeKingAnim : EnemyAnimBase
{
    protected override void Start()
    {
        base.Start();
    }

    public void OnMeleeAttack()
    {
        base.animator.SetTrigger("OnMeleeAttack");
    }


}
