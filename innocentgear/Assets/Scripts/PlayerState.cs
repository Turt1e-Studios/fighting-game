using System;
using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Frames _frames;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _frames = GameObject.Find("GameManager").GetComponent<Frames>();
    }

    public void Move(int startupFrames, int activeFrames, int recoveryFrames)
    {
        Startup(startupFrames, activeFrames, recoveryFrames);
    }

    IEnumerator WaitForFrames(int frames, Action action)
    {
        int initialFrame = _frames.CurrentFrame;
        yield return new WaitUntil(() => _frames.CurrentFrame - initialFrame >= frames);
        action();
    }

    void Startup(int startupFrames, int activeFrames, int recoveryFrames)
    {
        _spriteRenderer.color = Color.green;
        StartCoroutine(WaitForFrames(startupFrames, () => Active(activeFrames, recoveryFrames)));
    }

    void Active(int activeFrames, int recoveryFrames)
    {
        _spriteRenderer.color = Color.red;
        StartCoroutine(WaitForFrames(activeFrames, () => Recovery(recoveryFrames)));
    }

    void Recovery(int recoveryFrames)
    {
        _spriteRenderer.color = Color.blue;
        StartCoroutine(WaitForFrames(recoveryFrames, ResetState));
    }

    void ResetState()
    {
        _spriteRenderer.color = Color.white;
    }
}
