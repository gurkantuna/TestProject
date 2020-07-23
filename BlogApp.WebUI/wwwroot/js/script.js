function AjaxDelete(url, clickedObject) {
    debugger;
    bootbox.confirm('Do you really want to delete this object?', function (response) {
        if (response) {

            var id = $(clickedObject).data('id');
            debugger;
            $.ajax({
                type: 'get',
                url: url + id,
                success: function (jsonObject) {
                    console.log('Silinen nesne:');
                    console.log(jsonObject);
                    $(clickedObject).closest('tr').fadeToggle('slow');
                },
                error: function (response) {
                    console.log("Error!!!");
                    console.log(response);
                    debugger;

                    if (response.status == 401) {
                        bootbox.alert('You are not logged in!');
                    }
                    else if (response.status == 403) {
                        bootbox.alert('You have not authorized!');
                    }
                    else if (response.responseText.toLowerCase().includes('delete statement conflicted')) {
                        bootbox.alert("This object can't delete!\nYou have to delete dependency objects first.");
                    }
                    else {
                        bootbox.alert('An error occurred :' + response.responseText);
                    }
                }
            });
        }
    });
}