$(function () {
    $("#example1").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

});


$(function () {
    $("#example2").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example2_wrapper .col-md-6:eq(0)');

});


$(function () {
    $("#example3").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example3_wrapper .col-md-6:eq(0)');

});