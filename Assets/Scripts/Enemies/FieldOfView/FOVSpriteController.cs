using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace SaveOurSouls.Enemies.FieldOfView
{
    public class FOVSpriteController : MonoBehaviour
    {
        private void Awake()
        {
            this._renderer = this.gameObject.AddComponent<SpriteRenderer>();
        }

        public void Initialize(float pixelsPerUnit, float distance)
        {
            int textureSize = (int)Mathf.Round(2 * pixelsPerUnit * distance) + 2; 

            Texture2D texture = new Texture2D(textureSize, textureSize)
            {
                filterMode = FilterMode.Point
            };

            this._renderer.color = this._color;
            
            this._renderer.sprite = Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), Vector3.one * 0.5f, pixelsPerUnit);
            this._renderer.sortingOrder = 2;
            
        }

        public void RecalculteSpriteGeometry(Vector2[] uv, ushort[] triangles)
        {
            this._renderer.sprite.OverrideGeometry(uv, triangles);
        }

        //private Vector2 _direction;
        //private float _distance;
        //private float _angle;
        //private float _pixelsPerUnit;
        private SpriteRenderer _renderer;
        
        [SerializeField] private Color _color;
    }
}
