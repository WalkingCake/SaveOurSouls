using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SaveOurSouls.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        private void Update()
        {
            this._direction = Vector2.zero;
            this._speedMultiplier = 1f;
            if (Input.GetKey(this._upKeyCode))
            {
                this._direction += Vector2.up;
            }
            if (Input.GetKey(this._downKeyCode))
            {
                this._direction += Vector2.down;
            }
            if (Input.GetKey(this._leftKeyCode))
            {
                this._direction += Vector2.left;
            }
            if (Input.GetKey(this._rightKeyCode))
            {
                this._direction += Vector2.right;
            }
            if (Input.GetKey(this._shiftKeyCode))
            {
                this._speedMultiplier *= this._shiftMultiplier;
            }
            else if (Input.GetKey(this._seatKeyCode))
            {

                this._speedMultiplier *= this._seatMultiplier;
            }
        }

        private void FixedUpdate()
        {
            //this._rigidbody.AddForce(this._direction.normalized * this._speed * Time.fixedDeltaTime * SPEED_MULTIPLIER * this._speedMultiplier, ForceMode2D.Force);
            if (this._direction.sqrMagnitude > float.Epsilon)
            {
                this._rigidbody.velocity = this._direction.normalized * this._speed /* Time.fixedDeltaTime * SPEED_MULTIPLIER */* this._speedMultiplier;
            }
        }

        private float _speedMultiplier;
        private Vector2 _direction;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float _speed;
        [SerializeField] private float _shiftMultiplier;
        [SerializeField] private float _seatMultiplier;

        [Header("Movement KeyCodes")]
        [SerializeField] private KeyCode _upKeyCode = KeyCode.W;
        [SerializeField] private KeyCode _downKeyCode = KeyCode.S;
        [SerializeField] private KeyCode _leftKeyCode = KeyCode.A;
        [SerializeField] private KeyCode _rightKeyCode = KeyCode.D;
        [SerializeField] private KeyCode _shiftKeyCode = KeyCode.LeftShift;
        [SerializeField] private KeyCode _seatKeyCode = KeyCode.LeftControl;
        [SerializeField] private KeyCode _jumpKeyCode = KeyCode.Space;

        private const float SPEED_MULTIPLIER = 1000;
    }
}
