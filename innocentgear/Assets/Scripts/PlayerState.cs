using System;
using System.Collections;
using UnityEngine;

// Controls state of player when performing an attack.
public class PlayerState : MonoBehaviour
{
    [Header("Misc Player Info")]
    [SerializeField] private bool isPlayerOne;
    [SerializeField] private Color enemyColor;
    
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _playerMovement;
    private Normals _normals;
    private Frames _frames;
    private Collider2D[] _hitboxes;
    private bool _hitboxActive;
    private GameObject _boxes;
    private Blocking _blocking;
    private GameObject _blockingBoxes;
    private String _enemyLayer;
    private AttackMove _currentMove;
    private bool _alreadyHit;

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
        // Activates when hitbox hits an enemy hurtbox.
        if (_hitboxActive && !_alreadyHit)
        {
            foreach (Collider2D hitbox in _hitboxes)
            {
                if (hitbox.IsTouchingLayers(LayerMask.GetMask(_enemyLayer)))
                {
                    _playerMovement.GetEnemy().GetComponent<Blocking>().RecieveBlock(_currentMove);
                    _alreadyHit = true;
                    break;
                }
            }
        }
    }
    
    // Command to attack is activated
    public void Move(AttackMove move)
    {
        // Already hit to prevent multiple damage instances
        _currentMove = move;
        _alreadyHit = false;
        
        _blockingBoxes = _blocking.GetCurrentBoxes();
        _spriteRenderer = _blockingBoxes.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        
        _normals.enabled = false;
        _blocking.SetBlock(false);
        
        Startup(move);
    }

    // Frames before attack is activated
    void Startup(AttackMove move)
    {
        _spriteRenderer.color = Color.green;
        
        _playerMovement.SetMovement(false);
        
        StartCoroutine(WaitForFrames(move.startupFrames, () => Active(move)));
    }

    // Frames where attack is active
    void Active(AttackMove move)
    {
        // Activate the hitbox boxes
        _blockingBoxes.SetActive(false);
        var transform1 = transform;
        _boxes = Instantiate(move.boxes, transform1.position, transform1.rotation);
        _boxes.transform.SetParent(transform);
        
        // Change player appearance
        if (!isPlayerOne)
        {
            _boxes.transform.Find("Hexagon Flat-Top").GetComponent<SpriteRenderer>().color = enemyColor;
        }
        _playerMovement.FlipHitboxModel();
        
        // Set hitbox to be active so that it can be checked in the Update method
        _hitboxActive = true;
        _hitboxes = _boxes.transform.Find("Hitbox").GetComponentsInChildren<Collider2D>();

        StartCoroutine(WaitForFrames(move.activeFrames, () => Recovery(move)));
    }

    // Frames where player cannot do anything after an attack
    void Recovery(AttackMove move)
    {
        _spriteRenderer.color = Color.blue;
        
        // Remove hitbox again and get normal hurtbox
        Destroy(_boxes);
        _blockingBoxes.SetActive(true);
        _hitboxActive = false;
        
        StartCoroutine(WaitForFrames(move.recoveryFrames, ResetState));
    }

    // Reset the state of the player to normal after attack is over.
    void ResetState()
    {
        _spriteRenderer.color = Color.white;
        
        _playerMovement.SetMovement(true);
        _playerMovement.enabled = true;
        _normals.enabled = true;
        _blocking.SetBlock(true);
    }
    
    // Perform an action after a certain amount of frames.
    private IEnumerator WaitForFrames(int frames, Action action)
    {
        int initialFrame = _frames.CurrentFrame;
        yield return new WaitUntil(() => _frames.CurrentFrame - initialFrame > frames);
        action();
    }
}
