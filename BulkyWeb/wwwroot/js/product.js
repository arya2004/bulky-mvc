$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#myTable').DataTable({
        "ajax": {url: '/admin/product/getall'
    },
        "columns": [
        { data: 'name' },
        { data: 'position' },
        { data: 'salary' },
        { data: 'state_date' },
        { data: 'office' },
        { data: 'extn' }
    ]});

}

