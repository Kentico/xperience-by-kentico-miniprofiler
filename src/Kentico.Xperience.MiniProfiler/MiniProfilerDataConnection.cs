using System.Data;
using System.Data.Common;

using CMS.DataEngine;
using CMS.DataProviderSQL;

using Microsoft.Data.SqlClient;

using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace Kentico.Xperience.MiniProfiler;

/// <summary>
/// Custom data connection implementation wrapping the system connection.
/// </summary>
internal class MiniprofilerDataConnection : DataConnection
{
    /// <summary>
    /// Creates new instance of <see cref="MiniprofilerDataConnection"/>
    /// </summary>
    /// <param name="connectionString"></param>
    internal MiniprofilerDataConnection(string connectionString)
        : base(connectionString)
    {
    }


    /// <summary>
    /// Wraps the bulk insert operations connection string
    /// </summary>
    public override void BulkInsert(DataTable sourceData, string targetTable, BulkInsertSettings? insertSettings = default)
    {
        if (NativeConnection is ProfiledDbConnection profileDbConnection)
        {
            var originalConnection = NativeConnection;
            try
            {
                NativeDBConnection = profileDbConnection.WrappedConnection;
                using (StackExchange.Profiling.MiniProfiler.Current.CustomTiming("BulkInsert", $"BulkInsert - {targetTable}"))
                {
                    base.BulkInsert(sourceData, targetTable, insertSettings);
                }
            }
            finally
            {
                NativeConnection = originalConnection;
            }
        }
        else
        {
            base.BulkInsert(sourceData, targetTable, insertSettings);
        }
    }


    /// <summary>
    /// Begins a new transaction with original connection.
    /// </summary>
    public override void BeginTransaction()
    {
        if (NativeConnection is ProfiledDbConnection profileDbConnection)
        {
            NativeDBConnection = profileDbConnection.WrappedConnection;
        }

        base.BeginTransaction();
    }


    /// <summary>
    /// Begins a new transaction with original connection.
    /// </summary>
    public override void BeginTransaction(IsolationLevel isolation)
    {
        if (NativeConnection is ProfiledDbConnection profileDbConnection)
        {
            NativeDBConnection = profileDbConnection.WrappedConnection;
        }

        base.BeginTransaction(isolation);
    }


    /// <summary>
    /// Wraps the native connections
    /// </summary>
    protected override IDbConnection CreateNativeConnection() =>
        new ProfiledDbConnection((DbConnection)base.CreateNativeConnection(), StackExchange.Profiling.MiniProfiler.Current);


    /// <summary>
    /// Wraps the SQL command.
    /// </summary>
    protected override DbCommand CreateCommand(string cmdText)
    {
        var connection = NativeConnection is ProfiledDbConnection miniConnection ? miniConnection.WrappedConnection : NativeConnection;
        var transaction = Transaction is ProfiledDbTransaction miniTransaction ? miniTransaction.WrappedTransaction : Transaction;
        var command = new SqlCommand(cmdText, (SqlConnection)connection, (SqlTransaction)transaction);

        return new ProfiledDbCommand(command, (DbConnection)connection, StackExchange.Profiling.MiniProfiler.Current);
    }


    /// <summary>
    /// Wraps the <see cref="DbDataAdapter"/>.
    /// </summary>
    protected override DbDataAdapter CreateDataAdapter() =>
        new ProfiledDbDataAdapter(base.CreateDataAdapter(), StackExchange.Profiling.MiniProfiler.Current);
}
