using Assets.Scripts.Enemies.FieldOfView;
using Pathfinding;
using System;
using UnityEngine;

namespace SaveOurSouls.Enemies
{
    public class Enemy : MonoBehaviour
    {
        private void Start()
        {
            
            this._state = EnemyState.Stay;
            this._stayTimer = 0f;
            this._rigidbody.MovePosition(this._enemyPath.Current);
            this._seeker.StartPath(this._rigidbody.position, this._enemyPath.Next, this.OnPathCreated);

            this._sqrWaitingAreaRadius = this._waitionAreaRadius * this._waitionAreaRadius;
            this._currentSpeed = this._moveSpeed;

            this._fovController.OnPlayerEntered += this.OnPlayerNoticed;
            this._fovController.OnPLayerInside += this.OnPlayerSupervised;
        }

        private void OnPathCreated(Path path)
        {
            if(!path.error)
            {
                this._path = path;
                this._pathToPlayerPointer = 0;
            }
        }

        private void OnPlayerNoticed(Vector2 playerPosition)
        {
            this._state = EnemyState.RunToPlayer;
            this._seeker.StartPath(this._rigidbody.position, playerPosition, this.OnPathCreated);
            this._currentSpeed = this._runSpeed;

        }

        private void OnPlayerSupervised(Vector2 playerPosition)
        {
            this._updatePlayerPositionTimer += Time.deltaTime;
            if(this._updatePlayerPositionTimer >= this._updatePathToPlayerTime)
            {
                this._updatePlayerPositionTimer = 0;
                this._seeker.StartPath(this._rigidbody.position, playerPosition, this.OnPathCreated);
            }
        }

        private void MoveEnemy(EnemyState nextState, Action onStateSwitched = null)
        {
            if (this._path == null)
                return;

            if(this._pathToPlayerPointer >= this._path.vectorPath.Count)
            {
                this._state = nextState;
                onStateSwitched?.Invoke();
                return;
            }

            Vector2 currentTarget = (Vector2)this._path.vectorPath[this._pathToPlayerPointer] - this._rigidbody.position;
            Vector2 direction = currentTarget.normalized;

            this._rigidbody.velocity = direction * this._currentSpeed;
            this._fovController.SetDirection(direction);

            if (currentTarget.sqrMagnitude < this._sqrWaitingAreaRadius)
                this._pathToPlayerPointer++;
        }

        private void OnEnemyStay()
        {
            this._stayTimer += Time.fixedDeltaTime;
            if (this._stayTimer >= this._timeToStay)
            {
                this._stayTimer = 0;
                this._state = EnemyState.OnTheWay;
            }
        }
        
        private void OnEnemyOnTheWay()
        {
            this.MoveEnemy(EnemyState.Stay, () =>
            {
                this._enemyPath.MoveNext();
                this._seeker.StartPath(this._rigidbody.position, this._enemyPath.Next, this.OnPathCreated);
            });
        }

        private void OnEnemyRunToPlayer()
        {
            this.MoveEnemy(EnemyState.BackOnWay, () =>
            {
                this._currentSpeed = this._moveSpeed;
                this._seeker.StartPath(this._rigidbody.position, this._enemyPath.Current, this.OnPathCreated);
            });
        }

        private void OnEnemyBackOnWay()
        {
            this.MoveEnemy(EnemyState.Stay);
        }

        private void FixedUpdate()
        {
            switch (this._state)
            {
                case EnemyState.Stay:
                    this.OnEnemyStay();
                    break;
                case EnemyState.OnTheWay:
                    this.OnEnemyOnTheWay();
                    break;
                case EnemyState.RunToPlayer:
                    this.OnEnemyRunToPlayer();
                    break;
                case EnemyState.BackOnWay:
                    this.OnEnemyBackOnWay();
                    break;

            }
        }


        private EnemyState _state;
        private float _stayTimer;
        private float _updatePlayerPositionTimer;
        private float _sqrWaitingAreaRadius;
        private float _currentSpeed;

        private Path _path;
        private int _pathToPlayerPointer;
        
        [Header("Movement settings")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _runSpeed;
        
        [Header("Behaviiour fitchers")]
        [SerializeField] private bool _needToStayAtPoints;
        [SerializeField] private float _timeToStay;
        [SerializeField] private float _waitionAreaRadius;
        [SerializeField] private float _epsilon;
        
        [Header("Control links")]
        [SerializeField] private FOVController _fovController;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private EnemyPath _enemyPath;

        [Header("A* links")] 
        [SerializeField] private Seeker _seeker;
        [SerializeField] private float _updatePathToPlayerTime;
    }

    public enum EnemyState
    {
        Stay,
        OnTheWay,
        RunToPlayer,
        BackOnWay
    }
}
