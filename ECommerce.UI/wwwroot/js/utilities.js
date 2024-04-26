function removeItem(url, dataTable) {
    showInitialMessage().then((result) => {
        if (result.isConfirmed) {
            removeEntity(url, dataTable);
        }
    })
}

function removeItemWithAssociation(hasAssociationsUrl, removeAssociactionUrl, deleteUrl, entityName, dataTable) {
    showInitialMessage().then((result) => {
        if (result.isConfirmed) {
            fetch(hasAssociationsUrl, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        if (data.hasAssociations) {
                            removeAssociation(removeAssociactionUrl, deleteUrl, entityName, dataTable)
                        } else {
                            removeEntity(deleteUrl, dataTable);
                        }
                    } else {
                        toastr.error(data.message);
                    }
                })
                .catch(error => {
                    toastr.error(error.message);
                });
        }
    });
}


function removeEntity(url, dataTable = null) {
    fetch(url, {
        method: 'DELETE'
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                if (dataTable !== undefined && dataTable !== null) {
                    dataTable.ajax.reload();
                } else {
                    window.location.reload();
                }
                toastr.success(data.message);
            } else {
                toastr.error(data.message);
            }
        })
        .catch(error => {
            toastr.error(error.message);
        });
}

function removeAssociation(removeAssociactionUrl, deleteUrl, entityName, dataTable) {
    Swal.fire({
        title: 'Associated entities found!',
        text: `The ${entityName} is used by some products. Do you still want to remove it?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, remove associations!',
        cancelButtonText: 'No, cancel!'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(removeAssociactionUrl, {
                method: 'DELETE'
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        removeEntity(deleteUrl, dataTable);
                    } else {
                        toastr.error(data.message);
                    }
                })
                .catch(error => {
                    toastr.error(error.message);
                });
        }
    });
}

function showInitialMessage() {
    return Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    });
}