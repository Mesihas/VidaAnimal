using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidaAnimal.Services;
using System.Globalization;
using VidaAnimal.Models;
using VidaAnimal.Models.DTO;

namespace VidaAnimal.Controllers
{
  public class SalesApiController : Controller
  {
    private MyDbContext _context;
    private IDataService<Category> _categoryDataService;
    private IDataService<Product> _productDataService;
    private IDataService<Sales> _saleDataService;
    private IDataService<SalesDetail> _saleDetailService;
    private readonly ISalesDataService _salesrProvider;
    public SalesApiController(MyDbContext context,
                              IDataService<Category> categoryService,
                              IDataService<Product> productService,
                              IDataService<Sales> saleService,
                              IDataService<SalesDetail> saleDetailService,
                              ISalesDataService salesrProvider)
    {
      _context = context;
      _categoryDataService = categoryService;
      _productDataService = productService;
      _saleDataService = saleService;
      _saleDetailService = saleDetailService;
      _salesrProvider = salesrProvider;
    }

    [HttpPost("api/GetSales")]
    //  [Authorize]
    public JsonResult GetSales(int skip, int take, int page, int pageSize, DateTime startDate, DateTime endDate, List<GridSort> sort, GridFilters filter)
    {
      try
      {
        Vamo result = _salesrProvider.GetSales(skip, take, page, pageSize, startDate, endDate, sort, filter);

        return Json(new
        {
          Data = result.Data,
          Total = result.Total
        });

        //      return Json("ok");
      }
      catch (Exception ex)
      {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return Json(new { message = ex.Message });
      }
    }

    [HttpGet("api/GetsalesById")]
    //   [Authorize]
    public JsonResult GetSalesDetails(int id)
    {
      try
      {
        IEnumerable<SalesDetailDTOAux> result = _salesrProvider.GetSaleDetail(id);
        return Json(result);
      }
      catch (Exception ex)
      {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return Json(new { message = ex.Message });
      }
    }

    [HttpGet("api/getCategories")]
    [Authorize]
    public JsonResult GetCategories()
    {
      try
      {
        IEnumerable<Category> categories = _categoryDataService.GetAll();
        return Json(categories);
      }
      catch (Exception ex)
      {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return Json(new { message = ex.Message });
      }
    }

    [HttpGet("api/getProducts")]
    [Authorize]
    public JsonResult GetProducs(int id)
    {
      try
      {
        IEnumerable<Product> products = _productDataService.Query(p => p.CategoryId == id);
        return Json(products);
      }
      catch (Exception ex)
      {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return Json(new { message = ex.Message });
      }
    }

    [HttpPost("api/saveSale")]
    [Authorize]
    public JsonResult CreateOrder([FromBody] SalesOrder sale)
    {
      try
      {
        if (ModelState.IsValid)
        {
          CultureInfo MyCultureInfo = new CultureInfo("de-DE");
          string MyString = sale.SellingDate;
          DateTime MyDateTime = DateTime.Parse(MyString, MyCultureInfo);

          Sales sal = new Sales
          {
            ClientId = sale.ClientId,
            SellingDate = MyDateTime,
            PayWayId = sale.PayWayId,
            Obs = sale.Info
          };

          _saleDataService.Create(sal);
          int orderNumber = sal.SalesId;
          decimal total = 0;
          foreach (var item in sale.Items)
          {

            var price = System.Convert.ToDecimal(item.price.Replace(@"$", ""));
            var cost = System.Convert.ToDecimal(item.cost.Replace(@"$", ""));
            var totalItem = System.Convert.ToDecimal(item.totalItem.Replace(@"$", ""));

            SalesDetail salItems = new SalesDetail
            {
              SalesId = orderNumber,
              ProductId = item.productId,
              Quantity = item.quantity,
              Price = price,
              Cost = cost
            };

            _saleDetailService.Create(salItems);
            total = total + price * item.quantity;
          }
          sal.Total = total;
          _saleDataService.Update(sal);
          var message = "OK";

          return Json(message);
        }
        //invalid
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return Json(new { message = "fail", ModelState = ModelState });
      }
      catch (Exception ex)
      {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return Json(new { message = ex.Message });
      }
    }
  }
}


//IEnumerable<SalesDetailDTO> result = from sales in _context.TblSales
//                                     where sales.SalesId == id
//                                     join client in _context.TblClient on sales.ClientId equals client.ClientId
//                                     into Clientes
//                                     from m in Clientes.DefaultIfEmpty()
//                                     select new SalesDetailDTO
//                                     {
//                                       Id = sales.SalesId,
//                                       SaleDate = sales.SellingDate,
//                                       FirstName = m.FirstName,
//                                       LastName = m.LastName,
//                                     };


//IEnumerable<SalesDetailDTO> result = from sales in _context.TblSales
//                                     where sales.ClientId == id
//                                     join client in _context.TblClient on sales.ClientId equals client.ClientId
//                                     into Clientes
//                                     from m in Clientes.DefaultIfEmpty()
//                                     join saledetail in _context.TblSalesDetail on sales.SalesId equals saledetail.SalesId
//                                     into SalesDetail
//                                     from s in SalesDetail.DefaultIfEmpty()
//                                     select new SalesDetailDTO
//                                     {
//                                       Id = sales.SalesId,
//                                       SaleDate = sales.SellingDate,
//                                       FirstName = m.FirstName,
//                                       LastName = m.LastName,
//                                       Price = (decimal)s.Price
//                                     };


/////Query for sales 

//IEnumerable<SalesDetailDTO> result =
//                  from sales in _context.TblSales
//                  join client in _context.TblClient on sales.ClientId equals client.ClientId
//                  into Clientes
//                  from m in Clientes.DefaultIfEmpty()
//                  select new SalesDetailDTO
//                  {
//                    Id = sales.SalesId,
//                    SaleDate = sales.SellingDate,
//                    ClientName = m.FirstName + " " + m.LastName,
//                    Total = sales.Total.ToString("C"),
//                  };


//var total = result.Count();

//return Json(new { result, total });


/////Query for salesDetails

//IEnumerable<SalesDetailDTOAux> result = from salesDetail in _context.TblSalesDetail
//                                        where salesDetail.SalesId == id
//                                        join product in _context.TblProduct on salesDetail.ProductId equals product.ProductId
//                                        into Product
//                                        from m in Product.DefaultIfEmpty()
//                                        select new SalesDetailDTOAux
//                                        {
//                                          ProductId = salesDetail.ProductId,
//                                          ProductName = m.ProductName,
//                                          Quantity = salesDetail.Quantity,
//                                          Price = salesDetail.Price.ToString("C"),
//                                          TotalItem = (salesDetail.Quantity * salesDetail.Price).ToString("C")
//                                        };