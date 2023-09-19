/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 11/16/2021
    Desc    : Heart enemy animation
*/

public class HeartAnim : EnemyAnimBase
{
    protected override void Start()
    {
        base.Start();
    }

    public void EndAttack()
    {
        base.animator.SetTrigger("EndAttack");
    }
}
