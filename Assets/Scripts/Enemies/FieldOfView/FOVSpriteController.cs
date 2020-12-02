using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.Enemies.FieldOfView
{
    public class FOVSpriteController : MonoBehaviour
    {
        private void Awake()
        {
            this._renderer = this.gameObject.AddComponent<SpriteRenderer>();
        }
        public void Initialize(float pixelsPerUnit, float angle, float distance, Vector2 direction)
        {
            this._direction = direction;
            this._distance = distance;
            this._angle = angle;
            this._pixelsPerUnit = pixelsPerUnit;

            int textureSize = (int)Mathf.Round(2 * pixelsPerUnit * distance); 

            this._texture = new Texture2D(textureSize, textureSize)
            {
                filterMode = FilterMode.Point
            };
            this._renderer.sprite = Sprite.Create(this._texture, new Rect(0, 0, textureSize, textureSize), Vector3.zero, pixelsPerUnit);
            
        }

        public void RedrawTexture()
        {
            Vector2 center = new Vector2(this._texture.width / 2, this._texture.height / 2);
            Debug.Log(center);
            float convertedDistance = this._distance * this._pixelsPerUnit;
            Vector2 startPoint = center - this._direction.normalized * convertedDistance / 2;
            Debug.Log(startPoint);
            this._texture.SetPixel((int)startPoint.x, (int)startPoint.y, this._color);
            float sqrDistance = convertedDistance * convertedDistance;
            for (int y = 0; y < this._texture.height; y++)
            {
                for (int x = 0; x < this._texture.width; x++)
                {
                    Vector2 vector = new Vector2(x, y) - startPoint;
                    if (Vector2.Angle(this._direction, vector) < this._angle / 2 && vector.sqrMagnitude <= sqrDistance)
                    {
                        this._texture.SetPixel(x, y, this._color);
                    }
                    else
                    {
                        this._texture.SetPixel(x, y, new Color(0, 0, 0, 0));
                    }
                }
            }
            this._texture.Apply();
        }

        private Vector2 _direction;
        private float _distance;
        private float _angle;
        private float _pixelsPerUnit;
        private SpriteRenderer _renderer;
        private Texture2D _texture;
        
        [SerializeField] private Color _color;
    }
}
