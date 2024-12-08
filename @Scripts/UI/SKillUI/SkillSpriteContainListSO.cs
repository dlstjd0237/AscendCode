using System.Collections.Generic;
using UnityEngine;

namespace Baek.UI
{
    [CreateAssetMenu(menuName = "Baek/SO/SkillUIList")]
    public class SkillSpriteContainListSO : ScriptableObject
    {
        [SerializeField] private List<SkillSpriteContain> _list;
        public List<SkillSpriteContain> List { get { return _list; } }
    }
}

