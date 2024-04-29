document.addEventListener('DOMContentLoaded', function () {
    var openSidebarBtn = document.getElementById('sidebar-open-btn');
    var sidebar = document.getElementById('side_nav');
    openSidebarBtn.addEventListener('click', function () {
        if (sidebar.classList.contains('active')) {
            sidebar.classList.remove('active');
        } else {
            sidebar.classList.add('active');
        }
        console.log("Button:", openSidebarBtn);
        console.log("Sidebar:", sidebar);
    });
});