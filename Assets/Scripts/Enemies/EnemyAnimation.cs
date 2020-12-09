using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace SaveOurSouls.Enemies
{
    public class EnemyAnimation : MonoBehaviour
    {
        private void FixedUpdate()
        {
            Vector2 velocity = this._rigidbody.velocity;
            this._animator.SetBool("XgreaterY", Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y));
            this._animator.SetFloat("VelocityX", velocity.x);
            this._animator.SetFloat("VelocityY", velocity.y);
        }


        [SerializeField] private Rigidbody2D _rigidbody;

        [SerializeField] private Animator _animator;
    }
}
