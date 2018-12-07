using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using LodCore.QueryService.Queries.NotificationQuery;
using LodCore.QueryService.Views.NotificationView;
using LodCore.QueryService.DTOs;
using Dapper;
using MySql.Data.MySqlClient;
using System.Linq;

namespace LodCore.QueryService.Handlers
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly string _connectionString;
        public int PaginationSettings { get; }

        public NotificationHandler(string connectionString, int paginationSettings)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            PaginationSettings = paginationSettings;
        }

        public PageNotificationView Handle(PageNotificationForDeveloperQuery query)
        {
            List<NotificationView> result;

            using (var connection = new MySqlConnection(_connectionString))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                result = connection.Query<NotificationView>(query.Sql, new
                {
                    developerID = query.DeveloperID,
                    offset = query.Offset,
                    pageSize = query.PageSize
                }).AsList();
            }
            return new PageNotificationView(result);
        }
    }
}
