window.renderDoughnutChart = (labels, values) => {
    const ctx = document.getElementById('doughnutChart');

        if (window.myDoughnutInstance) window.myDoughnutInstance.destroy();

        const data = {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: ["#ff9696", "#ffd296", "#9df2a3"], 
                hoverOffset: 4
            }]
        };

        const config = {
            type: 'doughnut',
            data: data,
            options: {
                responsive: true,
                plugins: {
                    legend: { position: 'top' },
                    title: { display: true, text: 'Настроения за месеца', font: { size: 28 } }
                }
            }
        };

        window.myDoughnutInstance = new Chart(ctx, config);

};