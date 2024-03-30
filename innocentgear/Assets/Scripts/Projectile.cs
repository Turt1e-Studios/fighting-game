using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private bool _facingRight;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(1, 0, 0) * (speed * Time.deltaTime));
    }
}
