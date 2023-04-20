angular.module("MVCApp").service('ImportService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];

    list.uploadFile = function (payload) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Import/ImportData',
            data: payload,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        })
    }
    list.AddApplicants = function (requestBody) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Import/AddApplicants',
            data: JSON.stringify(requestBody)
        })
    }
    //list.AddApplicants = function (applicant) {
    //    debugger
    //    return $http({
    //        method: 'POST',
    //        url: $rootScope.apiURL + '/Import/AddApplicants',
    //        data: JSON.stringify(applicant)
    //    })
    //}
    return list;
}])