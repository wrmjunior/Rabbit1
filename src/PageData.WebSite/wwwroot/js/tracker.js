function savePageData() {
    var browserName = window.navigator.userAgent;

    if (window.navigator.appName != "")
        browsername = window.navigator.appName;

    let requestBody = {
        'Browser': browserName,
        'PageName': document.title,
        'PageParams': window.location.href
    };

    $.ajax({
        type: "POST",
        url: 'https://localhost:5004/api/pagedata/',
        contentType: 'application/json',
        type: 'POST',
        data: JSON.stringify(requestBody),
        success: function () {
            console.log("Thanks!");
        }
    })
}

savePageData();