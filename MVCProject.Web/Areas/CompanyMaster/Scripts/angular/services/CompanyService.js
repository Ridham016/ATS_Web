angular.module("MVCApp").service('CompanyService', ['$rootScope', '$http', function ($rootScope, $http) {
    var list = [];
    list.Register = function (companyDetailScope) {
        //debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Company/Register',
            data: JSON.stringify(companyDetailScope)
        })
    }
    list.GetAllCompany = function (companyDetailParams) {
        debugger
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Company/GetAllCompany',
            data: JSON.stringify(companyDetailParams)
        });
    }
    list.GetPositionDetails = function () {
        //debugger
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Company/GetPositionDetails'
        });
    }
    list.GetCompanyById = function (CompanyId) {
        return $http({
            method: 'Get',
            url: $rootScope.apiURL + '/Company/GetCompanyById?Id=' + CompanyId
        })
    }
    return list;
}])
