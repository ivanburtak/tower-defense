using System;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] path
    {
        get;
        private set;
    }

    void Awake()
    {
        path = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            path[i] = transform.GetChild(i);
        }
    }
}
