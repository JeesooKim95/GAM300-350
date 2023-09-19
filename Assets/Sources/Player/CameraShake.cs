/*  Class:               GAM300
 *  Team name:      Speaking Potato
 *  Date:                10/17/2021
 *  Contributor:       Sinil Kang
 *  Description:       Source file to shake camera.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // In order to use this function, 
    //  1. Declare "public CameraShake cameraShake;" on the top of the source code.
    //  2. Call "StartCoroutine(cameraShake.Shake(How long shaking continues, magnitude of shaking));
    public IEnumerator Shake(float duration, float strength)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            // Before continue next iteration, 
            // Wait until the next fram is drawn.
            yield return null;
        }

        transform.localPosition = originalPos;
    }
    public IEnumerator Shake(float duration, float strength, Vector3 originalPos)
    {
        //Debug.Log("Original Pos: " + originalPos);

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            // Before continue next iteration, 
            // Wait until the next fram is drawn.
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
