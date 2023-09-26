using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Garbage;

public class Garbage : MonoBehaviour
{
    //    public abstract class ServiceDescription
    //    {
    //        public Type ServiceType;
    //        public LifeTime LifeTime;

    //        public ServiceDescription(Type serviceType, LifeTime lifeTime)
    //        {
    //            ServiceType = serviceType;
    //            LifeTime = lifeTime;
    //        }
    //    }

    //    public enum LifeTime
    //    {
    //        Transient = 0,
    //        Scoped = 1,
    //        Singleton = 2,
    //    }

    //    public class TypeBasedServiceDescriptor : ServiceDescription
    //    {
    //        public Type ImplementationType;

    //        public TypeBasedServiceDescriptor(Type implementationType, Type serviceType, LifeTime lifeTime) : base(serviceType, lifeTime)
    //        {
    //            ImplementationType = implementationType;
    //        }
    //    }

    //    public class FactoryBasedServiceDescriptor : ServiceDescription
    //    {
    //        public Func<IScope, object> Factory;

    //        public FactoryBasedServiceDescriptor(Func<IScope, object> factory, Type serviceType, LifeTime lifeTime) : base(serviceType, lifeTime)
    //        {
    //            Factory = factory;
    //        }
    //    }

    //    public class InstanceBasedServiceDescriptor : ServiceDescription
    //    {
    //        public object Instance;

    //        public InstanceBasedServiceDescriptor(Type serviceType, object instance) : base(serviceType, LifeTime.Singleton)
    //        {
    //            Instance = instance;
    //        }
    //    }
    //    public class Registration
    //    {
    //        public IContainer ConfigureServices()
    //        {
    //            var builder = new ContainerBuilder();
    //            builder.RegisterTransient<IService, RoadFixer>();

    //            return builder.Build();
    //        }
    //    }

    //    public class ContainerBuilder : IContainerBuilder
    //    {

    //        private readonly List<ServiceDescription> _descriptors = new();
    //        public IContainer Build()
    //        {
    //            return new Container(_descriptors);
    //        }

    //        public void Register(ServiceDescription descriptor)
    //        {
    //            _descriptors.Add(descriptor);
    //        }
    //    }

    //    public class Container : IContainer
    //    {
    //        private Dictionary<Type, ServiceDescription> _descriptors = new();
    //        public Container(IEnumerable<ServiceDescription> descriptors)
    //        {
    //            _descriptors = descriptors.ToDictionary(x => x.ServiceType);
    //        }
    //        private class Scope : IScope
    //        {
    //            private readonly Container _container;

    //            public Scope(Container container)
    //            {
    //                _container = container;
    //            }

    //            public object Resolve(Type service) => _container.CreateInstance(service, this);

    //        }

    //        private object CreateInstance(Type service, IScope scope)
    //        {
    //            if (!_descriptors.TryGetValue(service, out var descriptor))
    //            {
    //                if (descriptor is InstanceBasedServiceDescriptor ib)
    //                    return ib.Instance;
    //                if (descriptor is FactoryBasedServiceDescriptor fb)
    //                    return fb.Factory(scope);
    //                var tb = (TypeBasedServiceDescriptor)descriptor;

    //                var ctor = tb.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
    //                var args = ctor.GetParameters();
    //                var argsForCtor = new object[args.Length];
    //                for (int i = 0; i < args.Length; i++)
    //                {
    //                    argsForCtor[i] = CreateInstance(args[i].ParameterType, scope);
    //                }
    //                return ctor.Invoke(argsForCtor);

    //            }
    //            else
    //            {
    //                return CreateInstance(service, scope);
    //            }

    //        }


    //        public IScope CreateScope()
    //        {
    //            return new Scope(this);
    //        }
    //    }

    //}
    //public interface IContainerBuilder
    //{
    //    public void Register(ServiceDescription descriptor);

    //    public IContainer Build();
    //}

    //public interface IContainer
    //{
    //    public IScope CreateScope();
    //}

    //public interface IScope
    //{
    //    public object Resolve(Type service);
    //}

    //public static class ContainerBuilderExtensions
    //{
    //    private static IContainerBuilder RegisterType(this IContainerBuilder builder, Type service, Type implamentation, LifeTime lifeTime)
    //    {
    //        builder.Register(new TypeBasedServiceDescriptor(implamentation, service, lifeTime));
    //        return builder;
    //    }
    //    public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type serviceInterface, Type serviceImplementation)
    //        => builder.RegisterType(serviceInterface, serviceImplementation, LifeTime.Singleton);
    //    public static IContainerBuilder RegisterTransient(this IContainerBuilder builder, Type serviceInterface, Type serviceImplementation)
    //        => builder.RegisterType(serviceInterface, serviceImplementation, LifeTime.Transient);
    //    public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type serviceInterface, Type serviceImplementation)
    //        => builder.RegisterType(serviceInterface, serviceImplementation, LifeTime.Scoped);

    //    //Generic
    //    //                                   TService -Абстракция(Животное)   TImplementation- Реализация(Кошка)       где      Кошка это Животное 
    //    public static IContainerBuilder RegisterTransient<TService, TImplementation>(this IContainerBuilder builder) where TImplementation : TService
    //        => builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Transient);

    //    public static IContainerBuilder RegisterScoped<TService, TImplamentation>(this IContainerBuilder builder) where TImplamentation : TService
    //        => builder.RegisterType(typeof(TService), typeof(TImplamentation), LifeTime.Scoped);
    //    public static IContainerBuilder RegisterSingleton<TService, TImplementation>(this IContainerBuilder builder) where TImplementation : TService
    //       => builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Singleton);
}
