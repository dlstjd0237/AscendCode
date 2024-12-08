using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using static Baek.Util.Util;
using static Baek.Define.Define;
using System.Threading.Tasks;
using Baek.Manager;
using FMODUnity;
using PJH.Scene;
using UAPT.UI;

namespace Baek.UI
{
    public class TitleMenuUI : PopupBaseUI
    {
        private const int _defualtFontSize = 120;
        private const int _hoverFontSize = 130;

        private readonly Color _defualtFontColor = new Color(1, 1, 1);
        private readonly Color _hoverFontColor = new Color(0.7735849f, 0.715388f, 0.581405f);

        private const int _defualtSpacing = 0;
        private const int _hoverSpacing = 10;

        [SerializeField] private PopupTypeSO _optionTypeSO;
        [SerializeField] private EventReference _btnClickEventReference, _btnEnterEventReference;

        private Dictionary<string,
            (UI_EventHandler handler, RectTransform rectTrm
            , Action<PointerEventData> onClick
            , Action<PointerEventData> enterMouse
            , Action<PointerEventData> exitMouse)> _menuItemDictionary = new();

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            FixedScreen.FixedScreenSet();
            SetText("PlayText", HandlePlayClick, HandleTextEnter, HandleTextExit);
            SetText("OptionText", HandleOptionClick, HandleTextEnter, HandleTextExit);
            SetText("ExitText", HandleExitClick, HandleTextEnter, HandleTextExit);


            return true;
        }

        private void HandleTextExit(PointerEventData evt)
        {
            var titleScene = (PJH.Manager.Managers.Scene.CurrentScene as TitleScene);
            if (!titleScene.CanClickUI()) return;
            if (_menuItemDictionary.TryGetValue(evt.pointerEnter.name, out var item))
            {
                TextMeshProUGUI text = item.handler.GetComponent<TextMeshProUGUI>();
                DOTween.To(() => text.fontSize, x => text.fontSize = x, _defualtFontSize, UIDuration);
                DOTween.To(() => text.characterSpacing, x => text.characterSpacing = x, _defualtSpacing, UIDuration);
            }
        }

        private void HandleTextEnter(PointerEventData evt)
        {
            var titleScene = (PJH.Manager.Managers.Scene.CurrentScene as TitleScene);
            if (!titleScene.CanClickUI()) return;
            RuntimeManager.PlayOneShot(_btnEnterEventReference);

            if (_menuItemDictionary.TryGetValue(evt.pointerEnter.name, out var item))
            {
                TextMeshProUGUI text = item.handler.GetComponent<TextMeshProUGUI>();
                DOTween.To(() => text.fontSize, x => text.fontSize = x, _hoverFontSize, UIDuration);
                DOTween.To(() => text.characterSpacing, x => text.characterSpacing = x, _hoverSpacing, UIDuration);
            }
        }

        private void SetText(string name
            , Action<PointerEventData> clickEvent
            , Action<PointerEventData> enterEvent
            , Action<PointerEventData> exitEvent)
        {
            var handler = FindChild<UI_EventHandler>(gameObject, name, true);
            Debug.Log(handler);
            var transform = handler.GetComponent<RectTransform>();
            if (NullCheck(handler) == false) return;

            handler.AddEventListener(EUIEventType.Click, clickEvent);
            handler.AddEventListener(EUIEventType.Enter, enterEvent);
            handler.AddEventListener(EUIEventType.Exit, exitEvent);


            _menuItemDictionary[name] = (handler, transform, clickEvent, enterEvent, exitEvent);
        }

        private void HandleExitClick(PointerEventData evt)
        {
            var titleScene = (PJH.Manager.Managers.Scene.CurrentScene as TitleScene);
            if (!titleScene.CanClickUI()) return;
            ActiveMenu(false);
            RuntimeManager.PlayOneShot(_btnClickEventReference);

            Application.Quit();
        }

        private void HandleOptionClick(PointerEventData evt)
        {
            var titleScene = (PJH.Manager.Managers.Scene.CurrentScene as TitleScene);
            if (!titleScene.CanClickUI()) return;
            ActiveMenu(false);
            RuntimeManager.PlayOneShot(_btnClickEventReference);

            Managers.UI.ShowPopup(_optionTypeSO);
        }

        private void HandlePlayClick(PointerEventData evt)
        {
            var titleScene = (PJH.Manager.Managers.Scene.CurrentScene as TitleScene);
            if (!titleScene.CanClickUI()) return;
            ActiveMenu(false);
            RuntimeManager.PlayOneShot(_btnClickEventReference);

            DOVirtual.DelayedCall(1, delegate { titleScene.PlayStartTimeline(); });
            //�̰��� �� ü���� �־����
        }

        public void HandleOptionClose()
        {
            //ActiveMenu
        }

        public async void ActiveMenu(bool value)
        {
            var positions = value == true
                ? new[] { new Vector2(60, 540), new Vector2(60, 330), new Vector2(60, 140) }
                : new[] { new Vector2(-500, 540), new Vector2(-500, 330), new Vector2(-500, 140) };
            int index = 0;
            foreach (var item in _menuItemDictionary.Values)
            {
                SetAnchorPos(item.rectTrm, positions[index++]);
                await Task.Delay(300);
            }
        }

        private void SetAnchorPos(RectTransform rect, Vector2 pos) =>
            rect.DOAnchorPos(pos, UIDuration).SetEase(Ease.InOutBack);


        private void OnDestroy()
        {
            foreach (var item in _menuItemDictionary.Values)
            {
                if (item.handler != null)
                {
                    item.handler.RemoveEventListener(EUIEventType.Click, item.onClick);
                    item.handler.RemoveEventListener(EUIEventType.Enter, item.enterMouse);
                    item.handler.RemoveEventListener(EUIEventType.Exit, item.exitMouse);
                }
            }
        }
    }
}