using System;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Npc : MonoBehaviour {

    public enum States
    {
        Init,
        Walking
    }

    [SerializeField] Walking walking;

    NavMeshAgent agent;
    StateMachine<States> fsm;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

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
        walking.Init(agent);
        while (fsm.State == States.Walking)
            yield return walking.WalkToRandomPosition(agent);
    }
    #endregion

    void OnDrawGizmosSelected() {
        GetState().OnDrawGizmosSelected();
    }

    #region States Data
    interface IStateHandler {
        void OnCollisionEnter(Collision other);
        void OnDrawGizmosSelected();
    }

    [Serializable]
    struct Walking : IStateHandler
    {
        [SerializeField] float maxDistance;
        [SerializeField] float maxWait;
        [SerializeField] float maxDuration;

        const float movementTrackInterval = 0.1f;

        bool didCollide;
        Vector3 initialPosition;

        public void Init(NavMeshAgent agent)
        {
            initialPosition = agent.transform.position;
        }

        public IEnumerator WalkToRandomPosition(NavMeshAgent agent)
        {
            didCollide = false;

            do {
                var nextPos = GetRandomWalkPoint();
                agent.destination = nextPos;

                while (agent.pathPending) {
                    yield return null;
                }
            } while (agent.pathStatus == NavMeshPathStatus.PathInvalid);

            yield return new WaitForSeconds(UnityEngine.Random.value * maxWait);

            Vector3 lastPosition = agent.transform.position;
            float movementDuration = 0;

            while (movementDuration < maxDuration && !didCollide) {

                yield return new WaitForSeconds(movementTrackInterval);
                movementDuration += movementTrackInterval;

                if (agent.remainingDistance <= agent.stoppingDistance) {
                    break;
                }

                var newPosition = agent.transform.position;
                if (Vector3.Distance(lastPosition, newPosition) < 0.1f)
                    break;
            }
        }

        void IStateHandler.OnCollisionEnter(Collision other)
        {
            didCollide = true;
        }

        Vector3 GetRandomWalkPoint()
        {
            NavMeshHit hit;

            Vector3 randomPosition;
            do {
                randomPosition = initialPosition + UnityEngine.Random.insideUnitSphere * maxDistance;
            } while (!NavMesh.SamplePosition(randomPosition, out hit, maxDistance, 1));

            return hit.position;
        }

        void IStateHandler.OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow * new Color(1, 1, 1, 0.1f);
            Gizmos.DrawSphere(initialPosition, maxDistance);
        }
    }
    #endregion
}
