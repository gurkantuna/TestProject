const deleteObject = (url, clickedObject) => {

    bootbox.confirm('Do you really want to delete this object?', function(removeOk) {
        if (removeOk) {

            const id = $(clickedObject).data('id');

            $.ajax({
                type: 'get',
                url: `${url}/${id}`,
                success: function(response) {
                    if (response.id) {
                        $(clickedObject).closest('tr').fadeToggle('slow');
                        console.log('Deleted object:');
                        console.log(response);
                    }
                    else {
                        if (response.includes("401")) {
                            bootbox.alert('You are not logged in!');
                        }
                        else if (response.includes("403") || response.includes("405")) {
                            bootbox.alert('You have not authorized!!');
                        }
                    }
                },
                error: function(response) {
                    console.log("Error!!!");
                    console.log(response);

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