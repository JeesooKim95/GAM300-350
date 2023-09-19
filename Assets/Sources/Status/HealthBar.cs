/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 09/25/2021
    Desc    : Health Bar
*/
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Status))]
public class HealthBar : MonoBehaviour
{
    public bool showHealthBar = true;

    [Header("Position")]
    public Vector2 position = new Vector2(0, 1.5f);

    [Header("Size of HealthBar")]
    public Vector2 size = new Vector2(1.5f, 0.2f);
    public float scale = 1.0f;
    public float innerRatio = 0.2f;

    [Header("Sprite")]
    public Sprite spriteSource = null;
    public Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1);
    public Color foregroundColor = new Color(0.8f, 0.8f, 0.8f, 1);

    [Header("Animation Effect")]
    public bool activeAnimation = true;
    public Color middelColor = new Color(1, 0.3f, 0.3f, 1);
    public float speed = 10;

    [Header("Canvas")]
    [Tooltip("Only player health need this")]
    public Transform UICanvas = null;

    protected Image foregroundImage = null;
    protected HealthBarAnimation anim = null;
    protected float maxHealth = 1;
    protected RectTransform holder = null;
    public string holderName = "Health Bar";

    // temp for tutorial dummy
    public bool rotateHealthBar = false;
    private Quaternion rot;

    // For boss shield 
    private GameObject shieldBar = null;

    // Reset health to full
    public void ResetHealth()
    {
        if (showHealthBar)
        {
            foregroundImage.fillAmount = 1;
            if (activeAnimation)
                anim.Initialize((int)maxHealth);
        }
    }

    // Set current health for health bar effect
    public void UpdateCurrentHealth(int curr_health)
    {
        if (showHealthBar)
        {
            foregroundImage.fillAmount = (float)curr_health / maxHealth;
            if (activeAnimation)
                anim.StartAnimation(curr_health);
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
        if (gameObject.name == "Player" || gameObject.name == "Boss")
        {
            UICanvas = GameObject.Find("UI").GetComponentInChildren<RectTransform>();
            if (UICanvas == null)
                UICanvas = GameObject.Find("Canvas").transform;
        }

        if (UICanvas == null)
        {
            Transform existingUI = transform.Find(name);
            if (transform.Find(name))
                GameObject.Destroy(existingUI.gameObject);

            UICanvas = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)).transform;
            UICanvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            RectTransform rect = UICanvas.GetComponent<RectTransform>();
            rect.SetParent(gameObject.transform);
            rect.localPosition = new Vector3(0, 0, 0);
            rect.sizeDelta = new Vector2(5, 5);
        }
    }

    // Create ui image object
    private Image CreateImage(string name, Sprite sprite, Color color, Image.Type type, Vector2 sizeRatio, RectTransform parent)
    {
        RectTransform obj = new GameObject(name, typeof(Image)).GetComponent<RectTransform>();
        obj.SetParent(parent);
        obj.localPosition = new Vector3(0, 0, 0);
        obj.sizeDelta = new Vector2(1, 1);
        obj.localScale = new Vector3(this.scale * sizeRatio.x, this.scale * sizeRatio.y, 1);
        Image image = obj.GetComponent<Image>();
        image.sprite = sprite;
        image.color = color;
        image.type = type;
        if (type == Image.Type.Filled)
        {
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillOrigin = (int)Image.OriginHorizontal.Left;
        }

        return image;
    }

    // Create health bar images
    private GameObject CreateHealthBar()
    {
        // Create holder for all healthbar images
        holder = new GameObject(holderName).AddComponent<RectTransform>();
        holder.SetParent(UICanvas.transform);
        holder.localPosition = new Vector3(position.x, position.y, 0);
        holder.localScale = this.size;
        holder.rotation = Quaternion.identity;

        // Create health bar
        CreateImage("background", spriteSource, backgroundColor, Image.Type.Simple, new Vector2(1, 1), holder);
        float respect_width = (size.x - innerRatio * size.y) / size.x;
        float respect_height = 1 - innerRatio;
        if (activeAnimation)
        {
            anim.middleImage = CreateImage("middle", spriteSource, middelColor, Image.Type.Filled, new Vector2(respect_width, respect_height), holder);
            anim.middleImage.fillOrigin = 1;
        }
        foregroundImage = CreateImage("foreground", spriteSource, foregroundColor, Image.Type.Filled, new Vector2(respect_width, respect_height), holder);
        foregroundImage.fillOrigin = 1;

        return holder.gameObject;
    }

    // Initialize health bar with max health of this object
    public void Initialize(int max_health, bool isShieldOn = false)
    {
        if (gameObject.name == "Boss")
        {
            holderName = "Boss Health Bar";
            Debug.Log("holdername changed");
        }

        if (showHealthBar)
        {
            InitUICanvas();
            maxHealth = (float)max_health;
            if (activeAnimation)
            {
                if (anim == null)
                    anim = new HealthBarAnimation();
                anim.isActive = activeAnimation;
                anim.Initialize(max_health);
                anim.SetAnimationSpeed(speed);
            }

            // If health bar doesn't exist
            if (UICanvas.Find(holderName) == null)
            {
                CreateHealthBar();
            }
            else // if health bar exist 
            {
                if (holder == null)
                {
                    holder = UICanvas.Find(holderName).GetComponent<RectTransform>();
                    holder.gameObject.SetActive(true);
                    //Debug.Log("holderName - " + holder.name);
                }
                foregroundImage = holder.Find("foreground").GetComponent<Image>();
                if (activeAnimation)
                {
                    anim.middleImage = holder.Find("middle").GetComponent<Image>();
                }

                if (holderName == "Boss Health Bar")
                {
                    shieldBar = holder.Find("shield").gameObject;
                }
            }
            foregroundImage.fillAmount = 1;
            anim.middleImage.fillAmount = 1;
        }

        if (rotateHealthBar == true && holder != null)
        {
            holder.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90);
        }
    }

    // Delete created ui and health bar
    // This function is used for custom Editor

    public virtual void RemoveUIandHealthBar()
    {
        if(UICanvas != null)
        {
            GameObject.DestroyImmediate(UICanvas.gameObject);
            UICanvas = null;
        }
    }

    // Update health bar animation
    public void UpdateHealthBar()
    {
        if(activeAnimation && anim != null)
        {
            anim.UpdateAnimation();
        }
    }

    // Haewon 11/15/2021 for shield purpose
    public void SetHolderName(string name)
    {
        holderName = name;
    }

    // Haewon 02/03/2022 for completely deleting remaining health bar
    public void SetZero()
    {
        anim.SetZero();
    }

    // Haewon 04/07/2022 for boss shield bar turn on/off
    public void SetShield(bool set)
    {
        if (shieldBar != null)
        {
            shieldBar.SetActive(set);
        }
    }
}
