using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] lightFrame;
    [SerializeField] private Light2D light;

    private int currentFrameLight;
    private float timer;
    private float framerate = .15f;
    private float intensityFloat = -.05f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
  
    void Update()
    {
        timer += Time.deltaTime;

        

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrameLight = (currentFrameLight + 1) % lightFrame.Length;
            spriteRenderer.sprite = lightFrame[currentFrameLight];
            if (currentFrameLight == 0)
            {
                intensityFloat *= -1;
            }
            light.intensity = light.intensity + intensityFloat;
        }
    }
}
