using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool shuttingDown = false;
    private static object locker = new object();
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (shuttingDown) //�÷��̾ ������ ������ �Ǵ� �޴��� ��ũ��Ʈ�� ���� ������ return null
            {
                return null;
            }

            lock (locker)
            {
                if (instance == null)
                {
                    instance = (T)FindAnyObjectByType(typeof(T));
                    if (instance == null)
                    {
                        GameObject temp = new GameObject(typeof(T).ToString());
                        instance = temp.AddComponent<T>();
                    }

                    DontDestroyOnLoad(instance);
                }
            }

            return instance;
        }
    }

    private void OnApplicationQuit() //����� ���� ���� ��������
    {
        shuttingDown = true;
    }

    protected virtual void OnDestroy()
    {
        shuttingDown = true;
    }
}