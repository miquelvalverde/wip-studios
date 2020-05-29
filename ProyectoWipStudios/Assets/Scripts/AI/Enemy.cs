using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviourPlayerGettable
{

    public struct Stats
    {
        private bool _isSeeingPlayer;
        public bool isSeeingPlayer
        {
            get { return _isSeeingPlayer; }
            set
            {
                _isSeeingPlayer = value;

                if (_isSeeingPlayer)
                    isPlayerFar = false;
            }
        }

        private bool _isPlayerFar;
        public bool isPlayerFar
        {
            get { return _isPlayerFar; }
            set
            {
                _isPlayerFar = value;
                if (_isPlayerFar)
                    isSeeingPlayer = false;
            }
        }

        public bool isPlayerClose;
    }

    [HideInInspector] public Stats stats = new Stats();

    public NavMeshAgent agent { get; private set; }

    public Vector3 position
    {
        get
        {
            return transform.position;
        }
    }

    private State _currentState;

    private State currentState
    {
        get
        {
            return _currentState;
        }

        set
        {
            if (value == null)
                return;

            if (_currentState != null)
                _currentState.ExitState();

            _currentState = value;

            _currentState.StartState(this);
        }
    }

    [SerializeField] private List<Transform> pathPoints = new List<Transform>();
    private Queue<Vector3> pathPositions = new Queue<Vector3>();

    public float chaseMaxDistance = 0;
    public float shootDistance = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        foreach (Transform t in pathPoints)
        {
            pathPositions.Enqueue(t.position);
        }

        pathPositions.Enqueue(position);

        State g = new IdleState();
        currentState = g;
    }

    private void Update()
    {
        if (stats.isSeeingPlayer && CheckPlayerDistance() > chaseMaxDistance)
            stats.isPlayerFar = true;

        if (stats.isSeeingPlayer && CheckPlayerDistance() < shootDistance)
            stats.isPlayerClose = true;

        currentState = currentState.ChangeState();

        currentState.UpdateState();
    }

    private float CheckPlayerDistance()
    {
        return Vector3.Distance(player.transform.position, position);
    }

    public Vector3 GetNextPosition()
    {
        Vector3 nextPosition = pathPositions.Dequeue();
        pathPositions.Enqueue(nextPosition);

        return nextPosition;
    }

    /*private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(position, Vector3.up, chaseMaxDistance);
        
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(position, Vector3.up, shootDistance);
    }*/

}
