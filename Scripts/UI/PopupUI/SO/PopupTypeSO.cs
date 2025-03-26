using UnityEngine;

namespace Baek.UI
{
    [CreateAssetMenu(menuName = "Baek/SO/PopupType")]
    public class PopupTypeSO : ScriptableObject
    {
        [SerializeField] private bool _isBlackBoardUI; public bool IsBlackBoardUI { get { return _isBlackBoardUI; } }
    }
}
