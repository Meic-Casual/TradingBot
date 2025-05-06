window.plotlyInterop = {
    renderLineChart: function (divId, labels, values, labelName) {
        const trace = {
            x: labels,
            y: values,
            type: 'scatter',
            mode: 'lines+markers',
            name: labelName
        };

        Plotly.newPlot(divId, [trace], {
            margin: { t: 30 },
            hovermode: 'x unified'
        });
    },

    renderMultiChart: function (divId, traces) {
        Plotly.newPlot(divId, traces, {
            margin: { t: 30 },
            hovermode: 'x unified'
        });
    }
};
