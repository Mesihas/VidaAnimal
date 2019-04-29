var dataSource = null;
var startPicker = null;
var endPicker = null;

$(function () {
  $(window).resize(function () {
    //   resizeGrid();   
  });



  initgrid();

  //$("#get").click(function () {
  //  $('#salesGrid').data('kendoGrid').dataSource.read();
  //});

});

function resizeGrid() {
  $("#ClientsGrid").height($(window).height() - 220);
  $("#ClientsGrid").data("kendoGrid").resize();
}


function initgrid() {

  dataSource = new kendo.data.DataSource({
    type: "json",
    transport: {
      read: {
        url: '/api/Client',
        type: "POST",
        complete: function (response) {
          if (response.status >= 400) {
            var msg = (eval("(" + response.responseText + ")")).Message;
            console.log(msg);
          }
          console.log("data");

        }
      },
      update: {
        url: "/api/client/addClient",
        type: "POST",
        //dataType: "jsonp"
        //data: function (e) {
        //  return { e };
        //}


      },
      //destroy: {
      //  url:  "/Products/Destroy",
      //  dataType: "jsonp"
      //},
      create: {
        url: "/api/client/addClient",
        dataType: "jsonp",
        type: "PUT",
        complete: function (response) {
          if (response.status >= 400) {
            var msg = (eval("(" + response.responseText + ")")).Message;
            console.log(msg);
          }
          console.log("saveDS");
          $("#ClientsGrid").data("kendoGrid").dataSource.read();
          grid = $("#ClientsGrid").data("kendoGrid");
     //     grid.refresh();
        }
      },
      //parameterMap: function (options, operation) {
      //  if (operation !== "read" && options.models) {
      //    return { models: kendo.stringify(options.models) };
      //  }
      //}
    },
    schema: {
      total: function (response) {
        return (response.total);
      },
      data: function (response) {
        console.log(response.clients);
        return (response.clients);
      },
      model: {
        id: "clientId",
        fields: {
          clientId: { editable: false },
          FirstName: { validation: { required: true } },
          lastName: { validation: { required: true } },
          telephone: { type: "number" },
          address: { editable: true },
          cuit: { editable: true }
        }
      }
    },
    pageSize: 5,
    serverPaging: true,
    serverFiltering: true,
    serverSorting: true,
  });

  $("#ClientsGrid").kendoGrid({
    dataSource: dataSource,
 //   groupable: false,
    //sortable: {
    //  mode: "multiple",
    //  allowUnsort: true
    //},
    sortable: true,
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
    height: 400,
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
        field: "clientId",
        title: "id "
      },
      {
        field: "firstName",
        title: "FirstName",
      },
      {
        field: "lastName",
        title: "LastName"
      },
      {
        field: "telephone",
        title: "Telephone"
      },
      {
        field: "address",
        title: "Address"
      },
      {
        field: "cuit",
        title: "CUIT"
      },
      //  { command: ["edit", "destroy"], title: "&nbsp;", width: "250px" }
      { command: ["edit"], title: "&nbsp;", width: "250px" }
    ],
    toolbar: ["create"],
    editable: "inline",
    save: function () {
     /// console.log("save");
    }
  });

  $(".k-grid").on("mousedown", ".k-button:not('.k-grid-cancel,.k-grid-update')", function (e) {
    var grid = $(this).closest(".k-grid");
    var editRow = grid.find(".k-grid-edit-row");

    if (editRow.length > 0) {
      alert("Please complete the editing operation before editing another row");
      e.preventDefault();
    }
  });

}

