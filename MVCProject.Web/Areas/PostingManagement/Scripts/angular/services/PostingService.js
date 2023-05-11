angular.module("MVCApp").service('PostingService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.GetApplicantList = function (applicantDetailScope) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Posting/GetApplicantList',
            data: JSON.stringify(applicantDetailScope)
        });
    }
    list.GetStatus = function () {
        //debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Posting/GetStatus'
        });
    }
    list.GetJobPostingList = function () {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Posting/GetJobPostingList'
        })
    }
    list.GetApplicantsById = function (applicantId) {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Posting/GetApplicant?ApplicantId=' + applicantId
        });
    }
    list.Register = function (applicantDetailScope) {
        //debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Posting/Register',
            data: JSON.stringify(applicantDetailScope)
        })
    }
    list.PostingRegister = function (jobpostingDetailScope) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/JobPosting/Register',
            data: JSON.stringify(jobpostingDetailScope)
        })
    }
    list.GetCompanyDetails = function () {
        //debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/JobPosting/GetCompanyDetails'
        });
    }

    list.PositionRegister = function (positionDetailScope) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/JobPosting/PositionRegister',
            data: JSON.stringify(positionDetailScope)
        })
    }

    list.GetPostingStatus = function () {
        //debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/JobPosting/GetPostingStatus'
        });
    }

    //list.GetPositionDetails = function () {
    //    return $rootScope.apiURL + '/JobPosting/GetPositionDetails?searchText=';
    //}

    list.GetPositionDetails = function () {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/JobPosting/GetPositionDetails?searchText='

        });
    }

    list.GetAllPostings = function (jobpostingDetailParams) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/JobPosting/GetAllPostings',
            data: JSON.stringify(jobpostingDetailParams)
        });
    }
    return list;
}])
