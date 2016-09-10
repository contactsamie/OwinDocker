var app = angular.module("OwinDockerApp", [
    "ngRoute", "jsonFormatter"
]);

app.config([
    "$routeProvider", function ($routeProvider) {
        //$routeProvider
        //    // Home
        //    .when("/", {
        //        templateUrl: "partials/home.html" /*,controller: ""*/
        //    })
        //    // else 404
        //    .otherwise("/404", { templateUrl: "partials/404.html", controller: "PageCtrl" });
    }
]);
angular.module("OwinDockerApp").factory("endpoints", function () {
    return {
        hub: "/signalr",
        webApi: "/api/values/ActorSystemStates"
    };
});
angular.module("OwinDockerApp").service("service", function ($q, $http, $rootScope) {
    this.POST = function (url, item) {
        var deferred = $q.defer();
        var load = JSON.stringify(item);
        $http.post(url, load, {
            headers: {
                'Content-Type': "application/json"
            }
        }).
            success(deferred.resolve).
            error(deferred.reject);
        $rootScope.allCurrentHttpPromises.push(deferred.promise);
        return deferred.promise;
    };
    this.GET = function (url) {
        var deferred = $q.defer();
        $http({
            method: "GET",
            url: url
        }).
            success(deferred.resolve).
            error(deferred.reject);
        $rootScope.allCurrentHttpPromises.push(deferred.promise);
        return deferred.promise;
    };
});
angular.module("OwinDockerApp").factory("hub", function (endpoints, $timeout) {
    $.connection.hub.url = endpoints.hub;
    var owinDockerHub = $.connection.owinDockerHub;
    return {
        ready: function (f) {
            $.connection.hub.start().done(function () {
                var arg = arguments;
                $timeout(function () {
                    f && f.apply(null, arg);
                });
            });
        },
        OwinDockerHub: owinDockerHub,
        server: owinDockerHub.server,
        client: function (name, f) {
            owinDockerHub.client[name] = function (response) {
                var arg = arguments;
                $timeout(function () {
                    f && f.apply(null, arg);
                });
            };
        }
    };
});

angular.module("OwinDockerApp").controller("OwinDockerCtrl", function ($scope, $rootScope, $http, $q, $timeout,$interval, hub) {
    
    var countries = [
        { Name: "", Id: 0 },
        { Name: "United States", Id: 1 },
        { Name: "Canada", Id: 2 },
        { Name: "United Kingdom", Id: 3 }
    ];

    var lastResponse = {};
    var lastResponseDict = {};
    $scope.realtime = false;
    $scope.updateGrid = function () {
        $scope.newUpdateAvailable = 0;
        $("#jsGrid")
            .jsGrid({
                width: "100%",
                height: "1000px",

                inserting: true,
                editing: true,
                sorting: true,
                paging: true,

                data: lastResponse,

                fields: [
                    { name: "Name", type: "text", width: 150, validate: "required" },
                    { name: "Age", type: "number", width: 50 },
                    { name: "Address", type: "text", width: 200 },
                    { name: "Country", type: "select", items: countries, valueField: "Id", textField: "Name" },
                    { name: "Married", type: "checkbox", title: "Is Married", sorting: false },
                    { type: "control" }
                ]
            });
    }
    $scope.newUpdateAvailable = 0;
    hub.client("inventoryData", function(response) {
            for (var i = 0; i < response.length; i++) {
                var newInventory = response[i];
                var productId = newInventory.ProductId;
                var cachedInv = lastResponseDict[productId];
                if (cachedInv) {
                    $scope.newUpdateAvailable++;
                    lastResponseDict[productId] = newInventory;
                }
                lastResponse = response;
               
                   
           
            }
        }
    );

    $interval(function() {
        $scope.updateGrid();
    },10000);
  
    hub.ready(function () {
        hub.server.getInventoryList();
    });
});