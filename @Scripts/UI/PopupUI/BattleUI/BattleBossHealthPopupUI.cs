using UnityEngine;
using DG.Tweening;
using static Baek.Define.Define;
using static Baek.Util.Util;

namespace Baek.UI
{
    public class BattleBossHealthPopupUI : PopupBaseUI
    {
        private const float _defualtPosY = 0;
        private RectTransform _rectTrm;
        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            //==== Rect Trm ====
            _rectTrm = FindChild<RectTransform>(gameObject, "BossHealthUIRoot", true);
            if (_rectTrm.ValueNullCheck() == false)
                return false;
            //==================

            return true;
        }



        public override void Show()
        {
            base.Show();
            _rectTrm.DOLocalMoveY(_defualtPosY, UIDuration * 0.75f);
        }

        public override void Close()
        {
            base.Close();
            _rectTrm.DOLocalMoveY(100, UIDuration);
        }
    }
}

