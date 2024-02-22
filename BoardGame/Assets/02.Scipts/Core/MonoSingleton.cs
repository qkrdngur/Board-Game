using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static bool _isQuitting = false; //����ǰų� disable�� �༮�ΰ�?

    public static T Instance
    {
        get
        {
            if (_isQuitting)
            {
                _instance = null;
            }

            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"{typeof(T).Name} is not exist");
                }
                else
                {
                    _isQuitting = false;
                }
            }
            return _instance;
        }
    }

    private void OnDisable()
    {
        _isQuitting = true;
        _instance = null;
    }
}
