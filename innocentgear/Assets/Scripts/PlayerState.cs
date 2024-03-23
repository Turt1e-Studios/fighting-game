using System;
using System.Collections;
using UnityEngine;

// Controls state of player when performing an attack.
public class PlayerState : MonoBehaviour
{
    [Header("Misc Player Info")]
    [SerializeField] private bool isPlayerOne;
    [SerializeField] private Color enemyColor;
    [SerializeField] private LayerMask enemyLayer;
    
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
            print("activated against " + _enemyLayer);
            foreach (Collider2D hitbox in _hitboxes)
            {
                if (hitbox.IsTouchingLayers(enemyLayer))
                {
                    print("touching enemy layer " + _enemyLayer);
                    _playerMovement.GetEnemy().GetComponent<Blocking>().RecieveBlock(_currentMove, transform.position.y <= -1f);
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
        if (move.isLow)
        {
            _spriteRenderer.color = new Color(0, 0.5f, 0);
        }
        else if (move.isHigh)
        {
            _spriteRenderer.color = new Color(0.5f, 1f, 0.5f);
        }
        else
        {
            _spriteRenderer.color = Color.green;
        }

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
        
        // Make player two be able to be hit during attack
        if (!isPlayerOne)
        {
            foreach (Transform child in _boxes.transform.Find("Hurtbox").transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Hurtbox2");
            }
        }
        
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
        // Remove hitbox again and get normal hurtbox
        Destroy(_boxes.transform.Find("Hitbox").gameObject);
        _hitboxActive = false;
        
        _boxes.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.blue;
        
        StartCoroutine(WaitForFrames(move.recoveryFrames, ResetState));
    }

    // Reset the state of the player to normal after attack is over.
    void ResetState()
    {
        _spriteRenderer.color = Color.white;
        
        Destroy(_boxes);
        _blockingBoxes.SetActive(true);
        
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
