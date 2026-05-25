using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] Path
    {
        get;
        private set;
    }

    void Awake()
    {
        Path = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Path[i] = transform.GetChild(i);
        }
    }
}
