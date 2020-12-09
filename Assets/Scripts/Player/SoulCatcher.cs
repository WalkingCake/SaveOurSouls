using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SaveOurSouls.Player
{
    public class SoulCatcher : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if(inventory != null && inventory.HasPickedSoul)
            {
                int soulsCount = inventory.HandOverSouls();
                string[] text = this._text.text.Split('\n');
                text[1] = (Int32.Parse(text[1]) + soulsCount).ToString();
                this._text.text = text[0] + "\n" + text[1];
            }
        }
        [SerializeField] private TextMeshPro _text;
    }
}
