using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [HideInInspector]
    public int playerIndex;
    public int points = 0;

    [SerializeField] private float speed = 10;
    [SerializeField] public float maxVelocityChange = 10.0f;
    [SerializeField] public bool canJump = true;
    [SerializeField] public float jumpHeight = 2.0f;


    
    private PlayerInput input;
    private new Rigidbody rigidbody;


    void Awake()
    {
        input = this.GetComponent<PlayerInput>();
        rigidbody = this.GetComponent<Rigidbody>();

        points = 0;

        playerIndex = input.myPlayerIndex;
        rigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // Calculate how fast we should be moving
        Vector3 targetVelocity = input.worldAxisDirectionClamped;
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rigidbody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Infectable infectable = collision.gameObject.GetComponent<Infectable>();

        if (infectable == null)
            return;

        infectable.Infect(this.gameObject);
            
    }
}
