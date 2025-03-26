using UnityEngine;

namespace Baek.Combat
{
    [CreateAssetMenu(menuName = "Baek/SO/DamageSO")]
    public class DamageSO : ScriptableObject
    {
        [SerializeField] private int _damage; public int Damage { get { return _damage; } }
    }
}

