using System;
using System.Collections;
using UnityEngine;

// The sysetem of frames that are used to keep track of player states.
public class Frames : MonoBehaviour
{
    public int CurrentFrame { get; private set; }

    private const int FPS = 60; // Do not change this
    private float _startTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
        //Time.fixedDeltaTime = 0.01666667f; // Set fixed update to 60 FPS.
    }

    // Update the current frame
    void Update()
    {
        float elapsedTime = Time.time - _startTime;
        if (elapsedTime * FPS > CurrentFrame)
        {
            CurrentFrame = (int) (elapsedTime * FPS);
        }
    }
}
