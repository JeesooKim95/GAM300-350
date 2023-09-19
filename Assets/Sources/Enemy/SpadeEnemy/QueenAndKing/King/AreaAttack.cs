using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            Vector3 knockback = new Vector3(0, 15f, 0);
            other.gameObject.GetComponent<Status>().OnTakeDamage(10, knockback);
        }
    }
}
