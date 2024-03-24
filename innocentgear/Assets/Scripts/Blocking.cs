using System;
using System.Collections;
using UnityEngine;

// Determines blocking behavior.
public class Blocking : MonoBehaviour
{
    [Header("Boxes")]
    [SerializeField] private GameObject standingBoxes;
    [SerializeField] private GameObject crouchingBoxes;
    [SerializeField] private float horizontalKnockback;
    [SerializeField] private float verticalKnockback;
    
    private PlayerInput _playerInput;
    private GameObject _activeBoxes;
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _playerMovement;
    private bool _canBlock;
    private bool _lowBlock;
    private bool _highBlock;
    private bool _isBlocking;
    private Frames _frames;
    private Health _health;
    private PlayerState _playerState;
    private bool _inCounterHit;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _frames = GameObject.Find("GameManager").GetComponent<Frames>();
        _health = GetComponent<Health>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerState = GetComponent<PlayerState>();
        
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
        
        if (!_canBlock) {
            return; // Needs to be able to block
        }
        print(gameObject + " " + _canBlock);
        
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
    public void RecieveBlock(AttackMove move, bool opponentGrounded)
    {
        if (move.isHigh && !_highBlock || move.isLow && !_lowBlock)
        {
            GetHit(move.activeFrames + move.recoveryFrames + move.onHit + HitStop(move.level), move.damage, opponentGrounded, move.counterHit);
        }
        else if (_isBlocking)
        {
            GetBlocked(move.activeFrames + move.recoveryFrames + move.onBlock);
        }
        else
        {
            GetHit(move.activeFrames + move.recoveryFrames + move.onHit, move.damage + HitStop(move.level), opponentGrounded, move.counterHit);
        }
    }

    private int HitStop(int level)
    {
        return 11 + level;
    }

    private int HitFramesFromLevel(int level)
    {
        return level switch
        {
            0 => 12,
            1 => 14,
            2 => 16,
            3 => 19,
            4 => 21,
            _ => 12
        };
    }
    
    private int BlockFramesFromLevel(int level)
    {
        if (transform.position.y <= -1f)
        {
            return HitFramesFromLevel(level) - 3;
        }
        else if (transform.position.y <= 0.5f)
        {
            return 5;
        }
        else
        {
            return 19;
        }
    }

    public bool InCounterHit()
    {
        return _inCounterHit;
    }

    private void GetHit(int hitstun, int damage, bool opponentGrounded, int counterHit)
    {
        float multiplier = 1f;
        int extraFrames = 0;
        float knockbackMultiplier = 1f;
        if (_playerState.IsRecovering())
        {
            print("counter hit!");
            _inCounterHit = true;
            multiplier = 1.1f;
            extraFrames = FrameAdvFromCounter(counterHit) + HitstopFromCounter(counterHit);
            knockbackMultiplier = 1f + (float) FrameAdvFromCounter(counterHit) / 36;
            SlowDown(SlowdownFromCounter(counterHit));
        }
        
        DisableControls();
        _health.Damage((int) (damage * multiplier));
        _spriteRenderer.color = Color.gray;

        float groundFactor = opponentGrounded ? 1 : 0; // only vertical knockback if grounded
        GetComponent<Rigidbody2D>().AddForce(knockbackMultiplier * Mathf.Sqrt(damage) * new Vector2(-1 * _playerMovement.GetDisplacement() / Math.Abs(_playerMovement.GetDisplacement()) * horizontalKnockback, groundFactor * verticalKnockback));
        StartCoroutine(WaitForFrames(hitstun + extraFrames, ReEnable));
    }

    private void SlowDown(int frames)
    {
        _playerMovement.GravityDown();
        StartCoroutine(WaitForFrames(frames, SpeedUp));
    }

    private void SpeedUp()
    {
        _playerMovement.GravityUp();
    }

    private int SlowdownFromCounter(int level)
    {
        return level switch
        {
            0 => 0,
            1 => 11,
            2 => 25,
            3 => 35,
            _ => 0
        };
    }

    private int FrameAdvFromCounter(int level)
    {
        return level switch
        {
            0 => 0,
            1 => 6,
            2 => 13,
            3 => 18,
            _ => 0
        };
    }

    private int HitstopFromCounter(int level)
    {
        return level switch
        {
            0 => 0,
            1 => 0,
            2 => 21,
            3 => 31,
            _ => 0
        };
    }

    private void GetBlocked(int blockstun)
    {
        DisableControls();
        
        _spriteRenderer.color = Color.yellow;
        StartCoroutine(WaitForFrames(blockstun, ReEnable));
    }

    private void DisableControls()
    {
        print("disabled controls");
        SetBlock(false);
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Normals>().enabled = false;
        //GetComponent<PlayerState>().enabled = false;
    }

    private void ReEnable()
    {
        SetBlock(true);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Normals>().enabled = true;
        _inCounterHit = false;
        //GetComponent<PlayerState>().enabled = true;
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
