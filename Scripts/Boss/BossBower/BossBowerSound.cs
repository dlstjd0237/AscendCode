using FMODUnity;
using UnityEngine;
using EventReference = FMODUnity.EventReference;

namespace Baek.Combat
{
    public class BossBowerSound : MonoBehaviour
    {
        [SerializeField] private EventReference _swingReference, _stabbingReference, _hitDownReference;
        public void GuardTwoSwing()
        {
            Debug.Log("���������¤��Ť���Ť����������");
            RuntimeManager.PlayOneShot(_swingReference, transform.position);
        }

        public void Stabbing()
        {
            RuntimeManager.PlayOneShot(_stabbingReference, transform.position);
        }

        public void HitDown()
        {
            RuntimeManager.PlayOneShot(_hitDownReference, transform.position);
        }
    }

}
