palletManagerModule.controller("usersCtrl", function ($scope, usersRepo, $location, toastr, session) {

    
    $scope.error = false;

    $scope.users = usersRepo.get();
    toastr.success("Client added successfully"); 

    $scope.authenticate = function (user) {

        var userData = usersRepo.authenticate(user).query();
        session.set(userData);
        $scope.$emit('loggedIn', { message: "true" });
      
        $location.path("/default");
        userData.$promise.catch(function (errorResponse) {
            console.log(errorResponse);
            toastr.error("Either username or password is invalid", "Authentication Failed");
          
        });
        //usersRepo.authenticate(user).then(function () {
        //        //  $location.url('/ClientManagement/AllClients');
        //        toastr.success('Client added successfully');
        //    },
        //    function (response) {
        //        $scope.error = true;
        //        toastr.error('Failed', 'error');
        //    });
        return userData;
    };


    $scope.save = function (user) {
        usersRepo.save(user).then(
            function () {
              //  $location.url('/ClientManagement/AllClients');
                toastr.success('Client added successfully');
            },
            function (response) {
                $scope.error = true;
               toastr.error('Failed', 'error');
            });
    };
  
});