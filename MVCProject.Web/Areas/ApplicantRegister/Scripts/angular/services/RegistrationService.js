﻿angular.module("MVCApp").service('RegistrationService', ['$rootScope', '$http', function ($rootScope,$http) {
    var list = [];
    list.GetAllApplicants = function (applicantDetailParams) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Registrations/GetAllApplicants',
            data: JSON.stringify(applicantDetailParams)
        })
    }
    list.Register = function (applicantDetailScope) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Registrations/Register',
            data: JSON.stringify(applicantDetailScope)
        })
    }
    list.GetApplicantsById = function (ApplicantId) {
        return $http({
            method: 'Get',
            url: $rootScope.apiURL + '/Registrations/GetApplicantById?ApplicantId=' + ApplicantId
        })
    }
    //list.GetApplicantList = function (isGetAll) {
    //    return $http({
    //        method: 'GET',
    //        url: $rootScope.apiURL + '/Registrations/GetApplicantList' + (angular.isDefined(isGetAll) ? '?isGetAll=' + isGetAll : '')
    //    });
    //};
    list.GetApplicantList = function (applicantDetailScope) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Registrations/GetApplicantList',
            data: JSON.stringify(applicantDetailScope)
        });
    };
    //list.uploadFileToUrl = function (fd) {
    //    debugger
    //    return $http.post('http://localhost:56562/api/Registrations/Upload', fd, {
    //        transformRequest: angular.identity,
    //        headers: { 'Content-Type': undefined }
    //    })
    //}
    list.AddFile = function (filedata) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Registrations/FileUpload',
            data: JSON.stringify(filedata)
        })
    }
    return list;
}])
