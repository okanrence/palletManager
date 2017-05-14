palletManagerModule.factory("usersRepo", function ($http, $q, $resource) {
    return {
        save: function (user) {
            var deferred = $q.defer();
            $http.post("/api/users", user).success(function () { deferred.resolve(); })
                .error(function () { deferred.reject(); });
            return deferred.promise;
        },

        //authenticate: function(user) {
        //    var deferred = $q.defer();
        //    $http.post("/api/users/auth", user).success(function() { deferred.resolve(); })
        //        .error(function() { deferred.reject(); });
        //    return deferred.promise;
        //},

        authenticate: function(user) {
          
            return $resource("/api/users/auth",
                {},
                {
                    query: {
                        method: "GET",
                        data: false,
                        headers: { "username": user.username, "password": user.password }
                    }
                });
        },

        get: function () {
            return $resource("/api/users").query();
        }
    }
});