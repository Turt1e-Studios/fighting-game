using System;
using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private bool isPlayerOne;
    
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _playerMovement;
    private Normals _normals;
    private Frames _frames;
    private Collider2D _hitbox;
    private bool _hitboxActive;
    private GameObject _boxes;
    private Blocking _blocking;
    private GameObject _blockingBoxes;
    private String _enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _frames = GameObject.Find("GameManager").GetComponent<Frames>();
        _playerMovement = GetComponent<PlayerMovement>();
        _normals = GetComponent<Normals>();
        _blocking = GetComponent<Blocking>();
        _enemyLayer = isPlayerOne ? "Hurtbox2" : "Hurtbox";
    }
    
    void Update()
    {
        // Other way to do this in the coroutine method was not consistent. This way is slightly inefficient however.
        if (_hitboxActive && _hitbox.IsTouchingLayers(LayerMask.GetMask(_enemyLayer)))
        {
            _playerMovement.GetEnemy().GetComponent<Blocking>().RecieveBlock();
        }
    }
    
    public void Move(AttackMove move)
    {
        //_playerMovement.enabled = false;
        _normals.enabled = false; // Just disables the script, there could be a better way to disable them.
        _blocking.SetBlock(false);
        Startup(move);
    }

    // Frames before attack is activated
    void Startup(AttackMove move)
    {
        _spriteRenderer.color = Color.green;
        StartCoroutine(WaitForFrames(move.startupFrames, () => Active(move)));
    }

    // Frames where attack is active
    void Active(AttackMove move)
    {
        _spriteRenderer.color = Color.red;

        //Time.timeScale = 0;
        _blockingBoxes = _blocking.GetCurrentBoxes();
        _blockingBoxes.SetActive(false);
        _boxes = Instantiate(move.boxes, transform.position, transform.rotation);
        _boxes.transform.SetParent(transform);
        
        _playerMovement.FlipHitboxModel();
        
        _hitboxActive = true;
        _hitbox = _boxes.transform.Find("Hitbox").GetComponent<Collider2D>();

        StartCoroutine(WaitForFrames(move.activeFrames, () => Recovery(move)));
    }

    // Frames where player cannot do anything
    void Recovery(AttackMove move)
    {
        _spriteRenderer.color = Color.blue;
        
        Destroy(_boxes);
        _blockingBoxes.SetActive(true);
        _hitboxActive = false;
        
        StartCoroutine(WaitForFrames(move.recoveryFrames, ResetState));
    }

    void ResetState()
    {
        _spriteRenderer.color = Color.white;
        _playerMovement.enabled = true;
        _normals.enabled = true;
        _blocking.SetBlock(true);
    }
    
    // Perform an action after a certain amount of frames.
    IEnumerator WaitForFrames(int frames, Action action)
    {
        int initialFrame = _frames.CurrentFrame;
        yield return new WaitUntil(() => _frames.CurrentFrame - initialFrame > frames);
        action();
    }
}
