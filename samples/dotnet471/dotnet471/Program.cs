using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace dotnet472
{
    // from: https://github.com/kikitux/FailoverSpawnSharp
    internal class Program
    {
        static void Main(string[] args)
        {
            var failoverSpawnConnector = new FailoverSpawnConnector();

            while (true)
            {
                var connectionInfo = failoverSpawnConnector.Connect();

                switch (connectionInfo.ConnectionState)
                {
                    case ConnectionState.Closed:
                        Console.WriteLine($"[{connectionInfo.ConnectionDateTime}]: {connectionInfo.DatabaseName}");
                        break;
                    default:
                        Console.WriteLine(
                            $"Error: ConnectionState should be {ConnectionState.Closed}, but was: {connectionInfo.ConnectionState}.");
                        break;
                }

                Console.WriteLine("");

                // Sleep for 1 second
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}