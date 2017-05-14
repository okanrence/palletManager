palletManagerModule.controller("navCtrl", function ($scope, $location, toastr, session) {
    this.user = session.get();
   
    //if (this.user.length > 0) {
    //    $scope.isLoggedIn = "t";
    //}
    $scope.User = this.user;

    $scope.$on('loggedIn', function (event, args) {
        $scope.isLoggedIn = args.message === "true";
        console.log(args.message);
    });

 
   
});;
