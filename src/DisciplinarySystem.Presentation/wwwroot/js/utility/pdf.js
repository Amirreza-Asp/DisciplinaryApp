function PDF(id) {
    document.getElementById(id).setAttribute("style","padding : 15px 20px;");
    printJS({
        printable: id,
        type: 'html',
        css: ['../../css/main.min.css',
            '../../lib/fontawesome-pro-6.1.1-web/css/all.css',
            '../../css/pages/dashboard.css'],
        targetStyles: ['*'],
        scanStyles: false
    })
}