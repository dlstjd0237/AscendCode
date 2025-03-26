using UnityEngine;

namespace Baek.UI
{
    [CreateAssetMenu(menuName = "Baek/SO/ModelValueSO/Float")]
    public class ModelValueFloatSO : ModelValueBaseSO
    {
        [SerializeField] private float _maxValue; public float MaxValue { get { return _maxValue; } }
        [SerializeField] private float _minValue; public float MinValue { get { return _minValue; } }
        [SerializeField] private bool _currentValueIsMaxValue = true; public bool CurrentValueIsMaxValue { get { return _currentValueIsMaxValue; } }

        [Header("If [CurrentValueIsMaxValue] is false")] [SerializeField] private float _currentValue; public float CurrentValue { get { return _currentValue; } }
    }
}

