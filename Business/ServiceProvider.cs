using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public class ServiceProvider
    {
        public static ServiceProvider Current;

        public ServiceProvider()
        {
            Current = this;
            Debug.WriteLine("Initialized service provider");
        }

        private readonly Dictionary<Type, object> _services = [];

        // Register a service
        public void Register<TInterface, TImplementation>()
            where TImplementation : TInterface, new()
        {
            _services[typeof(TInterface)] = new TImplementation();
        }

        // Register an instance
        public void RegisterInstance<TInterface>(TInterface instance)
        {
            _services[typeof(TInterface)] = instance;
        }

        // Resolve a service
        public TInterface Resolve<TInterface>()
        {
            if (_services.TryGetValue(typeof(TInterface), out var service))
            {
                return (TInterface)service;
            }

            throw new InvalidOperationException($"Service of type {typeof(TInterface).Name} is not registered.");
        }
    }

}
