using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;
using System;

namespace dotnet472
{

    public class FailoverSpawnConnector
    {
        private readonly string _connectionString;

        public FailoverSpawnConnector()
        {
             _connectionString = $"Data Source=192.168.56.11:1521; User Id = system; Password = Password1;";
        }

        public FailoverSpawnConnectionInfo Connect()
        {
            FailoverSpawnConnectionInfo connectionInfo;
            using(OracleConnection connection = new OracleConnection(_connectionString))
            {
                // create variables
                var dbName = new object();
                var sysdate = new object();

                // Create new stopwatch.
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                connection.Open();
                
                Console.WriteLine("Milliseconds elapsed open connection\t\t: {0}", stopwatch.ElapsedMilliseconds);

                // first run
                dbName = ExecuteQuery(connection,
                    @"select lower(sys_context('userenv','db_name')) as db_name from dual");
                sysdate = ExecuteQuery(connection, @"select sysdate from dual");
                Console.WriteLine("Milliseconds elapsed first run staments\t\t: {0}", stopwatch.ElapsedMilliseconds);

                // second run
                dbName = ExecuteQuery(connection,
                    @"select lower(sys_context('userenv','db_name')) as db_name from dual");
                sysdate = ExecuteQuery(connection, @"select sysdate from dual");
                Console.WriteLine("Milliseconds elapsed second run staments\t: {0}", stopwatch.ElapsedMilliseconds);

                connection.Dispose();
                connection.Close();
                
                OracleConnection.ClearPool(connection);
                OracleConnection.ClearAllPools();
                Console.WriteLine("Milliseconds elapsed close connection\t\t: {0}", stopwatch.ElapsedMilliseconds);
                stopwatch.Stop();

                Console.WriteLine("Connection state\t\t\t\t: " + connection.State);

                connectionInfo = new FailoverSpawnConnectionInfo()
                                 {
                                     ConnectionDateTime = sysdate.ToString(),
                                     DatabaseName = dbName.ToString(),
                                     ConnectionState = connection.State
                                 };
            }
            return connectionInfo;
        }

        private object ExecuteQuery(OracleConnection connection, string query)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;
            var reader = command.ExecuteReader();
            try
            {
                reader.Read();
                return reader.GetValue(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                reader.Close();
                return 1;
            }
        }
    }
}