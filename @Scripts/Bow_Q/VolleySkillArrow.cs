using System;
using PJH.Core;
using UnityEngine;
using Cysharp.Threading.Tasks;
using INab.Dissolve;
using PJH.Manager;

namespace Baek.Skill
{
    public class VolleySkillArrow : BowSkillArrow
    {
        private static readonly int DissolveAmountHash = Shader.PropertyToID("_DissolveAmount");
        private PoolManagerSO poolManager;
        [SerializeField] private PoolTypeSO _stormExplosionVariantType;
        public float maxDis;
        private Vector3 _startPos;
        private Rigidbody _rigidbodyCompo;
        private bool _stuck;

        private bool _isShotUp;

        private Dissolver _dissolver;
        private ParticleSystem[] _trails;
        private Transform _originParentTrm;


        public override void SetUpPool(Pool pool)
        {
            base.SetUpPool(pool);
            poolManager = Managers.Addressable.Load<PoolManagerSO>("PoolManager");

            _trails = GetComponentsInChildren<ParticleSystem>();
            _dissolver = GetComponent<Dissolver>();
            _rigidbodyCompo = GetComponent<Rigidbody>();

            Transform model = transform.Find("Model");
            MeshRenderer meshRenderer = model.GetComponent<MeshRenderer>();
            _dissolver.materials.AddRange(meshRenderer.materials);
            _originParentTrm = transform.parent;
        }

        public override void ResetItem()
        {
            base.ResetItem();
            _stuck = false;
            _rigidbodyCompo.isKinematic = false;
            transform.SetParent(_originParentTrm);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isShotUp || _stuck) return;

            if (other.TryGetComponent(out MInterface.IDamageable damageable))
            {
                Debug.Log("�̰� �ȵ�?");
                VolleySkillStormExplosion arrow =
                    poolManager.Pop(_stormExplosionVariantType) as VolleySkillStormExplosion;
                arrow.transform.position = transform.position;
                arrow.PlayParticle();
                _myPool.Push(this);
            }
            else
            {
                _stuck = true;
                transform.SetParent(other.transform);
                _rigidbodyCompo.isKinematic = true;
                Dissolve();
                EnableTrails(false);
            }
        }

        private void EnableTrails(bool enabled)
        {
            for (int i = 0; i < _trails.Length; i++)
            {
                if (enabled)
                    _trails[i].Play();
                else
                    _trails[i].Stop();
            }
        }

        public override void ShotUp(Quaternion rot)
        {
            base.ShotUp(rot);
            _startPos = transform.position;
            _isShotUp = true;
            EnableTrails(true);
        }

        protected override void Update()
        {
            if (_stuck) return;
            if (!_isShotUp) return;

            base.Update();
            float dis = Vector3.Distance(_startPos, transform.position);
            if (dis > maxDis)
            {
                Dissolve();
                _isShotUp = false;
            }
        }

        private async void Dissolve()
        {
            _dissolver.Dissolve();
            await UniTask.WaitForSeconds(_dissolver.duration + 2);

            foreach (var material in _dissolver.materials)
            {
                material.SetFloat(DissolveAmountHash, 0);
            }

            _myPool.Push(this);
        }
    }
}