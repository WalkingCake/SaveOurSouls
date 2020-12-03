using Assets.Scripts.Enemies.FieldOfView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SaveOurSouls.Enemies
{
    public class Enemy : MonoBehaviour
    {
        private void Awake()
        {
            this._state = EnemyState.MoveToPosition;
            this._timer = 0f;
            this._rigidbody.MovePosition(this._path.Current);
            _ = this._path.Next;

            this._sqrWaitingAreaRadius = this._waitionAreaRadius * this._waitionAreaRadius;
        }

        private void FixedUpdate()
        {
            switch (this._state)
            {
                case EnemyState.Stay:
                    this._timer += Time.fixedDeltaTime;
                    if(this._timer >= this._timeToStay)
                    {
                        this._timer = 0;
                        this._state = EnemyState.MoveToPosition;
                        _ = this._path.Next;
                    }
                    break;
                case EnemyState.MoveToPosition:
                    Vector2 currentPath = this._path.Current - (Vector2)this.transform.position;
                    Vector2 direction = currentPath.normalized;
                    if (currentPath.sqrMagnitude < this._sqrWaitingAreaRadius || (direction + this._path.TargetDirection).sqrMagnitude < this._epsilon)
                    {
                        this._rigidbody.MovePosition(this._path.Current);
                        this._rigidbody.velocity = Vector2.zero;
                        if(this._needToStayAtPoints)
                        {
                            this._state = EnemyState.Stay;
                            break;
                        }
                        else
                        {
                            currentPath -= this._path.Current;
                            currentPath += this._path.Next;
                            direction = currentPath.normalized;
                        }
                        
                    }
                    this._rigidbody.velocity = direction * this._moveSpeed;
                    break;
                    

            }
        }

        private EnemyState _state;
        private float _timer;
        private float _sqrWaitingAreaRadius;

        [Header("Movement settings")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _runSpeed;
        
        [Header("Behaviiour fitchers")]
        [SerializeField] private bool _needToStayAtPoints;
        [SerializeField] private float _timeToStay;
        [SerializeField] private float _waitionAreaRadius;
        [SerializeField] private float _epsilon;
        

        [SerializeField] private FOVController _fovController;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private EnemyPath _path;
    }

    public enum EnemyState
    {
        Stay,
        MoveToPosition,
        RunToPlayer,
        MoveToPreviousPosition
    }
}
