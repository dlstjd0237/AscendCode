using UnityEngine;
using UnityEngine.UI;
using static Baek.Util.Util;


namespace Baek.UI
{
    public class HealthAmountUI : InitBase
    {
        [SerializeField] private int _idx;
        public int Idx => _idx;

        private bool _enable = false;

        public bool Enable
        {
            get
            {
                return _enable;
            }
            set
            {
                ChangeEnable(value);
                _enable = false;
            }
        }

        private Image _healthImage;



        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            //==== Health Amount Image
            _healthImage = GetComponent<Image>();
            if (_healthImage.ValueNullCheck() == false)
                return false;
            //========================


            return true;
        }

        public void ChangeEnable(bool value) =>
            _healthImage.enabled = value;


    }
}


