angular.module("MVCApp").service("CommonEnums", [CommonEnums]);

function CommonEnums() {
    var vm = this;

    // Convert Enum to  Array of {Id, Name}
    vm.toArray = function (enumObject) {
        var _EnumName = "";
        var tempArray = [];
        for (var key in enumObject) {
            if (key == "_EnumName") {//Skip this key from enum, "_EnumName" contains object name of Enum
                _EnumName = enumObject[key];
                continue;
            }
            tempArray.push({ Id: enumObject[key], Name: enumResource[_EnumName][key] });
        }
        return tempArray;
    };

    vm.NoticePeriod = {
        1: 'Immediately',
        2: '0 to 15 days',
        3: '15 to 30 days',
        4: '30 to 45 days',
        5: '45 to 60 days'
    };

    vm.Mode = {
        0: 'Offline',
        1: 'Online'
    };

    // Permission Level
    vm.PermissionLevel = {
        _EnumName: "PermissionLevel",
        Admin: 1,        
    };

    

    return vm;
}