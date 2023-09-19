using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySoundPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().PlayBackgroundMusic(AudioManager.BGMType.MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
