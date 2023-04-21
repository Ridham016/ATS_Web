﻿angular.module("MVCApp").service('AccountService', ["$http", "$rootScope", function ($http, $rootScope) {
    var list = {};

    // Login
    list.DoLogin = function (Model) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Account/Login',
            data: JSON.stringify(Model)
        });
    };

    // Login
    list.DoLogOut = function () {
        return $http({
            method: 'GET',
            url: '/Account/Logout'
        });
    };

    list.SendResetPassword = function (user) {
        return $http({
            method: 'POST',
            url: $rootScope.apiURL + '/Account/SendResetPassword',
            data: JSON.stringify(user)
        });
    }

    // Change current role of user
    list.ChangeRole = function (roleId) {
        return $http({
            method: 'PUT',
            url: $rootScope.apiURL + '/Account/ChangeRole?roleId=' + roleId
        });
    }

    list.GetUserRoles = function (userId) {
        return $http({
            method: 'GET',
            url: $rootScope.apiURL + '/Account/GetUserRoles?UserId=' + userId
        });
    }

    return list;
} ]);