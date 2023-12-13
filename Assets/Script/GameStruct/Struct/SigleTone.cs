using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class SingleTon<T> : MonoBehaviourPunCallbacks where T : SingleTon<T>
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance == null)
            {
                instance = value;
                DontDestroyOnLoad(instance.gameObject);
            }
            else
            {
                Destroy(value.gameObject);
            }
        }
    }

    protected virtual void Awake()
    {
        Instance = (T)this;
    }
}
