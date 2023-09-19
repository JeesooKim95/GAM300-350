/*
    Team    : Speaking Potato
    Author  : Jina Hyun
    Date    : 09/26/2021
    Desc    : Player respawn
*/
using UnityEngine;

[RequireComponent(typeof(Status))]
public class PlayerRespawn : MonoBehaviour
{
    private Status status = null;
    private Vector3 savePoint = new Vector3(0, 0, 0);
    private Quaternion rotation = Quaternion.identity;
    private bool shouldRespawn = false;
    private Fade fade = null;

    // Set position Where to respawn
    public void SetSavePoint(Vector3 position)
    {
        savePoint = position;
        savePoint.z = gameObject.transform.position.z;
        rotation = gameObject.transform.rotation;
    }

    // Respawn the player at save point with full health
    public void Respawn()
    {
        shouldRespawn = true;
        fade.StartAction(FadeType.FadeInAndOut, 1);
    }

    private void Start()
    {
        status = gameObject.GetComponent<Status>();
        SetSavePoint(gameObject.transform.position);
        rotation = gameObject.transform.rotation;
        shouldRespawn = false;
        fade = GameObject.Find("LevelManager").GetComponent<Fade>();
    }

    private void Update()
    {
        // If the player should be respawned and the fade is enough dark,
        //      reset the health and move the player's location
        if(shouldRespawn && fade.IsDark())
        {
            gameObject.GetComponent<PlayerMovement>().InitPosition(savePoint);
            gameObject.transform.rotation = rotation;
            status.ResetHealth();
            shouldRespawn = false;
        }
    }
}
