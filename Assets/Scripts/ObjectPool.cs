using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject pooledObjectPrefab;
    [SerializeField]
    private int pooledAmount = 15;
    
    private List<GameObject> pooledObjects;
    public List<GameObject> PooledObjects
    { get { return pooledObjects; } }   
    void Awake()
    {
        pooledObjects = new List<GameObject>();
    }

    private void Start()
    {
        for (int i = 0; i < pooledAmount; i++)
        {
            CreatePooledObject();
        }
    }


    GameObject CreatePooledObject()
    {
        GameObject obj = Instantiate(pooledObjectPrefab);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        pooledObjects.Add(obj); 
        return obj;
    }

    public GameObject GetPooledObject()
    {
        GameObject poolObj = null;

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                poolObj = pooledObjects[i];
                break;
            }
        }

        if(!poolObj)
        {
            for (int i = 0; i < 5; i++)
            {
                if(i == 0)
                {
                    poolObj = CreatePooledObject();
                }
                else
                {
                    CreatePooledObject();
                }
            }
        }

        return poolObj;
    }

    public void UnActivateAll()
    {
        foreach (GameObject obj in pooledObjects)
        {
            obj.SetActive(false);   
        }
    }
}
