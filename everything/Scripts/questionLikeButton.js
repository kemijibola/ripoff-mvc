$(function () {

    if ($(".like-button").size() > 0) {

        var postClient = $.connection.questionLikeHub;

        postClient.client.updateLikeCount = function (post) {
            var counter = $(".like-count");
            $(counter).fadeOut(function () {
                $(this).text(post.LikeCount);
                $(this).fadeIn();

                var userLikeStatus = post.UserLikeStatus;
                if (userLikeStatus) {
           
                    $(".like-button").removeClass('fa fa-thumbs-up');
                    $(".like-button").addClass('fa fa-thumbs-down');
                    $(".like-button").addClass('btn btn-default');
                }
                else {
                    $(".like-button").removeClass('fa fa-thumbs-down');
                    $(".like-button").addClass('fa fa-thumbs-up');
                    $(".like-button").addClass('btn btn-default');
                }
                
            });
        };

        $(".like-button").on("click", function () {
            var code = $(this).attr("data-question");
            var user = $(".logged-user").attr("data-id");
            postClient.server.like(code, user);                  
        });
        
        $.connection.hub.start();
        
    }

});