using PJH.Manager;
using UnityEngine;
using UnityEngine.UI;
using static Baek.Util.Util;

namespace Baek.UI
{
    public class SoundPresenterUI : PresenterBaseUI
    {
        [SerializeField] private string _dataKey;
        private Slider _slider;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            // ==== Volume Slider ====
            _slider = GetComponent<Slider>();
            if (NullCheck(_slider) == false)
                return false;
            float value = PlayerPrefs.GetFloat(_dataKey, 0.5f);
            _slider.value = value;
            _slider.onValueChanged.AddListener(HandleValueChangeEvent);
            // ========================

            return true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _slider.onValueChanged.RemoveListener(HandleValueChangeEvent);
        }

        private void HandleValueChangeEvent(float value)
        {
            switch (_dataKey)
            {
                case "MasterVolume":
                    Managers.FMODSound.SetMainVolume(value);
                    break;
                case "MusicVolume":
                    Managers.FMODSound.SetMusicVolume(value);
                    break;
                case "SFXVoluem":
                    Managers.FMODSound.SetSFXVolume(value);
                    break;
            }
        }

        protected override void UpdateView()
        {
        }
    }
}