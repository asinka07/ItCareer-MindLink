window.updateBarChart = (labels, values, xLabel) => {
    if (!window.myBarChartInstance) return;

    window.myBarChartInstance.data.labels = labels;
    window.myBarChartInstance.data.datasets[0].data = values;

    if (window.myBarChartInstance.options.scales?.x?.title) {
        window.myBarChartInstance.options.scales.x.title.text = xLabel;
    }

    window.myBarChartInstance.update();
};

window.renderBarChart = (labels, values) => {

    const ctx = document.getElementById('barChart');

    if (window.myBarChartInstance) {
        window.myBarChartInstance.destroy();
    }

    window.myBarChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: "Брой записи",
                data: values,
                backgroundColor: "#decffc",
                borderRadius: 6
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Брой записи',
                    font: { size: 28 }
                },
                legend: { display: false }
            },
            scales: {
                x: {
                    display: true,
                    title: { display: true, text: "Ден от седмицата" }
                },
                y: {
                    beginAtZero: true,
                    display: true,
                    title: { display: true, text: "Брой записи" },
                    ticks: { stepSize: 1 }
                }
            }
        }
    });
};