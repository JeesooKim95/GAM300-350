/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 10/25/2021
    Desc    : Specialized Health bar class for player-only. Based on HealthBar.cs
*/

// Haewon, 11/15/2021 - Deprecated 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerHealthBar : HealthBar
{
    private Image shieldImage = null;
    private float maxShield = 1;
    private HealthBarAnimation shieldAnim = null;


    // Set current health/shield for health/shield bar effect
    public void UpdateCurrentShield(int curr_shield)
    {
        if (showHealthBar)
        {
            shieldImage.fillAmount = (float)curr_shield / maxShield;
            if (activeAnimation)
                shieldAnim.StartAnimation(curr_shield);
        }
    }

    protected override void InitUICanvas()
    {
        //Debug.Log("if (gameObject.tag == Player)2");
        UICanvas = GameObject.Find("UI").GetComponentInChildren<RectTransform>();
        if (UICanvas == null)
        {
            //Debug.Log("if (UICanvas == null)");
            UICanvas = GameObject.Find("Canvas").transform;
        }
    }

    // Initialize health bar with max health of this object
    public void Initialize(int max_health, int max_shield)
    {
        if (showHealthBar)
        {
            InitUICanvas();
            maxHealth = (float)max_health;
            maxShield = (float)max_shield;
            if (activeAnimation)
            {
                if (anim == null)
                {
                    anim = new HealthBarAnimation();
                }
                if (shieldAnim == null)
                {
                    shieldAnim = new HealthBarAnimation();
                }
                anim.isActive = activeAnimation;
                anim.Initialize(max_health);
                anim.SetAnimationSpeed(speed);
                shieldAnim.isActive = activeAnimation;
                shieldAnim.Initialize(max_shield);
                shieldAnim.SetAnimationSpeed(speed*10000);
            }

             // if health bar exist 
            {
                if (holder == null)
                    holder = UICanvas.Find(holderName).GetComponent<RectTransform>();
                shieldImage = holder.Find("shield").GetComponent<Image>();
                foregroundImage = holder.Find("foreground").GetComponent<Image>();
                anim.middleImage = holder.Find("middle").GetComponent<Image>();
                shieldAnim.middleImage = holder.Find("shieldMiddle").GetComponent<Image>();
            }
        }
    }

    // Delete created ui and health bar
    // This function is used for custom Editor
    public override void RemoveUIandHealthBar()
    {
        if (UICanvas && UICanvas.Find(holderName))
        {
            DestroyImmediate(UICanvas.Find(holderName).gameObject);
            holder = null;
        }
    }
}
