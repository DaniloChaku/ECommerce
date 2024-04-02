﻿let dataTable;

$(document).ready(function () {
    loadTableData();
});

function loadTableData() {
    $('#categoryTable').DataTable({
        ajax: { url: '/category/getall' },
        columns: [
            { data: "name", width: "75%" },
            {
                data: "id", render: function (data) {
                    return `<span class="p-xl-1">
                        <a>
                            <i class="bi bi-pencil-square text-warning" style="font-size: 1.1rem"></i>
                        </a>
                    </span>
                    <span class="p-xl-1">
                        <a>
                            <i class="bi bi-trash text-danger" style="font-size: 1.1rem"></i>
                        </a>
                    </span>`
                }
            }
        ]
    });
}