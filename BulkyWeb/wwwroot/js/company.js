var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#myTable').DataTable({
        "ajax": {url: '/admin/company/getall'
    },
        "columns": [
        { data: 'name' },
        { data: 'streetAddress' },
        { data: 'city' },
           { data: 'state' },
            { data: 'phoneNumber' },
        {
            data: 'id',
            "render": function (data) {
                return `<div class="btn-group" role="group">
                <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2">Edit</a>
                <a onClick=Delete('/admin/company/delete/${data}') class="btn btn-danger mx-2">Delete</a>
                </div>`
            }
            }

    ]});

}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}
