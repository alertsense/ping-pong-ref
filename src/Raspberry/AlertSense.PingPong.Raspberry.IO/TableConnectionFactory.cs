using System;
using System.Collections.Generic;
using System.Configuration;
using Raspberry.IO.GeneralPurpose;
using AlertSense.PingPong.Raspberry.Models;

namespace AlertSense.PingPong.Raspberry.IO
{
    public static class TableConnectionFactory
    {
        static readonly Dictionary<Type, Type> Connections = new Dictionary<Type, Type>();
        static readonly IGpioConnectionDriver Driver = GpioConnectionSettings.DefaultDriver;

        public static ITableConnection GetTableConnection(Table table)
        {
            Type connectionType = null;
            if (!Connections.TryGetValue(typeof (ITableConnection), out connectionType))
                throw new Exception("ITableConnection not registered with factory");

            var tableConnection = (ITableConnection) Activator.CreateInstance(connectionType);
            tableConnection.Table = table;

            return tableConnection;
        }

        public static void AddConnection<TInterface, TConcrete>()
        {
            Connections.Add(typeof(TInterface), typeof(TConcrete));
        }
    }
}
