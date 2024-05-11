/**
 * Represents the DataTable object used to display categories' data.
 * @type {DataTable}
 */
let dataTable;

/**
 * Event listener that loads table data when the DOM content is loaded.
 */
document.addEventListener("DOMContentLoaded", loadTableData);

/**
 * Loads data into the DataTable.
 */
function loadTableData() {
    // Initialize the DataTable
    dataTable = $('#categoriesTable').DataTable({
        ajax: { url: '/admin/categories/getall' }, // URL to fetch data from
        columns: [
            { data: "name", width: "75%" },
            {
                data: "id", render: function (data) {
                    return `<span class="p-1 d-flex align-content-center gap-2">
                        <a href="/admin/categories/upsert?id=${data}" class="text-decoration-none">
                            <i class="bi bi-pencil-square text-warning" style="font-size: 1.1rem"></i>
                        </a>
                        <button class="btn btn-link p-0 m-0" onclick="removeItemWithAssociation(
                            '/admin/products/hasReference?type=category&id=${data}',
                        '/admin/products/removeReference?type=category&id=${data}', 
                        '/admin/categories/delete?id=${data}', 'category', dataTable)">
                            <i class="bi bi-trash text-danger" style="font-size: 1.1rem"></i>
                        </button>
                    </span>`
                }
            }
        ],
        autoWidth: false // Disable automatic column width adjustment
    });
}