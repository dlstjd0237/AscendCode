using UnityEngine;
using System;
using static Baek.Util.Util;

namespace Baek.UI
{
    public class ModelBaseUI : InitBase
    {
        [SerializeField]
        private ModelValueFloatSO _valueSO;

        //=====MaxValue=====
        private float _maxValue;
        public float MaxValue => _maxValue;

        //====MinValue=====

        private float _minValue;
        public float MinValue => _minValue;

        //=====CurrentValue=====

        private float _currentValue;
        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                float vlaue = Mathf.Clamp(value, _minValue, _maxValue);
                if (_currentValue != vlaue)
                {
                    _currentValue = Mathf.Clamp(value, _minValue, _maxValue);
                    UpdateValue();
                }
            }
        }

        //=====Event=====
        public event Action ValueChangEvent;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            if (NullCheck(_valueSO) == false)
                return false;

            _valueSO = Instantiate(_valueSO);


            _maxValue = _valueSO.MaxValue;
            _minValue = _valueSO.MinValue;

            _currentValue = _maxValue;

            return true;
        }


        /// <summary>
        /// Add Value
        /// </summary>
        /// <param name="value">Value Add Amount</param>
        public virtual void Increment(float value) => CurrentValue += value;
        /// <summary>
        /// Remove Value
        /// </summary>
        /// <param name="value">Value Remove Amount</param>
        public virtual void Decrement(float value) => CurrentValue -= value;
        /// <summary>
        /// Reset Value
        /// </summary>
        public virtual void Restore() => CurrentValue = _maxValue;




        public virtual void UpdateValue() => ValueChangEvent?.Invoke();
    }
}


