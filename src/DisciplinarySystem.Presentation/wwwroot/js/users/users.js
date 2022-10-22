// validation for startDate and endDate
const startDate = document.getElementById("start-date");
const endDate = document.getElementById("end-date");
document.getElementById("submit-btn").addEventListener("click", (e) => {
    if (!startDate.value) {
        document.getElementById("start-date-validation").setAttribute("style", "visibility:visible");
        e.preventDefault();
    }
    if (!endDate.value) {
        document.getElementById("end-date-validation").setAttribute("style", "visibility:visible");
        e.preventDefault();
    }
})

// fix btn find position
const spanFullName = document.getElementById("full-valid");
const spanNatioalCode = document.getElementById("national-valid");
const btnContainer = document.getElementById("btn-container");
if (spanFullName.innerHTML || spanNatioalCode.innerHTML) {
    btnContainer.classList.remove("align-self-end");
    btnContainer.classList.add("align-self-center");
    btnContainer.classList.add("mt-2");
}
