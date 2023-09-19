using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveEmptyObject : MonoBehaviour
{
    public AudioSource audioSource;

    public bool shouldRemove = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shouldRemove == false)
        {
            return;
        }

        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            return;
        }

        if(audioSource.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
