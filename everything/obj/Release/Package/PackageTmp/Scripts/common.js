function getVerificationTokenName() {
	return "__RequestVerificationToken";
}

function getVerificationToken(formName) {
	var _formName = "#" + formName; 
	var result = "";
	$(_formName).children().each(		
		function () {			
			var child = $(this);			
			if (child.attr("name") == getVerificationTokenName()) {
				if (result == "") result = child.attr("value");
			}
		}
	);
	return result;
}
