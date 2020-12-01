using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace SaveOurSouls.Player
{
    public class CameraController : MonoBehaviour
    {
        private void Update()
        {
            this.transform.position = new Vector3(this._player.position.x, this._player.position.y, this.transform.position.z);
        }
        [SerializeField] private Transform _player;
    }
}
