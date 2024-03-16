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
    private float _lastPressedTime;
    private bool _isGrounded;
    private bool _usedAirMove;
    private bool _pressedFirstTime;
    private bool _sprinting;

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
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
            {
                SuperJump();
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

    void GroundDash()
    {
        // Wanted to make it transform based instead of rigidbody based but I give up
        
        //StartCoroutine(ChangeVelocity(groundDashSpeed * Vector2.left));
        //ChangeVelocity(groundDashSpeed * Vector2.left);
        //Invoke(ChangeVelocity(groundDashSpeed * Vector2.right), 3f);
        _rigidbody2D.velocity = new Vector2(-1f, 0f) * groundDashSpeed;
        //_rigidbody2D.AddForce(Vector2.left * groundDashSpeed, ForceMode2D.Impulse);
        StartCoroutine(ResetVelocity(groundDashDuration));
    }
    
    private IEnumerator ResetVelocity (float interval)
    {
        yield return new WaitForSeconds(interval);
        _rigidbody2D.velocity = new Vector2(0, 0);
    }

    IEnumerator ChangeVelocity(Vector2 velocity)
    {
        yield return new WaitForSeconds(3f);
        _changeInVelocity += velocity;
    }

    void SuperJump()
    {
        if (_usedAirMove) return;
        _rigidbody2D.AddForce(Vector2.up * superJumpSpeed, ForceMode2D.Impulse);
        _usedAirMove = true;
    }

    void AirDashBackwards()
    {
        float originalGravity = _rigidbody2D.gravityScale;
        //_rigidbody2D.gravityScale = 0f;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        _rigidbody2D.AddForce(Vector2.left * backwardsAirDashSpeed, ForceMode2D.Impulse);
        StartCoroutine(SetGravity(airDashBackwardsDuration));
    }

    void AirDashForwards()
    {
        float originalGravity = _rigidbody2D.gravityScale;
        //_rigidbody2D.gravityScale = 0f;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        _rigidbody2D.AddForce(Vector2.right * forwardsAirDashSpeed, ForceMode2D.Impulse);
        StartCoroutine(SetGravity(airDashForwardsDuration));
    }

    private IEnumerator SetGravity(float interval)
    {
        yield return new WaitForSeconds(interval);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        //_rigidbody2D.gravityScale = gravity;
    }

    // Player enters the ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(("Ground")))
        {
            _isGrounded = true;
            _usedAirMove = false;
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
}

