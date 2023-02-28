angular.module("MVCApp").service('ScheduleService', ['$http', function ($http) {
    var list = [];
    list.GetAllApplicants = function () {
        return $http({
            method: 'GET',
            url: 'http://localhost:56562/api/Schedules/GetAllApplicants'
        })
    }
    list.Register = function (applicantDetailScope) {
        return $http({
            method: 'POST',
            url: 'http://localhost:56562/api/Schedules/Register',
            data: JSON.stringify(applicantDetailScope)
        })
    }
    list.GetApplicantsById = function (ApplicantId) {
        return $http({
            method: 'Get',
            url: 'http://localhost:56562/api/Schedules/GetApplicantById?ApplicantId=' + ApplicantId
        })
    }
    list.GetApplicantList = function (isGetAll) {
        return $http({
            method: 'GET',
            url: 'http://localhost:56562/api/Schedules/GetApplicantList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
        });
    };
    return list;
}])
