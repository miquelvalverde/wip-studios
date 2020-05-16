using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MyMonoBehaivour
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

    [SerializeField] public Stats stats = new Stats();

    public NavMeshAgent agent { get; private set; }

    public Vector3 position
    {
        get
        {
            return transform.position;
        }
    }

    private AState _currentState;

    private AState currentState
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


    private int pathIndex;
    [SerializeField] private List<Transform> pathPoints = new List<Transform>();
    private Queue<Vector3> pathPositions = new Queue<Vector3>();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        foreach (Transform t in pathPoints)
        {
            pathPositions.Enqueue(t.position);
        }

        pathPositions.Enqueue(position);

        AState g = new IdleState();
        Debug.Log(g);
        currentState = g;
    }

    private void Update()
    {
        currentState = currentState.ChangeState();

        currentState.UpdateState();
    }

    public Vector3 GetNextPosition()
    {
        Vector3 nextPosition = pathPositions.Dequeue();
        pathPositions.Enqueue(nextPosition);

        return nextPosition;
    }
}
