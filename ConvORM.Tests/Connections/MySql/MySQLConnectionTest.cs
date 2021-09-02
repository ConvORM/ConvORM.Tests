using ConvORM.Driver.MySQL.Connections;
using ConvORM.Driver.MySQL.Connections.Parameters;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace ConvORM.Tests.Connections.MySql
{
    public class MySQLConnectionTest
    {
        private string GetConnectionStringFromEnvironmentVariable()
        {
            return Environment.GetEnvironmentVariable("MySQLConnectionString");
        }

        private MySQLConnectionParameters CreateParameters()
        {
            var connectionString = GetConnectionStringFromEnvironmentVariable().Split(';');
            var dictionary = DecodeStringsToDictionary(connectionString);

            dictionary.TryGetValue("Server", out string server);
            dictionary.TryGetValue("Port", out string port);
            dictionary.TryGetValue("Database", out string database);
            dictionary.TryGetValue("Uid", out string uid);
            dictionary.TryGetValue("Pwd", out string pwd);

            var mySQLConnectionParameters = new MySQLConnectionParameters
            {
                Server = server,
                Port = port,
                Database = database,
                Uid = uid,
                Pwd = pwd
            };

            return mySQLConnectionParameters;
        }

        private Dictionary<string,string> DecodeStringsToDictionary(string [] strings)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var s in strings)
            {
                if (String.IsNullOrEmpty(s))
                    continue;

                int position = s.IndexOf('=');
                var key = s.Substring(0, position);
                var value = s.Substring(position + 1, s.Length - (position + 1));
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        [Fact]
        public void MySQL_Connect_With_Connection_Strings()
        {
            var connection = new MySQLConnection(GetConnectionStringFromEnvironmentVariable());

            connection.Open().Should().BeTrue();
        }

        [Fact]
        public void MySQL_Connect_With_Parameters()
        {
            var connection = new MySQLConnection(CreateParameters());

            connection.Open().Should().BeTrue();
        }
    }
}
