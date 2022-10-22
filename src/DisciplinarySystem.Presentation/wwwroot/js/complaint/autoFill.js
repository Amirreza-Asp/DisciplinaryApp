
const nationalCodeInput = document.querySelector("#complaint-nationalCode");
const studentNumberInput = document.querySelector("#complaint-studentNumber");
const fullNameInput = document.querySelector("#complaint-fullName");
const collegeInput = document.querySelector("#complaint-college");
const fatherInput = document.querySelector("#complaint-father");
const gradeInput = document.querySelector("#complaint-grade");

nationalCodeInput.addEventListener("keyup", () => {
    if (nationalCodeInput.value.length === 10) {
      
        $.ajax({
            url: `/Complaint/GetUserInfo/${nationalCodeInput.value}`,
            type: "GET",
            success: function (info) {
                if (info.exists) {
                    studentNumberInput.value = info.data.studentNumber;
                    fullNameInput.value = info.data.fullName;
                    collegeInput.value = info.data.college;
                    fatherInput.value = info.data.father;
                    gradeInput.value = info.data.grade;
                }
            }
        })
    }
}) 
    