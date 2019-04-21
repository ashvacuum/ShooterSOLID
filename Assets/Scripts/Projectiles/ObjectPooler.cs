using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;
    public GameObject objectToPool;
    public bool shouldExpand = true;
}

public class ObjectPooler : MonoBehaviour
{

    public List<ObjectPoolItem> itemsToPool;
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledObjects;
    
    
    public bool shouldExpand = true;

    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    public void ActivateDeathTimer(GameObject objectToDeactive, float deathTime)
    {
        StopCoroutine(DeathTimer(objectToDeactive, deathTime));
        StartCoroutine(DeathTimer(objectToDeactive, deathTime));
    }

    public void ActivateDeathTimer(GameObject objectToDeactive)
    {
        StopCoroutine(DeathTimer(objectToDeactive));
        StartCoroutine(DeathTimer(objectToDeactive));
    }


    IEnumerator DeathTimer(GameObject objectToDeactive, float deathTime)
    {
        if (objectToDeactive.activeInHierarchy)
        {
            yield return new WaitForSeconds(deathTime);
            objectToDeactive.SetActive(false);
        } else
        {
            yield break;
        }        
    }

    IEnumerator DeathTimer(GameObject objectToDeactive)
    {
        yield return new WaitForSeconds(0.1f);
        objectToDeactive.SetActive(false);
    }


}
