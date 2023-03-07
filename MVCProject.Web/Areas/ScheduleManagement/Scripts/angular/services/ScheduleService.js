angular.module("MVCApp").service('ScheduleService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.GetApplicantList = function (applicantDetailScope) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Schedules/GetApplicantList',
            data: JSON.stringify(applicantDetailScope)
        });
    };
    return list;
}])
