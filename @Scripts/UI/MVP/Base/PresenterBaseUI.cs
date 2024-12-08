using UnityEngine;
using static Baek.Util.Util;

namespace Baek.UI
{
    public abstract class PresenterBaseUI : InitBase
    {
        [SerializeField] private bool _modelNotUsed;
        [SerializeField] private ModelBaseUI _model;

        public ModelBaseUI Model
        {
            get
            {
                if (NullCheck(_model) == false)
                {
                    return null;
                }
                return _model;
            }
            set => _model = value;
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            if (_modelNotUsed == false && NullCheck(_model) == true)
                _model.ValueChangEvent += HandleValueChanged;

            return true;
        }



        protected virtual void OnDestroy()
        {
            if (_modelNotUsed == false && NullCheck(_model) == true)
                _model.ValueChangEvent -= HandleValueChanged;
        }

        protected abstract void UpdateView();

        public virtual void HandleValueChanged()
        {
            UpdateView();
        }
    }

}
