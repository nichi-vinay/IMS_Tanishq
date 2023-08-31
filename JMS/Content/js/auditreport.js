function validateAuditDataForm() {
    var fileInput =
        document.getElementById('auditFile');

    var filePath = fileInput.value;

    var allowedExtensions =
        /(\.csv)$/i;

    if (!allowedExtensions.exec(filePath)) {
        alert('Invalid file type');
        fileInput.value = '';
        return false;
    }
   
}

