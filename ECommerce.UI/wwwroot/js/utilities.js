/**
 * Removes an entity after displaying an initial confirmation message.
 * @param {string} url - The URL to send the DELETE request to.
 * @param {DataTable} [dataTable=null] - Optional DataTable object to reload after removal.
 */
function removeItem(url, dataTable) {
    showInitialMessage().then((result) => {
        if (result.isConfirmed) {
            removeEntity(url, dataTable);
        }
    })
}

/**
 * Removes an entity after checking for associations with other entities.
 * @param {string} hasAssociationsUrl - The URL to check for associations.
 * @param {string} removeAssociactionUrl - The URL to remove associations.
 * @param {string} deleteUrl - The URL to delete the entity.
 * @param {string} entityName - The name of the entity being removed.
 * @param {DataTable} dataTable - The DataTable object to reload after removal.
 */
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

/**
 * Removes an entity from the database using the provided URL.
 * @param {string} url - The URL to send the DELETE request to.
 * @param {DataTable} [dataTable=null] - Optional DataTable object to reload after removal.
 */
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

/**
 * Removes associations between entities before deleting the main entity.
 * @param {string} removeAssociactionUrl - The URL to remove associations.
 * @param {string} deleteUrl - The URL to delete the main entity.
 * @param {string} entityName - The name of the entity being removed.
 * @param {DataTable} dataTable - The DataTable object to reload after removal.
 */
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

/**
 * Shows the initial confirmation message before performing an action.
 * @returns {Promise} A promise representing the result of the confirmation dialog.
 */
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