angular.module("MVCApp").service('ScheduleService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.GetApplicantsById = function (ApplicantId) {
        return $http({
            method: 'Get',
            url: $rootScope.apiURL + '/Schedules/GetApplicantById?ApplicantId=' + ApplicantId
        })
    }
    //list.GetApplicantList = function (isGetAll) {
    //    return $http({
    //        method: 'GET',
    //        url: 'http://localhost:56562/api/Schedules/GetApplicantList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
    //    });
    //};
    list.GetApplicantList = function (applicantDetailScope, isGetAll) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Registrations/GetApplicantList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : ''),
            data: JSON.stringify(applicantDetailScope)
        });
    };
    return list;
}])
