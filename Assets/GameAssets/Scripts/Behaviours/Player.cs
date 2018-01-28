using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {

    public int playerIndex { get { return input.myPlayerIndex; } }

    public int points = 0;

    [SerializeField] private GameObject spriteRoot;
    [SerializeField] private SpriteRenderer indicator;
    [SerializeField] private float speed = 10;
    [SerializeField] private float maxVelocityChange = 10.0f;
    [SerializeField] private float stealPointsPerSecond = 1;
    [SerializeField] Collider effectArea;

    readonly HashSet<Player> playersInArea = new HashSet<Player>();
    readonly Dictionary<Player, float> stealCredits = new Dictionary<Player, float>();

    private PlayerInput input;
    private new Rigidbody rigidbody;
    private new Animator animator;


    void Awake()
    {
        input = this.GetComponent<PlayerInput>();

        if (!indicator)
            Debug.Log("Please assing the child who has sprite renderer to be indicator");
        else
        {
            indicator.color = GameManager.Instance.GetPlayerColor(playerIndex);
        }

        points = 0;
    }

    void Start()
    {
        LoadSelectedCharacter();

        rigidbody = this.GetComponent<Rigidbody>();
        animator = this.GetComponentInChildren<Animator>();
        rigidbody.freezeRotation = true;
    }

    private void LoadSelectedCharacter()
    {
        var spritePrefab = GameManager.Instance.GetPlayerSpritePrefab(playerIndex);
        if (spritePrefab)
        {
            Destroy(spriteRoot);

            spriteRoot = Instantiate(spritePrefab);
            spriteRoot.gameObject.name = spriteRoot.gameObject.name.Replace("(Clone)", "");
            spriteRoot.transform.SetParent(transform, false);
        }
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

        UpdatePlayerArea();
    }

    void UpdatePlayerArea()
    {
        bool updated = false;
        foreach(var otherPlayer in playersInArea) {
            if (otherPlayer.points < points)
                continue;

            var pointsToSteal = Mathf.Min(otherPlayer.points, Time.fixedDeltaTime * stealPointsPerSecond);
            if (pointsToSteal <= 0)
                continue;

            if (!stealCredits.ContainsKey(otherPlayer))
                stealCredits[otherPlayer] = pointsToSteal;
            else
                stealCredits[otherPlayer] += pointsToSteal;

            int steal = (int)stealCredits[otherPlayer];
            if (steal > 0) {
                StealPoints(otherPlayer, steal);
            }
        }

        if (updated) {
            GameplayManager.Instance.UpdatePoints();
        }
    }

    void StealPoints(Player otherPlayer, int points)
    {
        stealCredits[otherPlayer] -= points;
        var steal = GameplayManager.Instance.infectables
            .Where(i => i.infectedBy == otherPlayer)
            .OrderByDescending(i => Vector3.SqrMagnitude(i.transform.position - transform.position))
            .Take(points);

        foreach (var i in steal) {
            i.Infect(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Infectable infectable = collision.gameObject.GetComponent<Infectable>();

        if (infectable != null)
            infectable.Infect(this);
    }

    void OnTriggerEnter(Collider other)
    {
        Player otherPlayer = other.gameObject.GetComponent<Player>();

        if (otherPlayer != null) {
            playersInArea.Add(otherPlayer);
            otherPlayer.playersInArea.Add(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Player otherPlayer = other.gameObject.GetComponent<Player>();

        if (otherPlayer != null) {
            playersInArea.Remove(otherPlayer);
            otherPlayer.playersInArea.Remove(this);
        }
    }
}
