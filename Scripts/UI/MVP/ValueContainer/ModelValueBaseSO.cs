using UnityEngine;
using static Baek.Define.Define;

namespace Baek.UI
{
    public class ModelValueBaseSO : ScriptableObject
    {
        [SerializeField] private EModelType _typeValue; public EModelType TypeValue { get { return _typeValue; } }
    }
}

