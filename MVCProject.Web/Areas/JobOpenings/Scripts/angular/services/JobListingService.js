angular.module("MVCApp").service('JobListingService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.GetJobPostingList = function () {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/JobListing/GetJobPostingList'
        })
    }

    list.GetDescription = function (PostingId) {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/JobListing/GetDescription?PostingId=' + PostingId
        })
    }
    return list;
}])
