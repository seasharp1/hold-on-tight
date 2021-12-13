using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DestroyedObjects
{
    static readonly List<GameObject> objs = new List<GameObject>();
    
    public static void Add(GameObject obj)
    {
        if (!objs.Contains(obj))
        {
            objs.Add(obj);
        }
    }

    public static void DisableObjs()
    {
        if (objs.Count != 0)
        {
            for (int i = 0; i <= objs.Count; ++i)
            {
                objs[i].SetActive(false);
            }
        }
    }
}
