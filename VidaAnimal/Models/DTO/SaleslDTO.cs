using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidaAnimal.Models.DTO
{
  public class SalesDetailDTOAux
  {
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public string Price { get; set; }
    public string TotalItem { get; set; }
  }

  public class SalesDetailDTO
    {
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public string ClientName { get; set; }
    public string Total { get; set; }
  }

  public class SalesOrder
  {
    public int ClientId { get; set; }
    public string SellingDate { get; set; }
    public int PayWayId { get; set; }
    public string Info { get; set; }
    public IEnumerable<SalesOrderItems> Items { get; set; }
  }

  public class SalesOrderItems
  {
    public int itemId { get; set; }
    public int productId { get; set; }
    public string productName { get; set; }
    public int quantity { get; set; }
    public string price { get; set; }
    public string cost { get; set; }
    public string totalItem { get; set; }
  }
}
