using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using VidaAnimal.Models;
using VidaAnimal.Models.DTO;

namespace VidaAnimal.Services
{
  public interface ISupplierProvider
  {
    IEnumerable<Supplier> Get();
  }

  public class SupplierProvider : ISupplierProvider
  {
    private readonly string connectionString;

    public SupplierProvider(string connectionString)
    {
      this.connectionString = connectionString;
    }

    public IEnumerable<Supplier> Get()
    {
      IEnumerable<Supplier> supplier = null;

      using (var connection = new SqlConnection(connectionString))
      {
        supplier = connection.Query<Supplier>("select * from TblSupplier");
      }

      return supplier;
    }
  }
}
