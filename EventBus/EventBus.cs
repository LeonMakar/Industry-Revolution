using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���������� �������� ���������� ����
/// </summary>
public class EventBus : IMainService, IService
{
    private Dictionary<string, List<object>> _signalCallbacks = new Dictionary<string, List<object>>();

    public void Subscrube<T>(Action<T> metod)
    {
        string key = typeof(T).Name;

        if (_signalCallbacks.ContainsKey(key))
            _signalCallbacks[key].Add(metod);
        else
            _signalCallbacks.Add(key, new List<object>() { metod });
    }


    /// <summary>
    /// �������� ��� ������ ����������� �� ������ �����
    /// </summary>
    /// <typeparam name="T"> ����� � �������� </typeparam>
    /// <param name="signal">����� � ��������</param>
    public void Invoke<T>(T signal)
    {
        string key = typeof(T).Name;
        if (_signalCallbacks.ContainsKey(key))
        {
            foreach (var obj in _signalCallbacks[key])
            {
                var metod = obj as Action<T>;
                metod?.Invoke(signal);
            }
        }
    }
    public void Unsubscribe<T>(Action<T> metod)
    {
        string key = typeof(T).Name;
        if (_signalCallbacks.ContainsKey(key))
            _signalCallbacks[key].Remove(metod);
        else
            Debug.LogErrorFormat("Trying to unsubscribe for not existing key! {0} ", key);
    }
}
