function PDF(id) {
    document.getElementById(id).setAttribute("style", "padding : 15px 20px;");
    printJS({
        printable: id,
        type: 'html',
        css: ['../../css/main.min.css', '../../lib/fontawesome-pro-6.1.1-web/css/all.min.css', '../../css/pages/full-information.css', '../../css/pages/print.css'],
        targetStyles: ['*'],
        scanStyles: false
    })
    document.getElementById(id).setAttribute("style", "");
}