using System;
using TMPro;
using UnityEngine;

namespace Observer
{
    public class AmmoTextObserver : MonoBehaviour, IObserver<int>
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private Gun gun;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(int value)
        {
            text.text = $"{value} / {gun.maxAmmo}";
        }

        private void OnEnable()
        {
            gun.CurrentAmmoInClip.Subscribe(this);
        }
    }
}
