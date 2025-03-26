using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PoolManager))]
public class PoolManagerEditor : Editor
{
    const string INFO = "������Ʈ �����ö�\n" +
        " void Start()\n{\n" +
        "   PoolManager.SpawnFromPool(�±� �̸�,��ġ, ����);\n}\n" +
        "\nǮ���� ������Ʈ�� ������ ��������. \n void OnDisable()\n{\n" +
        "   PoolManager.ReturnToPool(gameObject);   //�� ��ü�� �ѹ��� \n" +
        "   CancelInvoke();     //Monobehaviour�� Invoke�� �ִٸ�\n}";
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(INFO, MessageType.Info);
        base.OnInspectorGUI();
    }
}
#endif



public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private void Awake() => Instance = this;

    [Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }

    [SerializeField] Pool[] _pools;
    List<GameObject> _spawnObjects;
    Dictionary<string, Queue<GameObject>> _poolDictionary;
    readonly string INFO = "Ǯ���� ������Ʈ�� ������ �������� \n void OnDisable()\n{\n" +
        "   PoolManager.ReturnToPool(gameObject);   //�� ��ü�� �ѹ��� \n" +
        "   CancelInvoke();     //Monobehaviour�� Invoke�� �ִٸ�\n}";

    public static GameObject SpawnFromPool(string tag, Vector3 position) =>
        Instance._SpawnFromPool(tag, position, Quaternion.identity);
    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) =>
        Instance._SpawnFromPool(tag, position, rotation);
    public static T SpawnFromPool<T>(string tag, Vector3 position) where T : Component
    {
        GameObject obj = Instance._SpawnFromPool(tag, position, Quaternion.identity);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"������Ʈ ã������ ����");
        }
    }

    public static T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject obj = Instance._SpawnFromPool(tag, position, rotation);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"������Ʈ ã������ ����");
        }
    }
    public static List<GameObject> GetAllPools(string tag)
    {
        if (!Instance._poolDictionary.ContainsKey(tag))
            throw new Exception($"{ tag } �±װ� �ִ� Ǯ�� �������� ����");
        return Instance._spawnObjects.FindAll(x => x.name == tag);
    }

    public static List<T> GetAllPools<T>(string tag) where T : Component
    {
        List<GameObject> objects = GetAllPools(tag);

        if (!objects[0].TryGetComponent(out T component))
            throw new Exception($"������Ʈ ã������ ����");
        return objects.ConvertAll(x => x.GetComponent<T>());
    }

    public static void ReturnToPool(GameObject obj)
    {
        if (!Instance._poolDictionary.ContainsKey(obj.name))
            throw new Exception($"{obj.name} �±װ� �ִ� Ǯ�� �������� ����.");

        Instance._poolDictionary[obj.name].Enqueue(obj);
    }

    [ContextMenu("GetSpawnObjectsInfo")]
    private void GetSpawnObjectsInFo()
    {
        foreach (var pool in _pools)
        {
            int count = _spawnObjects.FindAll(x => x.name == pool.Tag).Count;
            Debug.Log($"{pool.Tag} count : {count}");
        }
    }

    private GameObject _SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(tag))
            throw new Exception($"{tag} �±װ� �ִ� Ǯ�� �������� ���� "); // �±װ� �������

        //ť�� ������ ���� �߰�
        Queue<GameObject> poolQueue = _poolDictionary[tag];
        if (poolQueue.Count <= 0)
        {
            Pool pool = Array.Find(_pools, x => x.Tag == tag);
            var obj = CreateNewObject(pool.Tag, pool.Prefab);
            ArrangePool(obj);
        }

        //ť���� ������ ���
        GameObject objectToSpawn = poolQueue.Dequeue(); //Dequeue�� ť���� �����ٴ� ����
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    private void Start()
    {
        _spawnObjects = new List<GameObject>();
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //�̸� ����
        foreach (Pool pool in _pools)
        {
            _poolDictionary.Add(pool.Tag, new Queue<GameObject>());
            for (int i = 0; i < pool.Size; i++)
            {
                var obj = CreateNewObject(pool.Tag, pool.Prefab);
                ArrangePool(obj);
            }

            //OnDisable�� ReturnToPool �������ο� �ߺ����� �˻�
            if (_poolDictionary[pool.Tag].Count <= 0)
                Debug.LogError($"{pool.Tag}{INFO}");
            else if (_poolDictionary[pool.Tag].Count != pool.Size)
                Debug.LogError($"{pool.Tag}�� ReturnToPool�� �ߺ�������.");
        }
    }

    private GameObject CreateNewObject(string tag, GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.name = tag;
        obj.SetActive(false); // ��Ȱ��ȭ�� ReturnToPool�� �ϹǷ� Enqueu�� ��
        return obj;
    }

    private void ArrangePool(GameObject obj)
    {
        //�߰��� ������Ʈ ��� ����
        bool isFind = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == transform.childCount - 1)
            {
                obj.transform.SetSiblingIndex(i);
                _spawnObjects.Insert(i, obj);
                break;
            }
            else if (transform.GetChild(i).name == obj.name) //���� �̸��� ������Ʈ �̸��� ������ ������Ʈ�� �� ã������
                isFind = true;
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                _spawnObjects.Insert(i, obj);
                break;
            }
        }
    }
}
