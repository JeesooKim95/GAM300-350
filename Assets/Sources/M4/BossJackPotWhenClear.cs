using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossJackPotWhenClear : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject coin;
    private float jackPotTimer = 10f;
    public bool bossDie = false;
    public bool isBossDieMusicPlayed = false;
    public Status BossHealth;
    public GameObject bossDeathAnimation;
    private bool onlyOnetime = false;
    private Vector3 bossDeathPos;
    private Quaternion bossRotation;
    private List<GameObject> coinsPool;
    private int coinIndex = 0;
    private int coinPoolCount = 300;

    void Start()
    {
        coinsPool = new List<GameObject>();

        for (int i = 0; i < coinPoolCount; ++i)
        {
            coinsPool.Add(Instantiate(coin, new Vector3(-100f, -100f, -100f), Quaternion.identity));
        }
    }
    private void JackPotWhenKilled()
    {
        if (coin != null)
        {
            if (coinIndex >= coinPoolCount)
                coinIndex = 0;
           
            GameObject obj = coinsPool[coinIndex++];

            obj.transform.position = gameObject.transform.position;
            if (obj)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();

                if (rb)
                {
                    Vector3 directionSpread = Vector3.zero;

                    float spreadForce = 30.0f;
                    directionSpread.x += Random.Range(-spreadForce, spreadForce);
                    directionSpread.y += Random.Range(-spreadForce, spreadForce);
                    directionSpread.z += Random.Range(-spreadForce, spreadForce);

                    rb.AddForce(directionSpread, ForceMode.Impulse);
                }

                rb.useGravity = true;
            }

            jackPotTimer -= Time.deltaTime;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (bossDie)
        {
            if (isBossDieMusicPlayed == false)
            {
                isBossDieMusicPlayed = true;
                onlyOnetime = true;
                FindObjectOfType<AudioManager>().PlayBackgroundMusic(AudioManager.BGMType.Victory);
            }
            if (jackPotTimer > 0f)
            {
                JackPotWhenKilled();
            }
            else
            {
                SceneManager.LoadScene("ClearScene");
            }
        }

        if (onlyOnetime == true)
        {
            if (BossHealth)
            {
                bossDeathPos = BossHealth.gameObject.transform.position;
                bossRotation = BossHealth.gameObject.transform.rotation;
                bossDeathPos = new Vector3(bossDeathPos.x, bossDeathPos.y - 7f, bossDeathPos.z);
            }

            GameObject death = GameObject.Instantiate(bossDeathAnimation, bossDeathPos, bossRotation);

            Destroy(death, 1f);
            onlyOnetime = false;
        }
    }
}
