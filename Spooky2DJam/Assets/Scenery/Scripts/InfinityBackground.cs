using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityBackground : MonoBehaviour
{
    [SerializeField]
    GameObject camera;
    float imageHeight = 19f; // Height of one image
    float startPosition;
    [SerializeField]
    float parallaxEffect = 0.3f; // Default parallax effect

    void Start()
    {
        startPosition = transform.position.y;
    }

    void FixedUpdate()
    {
        // Used to repead the bg
        float temp = (camera.transform.position.y * (1 - parallaxEffect));

        // Calculate the distance with the camera position in the Y axis (vertical) multiplied by the parallax effect variable.
        float distance = (camera.transform.position.y * parallaxEffect);

        // Change the position in the Y axis (vertical) of the current background gameObject (small, medium, big)
        transform.position = new Vector3(transform.position.x, startPosition + distance, transform.position.z);

        // Used to repeat the background
        if (temp > startPosition + imageHeight)
        {
            startPosition += imageHeight;
        }
        else if (temp < startPosition - imageHeight)
        {
            startPosition -= imageHeight;
        }
    }
}