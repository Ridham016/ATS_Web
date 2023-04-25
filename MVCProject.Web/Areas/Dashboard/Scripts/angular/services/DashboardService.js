angular.module("MVCApp").service('DashboardService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.GetCount = function (time) {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Dashboard/GetCounts?timeFrame=' + time
         })
    } 
    list.GetStackedCount = function () {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Dashboard/GetStackedCount'
        })
    } 
    return list;
}])
