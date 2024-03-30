function redirectToAction(url) {
    window.location.href = url;
}

function deleteUser(userId) {
    $.ajax({
        url: "/User/Delete",
        type: "DELETE",
        data: { userId },
        success: function (result) {
            let child = document.getElementById(userId);

            child.parentElement.removeChild(child);
        },
    });
}
