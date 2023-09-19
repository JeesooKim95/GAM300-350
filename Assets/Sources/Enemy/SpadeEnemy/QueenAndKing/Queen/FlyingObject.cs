using UnityEngine;
using System.Linq;

public class FlyingObject : MonoBehaviour
{
    public string[] tagsToCheck;

    public float impactRadius;

    public float destoryDelay;

    private GameObject impactFx;

    public int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        impactFx = transform.Find("ImpactFx").gameObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (tagsToCheck.Contains(other.tag))
        {
            Collider[] objectsinRange = Physics.OverlapSphere(transform.position, impactRadius);

            foreach (Collider col in objectsinRange)
            {
                Rigidbody enemy = col.GetComponent<Rigidbody>();

                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }

            impactFx.SetActive(true);
            impactFx.transform.SetParent(null);

            Destroy(impactFx, destoryDelay);
            Destroy(gameObject);

            Vector3 knockback = new Vector3(0, 0, 0);
            
            other.gameObject.GetComponent<Status>().OnTakeDamage(damage, knockback);

        }
    }

}
