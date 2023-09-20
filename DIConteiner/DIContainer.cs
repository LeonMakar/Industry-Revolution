using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIContainer
{
    public static DIContainer Instance { get; private set; }

    private Dictionary<string, object> _servises = new Dictionary<string, object>();

    public DIContainer()
    {
        Instance = this;
    }


    public void RegisterService<T>(T service) where T : class
    {
        if (!_servises.ContainsKey(typeof(T).Name))
            _servises.Add(typeof(T).Name, service);
        else
            Debug.Log("The same Key yet exist");
    }

    public T GetService<T>() where T : class
    {
        if (_servises.ContainsKey(typeof(T).Name))
            return _servises[typeof(T).Name] as T;
        else return null;
    }
}
