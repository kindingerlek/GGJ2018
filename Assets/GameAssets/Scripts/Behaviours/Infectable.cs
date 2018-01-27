using System;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(NavMeshAgent))]
public class Infectable : MonoBehaviour {

    public enum States
    {
        Init,
        Walking
    }

    public GameObject infectedBy = null;

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

    public void Infect(GameObject infector)
    {
        if (infectedBy.GetInstanceID() == infector.GetInstanceID())
            return;

        int infectRes = 1;

        if (infectRes == 1){
            Debug.Log(this.name + ": I was infected by " + infector.name);
            infectedBy = infector;
        } else if (infectRes == -1) {
            Debug.Log(infector.name + ": I was infected by " + this.name);
            infector.GetComponent<Infectable>().infectedBy = this.GetComponent<GameObject>();
        }
        
        Debug.Log(this.name + ": I was infected by " + infector.name);
        infectedBy = infector;

        renderer.material = infector.GetComponent<Renderer>().material;
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
        [SerializeField] float randomMovementRange;
        [SerializeField] float maxWaitBeforeWalk;

        bool didCollide;

        public IEnumerator WalkToRandomPosition(NavMeshAgent agent)
        {
            didCollide = false;
            var nextPos = GetRandomWalkPoint(agent.transform);
            agent.destination = nextPos;

            yield return new WaitForSeconds(UnityEngine.Random.value * maxWaitBeforeWalk);

            while (!didCollide && (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)) {
                yield return new WaitForSeconds(0.1f);
            }
        }

        void IStateHandler.OnCollisionEnter(Collision other)
        {
            didCollide = true;
        }

        Vector3 GetRandomWalkPoint(Transform transform)
        {
            var randomPosition = transform.position + UnityEngine.Random.insideUnitSphere * randomMovementRange;
            randomPosition.y = transform.position.y;
            return randomPosition;
        }
    }
    #endregion
}
