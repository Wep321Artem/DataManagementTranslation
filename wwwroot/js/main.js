$(document).ready(function () {
	$("#PhoneInput").inputmask("+79999999999");
	$('#Birthday').inputmask("datetime", {
		inputFormat: "dd.mm.yyyy",
		clearIncomplete: true,
		min: window.minDate,
		max: window.maxDate
	});
	$("#Email").inputmask({ alias: "email" });
	$("#CardCode, #Bonus, #Turnover, #Pincode").inputmask({ regex: "^[0-9]+$" });
	$("#FirstName, #LastName, #SurName, #City").inputmask({ regex: "^[А-Яа-яЁёA-Za-z]+$" });
});