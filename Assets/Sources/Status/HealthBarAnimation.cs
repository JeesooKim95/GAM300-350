/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 09/25/2021
    Desc    : Health Bar Aniation Effect
*/
using UnityEngine;
using UnityEngine.UI;

public class HealthBarAnimation
{
    public bool isActive = true;
    public bool isAnimFin = true;

    public Image middleImage = null;

    private float maxHealth = 10;
    private float curr = 10;
    private float target = 10;

    private float speed = 10;
    private float waitingFor = 0.2f;
    private float elapsedTime = 0;
    private bool waiting = false;

    // Set health decrease speed
    public void SetAnimationSpeed(float s)
    {
        speed = s;
    }

    // Initialize health bar with max health of this object
    public void Initialize(int max_health)
    {
        maxHealth = curr = target = (float)max_health;
        isAnimFin = true;
        elapsedTime = 0;
        waiting = false;
        if (middleImage)
            middleImage.fillAmount = 1;
    }

    // Start animation effect
    public void StartAnimation(int curr_health)
    {
        if(isActive)
        {
            if (target > 0)
            {
                isAnimFin = false;
                curr = target;
                target = curr_health;
                elapsedTime = 0;
                waiting = true;
                middleImage.fillAmount = curr / maxHealth;
            }
            else
            { 
                isAnimFin = true;
                middleImage.fillAmount = 0;
            }
        }
    }

    // Update animation
    public void UpdateAnimation()
    {
        if (isActive && curr > target)
        {
            // wait for certain time
            if (waiting == true)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= waitingFor)
                    waiting = false;
            }
            else // animate after certain amount of time has elapsed
            {
                curr -= Time.deltaTime * speed;
                if (curr < target)
                {
                    curr = target;
                    isAnimFin = true;
                }
                middleImage.fillAmount = curr / maxHealth;
            }
        }
    }

    public void SetZero()
    {
        middleImage.fillAmount = 0;
    }
}
