using System;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(NavMeshAgent))]
public class Infectable : MonoBehaviour {
    
    public Player infectedBy = null;
    public enum States
    {
        Init,
        Walking
    }

    [SerializeField] Walking walking;

    NavMeshAgent agent;
    private new Renderer renderer;
    private Material originalMaterial;

    StateMachine<States> fsm;

    public void Awake()
    {
        renderer = GetComponent<Renderer>();
        agent = GetComponent<NavMeshAgent>();

        originalMaterial = renderer.material;

        InitStates();
    }

    void InitStates()
    {
        fsm = StateMachine<States>.Initialize(this);
        fsm.ChangeState(States.Walking);
    }

    void OnCollisionEnter(Collision other)
    {
        GetState().OnCollisionEnter(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Infectable infectable = collision.gameObject.GetComponent<Infectable>();

        if (infectable == null)
            return;

        infectable.Infect(this.gameObject);
    }
    

    public void Infect(GameObject other)
    {
        if (infectedBy != null)
        {
            if (infectedBy.GetInstanceID() == other.GetInstanceID())
                return;
        }
        
        int i1 = this.infectedBy ? this.infectedBy.playerIndex : 0 ;
        int i2 = 0;


        if (other.GetComponent<Player>())
            i2 = other.GetComponent<Player>().playerIndex;

        else if (other.GetComponent<Infectable>())
            i2 = other.GetComponent<Infectable>().infectedBy == null ? 0 : other.GetComponent<Infectable>().infectedBy.playerIndex;



        RuleManager.collisionType infectRes = RuleManager.Instance.CompareInfectation(i1, i2);



        if (infectRes == RuleManager.collisionType.MeInfectOther){
            Debug.Log(this.name + ": I infect " + other.name);

            if (other.GetComponent<Infectable>())
                other.GetComponent<Infectable>().infectedBy = this.infectedBy;
            
            other.GetComponent<Renderer>().material = renderer.material;

        } else if (infectRes == RuleManager.collisionType.OtherInfectMe) {
            Debug.Log(this.name + ": I was infected by " + other.name);

            if (other.GetComponent<Infectable>())
                this.infectedBy = other.GetComponent<Infectable>().infectedBy; 

            renderer.material = other.GetComponent<Renderer>().material;
        }

        GameManager.Instance.CountPoints();
    }

    #region States
    IStateHandler GetState()
    {
        switch (fsm.State) {
            case States.Walking: return walking;
        }
        throw new ArgumentException();
    }

    public IEnumerator Walking_Enter()
    {
        while (fsm.State == States.Walking)
            yield return walking.WalkToRandomPosition(agent);
    }
    #endregion

    #region States Data
    interface IStateHandler {
        void OnCollisionEnter(Collision other);
    }

    [Serializable]
    struct Walking : IStateHandler
    {
        [SerializeField] float maxDistance;
        [SerializeField] float maxWait;
        [SerializeField] float maxDuration;

        const float movementTrackInterval = 0.1f;

        bool didCollide;

        public IEnumerator WalkToRandomPosition(NavMeshAgent agent)
        {
            didCollide = false;
            var nextPos = GetRandomWalkPoint(agent.transform);
            agent.destination = nextPos;

            yield return new WaitForSeconds(UnityEngine.Random.value * maxWait);

            Vector3 lastPosition = agent.transform.position;
            float movementDuration = 0;

            while (movementDuration < maxDuration && !didCollide) {
                if (!agent.pathPending) {
                    if (agent.pathStatus == NavMeshPathStatus.PathInvalid) {
                        break;
                    }

                    if (agent.remainingDistance <= agent.stoppingDistance) {
                        break;
                    }

                    var newPosition = agent.transform.position;
                    if (Vector3.Distance(lastPosition, newPosition) < 0.1f)
                        break;
                }
                yield return new WaitForSeconds(movementTrackInterval);
                movementDuration += movementTrackInterval;
            }
        }

        void IStateHandler.OnCollisionEnter(Collision other)
        {
            didCollide = true;
        }

        Vector3 GetRandomWalkPoint(Transform transform)
        {
            var randomPosition = transform.position + UnityEngine.Random.insideUnitSphere * maxDistance;
            randomPosition.y = transform.position.y;
            return randomPosition;
        }
    }
    #endregion
}
