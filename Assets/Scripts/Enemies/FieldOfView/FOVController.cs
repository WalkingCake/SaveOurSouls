using SaveOurSouls.Enemies.FieldOfView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace SaveOurSouls.Enemies.FieldOfView
{
    public class FOVController : MonoBehaviour
    {
        public event Action<Vector2> OnPlayerEntered;
        public event Action<Vector2> OnPLayerInside;

        private void Awake()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this._spriteController.Initialize(this._pixelsPerUnit, this._distance);
            this._rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, this._angle / this._rayCount)));
            this._triangles = this.CalculateTriangles();
        }

        public void SetDirection(Vector2 direction)
        {
            if (direction.sqrMagnitude > float.Epsilon)
                this._direction = direction;
        }

        private ushort[] CalculateTriangles()
        {
            ushort[] triangles = new ushort[(this._rayCount - 1) * 3];
            for(ushort i = 0; i < this._rayCount - 1; )
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = ++i;
                triangles[(i - 1) * 3 + 2] = ++i;
                i--;
            }
            return triangles;
        }
                
        private void RecalculateColliderForm(out Vector2[] points, out Vector2[] uv, out ushort[] triangles)
        {
            points = new Vector2[this._rayCount + 1];
            points[0] = Vector3.zero;
            Vector2 previousVector = Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, -this._angle / 2)))
                .MultiplyVector(this._direction.normalized * this._distance);
            
            for(ushort i = 1; i < points.Length; i++)
            {
                points[i] = previousVector;
                
                RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, points[i], this._distance, this.LayerMask);
                if(hitInfo.collider != null)
                {
                    points[i] = this.transform.worldToLocalMatrix.MultiplyPoint(hitInfo.point);
                }
                
                previousVector = this._rotationMatrix.MultiplyVector(previousVector);
                
            }

            float x, y;
            uv = new Vector2[points.Length];
            for(int i = 0; i < uv.Length; i++)
            {
                x = Mathf.Round((points[i].x + this._distance) * this._pixelsPerUnit);
                y = Mathf.Round((points[i].y + this._distance) * this._pixelsPerUnit);
                uv[i] = new Vector2(x, y); 
            }

            triangles = this._triangles;
        }

        private void Update()
        {
            this.RecalculateColliderForm(out Vector2[] points, out Vector2[] uv, out ushort[] triangles);
            this._colliderController.SetPoints(points);
            this._spriteController.RecalculteSpriteGeometry(uv, triangles);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        { 
            this.OnPlayerEntered?.Invoke(collision.transform.position);
             
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            this.OnPLayerInside?.Invoke(collision.transform.position);
        }



        private Matrix4x4 _rotationMatrix;
        private ushort[] _triangles;

        private LayerMask LayerMask => this._handler.LayerMask;

        [SerializeField] private EnemyHandler _handler;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private int _rayCount;
        [SerializeField] private float _angle;
        [SerializeField] private float _distance;
        [SerializeField] private float _pixelsPerUnit;
        [SerializeField] private FOVSpriteController _spriteController;
        [SerializeField] private FOVColliderController _colliderController;
        [SerializeField] private Rigidbody2D _rigidbody;
    }
}
