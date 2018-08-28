var onModalSuccess = function (result) {
    if (result.EnableError) {
        // Clear.
        $('#ModalTitleId').html("");
        $('#ModalThreaId').html("");
        $('#ModalThreadTextId').html("");
        $('#ModalCommentTextId').html("");

        // Setting.
        $('#ModalTitleId').html("");
        $('#ModalThreaId').append(result.ErrorTitle);
        $('#ModalThreadTextId').append(result.ErrorMsg);
        $('#ModalCommentTextId').append(result.ErrorMsg);

        // Show Modal.
        $('#ModalMsgBoxId').modal(
            {
                backdrop: 'static',
                keyboard: false
            });
    }
    else if (result.EnableSuccess) {
        // Clear.
        $('#ModalTitleId').html("");
        $('#ModalThreaId').html("");
        $('#ModalThreadTextId').html("");
        $('#ModalCommentTextId').html("");

        // Setting.
        $('#ModalTitleId').html("");
        $('#ModalThreaId').append(result.SuccessTitle);
        $('#ModalThreadTextId').append(result.SuccessMsg);
        $('#ModalCommentTextId').append(result.SuccessMsg);

        // Show Modal.
        $('#ModalMsgBoxId').modal(
            {
                backdrop: 'static',
                keyboard: false
            });

        // Resetting form.
        $('#ModalformId').get(0).reset();
    }
}
