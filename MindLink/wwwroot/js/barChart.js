window.renderChart = (labels, values) => {
    const ctx = document.getElementById('myChart');

    if (window.myChartInstance) {
        window.myChartInstance.destroy();
    }

    const barColors = ["blue", "indigo", "violet", "pink"];
    const colors = values.map((v, i) => barColors[i % barColors.length]);

    window.myChartInstance = new Chart(ctx, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [{
                backgroundColor: colors,
                data: values
            }]
        },
        options: {
            plugins: {
                legend: { display: false },
                title: {
                    display: true,
                    text: "Брой записи",
                    font: { size: 16 }
                }
            }
        }
    });
};
