using CMS.DataEngine;

namespace Kentico.Xperience.MiniProfiler;

/// <summary>
/// Wraps the system data connection.
/// </summary>
internal class MiniprofilerDataProvider : AbstractDataProvider
{
    public override IDataConnection GetNewConnection(string connectionString) =>
        new MiniprofilerDataConnection(connectionString);
}
