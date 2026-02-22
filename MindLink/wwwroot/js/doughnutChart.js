window.updateDoughnutChart = (labels, values, title) => {
    if (!window.myDoughnutInstance) return;

    window.myDoughnutInstance.data.labels = labels;
    window.myDoughnutInstance.data.datasets[0].data = values;

    if (title && window.myDoughnutInstance.options.plugins?.title) {
        window.myDoughnutInstance.options.plugins.title.text = title;
    }

    window.myDoughnutInstance.update();
};

window.renderDoughnutChart = (labels, values, title) => {
    const ctx = document.getElementById('doughnutChart');
    if (window.myDoughnutInstance) {
        window.myDoughnutInstance.destroy();
    }

    window.myDoughnutInstance = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: ["#ff9696", "#ffd296", "#9df2a3"],
                hoverOffset: 4
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { position: 'top' },
                title: { display: true, text: title || 'Брояч на настроенията', font: { size: 28 } }
            }
        }
    });
};
