using Api.Models;
using Api.Services;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Api.Repository
{
    public class OrderRequestRepository : IOrderRequestRepository
    {
        private readonly IDbConnection _db;
        public OrderRequestRepository(IOptions<ConnectionStringList> connectionStrings)
        {
            _db = new SqlConnection(connectionStrings.Value.DbConnectionString);
        }
        public void Create(OrderRequest request)
        {
            try
            {
                if (request != null)
                {
                    _db.Query<OrderRequest>(@"INSERT INTO ORDERREQUEST(id, productname, quantity, status) VALUES(@id, @productname, @quantity, @status);",request);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}