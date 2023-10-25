using System;
using System.Collections.Generic;
using UnityEngine;

public class DIContainer
{
}


public interface IContainer
{
    void Register<TService, TImplamentation>() where TImplamentation : TService;

    TService Resolve<TService, TImplamentation>(params Type[] typesToInject);
    object Resolve(Type service, Type implamentation, params Type[] typesToInject);

}

public class Container : IContainer
{
    private readonly Dictionary<Type, List<Type>> _container = new Dictionary<Type, List<Type>>();

    public void Register<TService, TImplamentation>() where TImplamentation : TService
    {
        if (!_container.ContainsKey(typeof(TService)))
        {
            _container[typeof(TService)] = new List<Type>() { typeof(TImplamentation) };
        }
        else
        {
            _container[typeof(TService)].Add(typeof(TImplamentation));
        }
    }
    public object Resolve(Type service, Type implamentation, params Type[] typesToInject)
    {
        if(_container.ContainsKey(service))
        {

            foreach (var item in _container[service])
            {
                if (item == implamentation)
                {
                    Type implamentationType = item;
                    if (implamentationType.GetMethod(nameof(Methodname.InjectSingletone)) != null)
                    {
                        var methodInfo = implamentationType.GetMethod(nameof(Methodname.InjectSingletone));
                        var parametersInfo = methodInfo.GetParameters();
                        object[] argumentsAsParameterForMethod = new object[parametersInfo.Length];

                        for (int i = 0; i < parametersInfo.Length; i++)
                        {
                            try
                            {
                                var parametrServiceType = parametersInfo[i].ParameterType;
                                if (typesToInject.Length == parametersInfo.Length)
                                    argumentsAsParameterForMethod[i] = CreatInstance(parametrServiceType, typesToInject[i]);

                            }
                            catch (Exception)
                            {
                                Debug.Log("Lenght of parameters does't match with lenth of typesToInject");
                            }

                        }
                        var objectForReturn = Activator.CreateInstance(implamentationType);
                        methodInfo.Invoke(objectForReturn, argumentsAsParameterForMethod);

                        return objectForReturn;

                    }
                    else
                        return Activator.CreateInstance(implamentationType);
                }
                else
                    Debug.LogErrorFormat($"Types {item} and {implamentation} dosnt compare");
            }
        }
        throw new InvalidOperationException($"Current {service} was not registered ");
    }
    public TService Resolve<TService, TImplamentation>(params Type[] typesToInject)
    {
        if (_container.ContainsKey(typeof(TService)))
        {

            foreach (var item in _container[typeof(TService)])
            {
                if (item == typeof(TImplamentation))
                {
                    Type implamentationType = item;
                    if (implamentationType.GetMethod(nameof(Methodname.InjectSingletone)) != null)
                    {
                        var methodInfo = implamentationType.GetMethod(nameof(Methodname.InjectSingletone));
                        var parametersInfo = methodInfo.GetParameters();
                        object[] argumentsAsParameterForMethod = new object[parametersInfo.Length];

                        for (int i = 0; i < parametersInfo.Length; i++)
                        {
                            try
                            {
                                var parametrServiceType = parametersInfo[i].ParameterType;
                                if (typesToInject.Length == parametersInfo.Length)
                                    argumentsAsParameterForMethod[i] = CreatInstance(parametrServiceType, typesToInject[i]);

                            }
                            catch (Exception)
                            {
                                Debug.Log("Lenght of parameters does't match with lenth of typesToInject");
                            }

                        }
                        var objectForReturn = (TService)Activator.CreateInstance(implamentationType);
                        methodInfo.Invoke(objectForReturn, argumentsAsParameterForMethod);

                        return objectForReturn;

                    }
                    else
                        return (TService)Activator.CreateInstance(implamentationType);
                }
                continue;

            }
        }

        throw new InvalidOperationException($"Current {typeof(TService)} was not registered ");

    }
    private object CreatInstance(Type serviceType, Type implamentation = null)
    {
        foreach (var item in _container[serviceType])
        {
            Console.WriteLine(item.Name);
            if (item == implamentation)
                return Activator.CreateInstance(item);
        }
        throw new InvalidOperationException("Not registered");
    }
}

public enum Methodname
{
    InjectSingletone = 0,
    InjectTemporary = 1,
}




