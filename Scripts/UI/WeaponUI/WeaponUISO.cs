using UnityEngine;

namespace Baek.UI
{
    [CreateAssetMenu(menuName = "Baek/SO/WeaponUI")]
    public class WeaponUISO : ScriptableObject
    {
        [SerializeField] private Color _weaponChoiceColor; public Color WeaponChoiceColor { get { return _weaponChoiceColor; } }
    }
}
