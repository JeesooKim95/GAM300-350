using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M5_SnowFieldManager : MonoBehaviour
{

    public GameObject queen = null;
    public GameObject king = null;
    public Status queenStatus;
    public Status kingStatus;

    public Portal portal;

    // Haewon
    private bool isQueenDead = false;
    private bool isKingDead = false;

    public GameObject bossDummy;
    Quaternion bossRot;
    void Start()
    {
        portal = FindObjectOfType<Portal>();
    }

    void Update()
    {
        if (isQueenDead == false)
        {
            if (CheckBossStatus(queenStatus) == true)
            {
                isQueenDead = true;
            }
        }


        if (isKingDead == false)
        {
            if (CheckBossStatus(kingStatus) == true)
            {
                isKingDead = true;
            }
        }
           

        if (isKingDead && isQueenDead)
        {
            portal.Activate();
        }
    }

    bool CheckBossStatus(Status bossStatus)
    {
        if (bossStatus.currentHealth < 0)
        {
            Vector3 diePos = bossStatus.gameObject.transform.position;
            diePos.y = -0.5f;

            if (bossDummy)
            {
                GameObject bossDummyObj = GameObject.Instantiate(bossDummy, diePos, bossRot);
                Destroy(bossDummyObj, 3f);
            }
            //bossStatus = null;
            return true;
        }
        return false;
    }
}
