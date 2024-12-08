using UnityEngine;
using DG.Tweening;
using static Baek.Define.Define;
using static Baek.Util.Util;
using PJH.Manager;
using TMPro;

namespace Baek.UI
{
    public class BattleCurrentFloorPopupUI : PopupBaseUI
    {
        private Material _mat;
        private TextMeshProUGUI _text;
        public override bool Init()
        {
            if (base.Init() == false)
                return false;


            _mat = Managers.Addressable.Load<Material>("CurrentFloorMat");
            if (NullCheck(_mat) == false)
                return false;


            _text = FindChild<TextMeshProUGUI>(gameObject, "CurrentFloorText", true);
            if (NullCheck(_text) == false)
                return false;


            _mat.SetFloat(DissolveAmountHash, 0);

            return true;
        }

        public void UpdateData(int currentFloor)
        {
            _text.text = $"{currentFloor.ToString()}Ãþ";
        }

        public override void Show()
        {
            _mat.DOFloat(1, DissolveAmountHash, UIDuration * 3).OnComplete(() => _text.DOFade(1, UIDuration * 6));
        }
        public override void Close()
        {
            _text.DOFade(0, UIDuration * 2).OnComplete(() => _mat.DOFloat(0, DissolveAmountHash, UIDuration * 5));
        }
    }
}

