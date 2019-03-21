﻿using Dapper;
using Newtonsoft.Json;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Simplic.DataPort.DB.Processing.Data
{
    public class DBProcessingSqlRepository : IDBProcessingRepository
    {
        private readonly ISqlService sqlService;
        private const string LogTableName = "DataPortDBLog";

        public DBProcessingSqlRepository(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        private string GetNullableText(bool @null)
        {
            return @null ? string.Empty : "NOT NULL";
        }

        private string GetPrimaryKeyText(bool primaryKey)
        {
            return primaryKey ? "PRIMARY KEY" : string.Empty;
        }

        public bool ColumnExists(string tableName, string columnName, string connectionName = "default")
        {
            const string sql = @"SELECT sc.column_id FROM SYS.SYSCOLUMN sc WHERE 
                sc.table_id = (SELECT table_id FROM SYS.SYSTABLE WHERE table_name = :tableName) 
                and sc.column_name = :columnName";

            return sqlService.OpenConnection(
                (connection) => connection.Query(sql, new { tableName, columnName }).Any(), connectionName);
        }

        public bool CreateTable(string tableName, string connectionName = "default")
        {
            const string sql = @"SELECT sc.column_id FROM SYS.SYSCOLUMN sc WHERE 
                sc.table_id = (SELECT table_id FROM SYS.SYSTABLE WHERE table_name = :tableName) 
                and sc.column_name = :columnName";

            return sqlService.OpenConnection(
                (connection) => connection.Execute(sql, new { tableName }) > 0, connectionName);
        }

        public bool CreateTable(TableSchemaModel tableSchema, string connectionName = "default")
        {
            var columnsBuilder = new string[tableSchema.Columns.Count];
            for (int i = 0; i < tableSchema.Columns.Count; i++)
            {
                var column = tableSchema.Columns[i];
                columnsBuilder[i] = $"[{column.Name}] {column.Type} {GetNullableText(column.Null)} {GetPrimaryKeyText(column.PrimaryKey)}";
            }

            var sql = $"CREATE TABLE IF NOT EXISTS [{tableSchema.TableName}] ({string.Join(",\n", columnsBuilder)});";

            return sqlService.OpenConnection(
                (connection) =>
                {
                    try
                    {
                        var result = connection.Execute(sql);
                        return result > 0;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }, connectionName);
        }

        public void InsertOrUpdate(string tableName, DataRow row, string connectionName = "default")
        {
            var columnsBuilder = new string[row.Table.Columns.Count];
            var paramColumns = new string[row.Table.Columns.Count];

            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                var column = row.Table.Columns[i];
                columnsBuilder[i] = $"[{column.ColumnName}]";
                paramColumns[i] = $":{column.ColumnName}";
            }

            var sql = $"INSERT INTO {tableName} ({string.Join(",", columnsBuilder)}) ON EXISTING UPDATE VALUES ({string.Join(",", paramColumns)})";
            sqlService.OpenConnection((connection) =>
            {
                try
                {
                    var parameters = new DynamicParameters();
                    for (int i = 0; i < columnsBuilder.Length; i++)
                    {
                        var item = columnsBuilder[i];
                        var val = row.ItemArray.GetValue(i);
                        parameters.Add($":{item}", val);
                    }

                    connection.Execute(sql, parameters);
                }
                catch (Exception ex)
                {
                    LogError(tableName, sql, row, ex.Message, connectionName);
                }
            });
        }

        public bool TableExists(string tableName, string connectionName = "default")
        {
            const string sql = "SELECT COUNT(*) FROM SYS.SYSTABLE WHERE SYS.SYSTABLE.table_name = :tableName";

            return sqlService.OpenConnection((connection) =>
            {
                var result = connection.Query<int>(sql, new { tableName }).FirstOrDefault();
                return result == 1;
            }, connectionName);
        }

        private void LogError(string tableName, string sqlUsed, DataRow row, string exceptionDetails, string connectionName)
        {
            var sql = $"INSERT INTO {LogTableName} (TableName, SqlQuery, Data, ExceptionDetails) VALUES (:TableName, :SqlQuery, :Data, :ExceptionDetails)";

            sqlService.OpenConnection((connection) =>
            {
                connection.Execute(sql, new {
                    TableName = tableName,
                    SqlQuery = sqlUsed,
                    Data = JsonConvert.SerializeObject(row),
                    ExceptionDetails = exceptionDetails });
            }, connectionName);
        }

        public IEnumerable<ErrorLogModel> GetAllErrorLog(string connectionName = "default")
        {
            var sql = $"SELECT * FROM {LogTableName} WHERE Handled = 0";
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<ErrorLogModel>(sql);
            }, connectionName);
        }

        public bool Retry(ErrorLogModel errorLogModel, string connectionName = "default")
        {
            throw new NotImplementedException();
        }

        public ErrorLogModel GetErrorLog(long id, string connectionName = "default")
        {
            var sql = $"select * from {LogTableName} where Id = :id";
            return sqlService.OpenConnection((connection) => {
                return connection.Query<ErrorLogModel>(sql, new { id }).FirstOrDefault();
            }, connectionName);
        }
    }
}
