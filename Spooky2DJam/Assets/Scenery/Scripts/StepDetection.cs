using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepDetection : MonoBehaviour
{
  AudioSource audioSource;
  [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(rb.velocity.x != 0)
        {
            audioSource.Play();
        }
    }
}
