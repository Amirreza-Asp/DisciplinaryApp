const sidebarLinks = document.querySelectorAll(".sidebar__link");
const hostname = window.location.hostname;

function CurrentShower() {
    const href = GetCurrent();
    if (!href || window.location.pathname === "/") {
        sidebarLinks[0].classList.add("active");
        return;
    }

    Array.from(sidebarLinks).forEach((link) => {
        link.classList.remove("active");
        if (link.getAttribute("href") === href)
            link.classList.add("active");
    });
}

Array.from(sidebarLinks).forEach((link) => {
    link.addEventListener("click", (event) => {
        SetCurrent(link.getAttribute("href"));
    })
})

function SetCurrent(current) {
    localStorage.setItem("seleted", current);
}

function GetCurrent() {
    return localStorage.getItem("seleted");
}


CurrentShower();