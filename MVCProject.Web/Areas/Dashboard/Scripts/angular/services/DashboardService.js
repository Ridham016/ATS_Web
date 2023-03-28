angular.module("MVCApp").service('DashboardService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.Calendar = function () {
        debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Dashboard/GetEvents'
        });
    }
    return list;
}])
