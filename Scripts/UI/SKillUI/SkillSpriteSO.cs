using UnityEngine;

namespace Baek.UI
{
    [CreateAssetMenu(menuName = "Baek/SO/SkillUI")]
    public class SkillSpriteSO : ScriptableObject
    {
        [SerializeField] private SkillSprite _skillSprite; public SkillSprite SkillSprite { get { return _skillSprite; } }
    }
}

