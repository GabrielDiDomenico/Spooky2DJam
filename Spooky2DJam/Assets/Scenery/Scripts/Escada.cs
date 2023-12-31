using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escada : MonoBehaviour
{
    public GameObject prefab; 
    public GameObject prefab4; 
    public GameObject prefabEnemy; 
    public GameObject playerRef; 

    public float stepLevel = 0f;
    private int currentLevel=0;
    public int levelToEnemy = 5;
    public int levelToEnemyDouble = 20;
    private bool firstTimeSpawn4 = true;

    void Start()
    {
        int numSteps = 150; // N�mero de degraus da escada
        float stepSizeX = 2.5f;
        float stepSizeY = -1.5f;
        float stepSizeZ = 0f;
        

        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < numSteps; i++)
        {
            GameObject step;
            stepLevel++;

            step = Instantiate(prefab, spawnPosition, Quaternion.identity);
            spawnPosition.x += stepSizeX;
            spawnPosition.y += stepSizeY;
            spawnPosition.z += stepSizeZ;
            if(stepLevel == 4)
            {
                if((Random.Range(1,10)%2==0 || firstTimeSpawn4) && stepSizeX > 0)
                {
                    if (Random.Range(0, 5) % 2 == 0)
                    {
                        Instantiate(prefabEnemy, new Vector3(9f, spawnPosition.y + 2, spawnPosition.z), Quaternion.identity);
                    }
                    Instantiate(prefab4, new Vector3(15.2f, spawnPosition.y, spawnPosition.z), Quaternion.identity);
                    
                    currentLevel++;
                }
                
                stepSizeX *=-1;
                stepLevel = 0;
                firstTimeSpawn4 = false;
            }
        }
        playerRef.GetComponent<PlayerController>().SetLevelsForLamps(currentLevel);
    }
}