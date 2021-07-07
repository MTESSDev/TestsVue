/*! DataTables traduction anglaise adaptée MTESS.*/
//Définition des textes anglais pour datatables
var datatables = datatables || {};
datatables.language = datatables.language || {};
datatables.language.english = datatables.language.english || {};

datatables.language.english =
{
    "sProcessing": "Processing...",
    "sSearch": "Search&nbsp;:",
    "sLengthMenu": "Show _MENU_ entries",
    "sInfo": "Showing _START_ to _END_ of _TOTAL_ entries",
    "sInfoEmpty": "Showing 0 to 0 of 0 entries",
    "sInfoFiltered": "(filtered from _MAX_ total entries)",
    "sInfoPostFix": "",
    "sLoadingRecords": "Loading...",
    "sZeroRecords": "No matching records found",
    "sEmptyTable": "No data available in table",
    "oPaginate": {
        "sFirst": '<span class="fas fa-angle-double-left" title="First page" aria-hidden="true"></span><span class="sr-only">Go to first page.</span>',
        "sPrevious": '<span class="fas fa-angle-left" title="Previous page" aria-hidden="true"></span><span class="sr-only">Go to previous page.</span>',
        "sNext": '<span class="fas fa-angle-right" title="Next page" aria-hidden="true"></span><span class="sr-only">(ENG)Go to next page.</span>',
        "sLast": '<span class="fas fa-angle-double-right" title="Last page" aria-hidden="true"></span><span class="sr-only">Go to last page.</span>',
        "sAvantNoPage": '<span class="sr-only">Go to page </span>',
        "sApresNoPage": '<span class="sr-only">.</span>',
        "sAvantNoPageActive": '<span class="sr-only">(ENG) Page </span>',
        "sApresNoPageActive": '<span class="sr-only">. Current page.</span>',
        "sInfoPageCourante": "Page _PAGE_ of _TOTALPAGES_ shown.",
    },
    "oAria": {
        "sSortAscending": " : activate to sort column ascending",
        "sSortDescending": " : activate to sort column descending"
    },
    "select": {
        "rows": {
            _: "%d rows selected",
            0: "No row selected",
            1: "1 row selected"
        }
    }
};