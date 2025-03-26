using Baek.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Baek.Manager
{
    public class UIManager
    {
        private int _order = 10;
        private int _showCount = 0;
        private bool _init = false;
        private Dictionary<string, PopupBaseUI> _popupDictionaory = new Dictionary<string, PopupBaseUI>();
        public Dictionary<string, PopupBaseUI> PopupDictionary => _popupDictionaory;
        private PopupBaseUI _blackBoardUI;
        private Queue<PopupBaseUI> _popupUIQueue = new Queue<PopupBaseUI>();

        public void SetPopup(PopupTypeSO type, PopupBaseUI ui)
        {
            if (type.IsBlackBoardUI == true)
                _blackBoardUI = ui;
            if (_popupDictionaory.ContainsKey(type.name))
                _popupDictionaory[type.name] = ui;
            else
                _popupDictionaory.Add(type.name, ui);

            if (_init == false)
                SceneManager.sceneLoaded += HandleSceneLoad;
        }

        private void HandleSceneLoad(Scene arg0, LoadSceneMode arg1)
        {
            _popupUIQueue = new Queue<PopupBaseUI>();
        }

        public void ShowPopup(PopupTypeSO type, bool isSort = true, int sortingOrder = 0)
        {
            if (_popupDictionaory.TryGetValue(type.name, out var ui) == false)
            {
                Debug.LogError($"Not Contain {type}");
                return;
            }

            _popupUIQueue.Enqueue(ui);
            if (ui.IsShowPopup == true)
                return;


            // === BlackBoard ===
            if (ui.IsUseBlackBoard == true)
                _blackBoardUI.Show();

            ui.Canvas.sortingOrder = isSort ? _order++ : sortingOrder;

            _showCount += 1;

            ui.Show();
        }

        public void ClosePopup()
        {
            if (_popupUIQueue.Count != 0)
                _popupUIQueue.Dequeue().Close();
        }

        public void ClosePopup(PopupTypeSO type)
        {
            if (_popupDictionaory.TryGetValue(type.name, out var popup) == false)
            {
                Debug.LogError($"Not Contain Key {type.ToString()}");
                return;
            }

            if (!_popupUIQueue.Contains(popup))
            {
                return;
            }

            _showCount -= 1;

            if (_showCount == 0 && _blackBoardUI.IsShowPopup == true)
                _blackBoardUI.Close();

            _popupUIQueue.Dequeue();
            _popupDictionaory[type.name].Close();
        }


        public void AllClosePopup()
        {
            foreach (var item in _popupDictionaory)
            {
                if (item.Value.IsShowPopup == true)
                {
                    item.Value.Close();
                }
            }
        }

        public void AllClear()
        {
            _popupDictionaory.Clear();
        }
    }
}