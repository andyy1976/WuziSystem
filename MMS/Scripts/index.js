$(function () {
    $('#demo1').lightTreeview();
    $('#demo2').lightTreeview({
        collapse: true,
        line: true,
        nodeEvent: false,
        unique: true,
        style: 'red',
        animate: 0
    });
    $('#demo3').lightTreeview({
        collapse: true,
        line: true,
        nodeEvent: true,
        unique: true,
        style: 'black',
        animate: 400
    });
    $('#demo4').lightTreeview({
        collapse: true,
        line: true,
        nodeEvent: true,
        unique: false,
        style: 'gray',
        animate: 100,
        fileico: true,
        folderico: true
    });
})