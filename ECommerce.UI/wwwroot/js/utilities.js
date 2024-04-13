function removeItem(url, dataTable) {
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
            fetch(url, { 
                method: 'DELETE'
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                } else {
                    toastr.error(data.message);
                }
                
            })
            .catch(error => {
                toastr.error(error.message);
            });
        }
    })
}