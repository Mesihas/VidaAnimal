var saleItems = [];
var itemId = 0;

$(function () {
  var categories = null;
  var products = null;
  var categoryId = 1;
  var item = null;
  grid(null);

  $.ajax({
    type: "Get",
    url: "/api/getCategories",
    dataType: "json",
    success: function (data) {
      categories = data;
      FillCategories(categories);
      GetProducts(categoryId);
    }
  });

  $.ajax({
    type: "Get",
    url: "/api/Client/getClientsDropDown",
    dataType: "json",
    success: function (data) {
      Clients = data;
      FillClients(Clients);
    }
  });
  formaDePago = [
    { PayWayId: 1, formaDePago: "Contado" },
    { PayWayId: 2, formaDePago: "Debito" },
    { PayWayId: 3, formaDePago: "Targeta" },
  ];

  FillFormasDePago(formaDePago)
  $("#CategoriesDropdown").change(function () {
    categoryId = this.value;
    GetProducts(categoryId);
  });
  $("#datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
});

function GetProducts(categoryId) {
  $.ajax({
    type: "Get",
    data: { id: categoryId },
    url: "/api/getProducts",
    dataType: "json",
    success: function (data) {
      products = data;
      $(productsDropdown).empty();
      FillProduts(products);
    }
  });
}

function FillClients(clients) {
  $.each(clients, function () {
    $("#ClientsDropdown").append($("<option />").val(this.clientId).text(this.lastName));
  });
}

function FillCategories(categories) {
  $.each(categories, function () {
    $("#CategoriesDropdown").append($("<option />").val(this.categoryId).text(this.name));
  });
}

function FillProduts(products) {
  $.each(products, function () {
    $("#productsDropdown").append($("<option />").val(this.productId).text(this.productName));
  });
}

function FillFormasDePago(formaDePago) {
  $.each(formaDePago, function () {
    $("#formaDePagoDropDown").append($("<option />").val(this.PayWayId).text(this.formaDePago));
  });
}

$('#addItem').on('click', function (event) {
  event.preventDefault(); 
  productId = $("#productsDropdown").val();
  productName = $("#productsDropdown option:selected").text();
  quantity = $("#Quantity").val();
  price = "$" + $("#Price").val();
  cost = "$" + $("#productsDropdown").val();
  itemId = ++itemId;
  //////////
  if (price && quantity) {
    totalItem = $("#Price").val() * $("#Quantity").val();
  }


  //////////
  var item = new saleItem(itemId, productId, productName, quantity, price, cost, totalItem);
  saleItems.push(item);
  RefreshGrid();
  CleanFileds();
  printItems(saleItems);
});

function grid(data) {
  $("#GridSaleItems").kendoGrid({
    dataSource: saleItems,
    change: function (e) {
    },
    columns: [
      {
        field: "itemId",
        hidden: false,
        width: 50
      },
      {
        field: "productName",
        title: "Producto ",
        width: 250
      },
      {
        field: "quantity",
        title: "Cant.",
        width: 50
      },
      {
        field: "price",
        title: "Precio",
        width: 50
      },
      {
        field: "cost",
        title: "cost",
        width: 50
      },
      { command: { text: "Eliminar item", click: DeleteRow }, title: " ", width: "180px" }]
  }).data("kendoGrid");
}

var saleItem = function (itemId, productId, productName, quantity, price, cost, totalItem) {
  this.itemId = itemId;
  this.productId = productId;
  this.productName = productName;
  this.quantity = quantity;
  this.price = price;
  this.cost = cost;
  this.totalItem = totalItem;
}

var saleMain = function (clientId, info, saleItems, payWayId, sellingDate) {
  this.ClientId = clientId;
  this.Info = info;
  this.Items = saleItems;
  this.PayWayId = payWayId;
  this.SellingDate = sellingDate;
}

function CleanFileds() {

  $("#CategoriesDropdown").val(1);
  $("#productsDropdown").text("");
  $("#Quantity").val("");
  $("#Price").val("");
  $("#CategoriesDropdown").focus();
  $("#CategoriesDropdown").trigger("change");
}

function printItems(saleItems) {
  for (var i = 0; i < saleItems.length; i++) {
    console.log("Print Items: " + saleItems[i].itemId);
  }
}

function DeleteRow(e) {
  e.preventDefault();
  var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

  for (var i = 0; i < saleItems.length; i++)
    if (saleItems[i].itemId === dataItem.itemId) {
      saleItems.splice(i, 1);
      break;
    }
  RefreshGrid();
}

function RefreshGrid() {
  $('#GridSaleItems').data('kendoGrid').dataSource.read();
  $('#GridSaleItems').data('kendoGrid').refresh();
}

$('#saveSale').on('click', function (event) {
  sellingDate = $("#datepicker").datepicker('getDate');
  var now = new Date();
  var day = sellingDate.getDate();
  var month = sellingDate.getMonth();
  var year = sellingDate.getFullYear();
  var hours = now.getHours();
  var minutes = now.getMinutes();
  var seconds = now.getSeconds();

  sd = new Date(year, month, day, hours, minutes, seconds);

  clientId = $("#ClientsDropdown").val();

  payWayId = $("#formaDePagoDropDown").val();
  info = $("#info").val();
  var sale = new saleMain(clientId, info, saleItems, payWayId, sd);

  $.ajax({
    type: "POST",
    url: "/api/saveSale",
    contentType: "application/json",
    dataType: 'json',
    data: JSON.stringify(sale) ,
    success: function (response) {
      //response ? alert("It worked!") : alert("It didn't work.");
      window.location.href = '/Sales/Index';
    }
  });

});
