using UnityEngine;
using PJH.Input;
using static Baek.Util.Util;
namespace Baek.UI
{
    public abstract class SceneUIHandler : InitBase
    {
        [SerializeField] protected PlayerInputSO _inputSO;
        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            if (NullCheck(_inputSO) == false)
                return false;

            RegisterEvent();

            return true;
        }

        protected abstract void RegisterEvent();
        protected abstract void UnregisterEvent();

        public virtual void OnDestroy() => UnregisterEvent();
    }
}

