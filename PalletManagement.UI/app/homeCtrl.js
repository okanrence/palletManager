palletManagerModule.controller("homeCtrl", function ($scope, $location, toastr, session) {
    
    $scope.error = false;

    $scope.user = session.get();

    toastr.success("Client added successfully"); 
  
});