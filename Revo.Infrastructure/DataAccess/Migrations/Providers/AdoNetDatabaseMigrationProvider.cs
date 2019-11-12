﻿using System.Data;
using System.Threading.Tasks;

namespace Revo.Infrastructure.DataAccess.Migrations.Providers
{
    public class AdoNetDatabaseMigrationProvider : AdoNetStubDatabaseMigrationProvider
    {
        public AdoNetDatabaseMigrationProvider(IDbConnection dbConnection, IDatabaseMigrationScripter scripter)
        {
            DbConnection = dbConnection;
            Scripter = scripter;
        }

        public IDbConnection DbConnection { get; }
        protected override IDatabaseMigrationScripter Scripter { get; }

        public override void Dispose()
        {
            try
            {
                base.Dispose();
            }
            finally
            {
                DbConnection.Dispose();
            }
        }

        protected override Task<IDbConnection> GetDbConnectionAsync()
        {
            if (DbConnection.State == ConnectionState.Closed)
            {
                DbConnection.Open();
            }

            return Task.FromResult(DbConnection);
        }
    }
}