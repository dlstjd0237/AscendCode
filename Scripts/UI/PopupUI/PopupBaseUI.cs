using UnityEngine;
using DG.Tweening;
using static Baek.Util.Util;
using static Baek.Define.Define;
using Baek.Manager;
using System;

namespace Baek.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PopupBaseUI : InitBase
    {
        //====PopupType
        [SerializeField] protected PopupTypeSO _uiType;

        //====UIActiveChecker====
        private bool _isShowPopup;
        public bool IsShowPopup => _isShowPopup;
        //=======================


        //==== Use Black Board ====
        [SerializeField] private bool _isUseblackBoard = true;
        public bool IsUseBlackBoard => _isUseblackBoard;
        //=========================


        //====Canvas==== 
        private Canvas _canvas;

        public Canvas Canvas
        {
            get => _canvas;
            set => _canvas = value;
        }
        //==============


        //====UIActiveEvent
        public event Action OnShowComplete;
        public event Action OnCloseComplete;
        //==============


        protected CanvasGroup _canvasGroup;


        public override bool Init()
        {
            if (base.Init() == false)
                return false;


            // ==== Canvas ====
            _canvas = GetOrAddCompoenet<Canvas>(gameObject);
            if (NullCheck(_canvas) == false)
                return false;
            // =================


            // ==== Canvas Group ====
            _canvasGroup = GetOrAddCompoenet<CanvasGroup>(gameObject);
            if (NullCheck(_canvasGroup) == false)
                return false;
            // ======================


            // ==== UI Type ====
            Managers.UI.SetPopup(_uiType, this);
            if (NullCheck(_uiType) == false)
                return false;
            // ==================


            return true;
        }

        /// <summary>
        /// override ���� �� base.Show�� ���� �Ʒ�����
        /// </summary>
        public virtual void Show() => CanvasGroupActive(true);


        /// <summary>
        /// override ���� �� base.Close ���� �Ʒ�����
        /// </summary>
        public virtual void Close() => CanvasGroupActive(false);


        protected void CanvasGroupActive(bool value, float duration = UIDuration)
        {
            int endValue = value == true ? 1 : 0;
            _canvasGroup.interactable = value;
            _canvasGroup.DOFade(endValue, duration).OnComplete(() =>
            {
                _isShowPopup = value;
                _canvasGroup.blocksRaycasts = value;
                if (value)
                    OnShowComplete?.Invoke();
                else
                    OnCloseComplete?.Invoke();
            }).SetUpdate(true);
        }

        protected void CanvasGroupActive(bool value, CanvasGroup canvas, float duration = UIDuration)
        {
            int endValue = value == true ? 1 : 0;
            canvas.interactable = value;
            canvas.DOFade(endValue, duration).OnComplete(() =>
            {
                _isShowPopup = value;
                canvas.blocksRaycasts = value;
                if (value)
                    OnShowComplete?.Invoke();
                else
                    OnCloseComplete?.Invoke();
            }).SetUpdate(true);
        }
    }
}