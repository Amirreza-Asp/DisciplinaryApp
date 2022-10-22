function IncHours(inputId, hoursId) {
	const input = document.getElementById(inputId);
	let hours = document.getElementById(hoursId);

	let num = Number.parseInt(hours.innerHTML);
	if (num == 23) num = 0;
	else num++;

	if (num < 10) num = `0${num}`;

	const value = input.value;
	timesSpilt = value.split(":");

	hours.innerHTML = num;
	input.value = num + ":" + timesSpilt[1];
}

function DecHours(inputId, hoursId) {
	const input = document.getElementById(inputId);
	let hours = document.getElementById(hoursId);

	let num = Number.parseInt(hours.innerHTML);
	if (num == 0) num = 23;
	else num--;

	if (num < 10) num = `0${num}`;

	const value = input.value;
	timesSpilt = value.split(":");

	hours.innerHTML = num;
	input.value = num + ":" + timesSpilt[1];
}

function IncMin(inputId, minId) {
	const input = document.getElementById(inputId);
	let mins = document.getElementById(minId);

	let num = Number.parseInt(mins.innerHTML);
	if (num == 59) num = 0;
	else num++;

	if (num < 10) num = `0${num}`;

	const value = input.value;
	timesSpilt = value.split(":");

	mins.innerHTML = num;
	input.value = timesSpilt[0] + ":" + num;
}

function DecMin(inputId, minId) {
	const input = document.getElementById(inputId);
	let mins = document.getElementById(minId);

	let num = Number.parseInt(mins.innerHTML);
	if (num == 0) num = 59;
	else num--;

	if (num < 10) num = `0${num}`;

	const value = input.value;
	timesSpilt = value.split(":");

	mins.innerHTML = num;
	input.value = timesSpilt[0] + ":" + num;
}

function TogglePicker(id) {
	console.log(document.getElementById(id));
	document.getElementById(id).classList.toggle("hide-picker");
}



function ShowCalender(selector) {
	const calender = document.querySelector(selector);
	calender.classList.toggle('hide');
}

function SubmitClick(event) {

	const userInput = document.querySelector('#invited-users');
	const users = document.querySelector(".guests");
	userInput.value = "";
	Array.from(users.children).forEach(item => {
		userInput.value += "\t" + item.getAttribute("id");
	})
	userInput.value = userInput.value.trim();
}

function SetUser(select) {
	var value = select.value;
	var text = select.options[select.selectedIndex].text;
	select.options[select.selectedIndex].remove();


	addPerson(select, text, value);

}

function addPerson(select, name, id) {
	const parent = document.querySelector(".guests")
	const fullName = name;
	const element = document.createElement("span")
	const elementParent = document.createElement("span")
	const icon = document.createElement("i")

	elementParent.addEventListener("click", (e) => {

		const option = document.createElement("option");
		option.innerHTML = name;
		option.value = id;
		select.appendChild(option);

		e.currentTarget.remove()
	})

	elementParent.classList.add("badge", "bg-primary", "guest")
	elementParent.setAttribute("id", id);
	icon.classList.add("fa", "fa-times", "ms-2")
	element.textContent = fullName

	elementParent.appendChild(element)
	elementParent.insertBefore(icon, element)
	parent.appendChild(elementParent)
}