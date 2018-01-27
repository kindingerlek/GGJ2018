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
