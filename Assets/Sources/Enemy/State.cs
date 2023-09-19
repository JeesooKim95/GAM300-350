/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/05/2021
    Desc    : Base class state used in enemy FSM.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public string name;
    protected EnemyBase enemy;
    
    public virtual void Initialize(GameObject enemyRef, EnemyAnimBase anim)
    {
        enemy = enemyRef.GetComponent<EnemyBase>();
    }
    public abstract void FixedUpdate();
    public abstract void Exit();
    public virtual void OnDrawGizmos() { }
}
