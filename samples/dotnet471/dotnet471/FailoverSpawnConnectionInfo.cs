using System.Data;

namespace dotnet472
{
    public class FailoverSpawnConnectionInfo
    {
        public string ConnectionDateTime { get; set; }
        public string DatabaseName { get; set; }
        public ConnectionState ConnectionState { get; set; }
    }
}