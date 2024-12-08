using Baek.Skill;
using DG.Tweening;
using FMODUnity;
using PJH.Agent.Player;
using PJH.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PJH.EquipmentSkillSystem
{
    public class VolleySkill : EquipmentSkill
    {
        [TabGroup("Info")] [SerializeField] private int _arrowAmount;
        [TabGroup("Info")] [SerializeField] private float _skillDis;

        [SerializeField] private EventReference _arrowShotEventReference;
        private PoolManagerSO poolManager;
        [TabGroup("Pool")] [SerializeField] private PoolTypeSO _arrowSkillType;

        public override void Init(Player player, Equipment.Equipment equipment)
        {
            base.Init(player, equipment);
            poolManager = Managers.Addressable.Load<PoolManagerSO>("PoolManager");

            _player.GetCompo<PlayerAnimator>().ShotVolleySkillArrowEvent += HandleShotVolleySkillArrowEvent;
        }

        private void HandleShotVolleySkillArrowEvent()
        {
            RaycastHit hit = Baek.Util.Util.GetMouseToRay(Camera.main);

            Vector3 targetPos = new Vector3(hit.point.x, _player.transform.position.y, hit.point.z);
            Vector3 baseDirection = (targetPos - _player.transform.position).normalized;
            float baseAngle = Mathf.Atan2(baseDirection.z, baseDirection.x) * Mathf.Rad2Deg;

            float spreadAngle = 45f; // ��ä�� ��ü ����
            float halfSpread = spreadAngle / 2f;

            for (int i = 0; i < _arrowAmount; i++)
            {
                float angleOffset = Mathf.Lerp(-halfSpread, halfSpread, (float)i / (_arrowAmount - 1));
                float arrowAngle = baseAngle + angleOffset;

                Vector3 direction = new Vector3(
                    Mathf.Cos(arrowAngle * Mathf.Deg2Rad),
                    0,
                    Mathf.Sin(arrowAngle * Mathf.Deg2Rad)
                ).normalized;

                Quaternion lookRot = Quaternion.LookRotation(direction);

                VolleySkillArrow arrow = poolManager.Pop(_arrowSkillType) as VolleySkillArrow;
                arrow.maxDis = 15;
                arrow.transform.position = transform.position; // ȭ���� ���� ��ġ
                RuntimeManager.PlayOneShot(_arrowShotEventReference, transform.position);
                arrow.ShotUp(lookRot); // ȭ�� �߻�
            }
        }


        public override void UseSkill(bool isHolding)
        {
            if (isHolding) return;
            base.UseSkill(isHolding);

            Vector3 worldMousePos = _player.PlayerInput.GetWorldMousePosition();

            _player.transform.DOLookAt(worldMousePos, .2f, AxisConstraint.Y);
            var animatorCompo = _player.GetCompo<PlayerAnimator>();

            animatorCompo.SetRootMotion(true);
            animatorCompo.PlaySkillAnimation(0);

            animatorCompo.EndUseSkillEvent += HandleEndUseSkillEvent;
        }

        private void HandleEndUseSkillEvent()
        {
            var animatorCompo = _player.GetCompo<PlayerAnimator>();

            animatorCompo.SetRootMotion(false);
            animatorCompo.EndUseSkillEvent -= HandleEndUseSkillEvent;
        }

        private void OnDestroy()
        {
            _player.GetCompo<PlayerAnimator>().ShotVolleySkillArrowEvent -= HandleShotVolleySkillArrowEvent;
        }

        public override bool AttemptUseSkill(bool isHolding = false)
        {
            Vector3 mousePos = _player.PlayerInput.GetWorldMousePosition();
            float mouseDis = Vector3.Distance(mousePos, _player.transform.position);

            if (mouseDis > _skillDis) return false;
            return base.AttemptUseSkill(isHolding);
        }
    }
}