﻿angular.module("MVCApp").service('RegistrationService', ['$http', function ($http) {
    var list = [];
    list.GetAllApplicants = function () {
        return $http({
            method: 'GET',
            url: 'http://localhost:56562/api/Registrations/GetAllApplicants'
        })
    }
    list.Register = function (applicantDetailScope) {
        return $http({
            method: 'POST',
            url: 'http://localhost:56562/api/Registrations/Register',
            data: JSON.stringify(applicantDetailScope)
        })
    }
    list.GetApplicantsById = function (ApplicantId) {
        return $http({
            method: 'Get',
            url: 'http://localhost:56562/api/Registrations/GetApplicantById?ApplicantId=' + ApplicantId
        })
    }
    list.GetApplicantList = function (isGetAll) {
        return $http({
            method: 'GET',
            url: 'http://localhost:56562/api/Registrations/GetApplicantList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
        });
    };
    list.uploadFileToUrl = function (fd) {
        debugger
        return $http.post('http://localhost:56562/api/Registrations/Upload', fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
    }
    return list;
}])
