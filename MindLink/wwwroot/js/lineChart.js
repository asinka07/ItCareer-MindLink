window.renderLineChart = (labels, values) => {
        const ctx = document.getElementById('lineChart');

    if (window.myLineChartInstance) window.myLineChartInstance.destroy();

    const data = {
        labels: labels,
        datasets: [{
            label: "Средно настроение",
            data: values,
            borderColor: "#8bc9c9",
            backgroundColor: "rgba(189, 255, 254,0.2)",
            fill: true,
            tension: 0.4
        }]
    };

    const config = {
        type: 'line',
        data: data,
        options: {
            responsive: true,
            plugins: {
                title: { display: true, text: 'Как се променя настроението ти', font: { size: 28 } }
            },
            interaction: { intersect: false },
            scales: {
                x: { display: true, title: { display: true, text: "Ден" } },
                y: {
                    beginAtZero: true,
                    min: 1,
                    max: 5,
                    display: true,
                    title: { display: true, text: "Настроение (1-5)" },
                    ticks: { stepSize: 1 }
                }
            }
        }
    };

    window.myLineChartInstance = new Chart(ctx, config);

};
