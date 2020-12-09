using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace SaveOurSouls.Player
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private void Awake()
        {
            this._plane = new Plane(this.transform.localToWorldMatrix.MultiplyVector(Vector3.back), Vector3.zero);
        }

        private void Update()
        {
            Vector2 offset = Vector3.zero;
            Ray ray = this._camera.ScreenPointToRay(Input.mousePosition);
            if (this._plane.Raycast(ray, out float distance))
            {
                offset = (ray.origin + ray.direction * distance - this._plane.ClosestPointOnPlane(this.transform.position)) * this._offsetMultiplier;
            }

            if (offset.magnitude > this._maxOffset)
                offset = offset.normalized * this._maxOffset;
            this.transform.position = new Vector3(
                this._player.transform.position.x + offset.x,
                this._player.transform.position.y + offset.y,
                this.transform.position.z);
        }

        private Plane _plane;

        [SerializeField] private Camera _camera;
        [SerializeField] private Rigidbody2D _player;
        [SerializeField] private float _offsetMultiplier;
        [SerializeField] private float _maxOffset;
    }
}
