using UnityEngine;
using static Baek.Define.Define;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UI;

namespace Baek.UI
{
    public class WeaponUI : InitBase
    {
        [SerializeField] private EWeapon _currentWeapon;
        [SerializeField] private WeaponUISO _currentWeaponSO;
        private Image _frame;
        private Color _defualtColor;
        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            //==== Frame ====
            _frame = GetComponent<Image>();
            _defualtColor = _frame.color;
            //===============

            return true;
        }


        public void UIChoice(bool value)
        {
            Color color = value ? _currentWeaponSO.WeaponChoiceColor : _defualtColor;
            _frame.DOColor(color, UIDuration);
        }

        private void OnValidate()
        {
            gameObject.name = $"Weapon_{_currentWeapon}UI";
        }
    }
}


