window.renderDoughnutChart = (labels, values) => {
    const ctx = document.getElementById('doughnutChart');

        if (window.myDoughnutInstance) window.myDoughnutInstance.destroy();

        const data = {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: ["#EF4444", "#FBBF24", "#10B981"], 
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
                    title: { display: true, text: 'Настроения за месеца' }
                }
            }
        };

        window.myDoughnutInstance = new Chart(ctx, config);

};