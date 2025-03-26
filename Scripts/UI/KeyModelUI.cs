using PJH.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Baek.Util.Util;

public class KeyModelUI : InitBase
{
    [SerializeField]
    private PlayerInputSO _playerInput;
    private TextMeshProUGUI _text;
    private Button _button;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //==== Input Setting ====
        if (_playerInput.ValueNullCheck() == false)
            return false;
        //=======================



        //==== Text Set ====
        _text = FindChild<TextMeshProUGUI>(gameObject, "KeyCode", true);
        if (_text.ValueNullCheck() == false)
            return false;
        //==================


        //==== Button Set ====
        _button = FindChild<Button>(gameObject, "KeyButton", true);
        if (_button.ValueNullCheck() == false)
            return false;
        //====================





        return true;
    }


    private void OnDestroy()
    {
    }


}
