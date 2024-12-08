using UnityEngine;
using System.Collections.Generic;
using System;
using PJH.Agent.Player;
using PJH.Equipment.Weapon;
using DG.Tweening;
using static Baek.Define.Define;
using static Baek.Util.Util;
using System.Collections;
using PJH.Core;
using PJH.Manager;
using PJH.Scene;
using PJH.EquipmentSkillSystem;
using FMODUnity;
using Game.Events;

namespace Baek.UI
{
    using UnityEngine.UI;

    public class SkillControllerUI : InitBase
    {
        [SerializeField] private EventReference _skillChangeReference;
        [SerializeField] private Sprite _lookImage;
        [SerializeField] private GameEventChannelSO _gameEventChannelSO;
        private PlayerEquipmentController _playerEquipment = null;

        private SkillSpriteContainListSO _skillListSO = null;

        private Material _frontMat = null;
        private Material _backMat = null;

        private bool _isUseBackImage = false;

        private Dictionary<ESkillKey, Image> _iconImageDictionary = new Dictionary<ESkillKey, Image>();
        private Dictionary<ESkillKey, Image> _backIconImageDictionary = new Dictionary<ESkillKey, Image>();

        private Dictionary<ESkillKey, SkillCoolTimeUI> _skillCoolTimeUIDictionary =
            new Dictionary<ESkillKey, SkillCoolTimeUI>();

        private PlayerWeapon _defualtWeapon;
        private EquipmentSkill[] _skill = new EquipmentSkill[3];

        private Sequence _weaponChangeSeq;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            //==== Resource Load ====
            _skillListSO = Managers.Addressable.Load<SkillSpriteContainListSO>("SkillList");
            _frontMat = Managers.Addressable.Load<Material>("FrontMat");
            _backMat = Managers.Addressable.Load<Material>("BackMat");
            //=======================


            //==== Dictionary Init ====
            foreach (ESkillKey item in Enum.GetValues(typeof(ESkillKey)))
            {
                string rootName = $"Skill_{item.ToString()}";

                GameObject root = FindChild(gameObject, rootName, true);

                Transform iconTrm = FindChild(root, "SkillIcon", true).transform;
                Transform backIconTrm = FindChild(root, "BackSkillIcon", true).transform;
                Transform coolTimeIcon = FindChild(root, "SkillCoolTimeFill", true).transform;

                Image iconImage = iconTrm.GetComponent<Image>();
                Image backIconImage = backIconTrm.GetComponent<Image>();
                SkillCoolTimeUI coolTimeUI = coolTimeIcon.GetComponent<SkillCoolTimeUI>();
                iconImage.material = _frontMat;
                backIconImage.material = _backMat;
                _backIconImageDictionary.Add(item, backIconImage);
                _iconImageDictionary.Add(item, iconImage);
                _skillCoolTimeUIDictionary.Add(item, coolTimeUI);
            }


            return true;
        }

        private void HandleGetStoreSkill(GetStoreSkill evt)
        {
            Debug.Log(34);
            var weapon = (Managers.Scene.CurrentScene as GameScene).Player.GetCompo<PlayerEquipmentController>()
                .GetWeapon();
            SetImageSprite(weapon);
            SetBackImageSprite(weapon);
        }


        private IEnumerator Start()
        {
            _playerEquipment = (Managers.Scene.CurrentScene as GameScene).Player.GetCompo<PlayerEquipmentController>();

            yield return YieldCache.WaitUntil(() => _playerEquipment.GetWeapon() != null); //�÷��̾� ������ �������� ��
            _gameEventChannelSO.AddListener<GetStoreSkill>(HandleGetStoreSkill);

            //==== Event Subscribe ====
            _playerEquipment.WeaponChangeEvent += HandleWeaponChange;
            //=========================

            PlayerWeapon weapon = _playerEquipment.GetWeapon();

            //==== Icon Sprite Init ====
            SetImageSprite(weapon);
            SetBackImageSprite(weapon);
            //==========================

            CoolTimeEventSet(weapon);

            _defualtWeapon = weapon;
        }


        private void HandleWeaponChange(PlayerWeapon weapon)
        {
            if (_weaponChangeSeq != null && _weaponChangeSeq.IsActive()) _weaponChangeSeq.Kill();
            CoolTimeEventUnSet(_defualtWeapon);
            CoolTimeEventSet(weapon);
            _defualtWeapon = weapon;

            if (_isUseBackImage == false)
                SetBackImageSprite(weapon); //�� ������ �̹��� ���ֱ�
            else
                SetImageSprite(weapon); //�⺻ ������ �̹��� ���ֱ�

            _frontMat.SetFloat(DissolveAmountHash, 1);
            _backMat.SetFloat(DissolveAmountHash, 1);

            RuntimeManager.PlayOneShot(_skillChangeReference);
            _weaponChangeSeq = DOTween.Sequence();
            if (_isUseBackImage == false)
            {
                _weaponChangeSeq.Append(_frontMat.DOFloat(0, DissolveAmountHash, 1f));
                _weaponChangeSeq.Append(_backMat.DOFloat(1, DissolveAmountHash, 1f));
            }
            else
            {
                _weaponChangeSeq.Append(_frontMat.DOFloat(1, DissolveAmountHash, 1f));
                _weaponChangeSeq.Append(_backMat.DOFloat(0, DissolveAmountHash, 1f));
            }

            _weaponChangeSeq.AppendCallback(() =>
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (_isUseBackImage == false)
                        _iconImageDictionary[(ESkillKey)i].transform.SetAsFirstSibling();
                    else
                        _backIconImageDictionary[(ESkillKey)i].transform.SetAsFirstSibling();
                }

                _isUseBackImage = !_isUseBackImage;
            });
        }


        private void CoolTimeEventSet(PlayerWeapon weapon)
        {
            for (int i = 0; i < 3; ++i)
            {
                SkillCoolTimeUI skillCoolTimeUI = _skillCoolTimeUIDictionary[(ESkillKey)i];
                skillCoolTimeUI.ReSetFile();
                if (weapon.EquipmentSkillSystemCompo.GetSkill(i) != null)
                    weapon.EquipmentSkillSystemCompo.GetSkill(i).OnCooldownEvent += skillCoolTimeUI.UpdateView;
            }
        }

        private void CoolTimeEventUnSet(PlayerWeapon weapon)
        {
            for (int i = 0; i < 3; ++i)
            {
                SkillCoolTimeUI skillCoolTimeUI = _skillCoolTimeUIDictionary[(ESkillKey)i];
                skillCoolTimeUI.ReSetFile();
                if (weapon.EquipmentSkillSystemCompo.GetSkill(i) != null)
                    weapon.EquipmentSkillSystemCompo.GetSkill(i).OnCooldownEvent -= skillCoolTimeUI.UpdateView;
            }
        }

        private void SetImageSprite(PlayerWeapon weapon) //Skill Sprite Change
        {
            SkillSpriteContain skillSpriteContain = WeaponToSkillContain(weapon);
            SkillSprite skillSprite = skillSpriteContain.WeaponSkill.SkillSprite;

            if (weapon.EquipmentSkillSystemCompo.GetSkill(0) != null &&
                weapon.EquipmentSkillSystemCompo.GetSkill(0).skillEnabled == true)
                _iconImageDictionary[ESkillKey.Q].sprite = skillSprite.QSkillSprite;
            else
                _iconImageDictionary[ESkillKey.Q].sprite = _lookImage;


            if (weapon.EquipmentSkillSystemCompo.GetSkill(1) != null &&
                weapon.EquipmentSkillSystemCompo.GetSkill(1).skillEnabled == true)
                _iconImageDictionary[ESkillKey.E].sprite = skillSprite.ESkillSprite;
            else
                _iconImageDictionary[ESkillKey.E].sprite = _lookImage;


            if (weapon.EquipmentSkillSystemCompo.GetSkill(2) != null &&
                weapon.EquipmentSkillSystemCompo.GetSkill(2).skillEnabled == true)
                _iconImageDictionary[ESkillKey.R].sprite = skillSprite.RSkillSprite;
            else
                _iconImageDictionary[ESkillKey.R].sprite = _lookImage;
        }


        private void SetBackImageSprite(PlayerWeapon weapon) //Skill Baek Sprite Change
        {
            SkillSpriteContain skillSpriteContain = WeaponToSkillContain(weapon);
            SkillSprite skillSprite = skillSpriteContain.WeaponSkill.SkillSprite;

            if (weapon.EquipmentSkillSystemCompo.GetSkill(0) != null &&
                weapon.EquipmentSkillSystemCompo.GetSkill(0).skillEnabled == true)
                _backIconImageDictionary[ESkillKey.Q].sprite = skillSprite.QSkillSprite;
            else
                _backIconImageDictionary[ESkillKey.Q].sprite = _lookImage;


            if (weapon.EquipmentSkillSystemCompo.GetSkill(1) != null &&
                weapon.EquipmentSkillSystemCompo.GetSkill(1).skillEnabled == true)
                _backIconImageDictionary[ESkillKey.E].sprite = skillSprite.ESkillSprite;
            else
                _backIconImageDictionary[ESkillKey.E].sprite = _lookImage;


            if (weapon.EquipmentSkillSystemCompo.GetSkill(2) != null &&
                weapon.EquipmentSkillSystemCompo.GetSkill(2).skillEnabled == true)
                _backIconImageDictionary[ESkillKey.R].sprite = skillSprite.RSkillSprite;
            else
                _backIconImageDictionary[ESkillKey.R].sprite = _lookImage;

            //SkillSpriteContain skillSprtie = WeaponToSkillContain(weapon);

            //_backIconImageDictionary[ESkillKey.Q].sprite = skillSprtie.WeaponSkill.SkillSprite.QSkillSprite;
            //_backIconImageDictionary[ESkillKey.R].sprite = skillSprtie.WeaponSkill.SkillSprite.RSkillSprite;
            //_backIconImageDictionary[ESkillKey.E].sprite = skillSprtie.WeaponSkill.SkillSprite.ESkillSprite;
        }

        private SkillSpriteContain WeaponToSkillContain(PlayerWeapon weapon)
        {
            SkillSpriteContain skillContain = new SkillSpriteContain();
            for (int i = 0; i < _skillListSO.List.Count; ++i)
            {
                skillContain = _skillListSO.List[i];
                if (skillContain.WeaponType.ToString() == weapon.name)
                {
                    return skillContain;
                }
            }

            return skillContain;
        }


        private void OnDestroy()
        {
            _playerEquipment.WeaponChangeEvent -= HandleWeaponChange;
            _gameEventChannelSO.RemoveListener<GetStoreSkill>(HandleGetStoreSkill);
        }
    }
}