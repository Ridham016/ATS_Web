angular.module("MVCApp").service('CalendarService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.Calendar = function (DateRange) {
        //debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Calendar/GetEventWithDate',
            data: JSON.stringify(DateRange)
        });
    }
    return list;
}])
