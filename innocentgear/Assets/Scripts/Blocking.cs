using System;
using System.Collections;
using UnityEngine;

// Determines blocking behavior.
public class Blocking : MonoBehaviour
{
    [Header("Boxes")]
    [SerializeField] private GameObject standingBoxes;
    [SerializeField] private GameObject crouchingBoxes;
    
    private PlayerInput _playerInput;
    private GameObject _activeBoxes;
    private SpriteRenderer _spriteRenderer;
    private bool _canBlock;
    private bool _lowBlock;
    private bool _highBlock;
    private bool _isBlocking;
    private Frames _frames;
    private Health _health;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _frames = GameObject.Find("GameManager").GetComponent<Frames>();
        _health = GetComponent<Health>();
        
        standingBoxes.SetActive(true);
        crouchingBoxes.SetActive(false);
        _activeBoxes = standingBoxes;
        
        _canBlock = true;
    }

    // Returns the normal boxes that are currently being used by the player
    public GameObject GetCurrentBoxes()
    {
        return _activeBoxes;
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderer = _activeBoxes.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        
        if (!_canBlock) return; // Needs to be able to block
        if (_playerInput.CurrentDirection is 1 or 2) // Low blocked
        {
            standingBoxes.SetActive(false);
            crouchingBoxes.SetActive(true);
            _activeBoxes = crouchingBoxes;
            _spriteRenderer.color = Color.cyan;
            _lowBlock = true;
            _highBlock = false;
            _isBlocking = true;
        }
        else if (_playerInput.CurrentDirection is 4 or 7) // High blocked
        {
            standingBoxes.SetActive(true);
            crouchingBoxes.SetActive(false);
            _activeBoxes = standingBoxes;
            _spriteRenderer.color = Color.magenta;
            _lowBlock = false;
            _highBlock = true;
            _isBlocking = true;
        }
        else // Not blocked
        {
            standingBoxes.SetActive(true);
            crouchingBoxes.SetActive(false);
            _activeBoxes = standingBoxes;
            _spriteRenderer.color = Color.white;
            _lowBlock = false;
            _highBlock = false;
            _isBlocking = false;
        }
    }
    
    // Player recieves and checks and attack
    public void RecieveBlock(AttackMove move)
    {
        if (move.isHigh && !_highBlock || move.isLow && !_lowBlock)
        {
            GetHit(move.activeFrames + move.recoveryFrames + move.onHit, move.damage);
        }
        else if (_isBlocking)
        {
            GetBlocked(move.activeFrames + move.recoveryFrames + move.onBlock);
        }
        else
        {
            GetHit(move.activeFrames + move.recoveryFrames + move.onHit, move.damage);
        }
    }

    private void GetHit(int hitstun, int damage)
    {
        DisableControls();
        _health.Damage(damage);
        _spriteRenderer.color = Color.gray;
        StartCoroutine(WaitForFrames(hitstun, ReEnable));
    }

    private void GetBlocked(int blockstun)
    {
        DisableControls();
        
        _spriteRenderer.color = Color.yellow;
        StartCoroutine(WaitForFrames(blockstun, ReEnable));
    }

    private void DisableControls()
    {
        SetBlock(false);
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Normals>().enabled = false;
        GetComponent<PlayerState>().enabled = false;
    }

    private void ReEnable()
    {
        SetBlock(true);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Normals>().enabled = true;
        GetComponent<PlayerState>().enabled = true;
    }

    // Prevents regular hitboxes from interfering when a player attacks
    public void SetBlock(bool block)
    {
        _canBlock = block;
    }
    
    // Perform an action after a certain amount of frames.
    private IEnumerator WaitForFrames(int frames, Action action)
    {
        int initialFrame = _frames.CurrentFrame;
        yield return new WaitUntil(() => _frames.CurrentFrame - initialFrame > frames);
        action();
    }
}
