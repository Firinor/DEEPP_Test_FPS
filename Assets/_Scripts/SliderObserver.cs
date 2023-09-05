using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Observers
{
    public class SliderObserver : MonoBehaviour, IObserver<float>
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private Player player;
        [SerializeField]
        private Attribute attribute;
        [SerializeField]
        private TextMeshProUGUI text;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(float value)
        {
            slider.value = value;
            text.text = $"{(int)value} / {player.MaxHealth}";
        }

        private void OnEnable()
        {
            player.CurrentHealth.Subscribe(this);
        }
    }
}