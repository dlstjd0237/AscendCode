using UnityEngine;

namespace Baek.UI
{
    public class ModelValueBoolSO : ModelValueBaseSO
    {
        [SerializeField] private bool _currentValue; public bool CurrentValue { get { return _currentValue; } }
        //[SerializeField] private bool _maxValue; public bool MaxValue { get { return _maxValue; } }
        //[SerializeField] private bool _minValue; public bool MinValue { get { return _minValue; } }

        //[SerializeField] private bool _currentValueIsMaxValue = true; public bool CurrentValueIsMaxValue { get { return _currentValueIsMaxValue; } }

        //[Header("If [CurrentValueIsMaxValue] is false")] [SerializeField] private float _currentValue; public float CurrentValue { get { return _currentValue; } }
    }
}

