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

    //private Dictionary<Type, object> _temporaryServices = new Dictionary<Type, object>();

    //private Dictionary<Type, Dictionary<Type, object>> _singletonServices = new Dictionary<Type, Dictionary<Type, object>>();

    private Dictionary<Type, Dictionary<Type, object>> _temporaryServices = new Dictionary<Type, Dictionary<Type, object>>();

    public bool CheckAvailabilitySingletoneServiceInInjector(Type type)
    {
        if (_singletonServices.ContainsKey(type))
            return true;
        else return false;
    }

    public void InjectingTemporaryServices<TImplamentationTypeWhoNeedInjection>(object serviceWhoNeedInjection, params Type[] typesOfImplamentation)
    {

        var method = typeof(TImplamentationTypeWhoNeedInjection).GetMethod(nameof(Methodname.InjectTemporary));
        var parameters = method.GetParameters();
        var arguments = new object[parameters.Length];
        if (typesOfImplamentation.Length == parameters.Length)
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterIType = parameters[i].ParameterType;

                if (_temporaryServices.ContainsKey(parameterIType))
                {
                    if (_temporaryServices[parameterIType].ContainsKey(typesOfImplamentation[i]))
                        arguments[i] = _temporaryServices[parameterIType][typesOfImplamentation[i]];
                }
                else
                    throw new InvalidOperationException($"{parameterIType} or {typesOfImplamentation[i]} dosnt builded in some container");
            }
        else
            throw new InvalidOperationException($"Length of parametrs for {typeof(TImplamentationTypeWhoNeedInjection)} in Inject method dosnt match with Injecting params lengh");


        method.Invoke(serviceWhoNeedInjection, arguments);
    }
    public void InjectingSingletoneServices<TImplamentationWhoNeedInjection>(object someObject)
    {

        var method = typeof(TImplamentationWhoNeedInjection).GetMethod(nameof(Methodname.InjectSingletone));
        var parameters = method.GetParameters();
        var arguments = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            var parameterIType = parameters[i].ParameterType;
            if (_singletonServices.ContainsKey(parameterIType))
                arguments[i] = _singletonServices[parameterIType];
            else
                throw new InvalidOperationException($"{parameterIType} dosnt builded in singletone container");
        }

        method.Invoke(someObject, arguments);
    }


    public void BuildSingletoneService<TService, TImplamentation>(params Type[] typesToResolve) where TImplamentation : TService
    {

        if (_singletonServices.ContainsKey(typeof(TService)))
            throw new InvalidOperationException($"{typeof(TImplamentation)} almost exist in Dictionary");
        else
            _singletonServices[typeof(TService)] = _container.Resolve<TService, TImplamentation>(typesToResolve);
    }
    public object Resolve(Type serviceType, Type implamentationType, params Type[] typesToInject)
    {
        return _container.Resolve(serviceType, implamentationType, typesToInject);
    }

    public void BuildTemporaryService<TService, TImplamentation>(params Type[] typesToResolve) where TImplamentation : TService
    {
        if (_temporaryServices.ContainsKey(typeof(TService)))
        {
            if (_temporaryServices[typeof(TService)].ContainsKey(typeof(TImplamentation)))
                _temporaryServices[typeof(TService)][typeof(TImplamentation)] = _container.Resolve<TService, TImplamentation>(typesToResolve);
            else
                InitTheInnerDictionaryField<TService, TImplamentation>(ref _temporaryServices, typesToResolve);
        }
        else
        {
            InitTheOuterDictionaryField<TService, TImplamentation>(ref _temporaryServices, typesToResolve);
        }
    }

    public void AddExistingSingletoneService<TService, TImplamentation>(object currentObject) where TImplamentation : TService
    {
        if (_singletonServices.ContainsKey(typeof(TService)))
            throw new InvalidOperationException($"{typeof(TImplamentation)} almost exist in Dictionary");
        else
            _singletonServices[typeof(TService)] = currentObject;
    }
    public void AddExistingTemporaryService<TService, TImplamentation>(object currentObject) where TImplamentation : TService
    {
        if (_temporaryServices.ContainsKey(typeof(TService)))
        {
            if (_temporaryServices[typeof(TService)].ContainsKey(typeof(TImplamentation)))
                _temporaryServices[typeof(TService)][typeof(TImplamentation)] = currentObject;
            else
                InitTheInnerDictionaryField<TService, TImplamentation>(ref _temporaryServices, currentObject);
        }
        else
        {
            InitTheOuterDictionaryField<TService, TImplamentation>(ref _temporaryServices, currentObject);
        }
    }

    public TService GetSingletoneService<TService, TImplamentation>() where TImplamentation : TService
    {
        if (_singletonServices.ContainsKey(typeof(TService)))
            return (TService)_singletonServices[typeof(TService)];
        else
            throw new InvalidOperationException($"{typeof(TService)} dosnt register in singletone dictionary for {typeof(TImplamentation)}");
    }
    public object GetSingletoneService(Type service)
    {
        if (_singletonServices.ContainsKey(service))
            return _singletonServices[service];
        else
            throw new InvalidOperationException($"{service} dosnt register in singletone dictionary for {service}");
    }

    public TService GetTemporaryService<TService, TImplamentation>() where TImplamentation : TService
    {
        if (_temporaryServices.ContainsKey(typeof(TService)))
        {
            if (_temporaryServices[typeof(TService)].ContainsKey(typeof(TImplamentation)))
                return (TService)_temporaryServices[typeof(TService)][typeof(TImplamentation)];
            else
                throw new InvalidOperationException($"{typeof(TImplamentation)} dosnt register in singletone dictionary");
        }
        else
            throw new InvalidOperationException($"{typeof(TService)} dosnt register in singletone dictionary");
    }

    private void InitTheOuterDictionaryField<TService, TImplamentation>(ref Dictionary<Type, Dictionary<Type, object>> dictionary, params Type[] typesToResolve)
    {
        dictionary.Add(typeof(TService), new Dictionary<Type, object>
        {
            [typeof(TImplamentation)] = _container.Resolve<TService, TImplamentation>(typesToResolve)
        });
    }
    private void InitTheInnerDictionaryField<TService, TImplamentation>(ref Dictionary<Type, Dictionary<Type, object>> dictionary, params Type[] typesToResolve)
    {
        dictionary[(typeof(TService))].Add(typeof(TImplamentation), _container.Resolve<TService, TImplamentation>(typesToResolve));

    }
    private void InitTheOuterDictionaryField<TService, TImplamentation>(ref Dictionary<Type, Dictionary<Type, object>> dictionary, object objectToAd)
    {
        dictionary.Add(typeof(TService), new Dictionary<Type, object>
        {
            [typeof(TImplamentation)] = objectToAd
        });
    }
    private void InitTheInnerDictionaryField<TService, TImplamentation>(ref Dictionary<Type, Dictionary<Type, object>> dictionary, object objectToAdd)
    {
        dictionary[(typeof(TService))].Add(typeof(TImplamentation), objectToAdd);
    }
}

public static class InjectorExtansions
{
    public static IInjectable Injecting(this IInjectable currentService, Injector injector)
    {
        List<IService> services = new List<IService>();
        foreach (var service in currentService.ServiceAndImplamentation)
        {
            if (injector.CheckAvailabilitySingletoneServiceInInjector(service.Key))
            {
                services.Add((IService)injector.GetSingletoneService(service.Key));
            }
            else
            {
                services.Add((IService)injector.Resolve(service.Key, service.Value));
            }
        }
        currentService.Inject(services.ToArray());
        return currentService;
    }
}

public interface IInjectable
{
    //Словарь с тем что ему требуется для инжекта которой в послудующем будет использлваться для статическом расширении Inject\

    Dictionary<Type, Type> ServiceAndImplamentation { get; }
    public void Inject(params IService[] services);
}
