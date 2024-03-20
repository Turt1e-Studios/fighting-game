using System;
using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _playerMovement;
    private Normals _normals;
    private Frames _frames;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _frames = GameObject.Find("GameManager").GetComponent<Frames>();
        _playerMovement = GetComponent<PlayerMovement>();
        _normals = GetComponent<Normals>();
    }

    public void Move(Collider2D hitbox, int startupFrames, int activeFrames, int recoveryFrames)
    {
        _playerMovement.enabled = false;
        _normals.enabled = false;
        Startup(hitbox, startupFrames, activeFrames, recoveryFrames);
    }

    IEnumerator WaitForFrames(int frames, Action action)
    {
        int initialFrame = _frames.CurrentFrame;
        yield return new WaitUntil(() => _frames.CurrentFrame - initialFrame > frames);
        action();
    }

    void Startup(Collider2D hitbox, int startupFrames, int activeFrames, int recoveryFrames)
    {
        _spriteRenderer.color = Color.green;
        StartCoroutine(WaitForFrames(startupFrames, () => Active(hitbox, activeFrames, recoveryFrames)));
    }

    IEnumerator WaitForFixedFrame(Action action)
    {
        yield return new WaitForFixedUpdate();
        action();
    }

    void ActivateHitbox(Collider2D hitbox, int activeFrames, int recoveryFrames)
    {
        if (hitbox.IsTouchingLayers(LayerMask.GetMask("Hurtbox")))
        {
            print("hit!");
        }
        StartCoroutine(WaitForFrames(activeFrames + 1, () => Recovery(hitbox, recoveryFrames)));
    }

    void Active(Collider2D hitbox, int activeFrames, int recoveryFrames)
    {
        _spriteRenderer.color = Color.red;
        hitbox.gameObject.SetActive(true);
        StartCoroutine(WaitForFixedFrame(() => ActivateHitbox(hitbox, activeFrames, recoveryFrames))); // Hitbox detection only occurs on the next fixed update.
    }

    void Recovery(Collider2D hitbox, int recoveryFrames)
    {
        _spriteRenderer.color = Color.blue;
        hitbox.gameObject.SetActive(false);
        StartCoroutine(WaitForFrames(recoveryFrames, ResetState));
    }

    void ResetState()
    {
        _playerMovement.enabled = true;
        _normals.enabled = true;
        _spriteRenderer.color = Color.white;
    }
}
