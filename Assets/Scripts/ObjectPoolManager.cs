using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance
    { get { return instance; } }

    public enum PoolTypes { SMALL_ROCKS, MEDIUM_ROCKS, BIG_ROCKS, Random_METEORS, Random_Triangles, Marsian}

    [SerializeField]
    private ObjectPool[] objectPools;

    void Awake()
    {
        instance = this;
    }

   
    public GameObject GetPooledObject(PoolTypes _type)
    {
        return objectPools[(int)_type].GetPooledObject();
    }

    public List<GameObject> GetAllPooledObjects(PoolTypes _type)
    {
        return objectPools[(int)_type].PooledObjects;
    }

    public void DeactivateAll()
    {
        foreach(ObjectPool pool in objectPools)
        {
            pool.UnActivateAll();
        }
    }
}
