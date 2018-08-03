window.onload = function () {

    var accessToken = document.getElementById('accessToken').innerText;

    if (!accessToken || accessToken === "") {
        return;
    }

    var embedUrl = document.getElementById('embedUrl').innerText;
    var reportId = document.getElementById('reportId').innerText;

    var container = document.getElementById("container");
    var config = {
        type: 'report',
        accessToken: accessToken,
        embedUrl: embedUrl,
        id: reportId,
        settings: {
            filterPaneEnabled: false,
            navContentPaneEnabled: false
        }
    };
    powerbi.embed(container, config);
    var element = powerbi.get(container);


    setInterval(function () {
        element.reload();
    }, 15 * 1000);
};