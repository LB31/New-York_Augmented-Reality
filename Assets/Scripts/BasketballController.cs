using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballController : MonoBehaviour
{
    public float force = 10;

    private Rigidbody rg;
    private float gravity = 9.81f;

    public Collider parentCollider;


    void Start()
    {
        rg = GetComponent<Rigidbody>();
        parentCollider = transform.parent.GetComponent<Collider>();
    }

    private void OnMouseDown()
    {
        rg.AddForce(transform.parent.TransformDirection(Vector3.up * force));
    }

    private void Update()
    {
        // manual gravity
        if (parentCollider.enabled)
        {
            rg.AddForce(transform.parent.TransformDirection(Vector3.down * gravity));
        }

        // reset ball when outside of field
        if(transform.position.magnitude > 9)
        {
            transform.localPosition = new Vector3(0, 0.2f, 0);
            rg.velocity = Vector3.zero;
            rg.angularVelocity = Vector3.zero;
        }


    }
}
