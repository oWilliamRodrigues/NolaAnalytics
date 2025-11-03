(() => {
    let charts = {};

    const destroyChart = (key) => {
        if (charts[key]) {
            try { charts[key].destroy(); } catch { }
            delete charts[key];
        }
    };

    const formatCurrency = (v) =>
        v == null ? "-" : new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(Number(v));

    const showError = (msg) => {
        console.error(msg);
        alert(msg);
    };

    // CARREGAR DADOS
    async function loadOverview(from, to, stores = [], channels = [], startHour = 0, endHour = 23) {
        try {
            $("#overviewText").html("Carregando...");
            const params = new URLSearchParams({ from, to, stores: stores.join(","), channels: channels.join(","), startHour, endHour });
            const data = await $.getJSON(`/Analytics/OverviewData?${params}`);

            const summaryHtml = `
                <div class="row">
                    <div class="col-md-3"><strong>Faturamento:</strong> ${formatCurrency(data.totalRevenue)}</div>
                    <div class="col-md-3"><strong>Pedidos:</strong> ${data.orders}</div>
                    <div class="col-md-3"><strong>Ticket Médio:</strong> ${formatCurrency(data.avgTicket)}</div>
                    <div class="col-md-3"><strong>Entrega média (min):</strong> ${(data.avgDeliverySeconds / 60).toFixed(1)}</div>
                </div>`;
            $("#overviewText").html(summaryHtml);
        } catch (err) {
            showError("Erro ao carregar Overview: " + (err?.responseText || err));
            $("#overviewText").html("<em>Erro ao carregar</em>");
        }
    }

    async function loadTopProducts(from, to, stores = [], channels = [], startHour = 0, endHour = 23) {
        try {
            const params = new URLSearchParams({ from, to, stores: stores.join(","), channels: channels.join(","), startHour, endHour });
            const data = await $.getJSON(`/Analytics/TopProductsData?${params}`);
            const labels = data.map(x => x.productName);
            const values = data.map(x => Number(x.revenue));

            destroyChart("topProductsChart");
            const ctx = document.getElementById("topProductsChart").getContext("2d");

            charts["topProductsChart"] = new Chart(ctx, {
                type: "bar",
                data: { labels, datasets: [{ data: values, backgroundColor: "#0d6efd" }] },
                options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { display: false } } }
            });

            let html = "<table class='table table-striped mt-2'><thead><tr><th>Produto</th><th>Unidades</th><th>Receita</th></tr></thead><tbody>";
            data.forEach(p => {
                html += `<tr><td>${p.productName}</td><td>${p.units}</td><td>${formatCurrency(p.revenue)}</td></tr>`;
            });
            html += "</tbody></table>";
            $("#topProductsPartial").html(html);

        } catch (err) {
            showError("Erro ao carregar Top Produtos: " + (err?.responseText || err));
        }
    }

    async function loadDelivery(from, to, stores = [], channels = [], startHour = 0, endHour = 23) {
        try {
            const params = new URLSearchParams({ from, to, stores: stores.join(","), channels: channels.join(","), startHour, endHour });
            const data = await $.getJSON(`/Analytics/DeliveryByRegionData?${params}`);
            const pageSize = 10;
            let currentPage = 1;
            const totalPages = Math.ceil(data.length / pageSize);

            const renderPage = (page) => {
                currentPage = page;
                const start = (page - 1) * pageSize;
                const end = Math.min(start + pageSize, data.length);
                const pageData = data.slice(start, end);

                let html = "<table class='table table-striped'><thead><tr><th>Cidade</th><th>Bairro</th><th>Entregas</th><th>Tempo Médio (min)</th><th>P90 (min)</th></tr></thead><tbody>";
                pageData.forEach(r => {
                    html += `<tr>
                    <td>${r.city}</td>
                    <td>${r.neighborhood}</td>
                    <td>${r.deliveries}</td>
                    <td>${r.avgMin.toFixed(1)}</td>
                    <td>${r.p90.toFixed(1)}</td>
                </tr>`;
                });
                html += "</tbody></table>";

                const maxPagesToShow = 7;
                let startPage = Math.max(1, page - 3);
                let endPage = Math.min(totalPages, page + 3);

                html += `<nav aria-label="Delivery pagination"><ul class="pagination justify-content-center">`;
                if (page > 1) html += `<li class="page-item"><a class="page-link" href="#">«</a></li>`;
                for (let i = startPage; i <= endPage; i++) html += `<li class="page-item ${i === page ? "active" : ""}"><a class="page-link" href="#">${i}</a></li>`;
                if (page < totalPages) html += `<li class="page-item"><a class="page-link" href="#">»</a></li>`;
                html += `</ul></nav>`;

                $("#deliveryPartial").html(html);

                $("#deliveryPartial .page-link").off("click").on("click", function (e) {
                    e.preventDefault();
                    const text = $(this).text();
                    let selectedPage = currentPage;
                    if (text === "«") selectedPage = currentPage - 1;
                    else if (text === "»") selectedPage = currentPage + 1;
                    else selectedPage = parseInt(text);
                    if (selectedPage >= 1 && selectedPage <= totalPages && selectedPage !== currentPage) renderPage(selectedPage);
                });
            };

            renderPage(currentPage);

            destroyChart("deliveryChart");
            const ctx = document.getElementById("deliveryChart").getContext("2d");
            const labels = data.map(x => `${x.city || ""} ${x.neighborhood || ""}`.trim());
            const values = data.map(x => Number(x.avgMin));

            charts["deliveryChart"] = new Chart(ctx, {
                type: "line",
                data: {
                    labels,
                    datasets: [{
                        data: values,
                        borderColor: "#198754",
                        backgroundColor: "rgba(25,135,84,0.2)",
                        tension: 0.25,
                        fill: true
                    }]
                },
                options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { display: false } } }
            });

        } catch (err) {
            showError("Erro ao carregar Delivery: " + (err?.responseText || err));
            $("#deliveryPartial").html("<em>Erro ao carregar</em>");
        }
    }

    async function loadChurn(from, to, stores = [], channels = [], startHour = 0, endHour = 23) {
        try {
            const params = new URLSearchParams({ from, to, stores: stores.join(","), channels: channels.join(","), startHour, endHour });
            const data = await $.getJSON(`/Analytics/ChurnedCustomersData?${params}`);
            const pageSize = 10;
            let currentPage = 1;
            const totalPages = Math.ceil(data.length / pageSize);

            const renderPage = (page) => {
                currentPage = page;
                const start = (page - 1) * pageSize;
                const end = Math.min(start + pageSize, data.length);
                const pageData = data.slice(start, end);

                let html = "<table class='table table-striped'><thead><tr><th>Cliente</th><th>Pedidos</th><th>Último Pedido</th></tr></thead><tbody>";
                pageData.forEach(c => {
                    const lastOrder = new Date(c.lastOrder);
                    html += `<tr>
                        <td>${c.customerName}</td>
                        <td>${c.totalOrders}</td>
                        <td>${lastOrder.toLocaleDateString("pt-BR")}</td>
                    </tr>`;
                });
                html += "</tbody></table>";

                const maxPagesToShow = 7;
                let startPage = Math.max(1, page - 3);
                let endPage = Math.min(totalPages, page + 3);

                html += `<nav aria-label="Churn pagination"><ul class="pagination justify-content-center">`;
                if (page > 1) html += `<li class="page-item"><a class="page-link" href="#">«</a></li>`;
                for (let i = startPage; i <= endPage; i++) html += `<li class="page-item ${i === page ? "active" : ""}"><a class="page-link" href="#">${i}</a></li>`;
                if (page < totalPages) html += `<li class="page-item"><a class="page-link" href="#">»</a></li>`;
                html += `</ul></nav>`;

                $("#churnPartial").html(html);

                $("#churnPartial .page-link").off("click").on("click", function (e) {
                    e.preventDefault();
                    const text = $(this).text();
                    let selectedPage = page;
                    if (text === "«") selectedPage = page - 1;
                    else if (text === "»") selectedPage = page + 1;
                    else selectedPage = parseInt(text);
                    if (selectedPage !== page) renderPage(selectedPage);
                });
            };

            renderPage(currentPage);
        } catch (err) {
            showError("Erro ao carregar clientes inativos: " + (err?.responseText || err));
            $("#churnPartial").html("<em>Erro ao carregar</em>");
        }
    }

    // DASHBOARD
    async function loadDashboard() {
        const from = $("#fromDate").val();
        const to = $("#toDate").val();
        const stores = $("#storeFilter").val() || [];
        const channels = $("#channelFilter").val() || [];
        const startHour = $("#startHour").val();
        const endHour = $("#endHour").val();

        await Promise.all([
            loadOverview(from, to, stores, channels, startHour, endHour),
            loadTopProducts(from, to, stores, channels, startHour, endHour),
            loadDelivery(from, to, stores, channels, startHour, endHour),
            loadChurn(from, to, stores, channels, startHour, endHour)
        ]);
    }

    // EXPORTAÇÃO
    const exportToExcel = () => {
        const tables = ["topProductsPartial", "deliveryPartial", "churnPartial"];
        const wb = XLSX.utils.book_new();
        tables.forEach(id => {
            const table = document.querySelector(`#${id} table`);
            if (table) {
                const ws = XLSX.utils.table_to_sheet(table);
                XLSX.utils.book_append_sheet(wb, ws, id);
            }
        });
        XLSX.writeFile(wb, "Dashboard.xlsx");
    };


    $(document).ready(() => {
        const today = new Date();
        const to = today.toISOString().slice(0, 10);

        const from = "2025-04-01";


        $("#fromDate").val(from);
        $("#toDate").val(to);

        $("#fromDate, #toDate").on("change", loadDashboard);
        $("#fromDate, #toDate").on("focus click", function () { this.showPicker?.(); });

        $("#exportExcel").on("click", exportToExcel);

        loadDashboard();
    });
})();
