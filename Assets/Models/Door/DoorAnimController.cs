/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 10/24/2021
    Desc    : Door animation controller
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
public class DoorAnimController : MonoBehaviour
{
    [Tooltip("Initialize door state")]
    public bool isDoorOpened = false;
    private Animator animator = null;
    private BoxCollider boxContoller = null;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        boxContoller = gameObject.GetComponent<BoxCollider>();

        animator.SetBool("isDoorOpened", isDoorOpened);
    }

    public void OnActiveDoor()
    {
        animator.SetTrigger("activateDoor");
        isDoorOpened = !isDoorOpened;
        boxContoller.isTrigger = isDoorOpened;
    }

    public bool IsDoorOpened()
    {
        return isDoorOpened;
    }
}
