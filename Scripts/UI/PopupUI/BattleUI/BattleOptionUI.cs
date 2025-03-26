using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Baek.Manager;
using static Baek.Util.Util;
using static Baek.Define.Define;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;
using System;
using FMODUnity;
using PJH.Scene;

namespace Baek.UI
{
    public class BattleOptionUI : PopupBaseUI
    {
        private readonly Color _defualtColor = new Color(0.553459f, 0.5215815f, 0.5029863f);
        private readonly Color _choiceColor = new Color(1, 1, 1);

        [SerializeField] private UnityEvent _optionCloseEvent;
        [SerializeField] private EventReference _btnClickEventReference;
        private Button _closeButton, _exitButton;
        private Toggle _fullScreenToggle, _damageShowToggle, _muteToggle;
        private UI_EventHandler _exitEventHandler, _optionTextHandler, _controlTextHandler;
        private CanvasGroup _optionCanvas, _controlCanvas;
        private TextMeshProUGUI _exitText, _optionText, _controlText;


        #region Button Hover

        private const float _dR = 0.4654087f;
        private const float _dG = 0.3088089f;
        private const float _dB = 0.3088089f;

        private const float _hR = 0.5911949f;
        private const float _hG = 0.06506855f;
        private const float _hB = 0.06506855f;

        #endregion

        public override bool Init()
        {
            if (base.Init() == false)
                return false;


            //==== CloseButton ====
            _closeButton = FindChild<Button>(gameObject, "CloseButton", true);
            if (NullCheck(_closeButton) == false) return false;
            _closeButton.onClick.AddListener(HandleCloseButtonClick);
            //=====================


            //==== ExitButton ====
            _exitButton = FindChild<Button>(gameObject, "ExitButton", true);
            _exitEventHandler = _exitButton.GetComponent<UI_EventHandler>();
            _exitText = FindChild<TextMeshProUGUI>(_exitButton.gameObject, "ExitText", true);
            if (NullCheck(_exitButton) == false) return false;
            if (NullCheck(_exitEventHandler) == false) return false;
            if (NullCheck(_exitText) == false) return false;
            _exitButton.onClick.AddListener(HandleExitButtonClick);
            _exitEventHandler.AddEventListener(EUIEventType.Enter, HandleExitButtonEnter);
            _exitEventHandler.AddEventListener(EUIEventType.Exit, HandlerExitButtonExit);
            //====================


            //==== Full Screen Toggle ====
            _fullScreenToggle = FindChild<Toggle>(gameObject, "FullScreenToggle", true);
            if (NullCheck(_fullScreenToggle) == false) return false;
            _fullScreenToggle.isOn = IntToBool(PlayerPrefs.GetInt(EBattleUIKey.FullScreenToggle.ToString(), 0));
            _fullScreenToggle.onValueChanged.AddListener(HandleFullScreenChange);
            //============================


            //==== Damage Show Toggle ====
            _damageShowToggle = FindChild<Toggle>(gameObject, "DamageShowToggle", true);
            if (NullCheck(_damageShowToggle) == false) return false;
            _damageShowToggle.isOn = IntToBool(PlayerPrefs.GetInt(EBattleUIKey.ShowDamageTextToggle.ToString(), 1));
            _damageShowToggle.onValueChanged.AddListener(HandleDamageShowChange);
            //============================


            //==== Mute Toggle ====
            _muteToggle = FindChild<Toggle>(gameObject, "MuteToggle", true);
            if (NullCheck(_muteToggle) == false) return false;
            _muteToggle.isOn = IntToBool(PlayerPrefs.GetInt(EBattleUIKey.HandleMuteToggle.ToString(), 0));
            _muteToggle.onValueChanged.AddListener(HandleMuteChange);
            //=====================


            //==== Option Canvas Group ====
            _optionCanvas = FindChild<CanvasGroup>(gameObject, "OptionInfoContain", true);
            _controlCanvas = FindChild<CanvasGroup>(gameObject, "ControllContain", true);
            if (_optionCanvas.ValueNullCheck() == false) return false;
            if (_controlCanvas.ValueNullCheck() == false) return false;
            //=============================


            //==== Option Text Handler ====
            _optionTextHandler = FindChild<UI_EventHandler>(gameObject, "OptionText", true);
            _optionText = _optionTextHandler.GetComponent<TextMeshProUGUI>();

            _controlTextHandler = FindChild<UI_EventHandler>(gameObject, "ControlText", true);
            Debug.Log($"COntrolTextHandler  = {_controlTextHandler}");
            _controlText = _controlTextHandler.GetComponent<TextMeshProUGUI>();

            if (_optionTextHandler.ValueNullCheck() == false) return false;
            if (_controlTextHandler.ValueNullCheck() == false) return false;

            _optionTextHandler.AddEventListener(EUIEventType.Click, HandleOptionTextClick);
            _controlTextHandler.AddEventListener(EUIEventType.Click, HandleControlTextClick);
            //=============================


            return true;
        }

        private void HandleControlTextClick(PointerEventData evt)
        {
            RuntimeManager.PlayOneShot(_btnClickEventReference);

            _optionText.DOColor(_defualtColor, UIDuration).SetUpdate(true);
            _controlText.DOColor(_choiceColor, UIDuration).SetUpdate(true);
            CanvasGroupActive(true, _controlCanvas);
            CanvasGroupActive(false, _optionCanvas);
        }

        private void HandleOptionTextClick(PointerEventData evt)
        {
            RuntimeManager.PlayOneShot(_btnClickEventReference);

            _controlText.DOColor(_defualtColor, UIDuration).SetUpdate(true);
            _optionText.DOColor(_choiceColor, UIDuration).SetUpdate(true);
            CanvasGroupActive(true, _optionCanvas);
            CanvasGroupActive(false, _controlCanvas);
        }

        private void HandlerExitButtonExit(PointerEventData evt) =>
            _exitText.DOColor(new Color(_dR, _dG, _dB), UIDuration);

        private void HandleExitButtonEnter(PointerEventData evt) =>
            _exitText.DOColor(new Color(_hR, _hG, _hB), UIDuration);

        private void HandleMuteChange(bool value)
        {
            RuntimeManager.PlayOneShot(_btnClickEventReference);

            PJH.Manager.Managers.FMODSound.SetMuteSound(value);
            PlayerPrefs.SetInt(EBattleUIKey.HandleMuteToggle.ToString(), BoolToInt(value));
        }


        private void HandleDamageShowChange(bool value)
        {
            //�̰��� �ؽ�Ʈ �� ���ִ°� �־����
            RuntimeManager.PlayOneShot(_btnClickEventReference);

            PlayerPrefs.SetInt(EBattleUIKey.ShowDamageTextToggle.ToString(), BoolToInt(value));
        }

        private void HandleFullScreenChange(bool value)
        {
            RuntimeManager.PlayOneShot(_btnClickEventReference);

            Screen.fullScreen = value;
            PlayerPrefs.SetInt(EBattleUIKey.FullScreenToggle.ToString(), BoolToInt(value));
        }

        private void HandleCloseButtonClick()
        {
            RuntimeManager.PlayOneShot(_btnClickEventReference);
            Managers.UI.ClosePopup(_uiType);
        }

        public override void Show()
        {
            base.Show();
            (PJH.Manager.Managers.Scene.CurrentScene as GameScene)?.PauseGame();
        }

        public override void Close()
        {
            base.Close();
            _optionCloseEvent?.Invoke();
            (PJH.Manager.Managers.Scene.CurrentScene as GameScene)?.ResumeGame();
        }

        private void HandleExitButtonClick()
        {
            RuntimeManager.PlayOneShot(_btnClickEventReference);

            Application.Quit();
        }

        public void OnDestroy()
        {
            if (_closeButton != null)
                _closeButton.onClick.RemoveListener(HandleCloseButtonClick);

            if (_exitButton != null)
                _exitButton.onClick.RemoveListener(HandleExitButtonClick);

            if (_exitEventHandler != null)
            {
                _exitEventHandler.RemoveEventListener(EUIEventType.Enter, HandleExitButtonEnter);
                _exitEventHandler.RemoveEventListener(EUIEventType.Exit, HandlerExitButtonExit);
            }

            if (_fullScreenToggle != null)
                _fullScreenToggle.onValueChanged.RemoveListener(HandleFullScreenChange);

            if (_damageShowToggle != null)
                _damageShowToggle.onValueChanged.RemoveListener(HandleDamageShowChange);

            if (_muteToggle != null)
                _muteToggle.onValueChanged.RemoveListener(HandleMuteChange);
        }
    }
}