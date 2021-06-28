// Class definition
var KTFormControls = function () {
	// Private functions
	var _initDemo1 = function () {
		FormValidation.formValidation(
			document.getElementById('kt_form_1'),
			{
				fields: {
					Department: {
						validators: {
							notEmpty: {
								message: 'Department is required'
							},						
						}
					},

					PatientFirstName: {
						validators: {
							notEmpty: {
								message: 'First Name is required'
							},
						}
					},

					PatientLastName: {
						validators: {
							notEmpty: {
								message: 'Last Name is required'
							},						
						}
					},

					CostCenterNumber: {
						validators: {
							notEmpty: {
								message: 'Center Number is required'
							},
						}
					},
					MRN: {
						validators: {
							notEmpty: {
								message: 'MRN is required'
							},
						}
					},
					PickupAddress: {
						validators: {
							notEmpty: {
								message: 'Pickup Address is required'
							},
						}
					},
					PickupCity: {
						validators: {
							notEmpty: {
								message: 'Pickup City is required'
							},
						}
					},
					PickupZip: {
						validators: {
							notEmpty: {
								message: 'Pickup Zip is required'
							},
						}
					},
					DestinationAddress: {
						validators: {
							notEmpty: {
								message: 'Destination Address is required'
							},
						}
					},
					DestinationCity: {
						validators: {
							notEmpty: {
								message: 'Destination City is required'
							},
						}
					},
					DestinationZip: {
						validators: {
							notEmpty: {
								message: 'Destination Zip is required'
							},
						}
					},
					Comments: {
						validators: {
							notEmpty: {
								message: 'Comments is required'
							},
						}
					},
					RequestedBy: {
						validators: {
							notEmpty: {
								message: 'Requested By City is required'
							},
						}
					},
				
				

					ContactPhone: {
						validators: {
							notEmpty: {
								message: 'Contact Phone is required'
							},
							phone: {
								country: 'US',
								message: 'The value is not a valid US phone number'
							}
						}
					},
					OfficePhone: {
						validators: {
							notEmpty: {
								message: 'Office Phone is required'
							},
							phone: {
								country: 'US',
								message: 'The value is not a valid US phone number'
							}
						}
					},
					CallBackNumber: {
						validators: {
							notEmpty: {
								message: 'CallBack Number is required'
							},
							phone: {
								country: 'US',
								message: 'The value is not a valid US phone number'
							}
						}
					},

					TransportType: {
						validators: {
							notEmpty: {
								message: 'Please select an option'
							}
						}
					},

					options: {
						validators: {
							choice: {
								min:2,
								max:5,
								message: 'Please select at least 2 and maximum 5 options'
							}
						}
					},

					memo: {
						validators: {
							notEmpty: {
								message: 'Please enter memo text'
							},
							stringLength: {
								min:50,
								max:100,
								message: 'Please enter a menu within text length range 50 and 100'
							}
						}
					},

					checkbox: {
						validators: {
							choice: {
								min:1,
								message: 'Please kindly check this'
							}
						}
					},

					checkboxes: {
						validators: {
							choice: {
								min:2,
								max:5,
								message: 'Please check at least 1 and maximum 2 options'
							}
						}
					},

					radios: {
						validators: {
							choice: {
								min:1,
								message: 'Please kindly check this'
							}
						}
					},
				},

				plugins: { //Learn more: https://formvalidation.io/guide/plugins
					trigger: new FormValidation.plugins.Trigger(),
					// Bootstrap Framework Integration
					bootstrap: new FormValidation.plugins.Bootstrap(),
					// Validate fields when clicking the Submit button
					submitButton: new FormValidation.plugins.SubmitButton(),
            		// Submit the form when all fields are valid
            		defaultSubmit: new FormValidation.plugins.DefaultSubmit(),
				}
			}
		);
	}

	var _initDemo2 = function () {
		FormValidation.formValidation(
			document.getElementById('kt_form_2'),
			{
				fields: {
					billing_card_name: {
						validators: {
							notEmpty: {
								message: 'Card Holder Name is required'
							}
						}
					},
					billing_card_number: {
						validators: {
							notEmpty: {
								message: 'Credit card number is required'
							},
							creditCard: {
								message: 'The credit card number is not valid'
							}
						}
					},
					billing_card_exp_month: {
						validators: {
							notEmpty: {
								message: 'Expiry Month is required'
							}
						}
					},
					billing_card_exp_year: {
						validators: {
							notEmpty: {
								message: 'Expiry Year is required'
							}
						}
					},
					billing_card_cvv: {
						validators: {
							notEmpty: {
								message: 'CVV is required'
							},
							digits: {
								message: 'The CVV velue is not a valid digits'
							}
						}
					},

					billing_address_1: {
						validators: {
							notEmpty: {
								message: 'Address 1 is required'
							}
						}
					},
					billing_city: {
						validators: {
							notEmpty: {
								message: 'City 1 is required'
							}
						}
					},
					billing_state: {
						validators: {
							notEmpty: {
								message: 'State 1 is required'
							}
						}
					},
					billing_zip: {
						validators: {
							notEmpty: {
								message: 'Zip Code is required'
							},
							zipCode: {
								country: 'US',
								message: 'The Zip Code value is invalid'
							}
						}
					},

					billing_delivery: {
						validators: {
							choice: {
								min:1,
								message: 'Please kindly select delivery type'
							}
						}
					},
					package: {
						validators: {
							choice: {
								min:1,
								message: 'Please kindly select package type'
							}
						}
					}
				},

				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					// Validate fields when clicking the Submit button
					submitButton: new FormValidation.plugins.SubmitButton(),
            		// Submit the form when all fields are valid
            		defaultSubmit: new FormValidation.plugins.DefaultSubmit(),
					// Bootstrap Framework Integration
					bootstrap: new FormValidation.plugins.Bootstrap({
						eleInvalidClass: '',
						eleValidClass: '',
					})
				}
			}
		);
	}

	return {
		// public functions
		init: function() {
			_initDemo1();
			_initDemo2();
		}
	};
}();

jQuery(document).ready(function() {
	KTFormControls.init();
});
