using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VidaAnimal.Models;
using VidaAnimal.Services;
using VidaAnimal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using VidaAnimal.Models.DTO;
using System.Data.SqlClient;
using Dapper;

namespace VidaAnimal.Controllers
{
  public class ClientApiController : Controller
  {
    private IDataService<Client> _ClientDataService;
      private readonly string connectionString = @"Data Source = NETDEV; Initial Catalog = VidaAnimal; Integrated Security=True; MultipleActiveResultSets=true";

    public ClientApiController(IDataService<Client> clientService)
    {
      _ClientDataService = clientService;
    }
  
    [HttpPost("api/Client")]
    //  [Authorize]
    public JsonResult GetClients(int skip, int take, int page, DateTime startDate, DateTime endDate, int pageSize, List<GridSort> sort, GridFilters filter)
    {
      try
      {
        IEnumerable<Client> clients = null;
        int total;
      //  List<GridSort> sort = null;
        using (var connection = new SqlConnection(connectionString))
        {
          var whereCLause = KendoUIQueryHelper.BuildWhereClause<ClientDTO>(filter);
          string mainSqlQuery = string.Format(@"SELECT ClientId, FirstName as firstName, LastName, Telephone, Address, CUIT FROM TblCLient {0}", whereCLause);

          total = connection.Query<Client>(mainSqlQuery).Count();
          var orderByClause = KendoUIQueryHelper.BuildOrderByClause(sort);
         
          orderByClause = string.IsNullOrEmpty(orderByClause) ? "ORDER BY ClientId, LastName" : orderByClause;

          var sqlQuery = string.Format(@"	
            SELECT * FROM (SELECT *, ROW_NUMBER() OVER({3}) AS Row FROM ({2}) D) Filter
            WHERE Row BETWEEN  (({0} - 1) * {1}  + 1) AND  ({0} * {1})", page, pageSize, mainSqlQuery, orderByClause);

          clients = connection.Query<Client>(sqlQuery);

          return Json(new ClientIndexViewModel
          {
            Clients = clients,
            Total = total
          });
        }
      }
      catch (Exception ex)
      {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return Json(new { message = ex.Message });
      }
    }

    [HttpPost("api/client/addClient")]
    public IActionResult UpdateClient(Client client)
    {
      try
      {
        //get the Branch from database
        //Client cli = _ClientDataService.GetSingle(x => x.ClientId == client.ClientId);
        //if (cli == null){ return Ok(); }
        if (ModelState.IsValid)
        {
          Client c = new Client
          {
            ClientId = client.ClientId,
            FirstName = client.FirstName,
            LastName = client.LastName,
            Telephone = client.Telephone,
            Address = client.Address,
            CUIT = client.CUIT
          };
          _ClientDataService.Update(c);
        }
        return Ok();
        
      }
      catch (Exception ex)
      {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return Json(new { message = ex.Message });
      }
    }


    [HttpPut("api/client/addClient")]
    public IActionResult AddClient(Client client)
    {
      try
      {
    //    check if client exist in database
        Client cli = _ClientDataService.GetSingle(x => x.ClientId == client.ClientId);
        if (cli != null) { return Ok(); }


        //if (ModelState.IsValid)
        //{
          Client c = new Client
          {
          //  ClientId = client.ClientId,
            FirstName = client.FirstName,
            LastName = client.LastName,
            Telephone = client.Telephone,
            Address = client.Address,
            CUIT = client.CUIT
          };
          _ClientDataService.Create(c);
    //    }
        return Ok();

      }
      catch (Exception ex)
      {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return Json(new { message = ex.Message });
      }
    }



    [HttpGet("api/Client/getClientsDropDown")]
    //  [Authorize]
    public JsonResult GetClientsForDropDown()
    {
      try
      {
        IEnumerable<Client> clients = _ClientDataService.GetAll();

        List<Client> list = clients.Select(c => new Client { 
          ClientId = c.ClientId,
          LastName = c.FirstName + " " + c.LastName 
        }).ToList();

        return Json(list);

      }
      catch (Exception ex)
      {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return Json(new { message = ex.Message });
      }

    }
















  }
}