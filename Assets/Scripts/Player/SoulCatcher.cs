using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                soulsCount = Int32.Parse(text[1]) + soulsCount;
                text[1] = soulsCount.ToString();
                if (soulsCount >= 10)
                    SceneManager.LoadScene(0);
                this._text.text = text[0] + "\n" + text[1];
            }
            
        }
        [SerializeField] private TextMeshPro _text;
    }
}
