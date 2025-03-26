using UnityEngine;
using static Baek.Util.Util;
using Cysharp.Threading.Tasks;
using PJH.Manager;
using PJH.Scene;
using System.Collections.Generic;
using System.Linq;

namespace Baek.UI
{
    public class HealthPresenterUI : PresenterBaseUI
    {
        private PJH.Combat.Health _health;
        private int _currentHealthAmount = 0;
        private Transform _healthAmountSpawnRoot;
        private HealthAmountUI[] _healthAmountArr = new HealthAmountUI[20];

        public override bool Init()
        {
            if (base.Init() == false)
                return false;


            //==== Health Amount Load ====
            _healthAmountSpawnRoot = FindChild<Transform>(gameObject, "HealthAmountSpawnRoot", true);
            if (NullCheck(_healthAmountSpawnRoot) == false) // HealthAmountSpawnRoot
                return false;
            //============================


            //==== Health Amount Arr ====
            _healthAmountArr = _healthAmountSpawnRoot.GetComponentsInChildren<HealthAmountUI>().ToArray();
            //===========================

            return true;
        }

        private void Start()
        {
            //==== Health Load ====
            GameScene gameScene = (Managers.Scene.CurrentScene as GameScene);
            _health = gameScene.Player.HealthCompo;
            if (NullCheck(_health) == false) // Health
                return;

            _health.ChangedHealthEvent -= HandleHealthChange;
            _health.ChangedHealthEvent += HandleHealthChange;
            //=====================
        }


        private void HandleHealthChange(int currentHealth)
        {
            _currentHealthAmount = currentHealth;
            UpdateView();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (NullCheck(_health) == true)
                _health.ChangedHealthEvent -= HandleHealthChange;
        }

        protected override void UpdateView()
        {
            HealthAllDisable();

            for (int i = 0; i < _currentHealthAmount; ++i)
            {
                _healthAmountArr[i].Enable = true;
            }
        }

        private void HealthAllDisable()
        {
            for (int i = 0; i < _healthAmountArr.Length; ++i)
            {
                _healthAmountArr[i].Enable = false;
            }
        }


    }
}