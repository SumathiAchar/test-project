using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace SSI.ContractManagement.Shared.Helpers.Unity
{
    public static class Factory
    {
        #region Container Methods

        private static IUnityContainer _container;
        private static readonly object ContainerAccess = new object();
        private static bool _isConfigured;

        // ReSharper disable once MemberCanBePrivate.Global
        private static IUnityContainer Container
        {
            get
            {
                Init();
                return _container;
            }
// ReSharper disable once UnusedMember.Local
            set { _container = value; }
        }

        /// <summary>
        /// Initialized the unity container and reads its configuration
        /// </summary>
        private static void Init()
        {
            if (_container == null || _isConfigured == false)
            {
                lock (ContainerAccess)
                {
                    if (_container == null || _isConfigured == false)
                    {
                        try
                        {
                            _container = new UnityContainer();
                            _container.LoadConfiguration();
                            _isConfigured = true;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Unity Load Error!!! - " + ex.Message);
                            throw new Exception(
                                "Unity Configuration Error - Check the <unity> section of your config file.", ex);
                        }
                    }
                }
            }
        }

        #endregion

        #region CreateInstance Methods

        public static T CreateInstance<T>(string connString, bool changestring)
        {
            // this is where Unity takes over and does its magic.
            if(changestring)
                return Container.Resolve<T>(
            new ParameterOverride(
              "connectionString", connString));
            return Container.Resolve<T>();
        }

        public static T CreateInstance<T>()
        {
            // this is where Unity takes over and does its magic.
            var obj = Container.Resolve<T>();
            return obj;
        }

        // ReSharper disable once UnusedMember.Global
        public static object CreateInstance(Type objectType)
        {
            // if the object is not registered, just try to create it.
            var retValue = Container.IsRegistered(objectType) ? Container.Resolve(objectType) : Activator.CreateInstance(objectType);
            return retValue;
        }

        public static T CreateInstance<T>(string name)
        {
            var obj = Container.Resolve<T>(name);
            return obj;
        }

        public static T CreateInstance<T>(string name, string connectionString)
        {
            //var obj = Container.Resolve<T>(name);
            var obj = Container.Resolve<T>(
            name, new ParameterOverride(
              "connectionString", connectionString));
            return obj;
        }

        #endregion

        #region RegisterInstance Methods

        // ReSharper disable once UnusedMember.Global
        public static IUnityContainer RegisterInstance<T>(T instance)
        {
            return Container.RegisterInstance(instance);
        }

        // ReSharper disable once UnusedMember.Global
        public static IUnityContainer RegisterInstance<T>(string name, T instance)
        {
            return Container.RegisterInstance(name, instance);
        }

        // ReSharper disable once UnusedMember.Global
        public static IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            return Container.RegisterInstance(t, name, instance, lifetime);
        }

        #endregion

        #region RegisterType Methods

        // ReSharper disable once UnusedMember.Global
        public static void Register(Type registerType, params InjectionMember[] injectionMembers)
        {
            Container.RegisterType(registerType, injectionMembers);
        }

        // ReSharper disable once UnusedMember.Global
        public static IUnityContainer RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            return Container.RegisterType<TFrom, TTo>(new TransientLifetimeManager());
        }

        // ReSharper disable once UnusedMember.Global
        public static IUnityContainer RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager) where TTo : TFrom
        {
            return Container.RegisterType<TFrom, TTo>(lifetimeManager);
        }

        // ReSharper disable once UnusedMember.Global
        public static IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager,
                                                   params InjectionMember[] injectionMembers)
        {
            return Container.RegisterType(from, to, name, lifetimeManager, injectionMembers);
        }

        #endregion

        #region Resolve Methods

        // ReSharper disable once UnusedMember.Global
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        // ReSharper disable once UnusedMember.Global
        public static T Resolve<T>(string name)
        {
            return Container.Resolve<T>(name);
        }

        // ReSharper disable once UnusedMember.Global
        public static object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return Container.Resolve(t, name, resolverOverrides);
        }

        // ReSharper disable once UnusedMember.Global
        public static IEnumerable<object> ResolveAll(Type objectType)
        {
            return Container.ResolveAll(objectType);
        }

        // ReSharper disable once UnusedMember.Global
        public static IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return Container.ResolveAll(t, resolverOverrides);
        }

        #endregion

        // ReSharper disable once UnusedMember.Global
        public static IUnityContainer Parent
        {
            get { return Container.Parent; }
        }

        // ReSharper disable once UnusedMember.Global
        public static IEnumerable<ContainerRegistration> Registrations
        {
            get { return Container.Registrations; }
        }

        // ReSharper disable once UnusedMember.Global
        public static bool IsRegistered(Type objectType)
        {
            return Container.IsRegistered(objectType);
        }

        // ReSharper disable once UnusedMember.Global
        public static bool IsRegistered<T>()
        {
            return Container.IsRegistered<T>();
        }

        // ReSharper disable once UnusedMember.Global
        public static IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            return Container.AddExtension(extension);
        }

        // ReSharper disable once UnusedMember.Global
        public static object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return Container.BuildUp(t, existing, name, resolverOverrides);
        }

        // ReSharper disable once UnusedMember.Global
        public static object Configure(Type configurationInterface)
        {
            return Container.Configure(configurationInterface);
        }

        // ReSharper disable once UnusedMember.Global
        public static IUnityContainer CreateChildContainer()
        {
            return Container.CreateChildContainer();
        }

        // ReSharper disable once UnusedMember.Global
        public static IUnityContainer RemoveAllExtensions()
        {
            return Container.RemoveAllExtensions();
        }

        // ReSharper disable once UnusedMember.Global
        public static void Teardown(object o)
        {
            Container.Teardown(o);
        }

        // ReSharper disable once UnusedMember.Global
        public static void Dispose()
        {
            Container.Dispose();
        }
    }
}
