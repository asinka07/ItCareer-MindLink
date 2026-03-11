window.updateDoughnutChart = (labels, values, title) => {
    if (!window.myDoughnutInstance) return;

    window.myDoughnutInstance.data.labels = labels;
    window.myDoughnutInstance.data.datasets[0].data = values;

    // Обнови total в центъра
    const total = values.reduce((a, b) => a + b, 0);
    window.myDoughnutInstance.options.plugins.centerText = { total };

    if (title && window.myDoughnutInstance.options.plugins?.title) {
        window.myDoughnutInstance.options.plugins.title.text = title;
    }

    window.myDoughnutInstance.update();
};

window.renderDoughnutChart = (labels, values) => {
    const ctx = document.getElementById('doughnutChart');
    if (window.myDoughnutInstance) {
        window.myDoughnutInstance.destroy();
    }

    const total = values.reduce((a, b) => a + b, 0);
    const colors = ["#9df2a3", "#ffd296", "#ff9696"]; // позитивно, неутрално, негативно

    // Plugin за текст в средата
    const centerTextPlugin = {
        id: 'centerTextPlugin',
        afterDraw(chart) {
            const { ctx, chartArea: { top, bottom, left, right } } = chart;
            const centerX = (left + right) / 2;
            const centerY = (top + bottom) / 2;

            ctx.save();
            ctx.font = 'bold 28px Arial';
            ctx.fillStyle = '#333';
            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';
            ctx.fillText(chart.options.plugins.centerText.total, centerX, centerY - 10);

            ctx.font = '14px Arial';
            ctx.fillStyle = '#888';
            ctx.fillText('записа', centerX, centerY + 18);
            ctx.restore();
        }
    };

    window.myDoughnutInstance = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: colors,
                hoverOffset: 4
            }]
        },
        options: {
            responsive: true,
            cutout: '60%',
            plugins: {
                centerText: { total },
                legend: { display: false },
                title: {
                    display: true,
                    text: 'Брояч на настроенията',  // ← същото като другите
                    font: { size: 28 }
                }
            }
        },
        plugins: [centerTextPlugin]
    });

    // Рендираме легендата ръчно отдолу
    renderDoughnutLegend(labels, values, colors);
};

function renderDoughnutLegend(labels, values, colors) {
    const existing = document.getElementById('doughnutLegend');
    if (existing) existing.remove();

    const legend = document.createElement('div');
    legend.id = 'doughnutLegend';
    legend.style.cssText = 'display:flex; flex-direction:column; gap:8px; margin-top:20px; width:65%; box-shadow: 3px 3px 20px 2px rgba(0, 0, 0, 0.1); padding: 12px; border-radius: 12px; border: 1px solid rgba(0, 0, 0, 0.1); ';

    labels.forEach((label, i) => {
        const item = document.createElement('div');
        item.style.cssText = 'display:flex; align-items:center; justify-content:space-between; font-size:14px;';

        item.innerHTML = `
            <div style="display:flex; align-items:center; gap:8px;">
                <span style="display:inline-block; width:12px; height:12px; border-radius:50%; background:${colors[i]};"></span>
                <span>${label}</span>
            </div>
            <span style="font-weight:bold;">${values[i]}</span>
        `;
        legend.appendChild(item);
    });

    const wrapper = document.getElementById('doughnutWrapper');
    wrapper.appendChild(legend);
}