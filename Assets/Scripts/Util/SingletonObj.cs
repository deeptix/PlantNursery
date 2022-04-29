using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonObj : MonoBehaviour
{
    public static SingletonObj instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
