using Player;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveOurSouls.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {

        public PlayerInventory Inventory => this._inventory;
        public bool IsRunning => this._isShiftDown;
        public bool IsSeating => this._isCtrlDown;
        private void Awake()
        {
            this._startPosition = this.transform.position;
        }

        public void ResetPosition()
        {
            this.transform.position = this._startPosition;
        }

        private void Update()
        {
            this._direction = Vector2.zero;
            this._speedMultiplier = 1f;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }

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

            this._isShiftDown = Input.GetKey(this._shiftKeyCode);

            this._isCtrlDown = Input.GetKey(this._seatKeyCode);

            if (this._isShiftDown)
            {
                this._speedMultiplier *= this._shiftMultiplier;
            }
            else if (this._isCtrlDown)
            {
                this._speedMultiplier *= this._seatMultiplier;
            }

            this._animator.SetBool("Shift", this._isShiftDown);
            this._animator.SetBool("Ctrl", this._isCtrlDown);
            this._animator.SetBool("XgreaterY", Mathf.Abs(this._direction.x) > Mathf.Abs(this._direction.y));
            this._animator.SetFloat("VelocityX", this._direction.x);
            this._animator.SetFloat("VelocityY", this._direction.y);
        }

        private void FixedUpdate()
        {
            //this._rigidbody.AddForce(this._direction.normalized * this._speed * Time.fixedDeltaTime * SPEED_MULTIPLIER * this._speedMultiplier, ForceMode2D.Force);
            if (this._direction.sqrMagnitude > float.Epsilon)
            {
                this._rigidbody.velocity = this._direction.normalized * this._speed * this._speedMultiplier * Mathf.Pow(this._pickingSoulMultiplier, this._inventory.PickedSoulsCount);
            }


        }

        private float _speedMultiplier;
        private Vector2 _direction;
        private bool _isShiftDown;
        private bool _isCtrlDown;

        private Vector2 _startPosition;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speed;
        [SerializeField] private float _pickingSoulMultiplier;
        [SerializeField] private float _shiftMultiplier;
        [SerializeField] private float _seatMultiplier;
        [SerializeField] private PlayerInventory _inventory;

        [Header("Movement KeyCodes")]
        [SerializeField] private KeyCode _upKeyCode = KeyCode.W;
        [SerializeField] private KeyCode _downKeyCode = KeyCode.S;
        [SerializeField] private KeyCode _leftKeyCode = KeyCode.A;
        [SerializeField] private KeyCode _rightKeyCode = KeyCode.D;
        [SerializeField] private KeyCode _shiftKeyCode = KeyCode.LeftShift;
        [SerializeField] private KeyCode _seatKeyCode = KeyCode.LeftControl;
    }
}
