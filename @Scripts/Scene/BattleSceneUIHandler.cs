using UnityEngine;
using Baek.Manager;
using UnityEngine.InputSystem;

namespace Baek.UI
{
    public class BattleSceneUIHandler : SceneUIHandler
    {
        [SerializeField] private PopupTypeSO _optionSO, _bossHealthSO;

        protected override void RegisterEvent()
        {
            _inputSO.EscEvent += HandleOptionEvent;
        }


        private void Update()
        {
            if (Keyboard.current.zKey.wasPressedThisFrame)
                Managers.UI.ShowPopup(_bossHealthSO);
            if (Keyboard.current.xKey.wasPressedThisFrame)
                Managers.UI.ClosePopup(_bossHealthSO);
        }

        private void HandleOptionEvent()
        {
            if (Managers.UI.PopupDictionary[_optionSO.name].IsShowPopup == false)
            {
                Managers.UI.ShowPopup(_optionSO);
            }
            else
            {
                Managers.UI.ClosePopup(_optionSO);
            }
        }

        protected override void UnregisterEvent()
        {
            _inputSO.EscEvent -= HandleOptionEvent;
        }
    }
}