// tripsController.js
(function () {

    "use strict";

    // Getting an existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    function tripsController($http) {

        var vm = this;

        // manually add trips
        //vm.trips = [{
        //    name: "US Trip",
        //    created: new Date()
        //}, {
        //    name: "World Trip",
        //    created: new Date()
        //}];

        vm.trips = [];
        vm.newTrip = {};
        vm.errorMessage = "";
        vm.isBusy = true;

        // get trips from api
        $http.get("/api/trips")
            .then(function (response) {
                // Success
                angular.copy(response.data, vm.trips);
            }, function (error) {
                // Failure
                vm.errorMessage = "Failed to load data: " + error;
            })
        .finally(function () {
            vm.isBusy = false;
        });

        //vm.addTrip = function () {
        //    vm.trips.push({ name: vm.newTrip.name, created: new Date() });
        //    vm.newTrip = {};
        //};

    }

})();