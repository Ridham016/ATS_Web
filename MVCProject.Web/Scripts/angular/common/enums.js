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
        0: 'Immediately',
        1: '0 to 15 days',
        2: '15 to 30 days',
        3: '30 to 45 days',
        4: '45 to 60 days'
    };

    // Permission Level
    vm.PermissionLevel = {
        _EnumName: "PermissionLevel",
        Admin: 1,        
    };

    

    return vm;
}