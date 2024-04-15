let dataTable;

document.addEventListener("DOMContentLoaded", loadTableData);

function loadTableData() {
    dataTable = $('#productsTable').DataTable({
        ajax: { url: '/product/getall' },
        columns: [
            { data: "name", width: "10%" },
            { data: "description", with: "30%"},
            { data: "price", with: "5%"},
            { data: "salePrice", with: "5%"},
            {
                data: "imageUrl",
                render: url => url === null ? "-" : "+"      
            },
            { data: "stock", with: "10%"},
            { data: "categoryName", with: "10%"},
            { data: "manufacturerName", with: "10%"},
            {
                data: "id", render: function (id) {
                    return `<span class="p-1 d-flex align-content-center gap-2">
                        <a href="/product/upsert?id=${id}" class="text-decoration-none">
                            <i class="bi bi-pencil-square text-warning" style="font-size: 1.1rem"></i>
                        </a>
                        <button class="btn btn-link p-0 m-0" onclick="removeItem('/product/delete?id=${id}', dataTable)">
                            <i class="bi bi-trash text-danger" style="font-size: 1.1rem"></i>
                        </button>
                    </span>`
                }
            }
        ],
        autoWidth: false
    });
}