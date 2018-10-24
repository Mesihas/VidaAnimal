
var map = null;
var zoom = 6;
var markers = [];
var labels = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
var labelIndex = 0;
var userSwitchTab = false;
var userZoom = null;
var lastSelectedVehicleCode = null;
var selectedRow = false;
var comesFromTimer = false;
var visibleGrid = "VehicleGrid";
var lastZoomSelected = null;

$(function () {

  loadVehicleData();

});

function loadVehicleData() {
  var dataSource = new kendo.data.DataSource({
    type: "json",
    transport: {
      read: {
        url: '/api/GetSales',
      }
    },
    schema: {
      total: function (response) {
        return (response.total);
      },
      data: function (response) {
        return (response.data);
      }
    },
    pageSize: 20,
    serverPaging: true,
    serverFiltering: true,
    serverSorting: true,
  });

  VehicleGrid(dataSource);

}


function VehicleGrid(dataSource) {
  var grid = $("#salesGrid").kendoGrid({
    dataSource: dataSource,
    groupable: false,
    sortable: {
      mode: "multiple",
      allowUnsort: true
    },
    pageable: {
      input: true,
      numeric: true,
      refresh: true,
      buttonCount: 5,
      pageSizes: true
    },
    selectable: true,
    height: $(window).height() - 290,
    scrollable: true,
    resizable: true,
    reorderable: true,
    filterable: {
      extra: false,
      operators: {
        string: {
          contains: "Contains",
          startswith: "Starts with",
          eq: "Is equal to",
        }
      }
    },
    dataBound: function (e) {
      var dataSource = $("#salesGrid").data("kendoGrid").dataSource;
      var filteredDataSource = new kendo.data.DataSource({
        data: dataSource.data(),
        filter: dataSource.filter()
      });

      filteredDataSource.read();
      var data = filteredDataSource.view();
    },
    columns: [
      {
        field: "id",
        title: "id "
      },
      {
        field: "saleDate",
        title: "saleDate",
        template: "#=kendo.toString(kendo.parseDate(saleDate), 'dd/MM/yyyy HH:mm') #"
      },
      {
        field: "clientName",
        title: "Client"
      },
      {
        field: "total",
        title: "Total"
      }
    ]
  }).data("kendoGrid")
}

//$("#salesGrid").on("click", function (e) {
//  var vehicleGrid = $("#salesGrid").data("kendoGrid");
//  var selectedVehicle = vehicleGrid.dataItem(vehicleGrid.select());
//  if (selectedVehicle) {
//    if (lastSelectedVehicleCode == selectedVehicle.Code && comesFromTimer == false) {
//      selectedRow = true;
//      comesFromTimer = false;
//    } else {
//      selectedRow = false;
//      comesFromTimer = false;
//    }
//    lastSelectedVehicleCode = selectedVehicle.Code;
//    selectVehicle(selectedVehicle);
//  }
//});

  //string mainSqlQuery = string.Format(@"SELECT
  //       V.VEHICLEID AS ID, V.VEHICLECODE AS Code, V.VEHICLE AS Name,
  //       F.FLEETID AS FleetID, F.FLEETCODE AS FleetCode, F.FLEET AS FleetName,
  //       VL.VehicleLocationID, VL.Latitude, VL.Longitude, VL.DateTime, VL.Description
  //       FROM VEHICLE V WITH(NOLOCK)
  //       JOIN FLEET F WITH(NOLOCK) ON F.FLEETID = V.FLEETID
  //       CROSS APPLY(
  //       SELECT TOP 1 VL. * FROM VehicleLocations VL WITH(NOLOCK)
  //       WHERE VL.VehicleID = V.VEHICLEID
  //       ORDER BY VL.DateTime DESC) AS VL {0}", whereClause);

//  var countSql = string.Format(@"SELECT COUNT(*) FROM ({0}) D", mainSqlQuery);
//  var total = context.Database.SqlQuery<int>(countSql).First();
//  var sqlQuery = string.Format(@"	
//      SELECT * FROM (SELECT *, ROW_NUMBER() OVER({3}) AS Row FROM ({2}) D) Filter
//      WHERE Row BETWEEN  (({0} - 1) * {1}  + 1) AND  ({0} * {1})",
//  page, pageSize, mainSqlQuery, orderByClause);