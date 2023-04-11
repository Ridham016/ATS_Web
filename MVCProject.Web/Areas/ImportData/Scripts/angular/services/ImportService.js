angular.module("MVCApp").service('ImportService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];

    list.uploadFile = function (file) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Import/ImportData',
            data: file
        })
    }
    list.AddApplicants = function (applicantDetailScope) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Import/AddApplicants',
            data: JSON.stringify(applicantDetailScope)
        })
    }
    return list;
}])