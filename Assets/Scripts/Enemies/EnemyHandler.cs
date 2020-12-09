using SaveOurSouls.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SaveOurSouls.Enemies
{
    public class EnemyHandler : MonoBehaviour
    {
        public LayerMask LayerMask { get; private set; }

        public void Update()
        {
            if (this._player.IsRunning)
                this.LayerMask = new LayerMask();
            else if (this._player.IsSeating)
                this.LayerMask = LayerMask.GetMask("Walls", "HalfWalls");
            else
                this.LayerMask = LayerMask.GetMask("Walls");
        }

        [SerializeField] private PlayerMovementController _player;
    }
}
