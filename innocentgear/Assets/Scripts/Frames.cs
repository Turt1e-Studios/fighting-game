using UnityEngine;

public class Frames : MonoBehaviour
{
    public int CurrentFrame { get; private set; }

    private const int FPS = 60; // Do not change this
    private float _startTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - _startTime;
        if (elapsedTime * FPS > CurrentFrame)
        {
            CurrentFrame = (int) (elapsedTime * FPS);
        }
    }
}
