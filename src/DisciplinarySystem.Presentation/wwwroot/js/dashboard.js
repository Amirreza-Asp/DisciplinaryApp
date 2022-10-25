/************** Chart Attribute ****************/

// complaints in last four years line chart
async function fetchLastFourYearsComplaints() {
    let data = "/Home/GetComplaintsInLastFourYears";
    let res = await fetch(data);
    res = await res.json()
    return res;
}

(async function () {
    let data = await fetchLastFourYearsComplaints();
    console.log(data)
    data = data.complaints
    let xValues = []
    for (let i = data.length - 1; i >= 0; i--)
        xValues.push(data[i].year);


    let yValues = []
    for (let i = data.length - 1; i >= 0; i--)
        yValues.push(data[i].complaintsNumber)

    let colors = []
    for (let i = 0; i < yValues.length; i++) {
        colors.push(RandomColor());
    }


    let backgroundColors = []
    let borderColors = []

    colors.forEach((color) => {
        backgroundColors.push(color + "4D");
        borderColors.push(color);
    })

    const info = {
        labels: xValues,
        datasets: [{
            label: 'تعداد شکایات در هر سال',
            data: yValues,
            backgroundColor: backgroundColors
            , borderColor: borderColors,
            borderWidth: 1
        }]
    };
    const config = {
        type: 'line',
        data: info,
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        },
    };

    const myChart = new Chart(
        document.getElementById('complaints-in-year'),
        config
    );
})()

// complaints in last 12 months bar chart
async function fetchLast12MonthComplaints() {
    let data = "/Home/GetComplaintsInLast12Months";
    let res = await fetch(data);
    res = await res.json()
    return res;
}

(async function () {
    let data = await fetchLast12MonthComplaints();

    let xValues = []
    for (let i = data.length - 1; i >= 0; i--)
        xValues.push(data[i].month);


    let yValues = []
    for (let i = data.length - 1; i >= 0; i--)
        yValues.push(data[i].complaintsNumber)

    let colors = []
    for (let i = 0; i < yValues.length; i++) {
        colors.push(RandomColor());
    }


    let backgroundColors = []
    let borderColors = []

    colors.forEach((color) => {
        backgroundColors.push(color + "4D");
        borderColors.push(color);
    })

    const info = {
        labels: xValues,
        datasets: [{
            label: 'تعداد شکایات در 12 ماه گذشته',
            data: yValues,
            backgroundColor: backgroundColors
            , borderColor: borderColors,
            borderWidth: 1
        }]
    };
    const config = {
        type: 'line',
        data: info,
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        },
    };

    const myChart = new Chart(
        document.getElementById('complaints-in-months'),
        config
    );
})()

// info polarArea chart
async function fetchInfo() {
    let data = "/Home/GetDisciplinaryInfo";
    let res = await fetch(data);
    res = await res.json()
    return res;
}

(async function () {
    let data = await fetchInfo();
    let xValues = []
    let yValues = []

    console.log(data)

    xValues.push("شکایات")
    yValues.push(data.complaints)

    xValues.push("پرونده ها")
    yValues.push(data.cases)

    xValues.push("نامه ها")
    yValues.push(data.epistles)

    let colors = []
    for (let i = 0; i < yValues.length; i++) {
        colors.push(RandomColor());
    }


    let backgroundColors = []
    let borderColors = []

    colors.forEach((color) => {
        backgroundColors.push(color + "4D");
        borderColors.push(color);
    })

    const info = {
        labels: xValues,
        datasets: [{
            label: 'اطلاعات سیستم',
            data: yValues,
            backgroundColor: backgroundColors
            , borderColor: borderColors,
            borderWidth: 1
        }]
    };
    const config = {
        type: 'polarArea',
        data: info,
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        },
    };

    const myChart = new Chart(
        document.getElementById('info'),
        config
    );
})()


// verdict polarArea chart
async function fetchVerdicts() {
    let data = "/Home/GetVerdictsCountInUsePrimaryVotes";
    let res = await fetch(data);
    res = await res.json()
    return res;
}

(async function () {
    let data = await fetchVerdicts();
    console.log(data)
    let xValues = []
    let yValues = []

    for (let i = 0; i < data.length; i++) {
        xValues.push(data[i].vote);
        yValues.push(data[i].count);
    }

    let colors = []
    for (let i = 0; i < yValues.length; i++) {
        colors.push(RandomColor());
    }

    let backgroundColors = []
    let borderColors = []

    colors.forEach((color) => {
        backgroundColors.push(color + "4D");
        borderColors.push(color);
    })

    const info = {
        labels: xValues,
        datasets: [{
            label: 'میزان استفاده از هر حکم برای پرونده ها',
            data: yValues,
            backgroundColor: backgroundColors
            , borderColor: borderColors,
            borderWidth: 1
        }]
    };
    const config = {
        type: 'bar',
        data: info,
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        },
    };

    const myChart = new Chart(
        document.getElementById('votes'),
        config
    );
})()



// complaints result polarArea chart
async function fetchComplaintsKindsResult() {
    let data = "/Home/GettingAllKindsOfComplaintResults";
    let res = await fetch(data);
    res = await res.json()
    return res;
}

(async function () {
    let data = await fetchComplaintsKindsResult();
    let xValues = []
    let yValues = []

    for (let i = 0; i < data.length; i++) {
        xValues.push(data[i].result);
        yValues.push(data[i].count);
    }

    let colors = []
    colors.push("#11f574");
    colors.push("#F511DE");

    let backgroundColors = []
    let borderColors = []

    colors.forEach((color) => {
        backgroundColors.push(color + "4D");
        borderColors.push(color);
    })

    const info = {
        labels: xValues,
        datasets: [{
            label: 'وضعیت شکایات',
            data: yValues,
            backgroundColor: backgroundColors
            , borderColor: borderColors,
            borderWidth: 1
        }]
    };
    const config = {
        type: 'doughnut',
        data: info,
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        },
    };

    const myChart = new Chart(
        document.getElementById('comlaints-result'),
        config
    );
})()



// case vote polarArea chart
async function fetchCaseVotesPart() {
    let data = "/Home/GettingCaseVotesPart";
    let res = await fetch(data);
    res = await res.json()
    return res;
}

(async function () {
    let data = await fetchCaseVotesPart();
    console.log(data)
    let xValues = []
    let yValues = []

    for (let i = 0; i < data.length; i++) {
        xValues.push(data[i].part);
        yValues.push(data[i].count);
    }

    let colors = []
    for (let i = 0; i < data.length; i++) {
        colors.push(RandomColor());
    }

    let backgroundColors = []
    let borderColors = []

    colors.forEach((color) => {
        backgroundColors.push(color + "4D");
        borderColors.push(color);
    })

    const info = {
        labels: xValues,
        datasets: [{
            label: 'وضعیت پرونده ها',
            data: yValues,
            backgroundColor: backgroundColors
            , borderColor: borderColors,
            borderWidth: 1
        }]
    };
    const config = {
        type: 'bar',
        data: info,
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        },
    };

    const myChart = new Chart(
        document.getElementById('cases-vote'),
        config
    );
})()


function RandomColor() {
    return "#" + Math.random().toString(16).substring(2, 8);
}