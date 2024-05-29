document.addEventListener('DOMContentLoaded', function () {
    var notificationIcon = document.getElementById('notification-icon');
    var popupContainer = document.getElementById('notification-popup-container');

    notificationIcon.addEventListener('click', function (event) {
        event.stopPropagation(); // Ngăn chặn sự kiện click lan ra ngoài

        // Toggle hiển thị pop-up
        if (popupContainer.style.display === 'none' || popupContainer.style.display === '') {
            popupContainer.style.display = 'block';
            loadNotifications();
            popupContainer.classList.add('fade-in');
            popupContainer.classList.remove('fade-out');
        } else {
            popupContainer.classList.add('fade-out');
            popupContainer.classList.remove('fade-in');
            setTimeout(function () {
                popupContainer.style.display = 'none';
            }, 500); // Delay to match the fade-out transition
        }
    });

    document.addEventListener('click', function (event) {
        // Ẩn pop-up khi click ra ngoài
        if (!notificationIcon.contains(event.target) && !popupContainer.contains(event.target)) {
            popupContainer.classList.add('fade-out');
            popupContainer.classList.remove('fade-in');
            setTimeout(function () {
                popupContainer.style.display = 'none';
            }, 500); // Delay to match the fade-out transition
        }
    });

    function loadNotifications() {
        fetch('/Notification/GetNotifications')
            .then(response => response.json())
            .then(data => {
                // Xóa thông báo hiện tại
                popupContainer.innerHTML = '';
                // Thêm thông báo mới
                if (data.length === 0) {
                    var noNotification = document.createElement('div');
                    noNotification.className = 'no-notification';
                    noNotification.innerText = 'No notifications';
                    popupContainer.appendChild(noNotification);
                } else {

                    data.forEach(notification => {
                        var notificationItem = document.createElement('div');
                        notificationItem.className = 'notification-item';

                        var notificationTitle = document.createElement('div');
                        notificationTitle.className = 'notification-title';
                        notificationTitle.innerText = notification.title;

                        var notificationMessage = document.createElement('div');
                        notificationMessage.className = 'notification-message';
                        notificationMessage.innerText = notification.message;

                        var notificationTime = document.createElement('div');
                        notificationTime.className = 'notification-time';
                        notificationTime.innerText = new Date(notification.createdAt).toLocaleString();

                        notificationItem.appendChild(notificationTitle);
                        notificationItem.appendChild(notificationMessage);
                        notificationItem.appendChild(notificationTime);
                        popupContainer.appendChild(notificationItem);
                    });
                }
            })
            .catch(error => {
                console.error("Error loading notifications:", error);
            });
    }
});