/*
    Team    : Speaking Potato
    Author  : Jeesoo Kim, and Sinil Kang
    Date    : 09/24/2021
    Desc    : Manager who control all audio stuff in a game.
*/

using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] soundList;
    public Sound[] shootingSoundList;
    public Sound[] walkingSoundList;
    public Sound[] bgmList;
    public Sound[] environmentalSFXList;
    public Sound[] drawCardSoundList;
    public Sound[] activateCardSoundList;
    public Sound[] dashSoundList;
    public Sound[] iceShootingSoundList;

    private Sound currentBGM;
    public Sound combatBGM = null;

    public static AudioManager instance;

    public float timer = 0f;
    public float itemTimer = 0f;

    int combatCounter = 0;
    float combatMaxVolume;
    float bgmMaxVolume;

    public bool isBossBattle = false;

    public enum BGMType
    {
        MainMenu,
        LegacyGameBGM,
        Victory,
        Death,
        Forest,
        Prison,
        Snow,
        Desert,
        Tutorial,
        Boss,
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        InitSoundList(soundList);
        InitSoundList(shootingSoundList);
        InitSoundList(walkingSoundList);
        InitSoundList(bgmList);
        InitSoundList(environmentalSFXList);
        InitSoundList(drawCardSoundList);
        InitSoundList(activateCardSoundList);
        InitSoundList(dashSoundList);
        InitSoundList(iceShootingSoundList);

        timer = 0f;
        itemTimer = 0f;

        combatBGM.source = gameObject.AddComponent<AudioSource>();
        combatBGM.source.clip = combatBGM.clip;
        combatBGM.source.volume = combatBGM.volume;
        combatBGM.source.pitch = combatBGM.pitch;
        combatBGM.source.loop = combatBGM.loop;

        combatMaxVolume = combatBGM.source.volume;
    }

    void Start()
    {
        Debug.Log("Scene Started!");
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        if(itemTimer > 0f)
        {
            itemTimer -= Time.deltaTime;
        }

        //Debug.Log(combatCounter);
    }

    public void Play(string name)
    {
        if (name == "ItemThrowed")
        {
            if (itemTimer > 0f)
            {
                return;
            }
            else
            {
                itemTimer = 0.05f;
            }
        }

        Sound s = Array.Find(soundList, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }

        if (s.loop == true)
        {
            s.source.loop = true;
            s.source.Play();
        }
        else
        {
            s.source.PlayOneShot(s.source.clip, s.source.volume);
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(soundList, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }
        s.source.Stop();
    }

    public void PlaySpatial(string name, Vector3 position)
    {
        if (name == "EnemyGetWaterDamage")
        {
            if (timer > 0f)
            {
                return;
            }
            else
            {
                timer = 0.025f;
            }
        }

        Sound s = Array.Find(soundList, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }
        if (name == "EnemyGetWaterDamage")
        {
            var tempGo = new GameObject("TempAudio");
            tempGo.transform.position = position;
            AudioSource aSource = tempGo.AddComponent<AudioSource>();
            aSource.clip = s.clip;
            aSource.pitch = UnityEngine.Random.Range(0.5f, 1.5f);
            aSource.volume = s.volume;
            aSource.spatialBlend = 1f;
            aSource.bypassEffects = true;
            aSource.bypassListenerEffects = true;
            
            aSource.Play();
            Destroy(tempGo, s.clip.length);
        }
        else
        {
            AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
        }
    }

    public void PlayBackgroundMusic(BGMType type)
    {
        // Select bgm depends on the type
        switch (type)
        {
            case BGMType.MainMenu:
                currentBGM = Array.Find(bgmList, sound => sound.name == "MainMenuBGM");
                break;
            case BGMType.LegacyGameBGM:
                currentBGM = Array.Find(bgmList, sound => sound.name == "Theme");
                break;
            case BGMType.Victory:
                currentBGM = Array.Find(bgmList, sound => sound.name == "VictoryBGM");
                break;
            case BGMType.Death:
                currentBGM = Array.Find(bgmList, sound => sound.name == "DeathBGM");
                break;
            case BGMType.Forest:
                currentBGM = Array.Find(bgmList, sound => sound.name == "ForestBGM");
                break;
            case BGMType.Prison:
                currentBGM = Array.Find(bgmList, sound => sound.name == "PrisonBGM");
                break;
            case BGMType.Snow:
                currentBGM = Array.Find(bgmList, sound => sound.name == "SnowBGM");
                break;
            case BGMType.Desert:
                currentBGM = Array.Find(bgmList, sound => sound.name == "DesertBGM");
                break;
            case BGMType.Tutorial:
                currentBGM = Array.Find(bgmList, sound => sound.name == "TutorialBGM");
                break;
            case BGMType.Boss:
                currentBGM = Array.Find(bgmList, sound => sound.name == "BossBGM");
                break;
            default:
                break;
        }

        if (currentBGM == null)
        {
            Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }



        currentBGM.source.loop = true;
        currentBGM.loop = true;
        bgmMaxVolume = currentBGM.volume;
        currentBGM.source.volume = bgmMaxVolume;

        // Pause all bgm
        foreach (Sound t in bgmList)
        {
            if (t.source.isPlaying)
            {
                t.source.Stop();
            }
        }
        combatBGM.source.Stop();
        combatCounter = 0;

        currentBGM.source.Play();
    }

    public void PlayEnvironmentalSFX(string name)
    {
        // Select bgm depends on the type
        Sound s = Array.Find(environmentalSFXList, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }


        s.source.loop = true;
        s.loop = true;

        // Pause all bgm
        foreach (Sound t in environmentalSFXList)
        {
            if (t.source.isPlaying)
            {
                t.source.Stop();
            }
        }

        s.source.Play();
    }

    public void PlayShoot(float volume = 1f)
    {
        PlaySoundList(shootingSoundList, volume);
    }

    public void PlayWalk()
    {
        PlaySoundList(walkingSoundList);
    }

    public void PlayDrawCard()
    {
        PlaySoundList(drawCardSoundList);
    }

    public void PlayDash()
    {
        PlaySoundList(dashSoundList);
    }

    public void PlayIceShoot(float volume = 1f)
    {
        PlaySoundList(iceShootingSoundList, volume);
    }

    private void PlaySoundList(Sound[] list, float volume = 1f)
    {
        int i = UnityEngine.Random.Range(0, list.Length);
        Sound s = list[i];
        if (s == null)
        {
            Debug.LogWarning("Sound: " + s.name + " not found !");
            return;
        }
        s.source.PlayOneShot(s.source.clip, s.source.volume * volume);
    }
    private void InitSoundList(Sound[] list)
    {
        foreach (Sound s in list)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Test()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Play("Top");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Play("Pair");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Play("TwoPair");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Play("Triple");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Play("FullHouse");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Play("Fourcard");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Play("Straight");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Play("Flush");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Play("StraightFlush");
        }
    }

    public void StartCombat()
    {

        if (combatCounter == 0)
        {
            if(currentBGM != null)
            {
                TransitionCombatBGM(true);
            }
        }
        combatCounter++;
    }
    public void EndCombat()
    {
        combatCounter--;
        if(combatCounter == 0)
        {
            TransitionCombatBGM(false);
        }
    }

    private void TransitionCombatBGM(bool isFadeToCombat)
    {
        // When boss level, do not play combat BGM
        if(isBossBattle == true)
        {
            return;
        }

        StopAllCoroutines();

        StartCoroutine(FadeTrack(isFadeToCombat));
    }

    private IEnumerator FadeTrack(bool isFadeToCombat)
    {
        float timeToFade = 0.75f;
        float timeElapsed = 0f;

        if(isFadeToCombat)
        {
            combatBGM.source.Play();

            while(timeElapsed < timeToFade)
            {
                combatBGM.source.volume = Mathf.Lerp(0, combatMaxVolume, timeElapsed / timeToFade);
                currentBGM.source.volume = Mathf.Lerp(bgmMaxVolume, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            currentBGM.source.Pause();
        }
        else
        {
            currentBGM.source.Play();

            while (timeElapsed < timeToFade)
            {
                combatBGM.source.volume = Mathf.Lerp(combatMaxVolume, 0, timeElapsed / timeToFade);
                currentBGM.source.volume = Mathf.Lerp(0, bgmMaxVolume, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            combatBGM.source.Pause();
        }
    }
}