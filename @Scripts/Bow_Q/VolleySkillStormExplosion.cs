using Baek.Combat;
using FMODUnity;
using PJH.Combat;
using PJH.Core;
using UnityEngine;

namespace Baek.Skill
{
    public class VolleySkillStormExplosion : InitBase, IPoolable
    {
        [SerializeField] private EventReference _explosionRefarence;
        [SerializeField] private DamageSO _damageSO;
        private ParticleSystem _particleSystem;
        private PoolTypeSO _poolType;
        public PoolTypeSO PoolType => _poolType;

        public GameObject GameObject => this.gameObject;

        private Pool _myPool;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            _particleSystem = GetComponent<ParticleSystem>();

            return true;
        }


        private void Update()
        {
            if (_particleSystem.isStopped)
            {
                _myPool.Push(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MInterface.IDamageable damageable))
            {
                CombatData combatData = new()
                {
                    damage = _damageSO.Damage,
                    hitPoint = other.transform.position,
                    damageCategory = PJH.Core.Define.EDamageCategory.Normal,
                };
                damageable.ApplyDamage(combatData);
            }
        }

        public void PlayParticle()
        {
            _particleSystem.Play();
            RuntimeManager.PlayOneShot(_explosionRefarence, transform.position);
        }

        public void ResetItem()
        {
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }
    }
}