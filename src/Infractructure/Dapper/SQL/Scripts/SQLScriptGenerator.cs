using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infractructure.Dapper.SQL.Scripts
{
    public class SQLScriptGenerator
    {
        internal static string GenereteGetByIdQuery(Guid id, string tableName)
        {
            return $@"SELECT *
	                FROM {tableName} t
                    WHERE t.Id = '{id}'";
        }

        internal static string GenerateUpdateQuery<T>(string tableName)
        {
            var updateQuery = new StringBuilder($"UPDATE {tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties<T>());

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }

        internal static string GenerateInsertQuery<T>(string tableName)
        {
            var insertQuery = new StringBuilder($"INSERT INTO {tableName} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties<T>());
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        internal static string GenerateTotalCountQuery(string tableName)
        {
            return $"SELECT COUNT(*)  FROM {tableName}";
        }

        internal static string GeneratePagedScript(int offset, int pagesize, string tableName)
        {
            return $@"SELECT *
	                FROM {tableName} t
                    Order by t.Id
	                OFFSET {offset} ROWS
	                FETCH NEXT {pagesize} ROWS ONLY";
        }

        private static IEnumerable<PropertyInfo> GetProperties<T>() => typeof(T).GetProperties();


        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            List<Type> collections = new List<Type>() { typeof(IEnumerable<>), typeof(IEnumerable) };

            return (from propertyInfo in listOfProperties
                    where !(propertyInfo.PropertyType != typeof(string) && propertyInfo.PropertyType.GetInterfaces().Any(i => collections.Any(c => i == c)))
                    select propertyInfo.Name).ToList();
        }


    }
}
