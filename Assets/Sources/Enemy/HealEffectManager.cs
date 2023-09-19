using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffectManager : MonoBehaviour
{
    private ParticleSystem effect;
    public float timer = 2.0f;
    private float currentTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<ParticleSystem>();
        effect.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentTimer > 0.0f)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0.0f)
            {
                Stop();
            }
        }
    }

    public void Play()
    {
        // ignore if particle is already playing
        if (currentTimer > 0.0f) return;

        if (effect != null)
        {
            effect.Play();
        }
        currentTimer = timer;
    }

    public void Stop()
    {
        effect.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
