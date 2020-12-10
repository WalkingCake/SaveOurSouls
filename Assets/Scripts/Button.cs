using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SaveOurSouls
{
    public class Button : MonoBehaviour
    {
        private void OnMouseEnter()
        {
            this.transform.position += (Vector3)this._offset;
        }
        private void OnMouseExit()
        {
            this.transform.position -= (Vector3)this._offset;
        }

        private void OnMouseDown()
        {
            this._text.SetActive(!this._text.activeInHierarchy);
        }



        [SerializeField] private Vector2 _offset = new Vector2(0, 0.2f);
        [SerializeField] private GameObject _text;
    }
}
