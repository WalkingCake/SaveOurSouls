using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SaveOurSouls.Enemies.FieldOfView
{
    public class FOVColliderController : MonoBehaviour
    {
        private void Awake()
        {
            this._collider = this.gameObject.AddComponent<PolygonCollider2D>();
            this._collider.isTrigger = true;
        }

        public void SetPoints(Vector2[] points)
        {
            this._collider.points = points;
        }

        private PolygonCollider2D _collider;
    }
}
