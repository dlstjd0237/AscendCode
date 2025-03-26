using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Baek.Define.Define;

namespace Baek.UI
{
    public class UI_EventHandler : MonoBehaviour
        , IPointerClickHandler
        , IPointerDownHandler
        , IPointerEnterHandler
        , IPointerExitHandler
        , IPointerMoveHandler
        , IPointerUpHandler
    {
        public event Action<PointerEventData>
            OnMouseClick
            , OnMouseDown
            , OnMouseUp
            , OnMouseEnter
            , OnMouseExit
            , OnMouseMove;

        private Dictionary<EUIEventType, Action<PointerEventData>> _eventDataDictionary = new Dictionary<EUIEventType, Action<PointerEventData>>();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_eventDataDictionary.ContainsKey(EUIEventType.Click))
                _eventDataDictionary[EUIEventType.Click]?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_eventDataDictionary.ContainsKey(EUIEventType.Down))
                _eventDataDictionary[EUIEventType.Down]?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_eventDataDictionary.ContainsKey(EUIEventType.Enter))
                _eventDataDictionary[EUIEventType.Enter]?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_eventDataDictionary.ContainsKey(EUIEventType.Exit))
                _eventDataDictionary[EUIEventType.Exit]?.Invoke(eventData);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_eventDataDictionary.ContainsKey(EUIEventType.Move))
                _eventDataDictionary[EUIEventType.Move]?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_eventDataDictionary.ContainsKey(EUIEventType.Up))
                _eventDataDictionary[EUIEventType.Up]?.Invoke(eventData);
        }
        public void AddEventListener(EUIEventType type, Action<PointerEventData> listener)
        {
            if (_eventDataDictionary.ContainsKey(type) == true)
                _eventDataDictionary[type] += listener;
            else
                _eventDataDictionary.Add(type, listener);
        }

        public void RemoveEventListener(EUIEventType type, Action<PointerEventData> listener)
        {
            if (_eventDataDictionary.ContainsKey(type) == true)
                _eventDataDictionary[type] -= listener;
        }

    }
}

