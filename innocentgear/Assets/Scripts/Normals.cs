using UnityEngine;

public class Normals : MonoBehaviour
{
    // See https://www.dustloop.com/w/Notation for fighting game notation

    //[SerializeField] private BoxCollider2D[] attackHitboxes;
    [SerializeField] private KeyCode punchKey;

    [SerializeField] private AttackMove fiveP;
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
            if (Input.GetKeyDown(punchKey) && _playerInput.CurrentDirection is 5 or 4)
            {
                _playerState.Move(fiveP);
            }
        }
    }
}
