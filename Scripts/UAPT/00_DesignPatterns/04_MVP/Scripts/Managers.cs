using UnityEngine;

namespace Baek.Manager
{
    public class Managers : MonoBehaviour
    {
        private static Managers s_inctance;
        private static Managers Instacne
        {
            get
            {
                Init();
                return s_inctance;
            }
        }

        private ResourceManager _resource = new ResourceManager();
        private UIManager _ui = new UIManager();

        public static ResourceManager Resource { get { return Instacne._resource; } }
        public static UIManager UI { get { return Instacne._ui; } }
        private static void Init()
        {
            if (s_inctance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<Managers>();
                }

                DontDestroyOnLoad(go);

                //�ʱ�ȭ
                s_inctance = go.GetComponent<Managers>();
            }
        }
    }

}
