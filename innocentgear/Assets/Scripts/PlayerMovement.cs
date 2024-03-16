using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpSpeed;
    
    private Vector2 _velocity;
    private bool _isGrounded;
    private bool _usedAirMove;

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),0);
        _velocity = input.normalized;

        transform.Translate(runSpeed * Time.deltaTime * _velocity);

        if (Input.GetButtonDown("Jump") && (_isGrounded || !_usedAirMove))
        {
            if (!_isGrounded)
            {
                _usedAirMove = true;
            }
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(("Ground")))
        {
            _isGrounded = true;
            _usedAirMove = false;
        }
    }
 
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(("Ground")))
        {
            _isGrounded = false;
        }
    }
}

