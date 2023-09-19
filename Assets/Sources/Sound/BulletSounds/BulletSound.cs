/*
    Team    : Speaking Potato
    Author  : Sinil Kang
    Date    : 11/04/2021
    Desc    : Source code which controls all sounds from bullets
*/

using System;
using UnityEngine;

public class BulletSound : MonoBehaviour
{
    public GameObject emptyObject;
    public Sound[] bulletSoundList;

    public GameObject myEmpty;

    public enum BulletSoundType
    {
        WALL,
        ENEMY,
    }

    // Start is called before the first frame update
    void Start()
    {
        InitSoundList(bulletSoundList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(BulletSoundType type)
    {
        Sound s = null;
        switch(type)
        {
            case BulletSoundType.WALL:
                s = Array.Find(bulletSoundList, sound => sound.name == "WALL");
                break;
            case BulletSoundType.ENEMY:
                s = Array.Find(bulletSoundList, sound => sound.name == "ENEMY");
                break;
            default:
                break;
        }

        if(s == null)
        {
            Debug.LogWarning("Sound: " + type + " not found!");
            return;
        }

        GameObject myEmpty = Instantiate(emptyObject);
        myEmpty.transform.position = transform.position;
        AudioSource newSource = myEmpty.AddComponent<AudioSource>();
        myEmpty.GetComponent<RemoveEmptyObject>().audioSource = newSource;
        myEmpty.GetComponent<RemoveEmptyObject>().shouldRemove = true;
        s.source = newSource;
        s.source.spatialBlend = 1;
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.maxDistance = 80f;
        s.source.rolloffMode = AudioRolloffMode.Linear;
        s.source.PlayOneShot(s.source.clip, s.source.volume);
    }

    private void InitSoundList(Sound[] list)
    {
        //foreach (Sound s in list)
        {

        }
    }
}
