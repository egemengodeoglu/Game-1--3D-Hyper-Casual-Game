using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<int, Queue<PoolObject>> poolDictionary = new Dictionary<int, Queue<PoolObject>>();
    private Queue<PoolObject> pooledObjects = new Queue<PoolObject>();

    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<PoolManager>();
            return _instance;
        }
    }



    private PoolObject CreatePoolObject(PoolObject prefab, int poolId)
    {
        PoolObject poolObject = Instantiate(prefab) as PoolObject;
        poolObject.poolid = poolId;
        poolObject.gameObject.SetActive(false);
        return poolObject;
    }

    public void CreatePool(PoolObject prefab, int poolSize)
    {
        int id = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(id))
        {
            poolDictionary.Add(id, new Queue<PoolObject>());

            for (int i = 0; i < poolSize; i++)
            {
                poolDictionary[id].Enqueue(CreatePoolObject(prefab, id));
            }
        }
    }

    public void NotUsedObjects()
    {
        for (int i = 0; i < 10; i++)
        {
            PoolObject pooledObj = pooledObjects.Dequeue();
            pooledObj.gameObject.SetActive(false);
            poolDictionary[pooledObj.poolid].Enqueue(pooledObj);
        }
    }


    public PoolObject UseObject(PoolObject prefab, Vector3 position, Quaternion rotation, bool setActive = true)
    {
        int id = prefab.GetInstanceID();

        //Controls if dictionary have created pool. if not creates a new one
        if (!poolDictionary.ContainsKey(id))
        {
            CreatePool(prefab, 1);
        }

        //Controls if pool have object that can be used. if not creates new poolobject
        if (poolDictionary[id].Count == 0)
        {
            poolDictionary[id].Enqueue(CreatePoolObject(prefab, id));
        }

        PoolObject gObject = poolDictionary[id].Dequeue();
        pooledObjects.Enqueue(gObject);
        gObject.transform.position = position;
        gObject.transform.rotation = rotation;
        gObject.gameObject.SetActive(setActive ? true : false);
        return gObject;
    }
}
