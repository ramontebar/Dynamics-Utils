function GetPhoto(recordId, entityType) {
    if (recordId == null || entityType == null)
        return;

    Xrm.Device.captureImage().then(
        function (imageContent) {
            if (imageContent != null) {
                Xrm.WebApi.updateRecord(entityType, recordId, { "entityimage": imageContent.fileContent }).then(
                    function (result) {
                        Xrm.Navigation.openAlertDialog({ "text": "Done! Form is going to be refreshed" }).then(
                            function () {
                                Xrm.Navigation.openForm({ "entityId": recordId, "entityName": entityType, "openInNewWindow": false })
                            }
                        );
                    }
                    ,
                    function (error) {
                        Xrm.Navigation.openErrorDialog({ "message": "Upps...we couldn't update the record. Error Message: " + error.message });
                    }
                );
            }
        }
        ,
        function (error) {
            Xrm.Navigation.openErrorDialog({ "message": "Upps...we couldn't capture the image. Error Message: " + error.message });
        }
    );
}
