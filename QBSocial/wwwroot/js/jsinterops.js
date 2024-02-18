function copyClipboard(oid) {
        if(navigator.clipboard) {
            navigator.clipboard.writeText(oid).then(() => {
                alert('Copied to clipboard: ' + oid)
            })
        } else {
            console.log('Browser Not compatible')
        }
    
   
}

function copyClipboard2(oid) {
    if(navigator.clipboard) {
        navigator.clipboard.writeText(oid).then(() => {
            // alert('Copied to clipboard: ' + oid)
            
        })
        return true;
    } else {
        console.log('Browser Not compatible')
        
    }
    return false; 


}

window.ScrollToBottom2 = (elementName) => {
    element = document.getElementById(elementName);
    element.scrollTop = element.scrollHeight - element.clientHeight;
}