using UnityEngine;

public class Blocking : MonoBehaviour
{
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

    public GameObject GetCurrentBoxes()
    {
        return _activeBoxes;
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderer = _activeBoxes.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        if (!_canBlock) return;
        if (_playerInput.CurrentDirection is 1 or 2)
        {
            standingBoxes.SetActive(false);
            crouchingBoxes.SetActive(true);
            _activeBoxes = crouchingBoxes;
            _spriteRenderer.color = Color.cyan;
            _lowBlock = true;
            _highBlock = false;
        }
        else if (_playerInput.CurrentDirection is 4 or 7)
        {
            standingBoxes.SetActive(true);
            crouchingBoxes.SetActive(false);
            _activeBoxes = standingBoxes;
            _spriteRenderer.color = Color.magenta;
            _lowBlock = false;
            _highBlock = true;
        }
        else
        {
            standingBoxes.SetActive(true);
            crouchingBoxes.SetActive(false);
            _activeBoxes = standingBoxes;
            _spriteRenderer.color = Color.white;
            _lowBlock = false;
            _highBlock = false;
        }
    }
    
    public void RecieveBlock()
    {
        print("got hit!");
    }

    public void SetBlock(bool block)
    {
        _canBlock = block;
    }
}
