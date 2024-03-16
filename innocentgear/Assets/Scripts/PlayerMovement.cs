using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float groundDashSpeed;

    private const float DelayBetweenPresses = 0.25f;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _velocity;
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
        CheckDoublePress(KeyCode.D, () => _sprinting = true, () => _sprinting = false);
        CheckDoublePress(KeyCode.A, Dash);

        // Move player horizontally
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),0);
        _velocity = input.normalized;
        float moveSpeed = runSpeed;
        if (_sprinting)
        {
            moveSpeed = sprintSpeed;
        }
        transform.Translate(moveSpeed * Time.deltaTime * _velocity);

        // Check for jumps, on ground or double jumping
        if (Input.GetButtonDown("Jump") && (_isGrounded || !_usedAirMove))
        {
            if (!_isGrounded)
            {
                _usedAirMove = true;
            }
            _rigidbody2D.AddForce(Vector2.up * jumpSpeed);
        }
    }

    void Dash()
    {
        _rigidbody2D.AddForce(Vector2.left * groundDashSpeed);
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

