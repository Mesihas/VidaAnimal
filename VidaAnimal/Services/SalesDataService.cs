using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using VidaAnimal.Models.DTO;




namespace VidaAnimal.Services
{
  public interface ISalesDataService
  {
    Vamo GetSales(int skip, int take, int page, int pageSize, DateTime startDate, DateTime endDate);
    //    Vamo GetSales(int skip, int take, int page, int pageSize, string whereClause, string orderByClause, List<GridSort> sort, GridFilters filter);
    IEnumerable<SalesDetailDTO> GetSaleByID(int id);
    IEnumerable<SalesDetailDTOAux> GetSaleDetail(int id);
  }

  public class SalesDataService : ISalesDataService
  {
    private readonly string connectionString;
    public SalesDataService(string connectionString)
    {
      this.connectionString = connectionString;
    }

    public Vamo GetSales(int skip, int take, int page, int pageSize, DateTime startDate, DateTime endDate)
    {
      try
      {
        IEnumerable<SalesDetailDTO> sales = null;
        int total;
        List<GridSort> sort = null;
        using (var connection = new SqlConnection(connectionString))
        {
          string sqlFormattedStartDate = startDate.ToString("yyyy-MM-dd HH:mm:ss");
          string sqlFormattedEndDate = endDate.ToString("yyyy-MM-dd HH:mm:ss");

          string mainSqlQuery = string.Format(@"SELECT SalesId as Id, SellingDate as SaleDate, FirstName as ClientName, '$'+ FORMAT(Total,'#,0.00') as Total " +
                         "FROM TblSales S INNER JOIN TblClient C ON C.ClientId = S.ClientId " +
                         "WHERE SellingDate between '{0}' and '{1}'", sqlFormattedStartDate, sqlFormattedEndDate);

          total = connection.Query<SalesDetailDTO>(mainSqlQuery).Count();

          var orderByClause = KendoUIQueryHelper.BuildOrderByClause(sort);

          orderByClause = string.IsNullOrEmpty(orderByClause) ? "ORDER BY Id, SaleDate" : orderByClause;

          var sqlQuery = string.Format(@"	
              SELECT * FROM (SELECT *, ROW_NUMBER() OVER({3}) AS Row FROM ({2}) D) Filter
              WHERE Row BETWEEN  (({0} - 1) * {1}  + 1) AND  ({0} * {1})", page, pageSize, mainSqlQuery, orderByClause);

          sales = connection.Query<SalesDetailDTO>(sqlQuery);
        }
        return (new Vamo
        {
          Data = sales,
          Total = total
        });
      }
      catch (Exception ex)
      {
        var msg = ex.ToString();
        return null;
      }
}

    //public Vamo GetSales(int skip, int take, int page, int pageSize, string orderByClause, string whereClause)
    //{
    //  IEnumerable<SalesDetailDTO> sales = null;
    //  int total;
    //  using (var connection = new SqlConnection(connectionString))
    //  {
    //    string mainSqlQuery = string.Format(@"SELECT SalesId as Id, SellingDate as SaleDate, FirstName as ClientName, '$'+ FORMAT(Total,'#,0.00') as Total " +
    //                   "FROM TblSales S INNER JOIN TblClient C ON C.ClientId = S.ClientId {0}", whereClause);

    //    total = connection.Query<SalesDetailDTO>(mainSqlQuery).Count();

    //    var sqlQuery = string.Format(@"	
    //          SELECT * FROM (SELECT *, ROW_NUMBER() OVER({3}) AS Row FROM ({2}) D) Filter
    //          WHERE Row BETWEEN  (({0} - 1) * {1}  + 1) AND  ({0} * {1})", page, pageSize, mainSqlQuery, orderByClause);

    //    sales = connection.Query<SalesDetailDTO>(sqlQuery);
    //  }
    //  return (new Vamo
    //  {
    //    Data = sales,
    //    Total = total
    //  });
    //}

    public IEnumerable<SalesDetailDTO> GetSaleByID(int id)
    {
      IEnumerable<SalesDetailDTO> sale = null;
      using (var connection = new SqlConnection(connectionString))
      {
        string query = "SELECT SalesId as Id, SellingDate as SaleDate, FirstName as ClientName, Total  " +
                       "FROM TblSales S INNER JOIN TblClient C ON C.ClientId = S.ClientId " +
                       "WHERE SalesId = @SalesId;";

        sale = connection.Query<SalesDetailDTO>(query, new { SalesId = id });
      }
      return sale;
    }
    public IEnumerable<SalesDetailDTOAux> GetSaleDetail(int id)
    {
      IEnumerable<SalesDetailDTOAux> saleDetail = null;
      using (var connection = new SqlConnection(connectionString))
      {
        string query = 
          "SELECT   SD.ProductId as ProductId, ProductName, Quantity, '$' + FORMAT(Price,'#,0.00') as Price, '$' + FORMAT((Quantity * Price ),'#,0.00') as TotalItem " +
          "FROM TblSalesDetail SD INNER JOIN TblProduct P ON P.ProductId = SD.ProductId " +
          "WHERE SalesId = @SalesId;";

        saleDetail = connection.Query<SalesDetailDTOAux>(query, new { SalesId = id });
      }
      return saleDetail;
    }

  }

  public class Vamo
  {
    public int Total { get; set; }
    public IEnumerable<SalesDetailDTO> Data { get; set; }
  }

}
