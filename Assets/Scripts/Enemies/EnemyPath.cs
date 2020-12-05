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
        public Vector2 Next { get; private set; }

        public Vector2 Current { get; private set; }

        public void MoveNext()
        {
            if (this._points == null)
                return;
            this.Current = this._points.Current;
            this._points.MoveNext();
            this.Next = this._points.Current;
        }
        
        private void Awake()
        {
            this._points = this.GetPointEnumerator(this._path);

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

        [SerializeField] private Vector2[] _path;
    }
}
