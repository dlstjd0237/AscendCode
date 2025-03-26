using TMPro;
using static Baek.Util.Util;
using UnityEngine.UI;
using YTH.Boss;
using PJH.Combat;
using DG.Tweening;
using static Baek.Define.Define;
using UnityEngine;
using PJH.Manager;

namespace Baek.UI
{
    public class BossHealthPresenterUI : PresenterBaseUI
    {
        private EnemySpawnDataListSO _spawnDataList;
        private BossEnemy _bossEnemy;
        private Health _health;

        private TextMeshProUGUI _bossNameText;
        private Image _fillImage;

        private int _currentHealth;

        private int _maxHealth => _health.MaxHealth;

        public override bool Init()
        {
            _spawnDataList = Managers.Addressable.Load<EnemySpawnDataListSO>("EnemySpawnDataList");
            if (base.Init() == false)
                return false;

            //==== BossNameText ====
            _bossNameText = FindChild<TextMeshProUGUI>(this.gameObject, "BossNameText", true);
            if (NullCheck(_bossNameText) == false)
                return false;
            //======================


            //==== FillImage ====
            _fillImage = FindChild<Image>(this.gameObject, "BossHealthFill", true);
            if (NullCheck(_fillImage) == false)
                return false;
            //===================


            return true;
        }

        public void SetOwner(BossEnemy bossEnemy)
        {
            if (_health != null)
                _health.ChangedHealthEvent -= HandleHealthChange;

            _bossNameText.text = _spawnDataList.spawnDataList[WaveManager.Instance.CurrentStage].bossName.ToString();
            _bossEnemy = bossEnemy;
            _health = _bossEnemy.HealthCompo;
            _health.ChangedHealthEvent += HandleHealthChange;
            _health.DeathEvent += () => { _health.ChangedHealthEvent -= HandleHealthChange; };
            HandleHealthChange(_health.MaxHealth);
        }

        private void HandleHealthChange(int currentHealth)
        {
            _currentHealth = currentHealth;
            UpdateView();
        }

        protected override void UpdateView()
        {
            _fillImage.DOFillAmount(((float)_currentHealth / (float)_health.MaxHealth), UIDuration * 3);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _health.ChangedHealthEvent -= HandleHealthChange;
        }
    }
}