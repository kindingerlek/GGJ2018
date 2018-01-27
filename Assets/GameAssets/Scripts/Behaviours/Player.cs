using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int playerIndex;

    [SerializeField] private float speed = 2f;
    [SerializeField] public float maxVelocityChange = 10.0f;
    [SerializeField] public bool canJump = true;
    [SerializeField] public float jumpHeight = 2.0f;
    [SerializeField] private bool grounded = false;



    private Vector3 moveDirection;
    private PlayerInput input;
    private new Rigidbody rigidbody;



    void Awake()
    {
        rigidbody.freezeRotation = true;
        rigidbody.useGravity = false;
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = input.worldAxisDirectionClamped;
            targetVelocity *= speed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rigidbody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        
    }
}
