using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioManager.BGMType bgmType;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().PlayBackgroundMusic(bgmType);
        
        if(bgmType == AudioManager.BGMType.Boss)
        {
            FindObjectOfType<AudioManager>().isBossBattle = true;
        }
        else if(bgmType == AudioManager.BGMType.MainMenu)
        {
            FindObjectOfType<AudioManager>().isBossBattle = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
