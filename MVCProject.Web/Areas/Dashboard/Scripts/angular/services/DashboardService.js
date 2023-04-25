angular.module("MVCApp").service('DashboardService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.Calendar = function (DateRange) {
        //debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Dashboard/GetEventWithDate',
            data: JSON.stringify(DateRange)
        });
    }
    list.GetCount = function (time) {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Dashboard/GetCounts?timeFrame=' + time
         })
    } 
    return list;
}])
