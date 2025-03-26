using UnityEngine;

namespace Baek.Define
{
    public static class Define
    {
        public static Vector2 ScreenSize = new Vector2(1920, 1080);
        public const float UIDuration = 0.5f;
        public static readonly int DissolveAmountHash = Shader.PropertyToID("_DissolveAmount");

        public enum EModelType
        {
            Int,
            Float,
            Bool
        }
        public enum EPopupUI
        {
            BlackBoard,
            Option
        }

        public enum EBattleUIKey
        {
            FullScreenToggle,
            ShowDamageTextToggle,
            HandleMuteToggle
        }

        public enum EWeapon
        {
            Sword,
            Bow,
            Hammer
        }

        public enum ESkillKey
        {
            Q = 0,
            E,
            R
        }

        public enum EUIEventType
        {
            Click,
            Down,
            Up,
            Enter,
            Exit,
            Move
        }
    }
}

