using UnityEngine;

public class Normals : MonoBehaviour
{
    // See https://www.dustloop.com/w/Notation for fighting game notation

    [SerializeField] private BoxCollider2D[] attackHitboxes;
    // Order of hitboxes: 5P
    
    private PlayerInput _playerInput;
    private PlayerState _playerState;
    private PlayerMovement _playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerState = GetComponent<PlayerState>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerMovement.IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.C) && _playerInput.CurrentDirection is 5 or 4)
            {
                FiveP();
            }
        }
    }
    
    void FiveP()
    {
        _playerState.Move(attackHitboxes[0], 4, 5, 9);
    }
}
