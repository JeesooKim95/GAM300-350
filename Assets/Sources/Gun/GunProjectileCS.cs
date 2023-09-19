using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectileCS : MonoBehaviour
{
    struct Projectile
    {
        Vector3 position;
        Vector3 randomVec;
    };
    
    public ComputeShader shader;
    public GameObject projectile;
    public int numberOfSpawn = 100;
    private int currentNumberOfSpawn = 0;
    List<GameObject> projectiles;
    List<Vector3> projectilesPositions;
    List<Vector3> projectilesVector;
    ComputeBuffer buffer;
    ComputeBuffer vectorBuffer;
    Vector3[] outputDatas;
    Vector3[] randomVecOutputDatas;
    // Start is called before the first frame update
    void Start()
    {
        projectiles = new List<GameObject>();
        projectilesPositions = new List<Vector3>();
        projectilesVector = new List<Vector3>();
        //currentNumberOfSpawn = numberOfSpawn;
        
        //RunShader();        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < numberOfSpawn; ++i)
            {
                projectiles.Add(Instantiate(projectile, transform.position, Quaternion.identity));
                projectilesPositions.Add(projectiles[i].transform.position);

                Vector3 randomVec;
                randomVec.x = Random.Range(-5.0f, 5.0f);
                randomVec.y = Random.Range(-5.0f, 5.0f);
                randomVec.z = Random.Range(-5.0f, 5.0f);
                projectilesVector.Add(randomVec);
            }
            if(buffer != null)
            {
                buffer.Release();
                vectorBuffer.Release();
            }

            buffer = new ComputeBuffer(projectilesPositions.Count, 12);
            vectorBuffer = new ComputeBuffer(projectilesVector.Count, 12);

            currentNumberOfSpawn += numberOfSpawn;

            outputDatas = new Vector3[currentNumberOfSpawn];
            randomVecOutputDatas = new Vector3[currentNumberOfSpawn];

        }
        RunShader();
        */
    }

    void RunShader()
    {
        if(currentNumberOfSpawn > 0)
        {
            int kernelHandle = shader.FindKernel("CSMain");

            shader.SetFloat("time", Time.deltaTime);
            shader.SetFloat("speed", 5.0f);
            
            buffer.SetData(projectilesPositions);
            shader.SetBuffer(kernelHandle, "positions", buffer);

            vectorBuffer.SetData(projectilesVector);
            shader.SetBuffer(kernelHandle, "randomVec", vectorBuffer);

            shader.Dispatch(kernelHandle, currentNumberOfSpawn, 1, 1);
            buffer.GetData(outputDatas);

            for(int i = 0 ; i < currentNumberOfSpawn; ++i)
            {
                projectiles[i].transform.position = outputDatas[i];
                projectilesPositions[i] = outputDatas[i];
            }
        }
    }
}
