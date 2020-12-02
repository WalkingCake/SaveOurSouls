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
            //SpriteRenderer renderer = this.gameObject.AddComponent<SpriteRenderer>();

            Vector2[] points;
            this.RecalculateColliderForm(out points, out _, out _);
            
            this._collider = this.gameObject.AddComponent<PolygonCollider2D>();
            this._collider.isTrigger = true;
            this._collider.points = points;

            this._spriteController.Initialize(this._pixelsPerUnit, this._angle, this._distance, this._direction);
            this._spriteController.RedrawTexture();
            //int textureSize = (int)Mathf.Ceil(this._distance * this._pixelsPerUnit * 2);
            //this._texture = new Texture2D(textureSize, textureSize)
            //{
            //    filterMode = FilterMode.Point
            //};
            //renderer.sprite = Sprite.Create(this._texture, new Rect(0, 0, textureSize, textureSize), Vector2.zero, this._pixelsPerUnit);
            
        }



        //private void OnDrawGizmos()
        //{
        //    if(this.points != null)
        //    {
        //        Gizmos.color = Color.red;
        //        foreach(Vector2 point in this.points)
        //        {
        //            Gizmos.DrawSphere((Vector2)this.transform.position + point, 0.3f);
        //        }
        //    }
        //}

        
        private void RecalculateColliderForm(out Vector2[] points, out Vector2[] uv, out ushort[] triangles)
        {
            points = new Vector2[this._rayCount + 1];
            points[0] = Vector3.zero;
            triangles = new ushort[points.Length * 3];
            float angleStep = this._angle / this._rayCount;
            points[1] = Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, -this._angle / 2)))
                .MultiplyVector(this._direction.normalized * this._distance);
            
            Matrix4x4 matrix = Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, angleStep)));
            
            for(int i = 2; i < points.Length; i++)
            {
                points[i] = matrix.MultiplyVector(points[i - 1]);
                RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, points[i], this._distance);
                if(hitInfo.collider != null)
                {
                    points[i] = this.transform.worldToLocalMatrix.MultiplyPoint(hitInfo.point);
                }
                triangles[(i - 1) * 3 - 3] = 0;
                triangles[(i - 1) * 3 - 2] = (ushort)(i - 1);
                triangles[(i - 1) * 3 - 1] = (ushort)i;
            }
            uv = points;

        }

        //private void RedrawTexture()
        //{
        //    Vector2 center = new Vector2(this._texture.width / 2, this._texture.height / 2);
        //    Debug.Log(center);
        //    float convertedDistance = this._distance * this._pixelsPerUnit;
        //    Vector2 startPoint = center - this._direction.normalized * convertedDistance / 2;
        //    Debug.Log(startPoint);
        //    this._texture.SetPixel((int)startPoint.x, (int)startPoint.y, this._color);
        //    float sqrDistance = convertedDistance * convertedDistance;
        //    for (int y = 0; y < this._texture.height; y++)
        //    {
        //        for (int x = 0; x < this._texture.width; x++)
        //        {
        //            Vector2 vector = new Vector2(x, y) - startPoint;
        //            if(Vector2.Angle(this._direction, vector) < this._angle / 2 && vector.sqrMagnitude <= sqrDistance)
        //            {
        //                this._texture.SetPixel(x, y, this._color);
        //            }
        //            else
        //            {
        //                this._texture.SetPixel(x, y, new Color(0, 0, 0, 0));
        //            }
        //        }
        //    }
        //    this._texture.Apply();
        //}


        private void Update()
        {
            Vector2[] points, uv;
            ushort[] triangles;
            this.RecalculateColliderForm(out points, out uv, out triangles);
            this._collider.points = points;
            
            //this._spriteController.RedrawTexture();
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"{collision.transform.name} entered.");
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log($"{collision.transform.name} exited.");
        }

        private PolygonCollider2D _collider;
        private Texture2D _texture;

        [SerializeField] private Vector2 _direction;
        [SerializeField] private int _rayCount;
        [SerializeField] private float _angle;
        [SerializeField] private float _distance;
        [SerializeField] private float _pixelsPerUnit;
        [SerializeField] FOVSpriteController _spriteController;
    }
}
