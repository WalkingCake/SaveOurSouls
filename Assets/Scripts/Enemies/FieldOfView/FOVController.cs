using SaveOurSouls.Enemies.FieldOfView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.Enemies.FieldOfView
{
    public class FOVController : MonoBehaviour
    {
        public event Action OnPlayerEntered;
        public event Action OnPLayerInside;
        public event Action OnPlayerExited;

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
                
                RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, points[i], this._distance, this._layerMask);
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
        private Matrix4x4 _matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 1));
        private void Update()
        {
            this.RecalculateColliderForm(out Vector2[] points, out Vector2[] uv, out ushort[] triangles);
            this._colliderController.SetPoints(points);
            this._spriteController.RecalculteSpriteGeometry(uv, triangles);
            this._direction = _matrix.MultiplyVector(this._direction);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            this.OnPlayerEntered?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            this.OnPlayerExited?.Invoke();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            this.OnPLayerInside?.Invoke();
        }

        private Matrix4x4 _rotationMatrix;
        private ushort[] _triangles;

        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private int _rayCount;
        [SerializeField] private float _angle;
        [SerializeField] private float _distance;
        [SerializeField] private float _pixelsPerUnit;
        [SerializeField] FOVSpriteController _spriteController;
        [SerializeField] FOVColliderController _colliderController;
    }
}
