/*
    Team    : Speaking Potato
    Author  : Haewon Shon
    Date    : 11/15/2021
    Desc    : To distinguish shield component from health - mimic HealthBar
*/

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStatus))]
public class ShieldBar : MonoBehaviour
{
    public bool showShieldBar = true;

    //[Header("Position")]
    //public Vector2 position = new Vector2(0, 1.5f);

    //[Header("Size of HealthBar")]
    //public Vector2 size = new Vector2(1.5f, 0.2f);
    //public float scale = 1.0f;
    //public float innerRatio = 0.2f;

    //[Header("Sprite")]
    //public Sprite spriteSource = null;
    //public Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1);
    //public Color foregroundColor = new Color(0.8f, 0.8f, 0.8f, 1);

    [Header("Animation Effect")]
    public bool activeAnimation = true;
    public Color middelColor = new Color(1, 0.3f, 0.3f, 1);
    public float speed = 10;

    [Header("Canvas")]
    [Tooltip("Only player health need this")]
    public Transform UICanvas = null;

    protected Image foregroundImage = null;
    protected HealthBarAnimation anim = null;
    protected float maxShield = 1;
    protected RectTransform holder = null;
    public string holderName = "Shield Bar";

    // temp for m3
    public bool rotate = false;
    private Quaternion rot;

    // Reset health to full
    public void ResetShield()
    {
        if (showShieldBar)
        {
            foregroundImage.fillAmount = 1;
            if (activeAnimation)
                anim.Initialize((int)maxShield);
        }
    }

    // Set current health for health bar effect
    public void UpdateCurrentShield(int curr_shield)
    {
        if (showShieldBar)
        {
            foregroundImage.fillAmount = (float)curr_shield / maxShield;
            if (activeAnimation)
                anim.StartAnimation(curr_shield);
        }

    }

    // Check if the effect has been finished
    public bool IsAnimationFinished()
    {
        if (activeAnimation)
            return anim.isAnimFin;
        return true;
    }

    // Set UI to draw healthbar
    // If UI doesn't exist for this object, create one
    protected virtual void InitUICanvas()
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
    public void Initialize(int max_shield)
    {

        if (showShieldBar)
        {
            InitUICanvas();
            maxShield = (float)max_shield;
            if (activeAnimation)
            {
                if (anim == null)
                    anim = new HealthBarAnimation();
                anim.isActive = activeAnimation;
                anim.Initialize(max_shield);
                anim.SetAnimationSpeed(speed);
            }

            if (holder == null)
            {
                holder = UICanvas.Find(holderName).GetComponent<RectTransform>();
                //Debug.Log("holderName - " + holder.name);
            }
            foregroundImage = holder.Find("foreground").GetComponent<Image>();
            if (activeAnimation)
            {
                anim.middleImage = holder.Find("middle").GetComponent<Image>();
            }
        }
    }

    // Delete created ui and health bar
    // This function is used for custom Editor
    public virtual void RemoveUIandShieldBar()
    {
        if (UICanvas)
        {
            DestroyImmediate(UICanvas);
            UICanvas = null;
        }
    }

    // Update health bar animation
    public void UpdateShieldBar()
    {
        if (activeAnimation)
        {
            anim.UpdateAnimation();
        }
    }
}