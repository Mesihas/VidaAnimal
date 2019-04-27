var dataSource = null;
var startPicker = null;
var endPicker = null;

$(function () {
  $(window).resize(function () {
    //   resizeGrid();   
  });

  $("#start").datepicker(
    {
    dateFormat: 'dd/mm/yy',
    changeMonth: true,
    changeYear: true
    }
  );
  $("#end").datepicker(
    {
      dateFormat: 'dd/mm/yy',
      changeMonth: true,
      changeYear: true
    }
  );

  $("#start").datepicker().datepicker('setDate', '-1m');
  $('#end').datepicker().datepicker('setDate', new Date());

  initgrid();

  $("#get").click(function () {
      $('#salesGrid').data('kendoGrid').dataSource.read();
  });

});

function resizeGrid() {
  $("#salesGrid").height($(window).height() - 220);
  $("#salesGrid").data("kendoGrid").resize();
}



function initgrid() {

   dataSource = new kendo.data.DataSource({
    type: "json",
    transport: {
      read: {
        url: '/api/GetSales',
        data: function () {
          var sd = $("#start").datepicker("getDate");
          var ed = $("#end").datepicker("getDate");
          startPicker = $.datepicker.formatDate('mm-dd-yy', sd);
          var endPicker = $.datepicker.formatDate('mm-dd-yy', ed);
          return { startDate: startPicker, endDate: endPicker }
        }
      },
    },
    schema: {
      total: function (response) {
        return (response.total);
      },
      data: function (response) {
        return (response.data);
      }
    },
    pageSize: 5,
    serverPaging: true,
    //  serverFiltering: true,
    //  serverSorting: true,
   });

  $("#salesGrid").kendoGrid({
    dataSource: dataSource,
    groupable: false,
    sortable: {
      mode: "multiple",
      allowUnsort: true
    },
    pageable: {
      numeric: true,
      refresh: true,
      buttonCount: 5,
      pageSizes: true,
      messages: {
        display: "{0} - {1} of {2} items", //{0} is the index of the first record on the page, {1} - index of the last record on the page, {2} is the total amount of records
        empty: "No hay items para mostrar",
        page: "Pagina",
        allPages: "Todos",
        of: "de {0}", //{0} is total amount of pages
        itemsPerPage: "items por pagina",
        first: "Ir a la primer pagina",
        previous: "Ir a la pagina previa",
        next: "r ala proxima pagina",
        last: "Ir a la ultima pagina",
        refresh: "Refrescar"
      }
    },
    selectable: "row",
    // navigatable: true,
    //   height: $(window).height() - 350,
    height: 250,
    scrollable: true,
    resizable: true,
    reorderable: true,
    filterable: {
      extra: false,
      operators: {
        string: {
          contains: "Contiene",
          startswith: "Comienza con",
          eq: "Es igual a",
        }
      }
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
    ],
    change: function (e) {
      var selectedRows = this.select();
      var dataItem = this.dataItem(selectedRows);
      var x = dataItem.id;
      GetSaleById(x);
    }
  });
}

function GetSaleById(x) {
  $.ajax({
    type: "Get",
    url: "api/GetsalesById",
    data: { id: x },
    dataType: "json",
    success: function (result) {
      showProducts(result);
    }
  });
}

function showProducts(result) {
  $('#testAJAX  tr').not(':first').remove();
  var html = '';
  for (var i = 0; i < result.length; i++)
    html += '<tr><td>' + result[i].productId +
      '</td><td>' + result[i].productName +
      '</td><td>' + result[i].quantity +
      '</td><td>' + result[i].price +
      '</td><td>' + result[i].totalItem + '</td></tr>';
  $('#testAJAX tr').first().after(html);
}

