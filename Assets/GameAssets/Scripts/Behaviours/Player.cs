using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public int playerIndex { get { return input.myPlayerIndex; } }

    public int points = 0;
    
    public Color color = Color.white;

    [SerializeField] private SpriteRenderer indicator;
    [SerializeField] private float speed = 10;
    [SerializeField] private float maxVelocityChange = 10.0f;

    
    private PlayerInput input;
    private new Rigidbody rigidbody;
    private new Animator animator;


    void Awake()
    {
        input = this.GetComponent<PlayerInput>();
        rigidbody = this.GetComponent<Rigidbody>();
        animator = this.GetComponentInChildren<Animator>();

        if (!indicator)
            Debug.Log("Please assing the child who has sprite renderer to be indicator");
        else
        {
            indicator.color = color;
        }

        points = 0;
        rigidbody.freezeRotation = true;
    }

    void Update()
    {
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        animator.SetFloat("Horizontal", input.axisHorizontal);
        animator.SetFloat("Vertical", input.axisVertical);
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
