angular.module("MVCApp").service('InterviewerService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.GetAllInterviewers = function (interviewerDetailParams) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Interviewers/GetAllInterviewers',
            data: JSON.stringify(interviewerDetailParams)
        })
    }
    list.Register = function (interviewerDetailScope) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Interviewers/Register',
            data: JSON.stringify(interviewerDetailScope)
        })
    }
    list.GetInterviewersById = function (InterviewerId) {
        return $http({
            method: 'Get',
            url: $rootScope.apiURL + '/Interviewers/GetInterviewerById?InterviewerId=' + InterviewerId
        })
    }
    list.GetCompanyDetails = function () {
        //debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Interviewers/GetCompanyDetails'
        });
    }
    return list;
}])
