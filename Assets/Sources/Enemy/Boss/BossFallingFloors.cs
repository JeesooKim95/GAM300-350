/*
    Team    : Speaking Potato
    Author  : Sean Kim
    Date    : 10/17/2021
    Desc    : Boss scene falling floors.
*/
using UnityEngine;

public class BossFallingFloors : MonoBehaviour
{
    public float FallingInterval = 2.0f;
    public float GenerationInterval = 1.0f;
    public GameObject Boss;

    private float fallingTimer = 0.0f;
    private float generateTimer = 0.0f;
    private GameObject player;
    private GameObject nearPlane = null;
    private Vector3 playerPos;
    private bool floorHasFallen = false;
    private bool floorChosen = false;
    private GameObject[] planes;
    private Color originalColor;
    private int bossHealth;
    private bool WarningFlag = false;
    private int TriggerHealth;
    //Respawn
    private GameObject respawnPoint;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        TriggerHealth = Boss.GetComponent<BossBehavior>().GetLastPhaseHealth();
        planes = GameObject.FindGameObjectsWithTag("Ground");
        originalColor = GameObject.FindGameObjectWithTag("Ground").GetComponent<Renderer>().material.color;
        player = GameObject.FindGameObjectWithTag("Player");
        audioManager = FindObjectOfType<AudioManager>();
        respawnPoint = GameObject.Find("PlayerSpawnPoint");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Boss != null)
        {
            bossHealth = Boss.GetComponent<Status>().GetHealth();
            if (bossHealth <= 1000)
            {
                playerPos = player.GetComponent<Transform>().position;
                fallingTimer += Time.deltaTime;
                //Red signifer to show that the floor will fall
                if (fallingTimer >= FallingInterval - 1.0f && !floorChosen)
                {
                    nearPlane = GetClosestPlane(playerPos);
                    if (!WarningFlag)
                    {
                        audioManager.PlaySpatial("BossFallingFloorWarning", nearPlane.transform.position);
                        nearPlane.GetComponent<Renderer>().material.color = Color.red;
                    }
                    else
                    {
                        nearPlane.GetComponent<Renderer>().material.color = Color.black;
                    }
                    floorChosen = true;
                }
                //Deactivate floor 
                if (fallingTimer >= FallingInterval && nearPlane != null && !floorHasFallen)
                {
                    audioManager.PlaySpatial("BossFallingFloor", nearPlane.transform.position);
                    nearPlane.SetActive(false);
                    nearPlane.GetComponent<Renderer>().material.color = Color.white;
                    floorHasFallen = true;
                    fallingTimer = 0.0f;
                }
                //Reactivate floor
                if (floorHasFallen)
                {
                    generateTimer += Time.deltaTime;
                    if (generateTimer >= GenerationInterval && nearPlane != null)
                    {
                        nearPlane.SetActive(true);
                        floorHasFallen = false;
                        floorChosen = false;
                        generateTimer = 0.0f;
                        nearPlane.GetComponent<Renderer>().material.color = originalColor;
                    }
                }
            }
            //If player falls, kill player and respawn
            if (playerPos.y < -15.0f)
            {
                //player.GetComponent<Status>().OnDeath();
                //player.GetComponent<PlayerRespawn>().Respawn();

                CharacterController playerController = player.GetComponent<CharacterController>();
                playerController.enabled = false;
                playerController.transform.position = respawnPoint.transform.position;
                playerController.enabled = true;
            }
        }
    }

    private GameObject GetClosestPlane(Vector3 playerpos)
    {
        GameObject closestPlane = null;
        float closesetDistanceSquared = Mathf.Infinity;
        foreach (GameObject ground in planes)
        {
            Vector3 direction = ground.transform.position - playerPos;
            float squaredDistance = direction.sqrMagnitude;
            if (squaredDistance < closesetDistanceSquared)
            {
                closesetDistanceSquared = squaredDistance;
                closestPlane = ground;
            }
        }
        return closestPlane;
    }
}
