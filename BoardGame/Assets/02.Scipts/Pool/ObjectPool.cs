using System.Collections;
using System.Collections.Generic;
using BoardGame.Util;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private ObjectPoolData[] poolData;
    [SerializeField] private Transform spawnPos;

    private readonly Dictionary<PoolObjectType, ObjectPoolData> _objectPoolData = new();
    private readonly Dictionary<PoolObjectType, Queue<GameObject>> _pool = new();

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        Initialize();
    }

    private void Initialize()
    {
        foreach(var data in poolData)
        {
            _objectPoolData.Add(data.poolType, data);
        }

        foreach(var data in _objectPoolData)
        {
            _pool.Add(data.Key, new Queue<GameObject>());

            var objectPoolData = data.Value;

            for(int i = 0; i < objectPoolData.poolCount; i++)
            {
                var poolObject = CreateNewObject(data.Key);
                _pool[data.Key].Enqueue(poolObject);
            }
        }
    }

    private GameObject CreateNewObject(PoolObjectType type)
    {
        GameObject GameObj = Instantiate(_objectPoolData[type].prefabs, transform);
        GameObj.SetActive(false);

        return GameObj;
    }

    public GameObject GetObject(PoolObjectType type)
    {
        if(_pool[type].Count > 0)
        {
            var obj = _pool[type].Dequeue();
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(spawnPos);
            return obj;
        }
        else
        {
            var obj = CreateNewObject(type);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(spawnPos);
            return obj;
        }
    }

    public void ReturnObject(PoolObjectType type, GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        _pool[type].Enqueue(obj);
    }
}
