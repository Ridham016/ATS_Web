(function () {
    'use strict';

    angular.module("MVCApp").controller('DashboardCtrl', [
        '$scope', 'uiCalendarConfig', 'DashboardService', DashboardCtrl
    ]);
    function DashboardCtrl($scope, uiCalendarConfig, DashboardService) {

        $scope.SelectedEvent = null;
        var isFirstTime = true;

        $scope.events = [];
        $scope.eventSources = [$scope.events];

        //$scope.Calendar = function () {
        //    debugger
        //    DashboardService.Calendar().then(function (data) {
        //        console.log(data.data.Result);
        //        angular.forEach(data.data.Result, function (value) {
        //            debugger
        //            $scope.events.push({
        //                title : 'Title',
        //                description: value.Description,
        //                start: moment(value.ScheduleDateTime).toDate(),
        //                end: moment(value.ScheduleDateTime).toDate()
        //            });
        //        });
        //        console.log($scope.events);
        //    });
        //}
        DashboardService.Calendar().then(function (data) {
            angular.forEach(data.data.Result, function (value) {
                $scope.events.push({
                    title: 'Title',
                    description: value.Description,
                    start: moment(value.ScheduleDateTime).toDate()
                });
            });
        });

        $scope.uiConfig = {
            calendar: {
                height: 500,
                editable: false,
                displayEventTime: false,
                header: {
                    left: '',
                    center: "March",
                    right: ''
                }
                //,eventClick: function (event) {
                //    $scope.SelectedEvent = event;
                //}
                ,dayRender: function (date, cell) {
                    $('.fc-day').each(function () {
                        var eventsForDay = $scope.events.filter(function (event) {
                            return moment(event.start).isSame($(this).data('date'), 'day');
                        }.bind(this));
                        if (eventsForDay.length > 0) {
                            $(this).addClass('has-events');
                            $(this).append('<div class="event-count">' + eventsForDay.length + '</div>');
                        }
                    });
                },
                dayClick: function (date, jsEvent, view) {
                    // Filter the events for the clicked day
                    var events = $scope.events.filter(function (event) {
                        return moment(event.start).isSame(date, 'day');
                    });
                    // Display the events however you like
                    console.log(events);
                    $scope.SelectedEvents = events;
                },
                eventRender: function (event, element) {
                    // Hide the events from the calendar
                    return false;
                }
            }
        };

    }
})();