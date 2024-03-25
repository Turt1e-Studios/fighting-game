using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private bool _facingRight;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float multiplier = _facingRight ? 1 : -1;
        transform.Translate(new Vector3(1, 0, 0) * (speed * Time.deltaTime));
    }

    // Should be 1 or -1
    public void SetDirection(bool facingRight)
    {
        // actually completely ignore this
        print("facing right: " + facingRight);
        _facingRight = facingRight;
    }
}
