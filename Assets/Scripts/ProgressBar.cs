using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SaveOurSouls
{
    public class ProgressBar : MonoBehaviour
    {
        public float Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = 1 - value;
                this.transform.localScale = new Vector3(this._value, 1, 1);
            }
        }

        private void Awake()
        {
            this._renderer = this.gameObject.AddComponent<SpriteRenderer>();
            Sprite sprite = Sprite.Create(new Texture2D((int)(this._width * this._pixelsPerUnit), (int)(this._height * this._pixelsPerUnit)),
                new Rect(0, 0, this._width, this._height),
                new Vector2(0.5f, 0.5f), this._pixelsPerUnit);
            this._renderer.color = this._color;
            this._renderer.sprite = sprite;
            this._renderer.sortingOrder = 10;

            this._value = 1;
        }
        
        private float _value;
        private SpriteRenderer _renderer;

        [SerializeField] private int _pixelsPerUnit;
        [SerializeField] private Color _color;
        [SerializeField] private float _width;
        [SerializeField] private float _height;
    }
}
