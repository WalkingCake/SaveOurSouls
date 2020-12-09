using SaveOurSouls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public bool HasPickedSoul => _pickedSoulsCount >= 1;
        public int PickedSoulsCount => this._pickedSoulsCount;

        private void Awake()
        {
            _soulsAround = new List<Soul>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var soul = other.GetComponent<Soul>();
            
            if (soul != null)
            {
                this._progressBar = soul.ProgressBar;
                _soulsAround.Add(soul);
                _anySoulsAround = true;
            }
        }

        private void Update()
        {
            if (_anySoulsAround && Input.GetKeyDown(KeyCode.E))
            {
                _pickingCorotuine = StartCoroutine(PickSouls());
            }

            if (_pickedSoulsCount > 0 && Input.GetKeyDown(KeyCode.R))
            {
                var soulDrop = Instantiate(_soulPrefab);
                soulDrop.transform.position = this.transform.position;
                _pickedSoulsCount--;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var soul = other.GetComponent<Soul>();

            if (soul != null)
            {
                _soulsAround.Remove(soul);
            }

            if (_soulsAround.Count == 0)
            {
                _anySoulsAround = false;
            }
        }


        private IEnumerator PickSouls()
        {

            float delayInSeconds = 1;

            float startTime = Time.time;
            float endTime = startTime + delayInSeconds;

            //_progressBar.gameObject.SetActive(true);

            while (Time.time < endTime)
            {
                float curDeltaTime = Time.time - startTime;
                float curPercentage = curDeltaTime / delayInSeconds;

                _progressBar.Value = curPercentage;

                if (!_anySoulsAround)
                {
                    StopCoroutine(this._pickingCorotuine);
                    //_progressBar.gameObject.SetActive(false);
                    yield break;
                }

                yield return new WaitForEndOfFrame();
            }


            int soulsCount = _soulsAround.Count;

            if (soulsCount > 0)
            {
                for (int i = soulsCount-1; i > -1 ; i--)
                {
                    Destroy(_soulsAround[i].gameObject);
                }
            }

            _pickedSoulsCount += soulsCount;
            _progressBar.gameObject.SetActive(false);
        }

        public int HandOverSouls()
        {
            int soulsCount = this._pickedSoulsCount;
            this._pickedSoulsCount = 0;
            return soulsCount;
        }

        private ProgressBar _progressBar;
        [SerializeField]
        private Soul _soulPrefab;
        [SerializeField]
        private int _pickedSoulsCount;

        private List<Soul> _soulsAround;
        private bool _anySoulsAround;

        private Coroutine _pickingCorotuine;
    }
}