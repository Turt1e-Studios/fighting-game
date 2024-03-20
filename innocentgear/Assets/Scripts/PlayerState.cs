using System;
using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _playerMovement;
    private Normals _normals;
    private Frames _frames;
    private Collider2D _hitbox;
    private bool _hitboxActive;

    void Update()
    {
        if (_hitboxActive)
        {
            print("hitbox active.");
        }
        // Other way to do this in the coroutine method was not consistent.
        if (_hitboxActive && _hitbox.IsTouchingLayers(LayerMask.GetMask("Hurtbox2")))
        {
            print("hit!");
        }
    }
    
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

    void Active(Collider2D hitbox, int activeFrames, int recoveryFrames)
    {
        _spriteRenderer.color = Color.red;
        hitbox.gameObject.SetActive(true);
        _hitboxActive = true;
        _hitbox = hitbox;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0.01f, 0f));
        StartCoroutine(WaitForFrames(activeFrames, () => Recovery(hitbox, recoveryFrames)));
        //StartCoroutine(WaitForFixedFrame(() => ActivateHitbox(hitbox, activeFrames, recoveryFrames))); // Hitbox detection only occurs on the next fixed update.
    }

    void Recovery(Collider2D hitbox, int recoveryFrames)
    {
        _spriteRenderer.color = Color.blue;
        _hitboxActive = false;
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
