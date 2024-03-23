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
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        
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
        }
        else if (_playerInput.CurrentDirection is 4 or 7) // High blocked
        {
            standingBoxes.SetActive(true);
            crouchingBoxes.SetActive(false);
            _activeBoxes = standingBoxes;
            _spriteRenderer.color = Color.magenta;
            _lowBlock = false;
            _highBlock = true;
        }
        else // Not blocked
        {
            standingBoxes.SetActive(true);
            crouchingBoxes.SetActive(false);
            _activeBoxes = standingBoxes;
            _spriteRenderer.color = Color.white;
            _lowBlock = false;
            _highBlock = false;
        }
    }
    
    // Player recieves and checks and attack
    public void RecieveBlock()
    {
        print("got hit!");
    }

    // Prevents regular hitboxes from interfering when a player attacks
    public void SetBlock(bool block)
    {
        _canBlock = block;
    }
}
