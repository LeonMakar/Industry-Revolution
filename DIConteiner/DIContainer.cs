using JetBrains.Annotations;
using System;
using System.Collections.Generic;

public class DIContainer
{
}
public interface IContainer
{
    void Register<TService, TImplamentation>() where TImplamentation : TService;

    TService Resolve<TService, TImplamentation>(params Type[] typesToInject);

}

public class Container : IContainer
{
    private readonly Dictionary<Type, List<Type>> _container = new Dictionary<Type, List<Type>>();


    public void Register<TService, TImplamentation>() where TImplamentation : TService
    {
        if (!_container.ContainsKey(typeof(TService)))
            _container[typeof(TService)] = new List<Type>() { typeof(TImplamentation) };
        else
        {
            _container[typeof(TService)].Add(typeof(TImplamentation));
        }
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
                    if (implamentationType.GetMethod(nameof(Methodname.Inject)) != null)
                    {
                        var methodInfo = implamentationType.GetMethod(nameof(Methodname.Inject));
                        var parametersInfo = methodInfo.GetParameters();
                        object[] argumentsAsParameterForMethod = new object[parametersInfo.Length];

                        for (int i = 0; i < parametersInfo.Length; i++)
                        {
                            var parametrServiceType = parametersInfo[i].ParameterType;

                            if (typesToInject.Length == parametersInfo.Length)
                                argumentsAsParameterForMethod[i] = CreatInstance(parametrServiceType, typesToInject[i]);
                            else
                                argumentsAsParameterForMethod[i] = CreatInstance(parametrServiceType);

                        }
                        var objectForReturn = (TService)Activator.CreateInstance(implamentationType);
                        methodInfo.Invoke(objectForReturn, argumentsAsParameterForMethod);

                        return objectForReturn;

                    }
                    else
                        return (TService)Activator.CreateInstance(implamentationType);
                }
            }
        }
        else
            throw new ArgumentNullException($"Current {typeof(TService)} was not registered ");
        throw new ArgumentNullException($"Current {typeof(TService)} was not registered ");

    }
    private object CreatInstance(Type serviceType, Type implamentation = null)
    {
        foreach (var item in _container[serviceType])
        {
            Console.WriteLine(item.Name);
            if (item == implamentation)
            {
                return Activator.CreateInstance(item);
            }
            else
                return Activator.CreateInstance(item);
        }
        throw new ArgumentNullException("Not registered");
    }
}

public enum Methodname
{
    Inject = 0,
}




