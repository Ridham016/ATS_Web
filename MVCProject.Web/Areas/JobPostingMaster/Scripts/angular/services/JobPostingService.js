angular.module("MVCApp").service('JobPostingService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.Register = function (jobpostingDetailScope) {
        debugger
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
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Position/Register',
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
    list.GetPostingById = function (PostingId) {
        return $http({
            method: 'Get',
            url: $rootScope.apiURL + '/JobPosting/GetPostingById?PostingId=' + PostingId
        })
    }
    return list;
}])
