using UnityEngine;
using UnityEngine.InputSystem;
using Baek.Manager;
using Baek.UI;

public class UITest : MonoBehaviour
{
    [SerializeField] private PopupTypeSO _optionUI;
    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
            Managers.UI.ShowPopup(_optionUI);
        if (Keyboard.current.wKey.wasPressedThisFrame)
            Managers.UI.ClosePopup(_optionUI);
    }
}
