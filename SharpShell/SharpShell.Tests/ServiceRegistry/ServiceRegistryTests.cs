using NUnit.Framework.Legacy;
using System;
using NUnit.Framework;
using SharpShell.Registry;

namespace SharpShell.Tests.ServiceRegistry
{
    public class ServiceRegistryTests
    {
        [Test]
        public void Default_IRegistry_Is_A_WindowsRegistry()
        {
            //  By default, the service registry should provide a WindowsRegsitry for IRegistry.
            var registry = SharpShell.ServiceRegistry.ServiceRegistry.GetService<IRegistry>();
            ClassicAssert.IsInstanceOf(typeof(WindowsRegistry), registry);
        }

        [Test]
        public void Registered_Service_Provider_Is_Used_To_Get_Service()
        {
            //  First, register a service provider.
            bool serviceProviderCalled = false;
            var windowsRegistry = new WindowsRegistry();
            Func<IRegistry> serviceProvider = () =>
            {
                serviceProviderCalled = true;
                return windowsRegistry;
            };
            SharpShell.ServiceRegistry.ServiceRegistry.RegisterService(serviceProvider);

            //  Get the service.
            var service = SharpShell.ServiceRegistry.ServiceRegistry.GetService<IRegistry>();

            //  Assert we called our custom function, and created the service.
            ClassicAssert.IsTrue(serviceProviderCalled);
            ClassicAssert.AreSame(windowsRegistry, service);
        }

        [Test]
        public void Getting_An_Unregistered_Service_Throws_A_Meaningful_Exception()
        {
            //  Try and get a service which we haven't provided for.
            try
            {
                SharpShell.ServiceRegistry.ServiceRegistry.GetService<IDisposable>();
                ClassicAssert.Fail("GetService should throw an exception.");
            }
            catch (Exception exception)
            {
                ClassicAssert.That(exception.Message, Contains.Substring("IDisposable"));
            }
        }
    }
}
