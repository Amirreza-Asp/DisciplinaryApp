const days = document.querySelectorAll(".calender-day");

days.forEach(day => {
    day.addEventListener("contextmenu", (e) => {
       
    })
})

function addModal(day) {
    const modalContainer = document.createElement("div");
    modalContainer.classList.add("my-modal-container");

    const modalBody = document.createElement("div");
    modalBody.className = "modal-body";

    const addIcon = document.createElement("i");
    addIcon.className = "fa fa-plus me-3";

    const addBtn = document.createElement("a");
    addBtn.setAttribute("href", "/Meeting/Create");
    addBtn.className = "btn btn--add";
    addBtn.innerText = "افزودن جلسه";
    addBtn.appendChild(addIcon);

    const seeMeetingsIcon = document.createElement("i");
    seeMeetingsIcon.className = "fa fa-eye me-3";

    const seeMeetingsBtn = document.createElement("a");
    seeMeetingsBtn.className = "btn btn--show";
    seeMeetingsBtn.setAttribute("href", `/Meeting/DayMeetings/${day.children[0].innerText}`);
    seeMeetingsBtn.innerText = "مشاهده جلسات";
    seeMeetingsBtn.appendChild(seeMeetingsIcon);

    modalBody.appendChild(addBtn);
    modalBody.appendChild(seeMeetingsBtn);

    modalContainer.appendChild(modalBody)
    day.appendChild(modalContainer);
}