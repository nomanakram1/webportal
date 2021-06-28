"use strict";
var KTDatatablesExtensionsResponsive = function() {

	var initTable1 = function() {
		var table = $('#kt_datatable');

		table.DataTable({
            responsive: true,
            "info": false,
            columns: [
                { data: 'ReservationId', visible: false, searchable: false },
                { data: 'ReservationUI' },
                { data: 'Department', "bSortable": true },
                { data: 'TransportDate' },
                { data: 'CostCenterNumber', "bSortable": false },
                { data: 'MRN', "bSortable": false },
                { data: 'PatientFirstName', "bSortable": false },
                { data: 'PatientLastName', "bSortable": false },
                { data: 'PickupAddress', "bSortable": false },
                { data: 'PickupCity', "bSortable": false },
                { data: 'ContactPhone', "bSortable": false },
                { data: 'DestinationAddress', "bSortable": false },
                { data: 'DestinationCity', "bSortable": false },
                { data: 'OfficePhone', "bSortable": false },
                { data: 'TransportType', "bSortable": false },
                { data: 'PickupTime', "bSortable": false },
                { data: 'AppointmentTime', "bSortable": false },
                { data: 'ReturnTime', "bSortable": false },
                { data: 'Comments', "bSortable": false },
                { data: 'RequestedBy', "bSortable": false },
                { data: 'CallBackNumber', "bSortable": false },
                { data: 'CreatedOn', "bSortable": false },
                //{ data: 'Mileage', "bSortable": false },
            ],
            rowId: "ReservationId"
        });

        $.ajax({
            url: "/Home/GetReservations",
            type: "GET",
            success: function (response, status) {
                if (status == "success") {
                    console.log(response);
                    var table = $('#kt_datatable').DataTable();
                    response.map(element => {
                        var obj = {
                            "ReservationId": element["reservationId"],
                            "ReservationUI": element["reservationUI"],
                            "Department": element["department"],
                            "TransportDate": element["transportDate"].substr(0, 10),
                            "CostCenterNumber": element["costCenterNumber"],
                            "MRN": element["mrn"],
                            "PatientFirstName": element["patientFirstName"],
                            "PatientLastName": element["patientLastName"],
                            "PickupAddress": element["pickupAddress"],
                            "PickupCity": element["pickupCity"],
                            "ContactPhone": element["contactPhone"],
                            "DestinationAddress": element["destinationAddress"],
                            "DestinationCity": element["destinationCity"],
                            "OfficePhone": element["officePhone"],
                            "TransportType": element["transportType"],
                            "PickupTime": element["pickupTime"].substr(11, element["pickupTime"].length),
                            "AppointmentTime": element["appointmentTime"].substr(11, element["appointmentTime"].length),
                            "ReturnTime": element["returnTime"].substr(11, element["returnTime"].length),
                            "Comments": element["comments"],
                            "RequestedBy": element["requestedBy"],
                            "CallBackNumber": element["callBackNumber"],
                            "CreatedOn": element["createdOn"].substr(0, 10),
                            //"Mileage": parseFloat(element["mileage"]).toFixed(2) + "KM"
                        }
                        table.row.add(obj).draw();
                    })
                    table.responsive.recalc()
                }
            }
        })
	};

	return {
		//main function to initiate the module
		init: function() {
			initTable1();
		}
	};
}();

jQuery(document).ready(function() {
	KTDatatablesExtensionsResponsive.init();
});
