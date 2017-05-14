
var palletManagerModule = angular.module("palletManagerModule", ["ngRoute", "ngResource"])
    .config(function ($routeProvider, $locationProvider) {
        $routeProvider.when("/default",
            {
                templateUrl: "templates/default-view.html",
                controller: "homeCtrl"
            });
        $routeProvider.when("/",
           {
               templateUrl: "templates/login.html",
               controller: "usersCtrl"
           }).otherwise({
               templateUrl: "templates/login.html",
               controller: "usersCtrl"
           });


        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });

  
    });

palletManagerModule.factory("session",
    function() {
        var savedData = {};

        function set(data) {
            savedData = data;
        };

        function get() {
            return savedData;
        };

        return {
            set: set,
            get: get
        }
    });

angular.module("palletManagerModule").value("toastr", toastr);


