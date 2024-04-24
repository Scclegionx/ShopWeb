document.addEventListener("DOMContentLoaded", function () {
    // Get all dropdown toggles
    var dropdownToggles = document.querySelectorAll('#admin-sidebar .dropdown-toggle');

    // Loop through each dropdown toggle
    dropdownToggles.forEach(function (toggle) {
        // Add click event listener to each dropdown toggle
        toggle.addEventListener('click', function (e) {
            e.preventDefault(); // Prevent default link behavior

            // Find the associated dropdown menu
            var dropdownMenu = this.nextElementSibling;

            // Toggle the 'show' class to display/hide the dropdown menu
            if (dropdownMenu.style.display === 'block') {
                dropdownMenu.style.display = 'none';
            } else {
                // Hide all other dropdown menus
                var allDropdownMenus = document.querySelectorAll('#admin-sidebar .dropdown-menu');
                allDropdownMenus.forEach(function (menu) {
                    menu.style.display = 'none';
                });

                // Show the current dropdown menu
                dropdownMenu.style.display = 'block';
            }
        });
    });

    // Close dropdown menus when clicking outside
    document.addEventListener('click', function (e) {
        var dropdownToggles = document.querySelectorAll('#admin-sidebar .dropdown-toggle');
        dropdownToggles.forEach(function (toggle) {
            var dropdownMenu = toggle.nextElementSibling;
            if (!toggle.contains(e.target) && !dropdownMenu.contains(e.target)) {
                dropdownMenu.style.display = 'none';
            }
        });
    });
});
