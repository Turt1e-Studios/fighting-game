using System;
using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

// Controls the general movement of the player.
public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject enemyPlayer;
    [SerializeField] private bool isPlayerOne;
    [Header("Movement Values")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float groundDashSpeed;
    [SerializeField] private float backwardsAirDashSpeed;
    [SerializeField] private float forwardsAirDashSpeed;
    [SerializeField] private float groundDashDuration = 1f;
    [SerializeField] private float airDashBackwardsDuration = 0.2f;
    [SerializeField] private float airDashForwardsDuration = 0.4f;
    [SerializeField] private float superJumpSpeed;
    [Header("Movement Keys")]
    [SerializeField] private KeyCode upMovementKey;
    [SerializeField] private KeyCode downMovementKey;
    [SerializeField] private KeyCode leftMovementKey;
    [SerializeField] private KeyCode rightMovementKey;
    

    private const float DelayBetweenPresses = 0.25f;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _velocity;
    private Vector2 _changeInVelocity;
    private KeyCode _doublePressedKey;
    private KeyCode _previousKey;
    private float _lastPressedTime;
    private bool _isGrounded;
    private bool _usedAirMove;
    private bool _pressedFirstTime;
    private bool _pressedFirstTimeDouble;
    private bool _sprinting;
    private bool _airDashed;
    private bool _superJumpLeft;
    private bool _superJumpRight;
    private String _player;
    private float _oppositeDirectionMultiplier = 1;
    private bool _isFlipped;
    private float _enemyDisplacement;
    private PlayerInput _playerInput;
    private Transform _transform;
    private bool _canMove;

    // Returns whether the player is grounded
    public bool IsGrounded()
    {
        return _isGrounded;
    }

    // Returns the enemy player object
    public GameObject GetEnemy()
    {
        return enemyPlayer;
    }

    public float GetDisplacement()
    {
        return _enemyDisplacement;
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _transform = GetComponent<Transform>();

        // Set information based on which player it is
        _player = isPlayerOne ? "Horizontal1" : "Horizontal2";
        _enemyDisplacement = isPlayerOne ? 1f : -1f;
        if (!isPlayerOne)
        {
            FlipControls();
        }
        _canMove = true;
    }

    // Flip the controls, movement, and appearance of the player
    private void FlipControls()
    {
        _isFlipped = !_isFlipped;
        (leftMovementKey, rightMovementKey) = (rightMovementKey, leftMovementKey);
        _oppositeDirectionMultiplier *= -1;
        _playerInput.ChangeDirection();
        Vector3 flippedScale = _transform.localScale;
        flippedScale.x *= -1;
        _transform.localScale = flippedScale;
    }
    
    // Check whether two numbers have opposite signs
    private bool OppositeSigns(float x, float y)
    {
        return x < 0? y >= 0: y < 0;
    }

    // Only flip the model of the player
    public void FlipHitboxModel()
    {
        if (_isFlipped)
        {
            Vector3 flippedScale = _transform.localScale;
            flippedScale.x *= -1;
            _transform.localScale = flippedScale;
        }
    }

    public bool IsFlipped()
    {
        return _enemyDisplacement < 0;
    }

    // Flip controls if players switch sides
    private void CheckFlipControls()
    {
        if (OppositeSigns(enemyPlayer.transform.position.x - _transform.position.x, _enemyDisplacement))
        {
            FlipControls();
        }
    }

    public void SetMovement(bool set)
    {
        _canMove = set;
    }

    void Update()
    {
        if (!_canMove) return;
            
        CheckFlipControls();
        _enemyDisplacement = enemyPlayer.transform.position.x - _transform.position.x;
        
        // Check for various double press commands
        if (_isGrounded)
        {
            CheckDoublePress(rightMovementKey, () => _sprinting = true, () => _sprinting = false);
            CheckDoublePress(leftMovementKey, GroundDash);
            
            CheckPreviousKey(leftMovementKey, downMovementKey, () => _superJumpLeft = true, () => _superJumpLeft = false);
            CheckPreviousKey(rightMovementKey, downMovementKey, () => _superJumpRight = true, () => _superJumpRight = false);

            if (_superJumpLeft && !_superJumpRight)
            {
                CheckPreviousKey(upMovementKey, leftMovementKey, SuperJump);
            }
            else if (_superJumpRight && !_superJumpLeft)
            {
                CheckPreviousKey(upMovementKey, rightMovementKey, SuperJump);
            }
            else
            {
                CheckPreviousKey(upMovementKey, downMovementKey, SuperJump);
            }
        }
        else
        {
            CheckDoublePress(rightMovementKey, AirDashForwards);
            CheckDoublePress(leftMovementKey, AirDashBackwards);
        }
        
        // Move player horizontally
        Vector2 input = new Vector2(Input.GetAxisRaw(_player),0);
        _velocity = input.normalized;
        float moveSpeed = runSpeed;
        if (_sprinting)
        {
            moveSpeed = sprintSpeed;
        }
        _transform.Translate(Time.deltaTime * (moveSpeed *  _velocity + _changeInVelocity));

        // Check for jumps on the ground or in the air
        if (Input.GetKeyDown(upMovementKey) && (_isGrounded || !_usedAirMove))
        {
            if (!_isGrounded)
            {
                _usedAirMove = true;
            }
            _rigidbody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
        
        CheckIfGrounded();
    }

    private void CheckIfGrounded()
    {
        _isGrounded = transform.position.y < -1.0f;
    }

    // Reset velocity quickly after dashing
    void GroundDash()
    {
        _rigidbody2D.velocity = new Vector2(-1f * _oppositeDirectionMultiplier, 0f) * groundDashSpeed;
        StartCoroutine(ResetVelocity(groundDashDuration));
    }
    
    // Reset the player's velocity after a certain amount of time
    private IEnumerator ResetVelocity (float interval)
    {
        yield return new WaitForSeconds(interval);
        _rigidbody2D.velocity = new Vector2(0, 0);
    }

    // Super jump command
    void SuperJump()
    {
        if (_usedAirMove) return;

        Vector2 direction = Vector2.up.normalized;
        if (_superJumpLeft)
        {
            direction = (Vector2.left * _oppositeDirectionMultiplier + Vector2.up).normalized;
        }
        else if (_superJumpRight)
        {
            direction = (Vector2.right * _oppositeDirectionMultiplier + Vector2.up).normalized;
        }
        _rigidbody2D.AddForce(direction * superJumpSpeed, ForceMode2D.Impulse);

        if (_superJumpLeft) _superJumpLeft = false;
        if (_superJumpRight) _superJumpRight = false;
        
        _usedAirMove = true;
    }

    // Air dash backwards command
    void AirDashBackwards()
    {
        if (_airDashed) return;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        _rigidbody2D.AddForce(Vector2.left * (backwardsAirDashSpeed * _oppositeDirectionMultiplier), ForceMode2D.Impulse);
        StartCoroutine(SetGravity(airDashBackwardsDuration));
        _airDashed = true;
    }

    // Air dash forwards command
    void AirDashForwards()
    {
        if (_airDashed) return;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        _rigidbody2D.AddForce(Vector2.right * (forwardsAirDashSpeed * _oppositeDirectionMultiplier), ForceMode2D.Impulse);
        StartCoroutine(SetGravity(airDashForwardsDuration));
        _airDashed = true;
    }

    // Prevent player from falling when air dashing
    private IEnumerator SetGravity(float interval)
    {
        yield return new WaitForSeconds(interval);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Reset variables after hitting the ground again
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(("Ground")))
        {
            //_isGrounded = true;
            _usedAirMove = false;
            _airDashed = false;
        }
    }
 
    // Player leaves the ground
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(("Ground")))
        {
            //_isGrounded = false;
        }
    }

    // Calls the function "action" upon a double press. Activates negativeAction upon the end of the double press.
    void CheckDoublePress(KeyCode key, Action action, Action negativeAction = null)
    {
        if (Input.GetKeyDown(key))
        {
            // Pressed a key already but a different key
            if (_pressedFirstTime && key != _doublePressedKey)
            {
                _pressedFirstTime = false;
            }
            
            // If already pressed and second time hit within time limit
            if (_pressedFirstTime && Time.time - _lastPressedTime <= DelayBetweenPresses)
            {
                action();
                _pressedFirstTime = false;
            }
            else // Hit the first time normally
            {
                _pressedFirstTime = true;
                _doublePressedKey = key;
            }
            _lastPressedTime = Time.time;
        }
        // Reset boolean if button wasn't pressed soon enough
        if (_pressedFirstTime && Time.time - _lastPressedTime > DelayBetweenPresses)
        {
            _pressedFirstTime = false;
        }
        // Remove effect when button not pressed
        if (!_pressedFirstTime && Input.GetKeyUp(key))
        {
            negativeAction?.Invoke();
        }
    }
    
    // Perform a command after two consecutive key presses
    void CheckPreviousKey(KeyCode key, KeyCode previousKey, Action action, Action negativeAction = null)
    {
        if (Input.GetKeyDown(previousKey))
        {
            _previousKey = previousKey;
            _pressedFirstTimeDouble = true;
            _lastPressedTime = Time.time;
            return;
        }
        
        if (Input.GetKeyDown(key))
        {
            if (_pressedFirstTime && _previousKey != previousKey)
            {
                _pressedFirstTimeDouble = false;
            }
            
            if (_pressedFirstTimeDouble && Time.time - _lastPressedTime <= DelayBetweenPresses)
            {
                action();
                _pressedFirstTimeDouble = false;
            }
            else // Hit the first time normally
            {
                _pressedFirstTimeDouble = true;
            }
            _lastPressedTime = Time.time;
        }
        // Reset boolean if button wasn't pressed soon enough
        if (_pressedFirstTimeDouble && Time.time - _lastPressedTime > DelayBetweenPresses)
        {
            _pressedFirstTime = false;
            negativeAction?.Invoke();
        }
        // Remove effect when button not pressed
        if (!_pressedFirstTime && Input.GetKeyUp(key))
        {
            negativeAction?.Invoke();
        }
    }

    public void GravityDown()
    {
        _rigidbody2D.gravityScale /= 2;
    }

    public void GravityUp()
    {
        _rigidbody2D.gravityScale *= 2;
    }
}

