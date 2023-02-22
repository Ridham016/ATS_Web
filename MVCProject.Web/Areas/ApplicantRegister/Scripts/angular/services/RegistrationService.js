angular.module("MVCApp").service('RegistrationService',['$http', function ($http) {
        var list = [];
    list.GetAllApplicants = function () {
        return $http({
            method: 'GET',
            url: 'http://localhost:56562/api/Registrations/GetAllApplicants'
        })
    };
    list.Register = function (data) {
        return $http({
            method: 'POST',
            url: 'http://localhost:56562/api/Registrations/Register/ApplicantId=' + data.ApplicantId,
            data: JSON.stringify(data)
        })
    };
        return list;
    }])