window.updateLineChart = (labels, values, xLabel) => {
    if (!window.myLineChartInstance) return;

    window.myLineChartInstance.data.labels = labels;
    window.myLineChartInstance.data.datasets[0].data = values;

    if (window.myLineChartInstance.options.scales.x.title) {
        window.myLineChartInstance.options.scales.x.title.text = xLabel;
    }

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
                tension: 0.4,
                spanGaps: false,
                pointRadius: 6,
                pointHoverRadius: 8   
            }]
        },
        options: {
            responsive: true,
            clip: false,
            plugins: {
                title: {
                    display: true,
                    text: 'Как се променя настроението ти',
                    font: { size: 28 }
                },
                legend: {
                    display: false
                }
            },
            interaction: { intersect: false },
            scales: {
                x: {
                    display: true,
                    title: { display: true, text: "Ден от седмицата" }
                },
                y: {
                    beginAtZero: true,
                    min: 1,
                    max: 3,
                    display: true,
                    title: { display: true, text: "Настроение (1-3)" },
                    ticks: { stepSize: 1 }
                }
            }
        }
    });
};