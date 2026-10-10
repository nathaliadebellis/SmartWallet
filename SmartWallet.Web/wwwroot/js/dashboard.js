(function () {
    const dataElement = document.getElementById('dashboardChartData');

    if (!dataElement || typeof Chart === 'undefined')
        return;

    const data = JSON.parse(dataElement.textContent);

    const currency = new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' });
    const compact = new Intl.NumberFormat('pt-BR', { notation: 'compact', maximumFractionDigits: 1 });

    const colors = {
        income: '#2a78d6',
        expenses: '#eb6834',
        text: '#52514e',
        muted: '#898781',
        grid: '#e1e0d9'
    };

    Chart.defaults.font.family = getComputedStyle(document.body).fontFamily;
    Chart.defaults.font.size = 12;
    Chart.defaults.color = colors.muted;

    // Escreve o valor na ponta de cada barra horizontal.
    const barValueLabels = {
        id: 'barValueLabels',
        afterDatasetsDraw(chart) {
            const { ctx } = chart;
            const dataset = chart.data.datasets[0];

            ctx.save();
            ctx.fillStyle = colors.text;
            ctx.font = `12px ${Chart.defaults.font.family}`;
            ctx.textBaseline = 'middle';
            ctx.textAlign = 'left';

            chart.getDatasetMeta(0).data.forEach((bar, index) => {
                ctx.fillText(currency.format(dataset.data[index]), bar.x + 8, bar.y);
            });

            ctx.restore();
        }
    };

    const valueScale = {
        beginAtZero: true,
        border: { display: false },
        grid: { color: colors.grid, drawTicks: false },
        ticks: { padding: 8, maxTicksLimit: 5, callback: value => compact.format(value) }
    };

    const categoryScale = {
        border: { display: false },
        grid: { display: false },
        ticks: { color: colors.text }
    };

    const categoryCanvas = document.getElementById('expensesByCategoryChart');

    if (categoryCanvas) {
        new Chart(categoryCanvas, {
            type: 'bar',
            data: {
                labels: data.expensesByCategory.map(c => c.label),
                datasets: [{
                    label: 'Despesas',
                    data: data.expensesByCategory.map(c => c.total),
                    backgroundColor: colors.income,
                    borderRadius: 4,
                    maxBarThickness: 20
                }]
            },
            options: {
                indexAxis: 'y',
                responsive: true,
                maintainAspectRatio: false,
                layout: { padding: { right: 96 } },
                scales: { x: valueScale, y: categoryScale },
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        callbacks: { label: context => currency.format(context.parsed.x) }
                    }
                }
            },
            plugins: [barValueLabels]
        });
    }

    const monthlyCanvas = document.getElementById('monthlySummaryChart');

    if (monthlyCanvas) {
        const series = (label, key, color) => ({
            label,
            data: data.monthlySummary.map(m => m[key]),
            backgroundColor: color,
            borderRadius: 4,
            maxBarThickness: 20,
            categoryPercentage: 0.6,
            barPercentage: 0.85
        });

        new Chart(monthlyCanvas, {
            type: 'bar',
            data: {
                labels: data.monthlySummary.map(m => m.label),
                datasets: [
                    series('Receitas', 'income', colors.income),
                    series('Despesas', 'expenses', colors.expenses)
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                interaction: { mode: 'index', intersect: false },
                scales: { x: categoryScale, y: valueScale },
                plugins: {
                    legend: {
                        position: 'top',
                        align: 'start',
                        labels: { color: colors.text, boxWidth: 10, boxHeight: 10, useBorderRadius: true, borderRadius: 2 }
                    },
                    tooltip: {
                        callbacks: {
                            label: context => `${context.dataset.label}: ${currency.format(context.parsed.y)}`
                        }
                    }
                }
            }
        });
    }
})();
