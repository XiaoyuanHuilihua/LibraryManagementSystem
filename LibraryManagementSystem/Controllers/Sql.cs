using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    public static class Sql
    {
        private const int MinConnections = 5;
        private const int MaxConnections = 10;

        private static readonly List<OracleConnection> freeConnections;
        private static readonly LinkedList<OracleCommand> waitingCommands;
        private static int connectionsCount;

        /// <summary>
        /// 构造函数
        /// </summary>
        static Sql()
        {
            freeConnections = new List<OracleConnection>(MinConnections);
            waitingCommands = new LinkedList<OracleCommand>();

            for (var i = 0; i < MinConnections; i++)
            {
                var connection = CreateConnection();
                freeConnections.Add(connection);
            }
        }

        /// <summary>
        /// 执行SQL语句(INSERT,UPDATE)
        /// 例子:Sql.Execute("UPDATE student SET a = @0 WHERE b = @1", [a], [b]);
        /// </summary>
        /// <param name="sql">SQL语句(INSERT,UPDATE)</param>
        /// <param name="args">参数</param>
        /// <exception cref="OracleException" />
        public static void Execute(string sql, params object[] args)
        {
            var command = CreateCommand(sql, args);
            using var transaction = command.Connection.BeginTransaction();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (OracleException)
            {
                transaction.Rollback();
                throw;
            }
            transaction.Commit();
            ReleaseCommand(command);
        }

        /// <summary>
        /// 执行SQL语句(SELECT)
        /// 例子:var sql = Sql.Read("SELECT * FROM [a] WHERE [b]);
        /// </summary>
        /// <param name="sql">SQL语句(SELECT)</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        /// <exception cref="OracleException" />
        public static DataRowCollection Read(string sql, params object[] args)
        {
            var command = CreateCommand(sql, args);
            using var reader = new OracleDataAdapter(command);
            using var table = new DataTable();
            _ = reader.Fill(table);
            ReleaseCommand(command);
            return table.Rows;
        }

        /// <summary>
        /// 链接数据库
        /// </summary>
        /// <returns>OracleConnection</returns>
        private static OracleConnection CreateConnection()
        {
            const string ConnectionString = "User ID=root21; Password=123456; Data Source=" + "(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = pdborcl)))";
            var connection = new OracleConnection(ConnectionString);
            try
            {
                connection.Open();
            }
            catch (OracleException)
            {
                connection.Close();
                connection.Dispose();
                throw;
            }
            connectionsCount++;
            return connection;
        }

        /// <summary>
        /// 生成命令
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>OracleCommand</returns>
        private static OracleCommand CreateCommand(string sql, params object[] args)
        {
            var command = new OracleCommand
            {
                CommandText = sql,
            };
            for (var i = 0; i < args.Length; i++)
            {
                var name = $"@{i}";
                var value = args[i];
                var parameter = new OracleParameter(name, value);
                var parameters = command.Parameters;
                parameters.Add(parameter);
            }
            lock (freeConnections)
            {
                if (freeConnections.Count > 0)
                {
                    var index = freeConnections.Count - 1;
                    command.Connection = freeConnections[index];
                    freeConnections.RemoveAt(index);
                }
                else
                {
                    if (connectionsCount < MaxConnections)
                    {
                        command.Connection = CreateConnection();
                    }
                    else
                    {
                        waitingCommands.AddLast(command);
                    }
                }
            }
            while (command.Connection == null) ;
            return command;
        }

        /// <summary>
        /// 释放命令
        /// </summary>
        /// <param name="command">命令</param>
        private static void ReleaseCommand(OracleCommand command)
        {
            var connection = command.Connection;
            command.Dispose();
            lock (freeConnections)
            {
                if (freeConnections.Count < MinConnections)
                {
                    if (waitingCommands.Count > 0)
                    {
                        var waitingCommand = waitingCommands.First.Value;
                        waitingCommands.RemoveFirst();
                        waitingCommand.Connection = connection;
                    }
                    else
                    {
                        freeConnections.Add(connection);
                    }
                }
                else
                {
                    connection.Dispose();
                    connectionsCount--;
                }
            }
        }
    }
}
