using TMPro;
using UnityEngine.UI;
using static Baek.Util.Util;

namespace Baek.UI
{
    public class SkillCoolTimeUI : InitBase
    {
        private TextMeshProUGUI _coolTimeText;
        private Image _coolTimeImage;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            _coolTimeImage = GetComponent<Image>();
            _coolTimeText = FindChild<TextMeshProUGUI>(gameObject);


            return true;
        }


        public void ReSetFile()
        {
            _coolTimeText.text = string.Empty;
            _coolTimeImage.fillAmount = 0;
        }

        public void UpdateView(float current, float total)
        {
            if (current == 0)
            {
                _coolTimeText.text = string.Empty;
            }
            else
            {
                _coolTimeText.text = (current).ToString("F1");
                _coolTimeImage.fillAmount = (current / total    );
            }
        }
    }
}
