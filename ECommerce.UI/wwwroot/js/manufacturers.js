﻿let dataTable;

document.addEventListener("DOMContentLoaded", loadTableData);

function loadTableData() {
    dataTable = $('#manufacturersTable').DataTable({
        ajax: { url: '/manufacturer/getall' },
        columns: [
            { data: "name", width: "20%" },
            { data: "description", width: "70%" },
            {
                data: "id", render: function (data) {
                    return `<span class="p-1 d-flex align-content-center gap-2">
                        <a href="/manufacturer/upsert?id=${data}" class="text-decoration-none">
                            <i class="bi bi-pencil-square text-warning" style="font-size: 1.1rem"></i>
                        </a>
                        <button class="btn btn-link p-0 m-0" onclick="removeItemWithAssociation(
                            '/product/hasAssociation?type=manufacturer&id=${data}',
                        '/product/removeAssociation?type=manufacturer&id=${data}', 
                        '/manufacturer/delete?id=${data}', 'manufacturer', dataTable)">
                            <i class="bi bi-trash text-danger" style="font-size: 1.1rem"></i>
                        </button>
                    </span>`
                }
            }
        ],
        autoWidth: false
    });
}
