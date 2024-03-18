document.getElementById('toggleButton').addEventListener('click', function () {
    var sidebar = document.getElementById('admin-sidebar');
    sidebar.style.left = '0px'; // Show the navbar
});

document.getElementById('hideButton').addEventListener('click', function () {
    var sidebar = document.getElementById('admin-sidebar');
    sidebar.style.left = '-250px'; // Hide the navbar
});