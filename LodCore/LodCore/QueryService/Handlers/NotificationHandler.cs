﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using LodCore.QueryService.Queries.NotificationQuery;
using LodCore.QueryService.Views.NotificationView;
using LodCore.QueryService.DTOs;
using Dapper;

namespace LodCore.QueryService.Handlers
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly string _connectionString;

        public NotificationHandler(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public PageNotificationView Handle(PageNotificationForDeveloperQuery query)
        {
            List<NotificationView> result;

            using (var connection = new SqlConnection(_connectionString))
            {
                result = connection.Query<NotificationView>(query.Sql, new {
                    developerID = query.DeveloperID,
                    offset = query.Offset,
                    pageSize = query.PageSize
                }).AsList();
            }

            return new PageNotificationView(result);
        }
    }
}
