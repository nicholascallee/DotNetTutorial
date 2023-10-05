$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [

            { data: "name", "width": "15%" },
            { data: "email", "width": "15%" },
            { data: "phoneNumber", "width": "15%" },
            { data: "company.name", "width": "15%" },
            { data: "", "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2">
                            <i class="bi bi-pencil-square"></i> Edit</a>
                     </div>`
                },
                "width": "25%"
            }
        ]
    });
}

//function Delete(url) {
//    const swalWithBootstrapButtons = Swal.mixin({
//        customClass: {
//            confirmButton: 'btn btn-success',
//            cancelButton: 'btn btn-danger'
//        },
//        buttonsStyling: false
//    })

//    swalWithBootstrapButtons.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonText: 'Yes, delete it!',
//        cancelButtonText: 'No, cancel!',
//        reverseButtons: true
//    }).then((result) => {
//        if (result.isConfirmed) {

//            $.ajax({
//                url: url,
//                type: 'DELETE',
//                success: function (data) {
//                    dataTable.ajax.reload();
//                    toastr.success(data.message);
//                }

//            })
//        } else if (
//            /* Read more about handling dismissals below */
//            result.dismiss === Swal.DismissReason.cancel
//        ) {
//            swalWithBootstrapButtons.fire(
//                'Cancelled',
//                'Your Product is safe :)',
//                'error'
//            )
//        }
//    })
//}