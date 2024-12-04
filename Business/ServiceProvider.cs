using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public class ServiceProvider
    {
        private static ServiceProvider? current;
        public static ServiceProvider Current
        {
            get
            {
                if (current == null)
                {
                    throw new Exception("ServiceProvider has not been initialized.");
                }
                else
                {
                    return current;
                }
            }
            set
            {
                current = value;
            }
        }

        public ServiceProvider()
        {
            current = this;
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
            if (instance == null)
            {
                return;
            }

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
