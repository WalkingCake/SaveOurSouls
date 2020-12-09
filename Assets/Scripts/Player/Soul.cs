using SaveOurSouls;
using SaveOurSouls.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Soul : MonoBehaviour
    {
        public ProgressBar ProgressBar => this._progressBar;

        private void Awake()
        {
            this._progressBar.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent<PlayerMovementController>(out _))
                this._progressBar.gameObject.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            this._progressBar.gameObject.SetActive(false);
        }

        [SerializeField] private ProgressBar _progressBar;
    }
}
