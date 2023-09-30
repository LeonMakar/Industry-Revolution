using System;
using System.Collections.Generic;
using UnityEngine;

public class Injector
{
    private IContainer _container;

    public Injector(IContainer container)
    {

        _container = container;
    }

    private Dictionary<Type, object> _singletonServices = new Dictionary<Type, object>();
    private Dictionary<Type, object> _temporaryServices = new Dictionary<Type, object>();


    public void Injecting<TServiceWhoNeedInjectionType>(object serviceWhoNeedInjection)
    {

        var method = typeof(TServiceWhoNeedInjectionType).GetMethod(nameof(Methodname.Inject));
        var parameters = method.GetParameters();
        var arguments = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            var parameterIType = parameters[i].ParameterType;
            if (_singletonServices.ContainsKey(parameterIType))
                arguments[i] = _singletonServices[parameterIType];
            else if (_temporaryServices.ContainsKey(parameterIType))
                arguments[i] = _temporaryServices[parameterIType];
            else
                throw new InvalidOperationException($"{parameterIType} dosnt builded in some container");
        }

        method.Invoke(serviceWhoNeedInjection, arguments);
    }
    public void BuildSingletoneService<TService, TImplamentation>(params Type[] typesToResolve) where TImplamentation : TService
    {
        if (_singletonServices.ContainsKey(typeof(TImplamentation)))
            Debug.LogErrorFormat(typeof(TImplamentation) + " almost exist in Dictionary");
        else
            _singletonServices.Add(typeof(TImplamentation), _container.Resolve<TService, TImplamentation>(typesToResolve));
    }

    public void BuildTemporaryService<TService, TImplamentation>(params Type[] typesToResolve) where TImplamentation : TService
    {
        if (_temporaryServices.ContainsKey(typeof(TImplamentation)))
            _temporaryServices[(typeof(TImplamentation))] = _container.Resolve<TService, TImplamentation>(typesToResolve);
        else
            _temporaryServices.Add(typeof(TImplamentation), _container.Resolve<TService, TImplamentation>(typesToResolve));
    }

    public void AddExistingSingletoneService<TImplamentation>(object currentObject)
    {
        if (_singletonServices.ContainsKey(typeof(TImplamentation)))
            Debug.LogErrorFormat(typeof(TImplamentation) + " almost exist in Dictionary");
        else
            _singletonServices.Add(typeof(TImplamentation), currentObject);

    }
    public void AddExistingTemporaryService<TImplamentation>(object currentObject)
    {
        if (_temporaryServices.ContainsKey(typeof(TImplamentation)))
            _temporaryServices[(typeof(TImplamentation))] = currentObject;

        else
            _temporaryServices.Add(typeof(TImplamentation), currentObject);

    }

    public TService InjectSingletoneService<TImplamentation, TService>() where TImplamentation : TService
    {
        if (_singletonServices.ContainsKey(typeof(TImplamentation)))
            return (TService)_singletonServices[typeof(TImplamentation)];
        else
            throw new InvalidOperationException($"{typeof(TImplamentation)} dosnt register in singletone dictionary");
    }

    public TService InjectTemporaryService<TImplamentation, TService>() where TImplamentation : TService
    {
        if (_temporaryServices.ContainsKey(typeof(TImplamentation)))
            return (TService)_temporaryServices[typeof(TImplamentation)];
        else
            throw new InvalidOperationException("Current implamentation dosnt register in temporary dictionary");
    }


}
