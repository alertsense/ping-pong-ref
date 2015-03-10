using System;
using System.Collections.Generic;
using System.Configuration;
using Raspberry.IO.GeneralPurpose;

namespace AlertSense.PingPongRef.Raspberry.IO
{
    public static class ConnectionFactory
    {
        static readonly Dictionary<Type, Type> Connections = new Dictionary<Type, Type>();
        static readonly IGpioConnectionDriver Driver = GpioConnectionSettings.DefaultDriver;

        public static ITableConnection GetTableConnection()
        {
            Type connectionType = null;
            if (!Connections.TryGetValue(typeof (ITableConnection), out connectionType))
                throw new Exception("ITableConnection not registered with factory");

            var tableConnection = (ITableConnection) Activator.CreateInstance(connectionType);
            tableConnection.Settings = TableSettings.Default;
            return tableConnection;
        }

        public static void AddConnection<TInterface, TConcrete>()
        {
            Connections.Add(typeof(TInterface), typeof(TConcrete));
        }
    }
}
