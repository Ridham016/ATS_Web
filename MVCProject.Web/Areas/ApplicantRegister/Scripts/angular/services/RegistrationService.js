angular.module("MVCApp").service('RegistrationService', ['$rootScope', '$http', function ($rootScope,$http) {
    var list = [];
    list.GetAllApplicants = function (applicantDetailParams) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Registrations/GetAllApplicants',
            data: JSON.stringify(applicantDetailParams)
        })
    }
    list.Register = function (applicantDetailScope) {
        //debugger
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
    }
    //list.uploadFileToUrl = function (fd) {
    //    debugger
    //    return $http.post('http://localhost:56562/api/Registrations/Upload', fd, {
    //        transformRequest: angular.identity,
    //        headers: { 'Content-Type': undefined }
    //    })
    //}
    list.AddFile = function (filedata, Id) {
        //debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Registrations/FileUpload?ApplicantId=' + Id + '&databaseName=' + $rootScope.userContext.CompanyDB,
            data: JSON.stringify(filedata)
        })
    }
    list.uploadFile = function (payload) {
        //debugger
        return $http({
            url: $rootScope.apiURL + '/Upload/UploadFile?databaseName=' + $rootScope.userContext.CompanyDB,
            method: 'POST',
            data: payload,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        });
    }
    list.GetFiles = function (ApplicantId) {
        return $http({
            method: 'Get',
            url: $rootScope.apiURL + '/Registrations/GetFileOfApplicant?ApplicantId=' + ApplicantId
        })
    }
    list.DeleteFile = function (FileId) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Registrations/DeleteFile?FileId=' + FileId
            })
    }
    list.Export = function() {
        //debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Registrations/ExportToXl'
        });
    }

    list.GetJobPostingList = function () {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Registrations/GetJobPostingList'
        })
    }
    return list;
}])
