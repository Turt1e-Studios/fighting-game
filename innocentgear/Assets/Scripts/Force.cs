using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    [SerializeField] private Vector2 force;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement playerMovement = transform.parent.GetComponent<PlayerMovement>();
        int direction = playerMovement.GetDisplacement() > 0 ? 1 : -1;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x * direction, force.y), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
