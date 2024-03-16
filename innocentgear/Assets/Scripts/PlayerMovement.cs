using System;
using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
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

    public bool IsGrounded()
    {
        return _isGrounded;
    }
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check for double presses
        if (_isGrounded)
        {
            CheckDoublePress(KeyCode.D, () => _sprinting = true, () => _sprinting = false);
            CheckDoublePress(KeyCode.A, GroundDash);
            
            CheckPreviousKey(KeyCode.A, KeyCode.S, () => _superJumpLeft = true, () => _superJumpLeft = false);
            CheckPreviousKey(KeyCode.D, KeyCode.S, () => _superJumpRight = true, () => _superJumpRight = false);

            if (_superJumpLeft && !_superJumpRight)
            {
                CheckPreviousKey(KeyCode.W, KeyCode.A, SuperJump);
            }
            else if (_superJumpRight && !_superJumpLeft)
            {
                CheckPreviousKey(KeyCode.W, KeyCode.D, SuperJump);
            }
            else
            {
                CheckPreviousKey(KeyCode.W, KeyCode.S, SuperJump);
            }
        }
        else
        {
            CheckDoublePress(KeyCode.D, AirDashForwards);
            CheckDoublePress(KeyCode.A, AirDashBackwards);
        }
        
        // Move player horizontally
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),0);
        _velocity = input.normalized;
        float moveSpeed = runSpeed;
        if (_sprinting)
        {
            moveSpeed = sprintSpeed;
        }
        transform.Translate(Time.deltaTime * (moveSpeed *  _velocity + _changeInVelocity));

        // Check for jumps, on ground or double jumping
        if (Input.GetKeyDown(KeyCode.W) && (_isGrounded || !_usedAirMove))
        {
            if (!_isGrounded)
            {
                _usedAirMove = true;
            }
            _rigidbody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    // Reset velocity quickly after dashing
    void GroundDash()
    {
        _rigidbody2D.velocity = new Vector2(-1f, 0f) * groundDashSpeed;
        StartCoroutine(ResetVelocity(groundDashDuration));
    }
    
    private IEnumerator ResetVelocity (float interval)
    {
        yield return new WaitForSeconds(interval);
        _rigidbody2D.velocity = new Vector2(0, 0);
    }

    void SuperJump()
    {
        if (_usedAirMove) return;

        Vector2 direction = Vector2.up.normalized;
        if (_superJumpLeft)
        {
            //print("super jump left.");
            direction = (Vector2.left + Vector2.up).normalized;
        }
        else if (_superJumpRight)
        {
            //print("super jump right.");
            direction = (Vector2.right + Vector2.up).normalized;
        }
        _rigidbody2D.AddForce(direction * superJumpSpeed, ForceMode2D.Impulse);

        if (_superJumpLeft) _superJumpLeft = false;
        if (_superJumpRight) _superJumpRight = false;
        
        _usedAirMove = true;
    }

    void AirDashBackwards()
    {
        if (_airDashed) return;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        _rigidbody2D.AddForce(Vector2.left * backwardsAirDashSpeed, ForceMode2D.Impulse);
        StartCoroutine(SetGravity(airDashBackwardsDuration));
        _airDashed = true;
    }

    void AirDashForwards()
    {
        if (_airDashed) return;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        _rigidbody2D.AddForce(Vector2.right * forwardsAirDashSpeed, ForceMode2D.Impulse);
        StartCoroutine(SetGravity(airDashForwardsDuration));
        _airDashed = true;
    }

    // Prevent player from falling when air dashing
    private IEnumerator SetGravity(float interval)
    {
        yield return new WaitForSeconds(interval);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Player enters the ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(("Ground")))
        {
            _isGrounded = true;
            _usedAirMove = false;
            _airDashed = false;
        }
    }
 
    // Player leaves the ground
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(("Ground")))
        {
            _isGrounded = false;
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
    
    void CheckPreviousKey(KeyCode key, KeyCode previousKey, Action action, Action negativeAction = null)
    {
        //print(key + " " + previousKey + " " + action);

        if (Input.GetKeyDown(previousKey))
        {
            _previousKey = previousKey;
            _pressedFirstTimeDouble = true;
            _lastPressedTime = Time.time;
            return;
        }

        if (Input.GetKeyDown(key))
        {
            //print(_pressedFirstTime + " " + (_previousKey == previousKey) + " " + (Time.time - _lastPressedTime <= DelayBetweenPresses));

            if (_pressedFirstTime && _previousKey != previousKey)
            {
                _pressedFirstTimeDouble = false;
            }
            
            // If already pressed and second time hit within time limit
            if (_pressedFirstTimeDouble && Time.time - _lastPressedTime <= DelayBetweenPresses)
            {
                action();
                _pressedFirstTimeDouble = false;
            }
            else // Hit the first time normally
            {
                _pressedFirstTimeDouble = true;
                //_previousKey = key;
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
}

