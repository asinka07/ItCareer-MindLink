window.updateLineChart = (labels, values, xLabel) => {
    if (!window.myLineChartInstance) return;

    window.myLineChartInstance.data.labels = labels;
    window.myLineChartInstance.data.datasets[0].data = values;

    window.myLineChartInstance.options.scales.x.title.text = xLabel;

    window.myLineChartInstance.update();
};

window.renderLineChart = (labels, values) => {

    const ctx = document.getElementById('lineChart');

    if (window.myLineChartInstance) {
        window.myLineChartInstance.destroy();
    }

    window.myLineChartInstance = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: "Средно настроение",
                data: values,
                borderColor: "#8bc9c9",
                backgroundColor: "rgba(189, 255, 254,0.2)",
                fill: true,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Как се променя настроението ти',
                    font: { size: 28 }
                }
            },
            interaction: { intersect: false },
            scales: {
                x: {
                    display: true,
                    title: { display: true, text: "Ден от месеца" }
                },
                y: {
                    beginAtZero: true,
                    min: 5,
                    max: 1,
                    display: true,
                    title: { display: true, text: "Настроение (1-5)" },
                    ticks: { stepSize: 1 }
                }
            }
        }
    });
};