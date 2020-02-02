function savePageData(){
    var browserName = window.navigator.userAgent;
    
    if(window.navigator.appName != "")
        browsername = window.navigator.appName;
    
    let requestBody = {
        'BrowserName': browserName,
        'PageName': document.title,
        'PageParams': window.location.href
    };

    fetch('https://localhost:5001/api/pagedata/', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestBody)
    }).then( (response) => { 
        console.log("page data saved ok", response)
     });;
}

savePageData();