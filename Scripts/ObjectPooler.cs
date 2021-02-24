using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler Instance;
    public void Awake()
    {
        Instance = this;
    }

    #endregion

    private System.Random random;
    private PlayerController createrObjects;
    public Action<Vector3> CreateObjects;
    public List<Pool> redPools;
    public GameObject plane1, plane2;
    public GameObject repeater1, repeater2;
    public static Queue<GameObject> pooledObjects;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    
    void Start()
    {
        random = new System.Random();
        createrObjects = GameObject.FindObjectOfType<PlayerController>();
        createrObjects.CreaterObjects += CreaterObjects;
        pooledObjects = new Queue<GameObject>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        Starter();
    }
    
    private void Starter()
    {
        plane1.transform.position = new Vector3(0f, 0f, 48f);
        plane2.transform.position = new Vector3(0f, 0f, 148f);
        repeater1.transform.position = new Vector3(0f, 0f, 101f);
        repeater2.transform.position = new Vector3(0f, 0f, 201f);
        for (int i = 1; i <= 20; i++)
        {
            CreateObjects.Invoke(new Vector3(0f, 0f, i * 10f));
            
        }
        foreach (Pool pool in redPools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                obj.tag = pool.tag;
                pooledObjects.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }

    }

    private void CreaterObjects(float playerZ)
    {
        GameObject tmpGameObject;
        int tmp = (int)playerZ;
        tmp /= 10;
        tmp /= 10;
        if (tmp % 2 == 1)
        {
            plane1.transform.position = new Vector3(0f, 0f, tmp * 100 + 148);
            repeater1.transform.position = new Vector3(0f, 0f, tmp * 100 + 200f);
        }
        else
        {
            plane2.transform.position = new Vector3(0f, 0f, tmp * 100 + 148);
            repeater2.transform.position = new Vector3(0f, 0f, tmp * 100 + 200f);
        }

        for(int i = 0; i < 10; i++)
        {
            CreateObjects.Invoke(new Vector3(0f, 0f, playerZ + 110 + i*10f));
        }

        for(int i = 0; i < 10; i++)
        {
            tmpGameObject = pooledObjects.Dequeue();
            Debug.Log(tmpGameObject.tag+"");
            Debug.Log(poolDictionary[tmpGameObject.tag].ToString());
            poolDictionary[tmpGameObject.tag].Enqueue(tmpGameObject);
            tmpGameObject.SetActive(false);
        }
    }

    public GameObject SpawnFromPool ( string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "doesn't excist.");
            return null;
        }
        
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        pooledObjects.Enqueue(objectToSpawn);
        return objectToSpawn;
    }


}
