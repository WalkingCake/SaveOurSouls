using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.Enemies.FieldOfView
{
    public class FieldOfView : MonoBehaviour
    {
        public event Action OnPlayerEntered;
        public event Action OnPLayerInside;
        public event Action OnPlayerExited;
       
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"{collision.transform.name} entered.");
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log($"{collision.transform.name} exited.");
        }

        [SerializeField] private Collider2D _collider;
    }
}
