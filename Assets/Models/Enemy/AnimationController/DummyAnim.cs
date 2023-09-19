/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 03/01/2022
    Desc    : Dummy animation controller
*/

using UnityEngine;

public class DummyAnim : MonoBehaviour
{
    private Animator animator = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPushed()
    {
        if (animator != null)
            animator.SetTrigger("OnPushed");
    }

    public void OnDied()
    {
        if (animator != null)
            animator.SetBool("IsDied", true);
    }
}
