using UnityEngine;
using UnityEngine.UI;
using static Baek.Util.Util;
using DG.Tweening;
using PJH.Manager;
using PJH.Scene;

namespace Baek.UI
{
    public class SteminerPresenterUI : PresenterBaseUI
    {
        [SerializeField] private float _duration = 0.2f;
        private PJH.Agent.Player.Player _player;
        private Image _leftImage, _rightImage;
        private int _currentValue;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            //==== FillImage Find ====
            _leftImage = FindChild<Image>(gameObject, "LeftFill", true);
            _rightImage = FindChild<Image>(gameObject, "RightFill", true);
            if (NullCheck(_leftImage) == false) //LeftImage
                return false;
            if (NullCheck(_rightImage) == false) //RightImage
                return false;
            //========================


            return true;
        }

        private void Start()
        {
            _player = (Managers.Scene.CurrentScene as GameScene).Player;
            _player.CurrentStaminaChangedEvent += HandleValueChanged;
        }


        private void HandleValueChanged(int value)
        {
            _currentValue = value;
            UpdateView();
        }

        public void SteminerRecovery(float value)
        {
            Model?.Increment(value);
        }

        public void SteminerReduction(float value)
        {
            Model?.Decrement(value);
        }

        public void SteminerRestore()
        {
            Model?.Restore();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _player.CurrentStaminaChangedEvent -= HandleValueChanged;
        }

        protected override void UpdateView()
        {
            float value = (float)_currentValue / _player.MaxStamina;
            _leftImage.DOFillAmount(value, _duration);
            _rightImage.DOFillAmount(value, _duration);
        }
    }
}