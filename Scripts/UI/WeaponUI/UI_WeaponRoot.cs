using System.Collections.Generic;
using static Baek.Util.Util;
using static Baek.Define.Define;
using System;
using PJH.Agent.Player;
using PJH.Equipment.Weapon;
using PJH.Manager;
using PJH.Scene;

namespace Baek.UI
{
    public class UI_WeaponRoot : InitBase
    {
        private PlayerEquipmentController _playerEquipmentController;

        private Dictionary<EWeapon, WeaponUI> _weaponDictionary = new Dictionary<EWeapon, WeaponUI>();

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            foreach (EWeapon item in Enum.GetValues(typeof(EWeapon)))
            {
                WeaponUI weapon = FindChild<WeaponUI>(gameObject, $"Weapon_{item.ToString()}UI", true);
                _weaponDictionary.Add(item, weapon);
            }


            return true;
        }

        private void Start()
        {
            Player player = (Managers.Scene.CurrentScene as GameScene).Player;
            _playerEquipmentController = player.GetCompo<PlayerEquipmentController>();
            _playerEquipmentController.WeaponChangeEvent += HandleWeaponChange;
        }

        private void HandleWeaponChange(PlayerWeapon evt)
        {
            string typeName = evt.name;
            foreach (var item in _weaponDictionary)
            {
                item.Value.UIChoice(false);
            }

            EWeapon weaponType = typeName == "Bow" ? EWeapon.Bow : typeName == "Sword" ? EWeapon.Sword : EWeapon.Hammer;
            _weaponDictionary[weaponType].UIChoice(true);
        }

        private void OnDestroy()
        {
            _playerEquipmentController.WeaponChangeEvent -= HandleWeaponChange;
        }
    }
}