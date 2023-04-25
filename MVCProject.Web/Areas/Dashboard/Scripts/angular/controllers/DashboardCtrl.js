(function () {
    'use strict';

    angular.module("MVCApp").controller('DashboardCtrl', [
        '$scope', 'uiCalendarConfig','$rootScope','CommonEnums', 'DashboardService', DashboardCtrl
    ]);
    function DashboardCtrl($scope, uiCalendarConfig, $rootScope, CommonEnums, DashboardService) {


        $scope.getCounts = function (time) {
            $scope.timeFrame = time;
            DashboardService.GetCount(time).then(function (res) {
                if (res) {
                    if (res.data.MessageType = messageTypes.Success) {
                        $scope.Counts = res.data.Result;
                        var colorPalette = ['#00D8B6', '#008FFB', '#FEB019', '#FF4560', '#775DD0']

                        var optionDonut = {
                            chart: {
                                type: 'donut',
                                width: '100%',
                                height: 400
                            },
                            dataLabels: {
                                enabled: false,
                            },
                            plotOptions: {
                                pie: {
                                    customScale: 0.8,
                                    donut: {
                                        size: '70%',
                                    },
                                    offsetY: 20,
                                },
                                stroke: {
                                    colors: undefined
                                }
                            },
                            colors: colorPalette,
                            title: {
                                text: 'Applicant Status',
                                align: 'center',
                                style: {
                                    fontSize: '24px',
                                    fontWeight: 'bold',
                                    color: '#000'
                                },
                            },
                            series: [$scope.Counts.Registered, $scope.Counts.Shortlisted, $scope.Counts.Discarded, $scope.Counts.InterviewScheduled, $scope.Counts.Hold, $scope.Counts.ApplicantsHired, $scope.Counts.Rejected, $scope.Counts.InterviewCancelled],
                            labels: ['Registered', 'Shortlisted', 'Discarded', 'Interview Scheduled', 'Hold', 'Hired', 'Rejected', 'Interview Cancelled'],
                            legend: {
                                position: 'bottom'
                            }
                        }

                        var donut = new ApexCharts(
                            document.querySelector("#donut"),
                            optionDonut
                        )
                        donut.render();
                        window.dispatchEvent(new Event('resize'));
                    } else if (res.data.MessageType == messageTypes.Error) {
                        toastr.error(res.data.Message, errorTitle);
                    } else if (res.data.MessageType == messageTypes.Warning) {
                        toastr.warning(res.data.Message, warningTitle);
                    }
                }
            })
        }

    }
})();