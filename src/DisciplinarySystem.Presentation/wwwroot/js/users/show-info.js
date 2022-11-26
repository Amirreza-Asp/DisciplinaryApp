// auto fill fullname with national code
function FindUserInfo(input) {
	const nationalCode = input.value;
	if (nationalCode.length === 10) {
		$.ajax({
			url: `/User/GetUserInfo/${nationalCode}`,
			type: "GET",
			success: function (data) {
				if (data.exists) {
					document.getElementById("phone-number").value = data.info.phoneNumber;
				}
			}
		})
	}
}

// show user info with natioanl code
function ShowInfo() {
	const nationalCode = document.getElementById("national-code").value;
	const loadIcon = document.querySelector(".load-icon");
	loadIcon.classList.add("load");
	if (nationalCode.length !== 10) {
		toastr.error('کد ملی وارد شده اشتباه است');
		loadIcon.classList.remove("load");
	}

	$.ajax({
		url: `/User/GetUserInfo/${nationalCode}`,
		type: "GET",
		success: function (data) {
			loadIcon.classList.remove("load");
			if (!data.exists) {
				toastr.error('کد ملی وارد شده اشتباه است');
			}
			else {
				document.querySelector('#phone').value = data.info.phoneNumber;
				document.querySelector('#fullName').value = data.info.fullName;
				document.querySelector('#email').value = data.info.email;
				document.querySelector('#nationalCode').value = data.info.nationalCode;
				document.querySelector('.my-modal').classList.add("show");
			}
		}
	})
}