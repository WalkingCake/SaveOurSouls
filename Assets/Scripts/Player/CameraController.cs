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
            this.transform.position = new Vector3(
                this._player.transform.position.x,
                this._player.transform.position.y,
                this.transform.position.z);
        }

        [SerializeField] private Rigidbody2D _player;
    }
}
