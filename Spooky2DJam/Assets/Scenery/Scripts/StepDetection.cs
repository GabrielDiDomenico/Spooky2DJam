using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepDetection : MonoBehaviour
{
    AudioSource audioSource;
    private Rigidbody2D rb;

    private void Awake()
    {
        Rigidbody2D[] gos = FindObjectsByType<Rigidbody2D>(0);
        foreach(Rigidbody2D go in gos)
        {
            if(go.gameObject.name == "Player")
            {
                rb = go;
            }
        }
    }

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(rb.velocity.x != 0)
        {
            if(!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}
