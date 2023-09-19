/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/17/2021
    Desc    : Following spotlight.
*/
using UnityEngine;

public class FollowingSpotlight : MonoBehaviour
{
    public bool IsBossLight = false;
    public bool IsPlayerLight = false;
    private GameObject target;
    void Start()
    {
        if (IsBossLight)
        {
            target = GameObject.FindGameObjectWithTag("Boss");

        }
        else if (IsPlayerLight)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.transform);
        }
    }
}
