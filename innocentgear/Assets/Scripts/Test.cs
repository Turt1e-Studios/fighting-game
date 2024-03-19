using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    private Collider2D col;
    
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, mask);
        print(cols);
        foreach(Collider c in cols) Debug.Log(c.name);

        if (col.IsTouchingLayers(mask))
        {
            print("finally bruh");
        }
    }

    private void FixedUpdate()
    {
        if (col.IsTouchingLayers(mask))
        {
            print("finally bruh");
        }
    }
}
