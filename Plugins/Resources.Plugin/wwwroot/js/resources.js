﻿var resources = (function () {
    var root = {

        itemResponse: function (id, v) {
            let json = {
                id: id,
                value: v
            };

            $.ajax({
                type: 'POST',
                url: '/Resources/ItemResponse/',
                data: json,
                cache: false,
                success: function (response) {
                    if (response.success) {
                        let responseData = JSON.parse(response.responseData);
                        let likeName = "itemLikeCount" + responseData.itemId;
                        let likeSpan = document.getElementById(likeName);
                        likeSpan.innerText = responseData.likes;
                        let dislikeName = "itemDislikeCount" + responseData.itemId;
                        let dislikeSpan = document.getElementById(dislikeName);
                        dislikeSpan.innerText = responseData.dislikes;
                    }
                },
            });
        },
    };

    return root;
})();