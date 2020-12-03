using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SaveOurSouls.Enemies
{
    public class EnemyPath : MonoBehaviour
    {
        public Vector2 Next
        {
            get
            {
                Vector2 previous = this._points.Current;
                this._points.MoveNext();
                this.TargetDirection = (this._points.Current - previous).normalized;
                return this._points.Current;
            }
        }

        public Vector2 Current
        {
            get
            {
                return this._points.Current;
            }
        }

        public Vector2 TargetDirection { get; private set; }

        private void Awake()
        {
            EdgeCollider2D edges = this.GetComponent<EdgeCollider2D>();
            this._points = this.GetPointEnumerator(edges.points.Select(point => (Vector2)this.transform.localToWorldMatrix.MultiplyPoint(point)).ToArray());
            EdgeCollider2D.Destroy(edges);
            this.TargetDirection = Vector2.zero;
        }
        
        private IEnumerator<Vector2> GetPointEnumerator(Vector2[] points)
        {
            int addable = 1;
            for(int index = 0; ; index += addable)
            {
                if(index >= points.Length || index < 0)
                {
                    if (this._goBack)
                    {
                        addable = -addable;
                        index += addable;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                yield return points[index];
            }
        }

        private IEnumerator<Vector2> _points;

        [SerializeField] private bool _goBack;
    }
}
