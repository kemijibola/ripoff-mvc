$(function () {

    $.ajaxSetup({ cache: false });

    $("a[data-modal]").on("click", function (e) {

        // hide dropdown if any
        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');


        $('#myModalContent2').load(this.href, function () {


            $('#myModal2').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');

            bindForm(this);
        });

        return false;

    });


});

function bindForm(dialog) {

    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal2').modal('hide');
                    //Refresh
                    location.reload();
                    console.log("test");
                } else {
                    $('#myModalContent2').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}