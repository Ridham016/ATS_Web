angular.module("MVCApp").service('InterviewerService', ['$http', function ($http) {
    var list = [];
    list.GetAllInterviewers = function () {
        return $http({
            method: 'GET',
            url: 'http://localhost:56562/api/Interviewers/GetAllInterviewers'
        })
    }
    list.Register = function (interviewerDetailScope) {
        return $http({
            method: 'POST',
            url: 'http://localhost:56562/api/Interviewers/Register',
            data: JSON.stringify(interviewerDetailScope)
        })
    }
    list.GetInterviewersById = function (InterviewerId) {
        return $http({
            method: 'Get',
            url: 'http://localhost:56562/api/Interviewers/GetInterviewerById?InterviewerId=' + InterviewerId
        })
    }
    return list;
}])
