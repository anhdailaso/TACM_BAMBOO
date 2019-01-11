using System;
using System.Linq;
using Microsoft.Practices.Unity;

namespace Biz.TACM
{
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            AutoRegisterInterfaces(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        #endregion

        private static void RegisterTypes(IUnityContainer container)
        {
            // Put your custom registration here
        }

        private static void AutoRegisterInterfaces(IUnityContainer container)
        {
            var allowedRootNamespaces = new[] { "Biz.TACM", "Biz.Lib.TACM" };

            var interfacesWithOneImplementation = AllClasses.FromLoadedAssemblies()
                .Where(TypeNamespacesStartWith(allowedRootNamespaces))
                .SelectMany(t =>
                    t.GetInterfaces()
                        .Select(i => new
                        {
                            ClassType = t,
                            Interface = i
                        }))
                .Where(x => !container.IsRegistered(x.Interface))
                .GroupBy(x => x.Interface)
                .Where(x => x.Count() == 1)
                .SelectMany(x => x);

            foreach (var registerTypePair in interfacesWithOneImplementation)
            {
                container.RegisterType(registerTypePair.Interface, registerTypePair.ClassType);
            }
        }

        private static Func<Type, bool> TypeNamespacesStartWith(string[] allowedRootNamespaces)
        {
            return t => allowedRootNamespaces.Any(allowedNamespace =>
                t.Namespace != null
                && t.Namespace.StartsWith(allowedNamespace, StringComparison.OrdinalIgnoreCase));
        }

    }
}