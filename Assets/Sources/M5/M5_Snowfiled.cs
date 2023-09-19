using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M5_Snowfiled : MonoBehaviour
{
    public M5_ForestWave trigger;
    private MeshRenderer meshRenderer = null;

    public bool triggerCheck;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        triggerCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger.colliderCheck == true)
        {
            meshRenderer.enabled = false;
            triggerCheck = true;
        }
            

    }
}
