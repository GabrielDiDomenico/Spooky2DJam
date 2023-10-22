using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    public GameObject wallRight; 
    public GameObject wallLeft; 


    void Start()
    {
        int numSteps = 50; // Número de paredes
        float stepSizeXRight = 1.35f;
        float stepSizeY = -10f;
        float stepSizeZ = 0f;

        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < numSteps; i++)
        {
            spawnPosition.x = stepSizeXRight;
            
            GameObject step = Instantiate(wallRight, spawnPosition, Quaternion.identity);
            spawnPosition.x = 0;
            step = Instantiate(wallLeft, spawnPosition, Quaternion.identity);
            spawnPosition.y += stepSizeY;
            spawnPosition.z += stepSizeZ;


        }
        
    }
}
