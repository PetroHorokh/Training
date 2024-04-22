//document.addEventListener('DOMContentLoaded', () => {
//    document.querySelectorAll('.nav-link').forEach(link => {
//        if (link.getAttribute('data-url') == location.pathname) {
//            link.classList.add('active');
//        } else {
//            link.classList.remove('active');
//        }
//    });
//})

function grid_dataSource_beforeSend(op, ajax) {
    ajax.headers = { RequestVerificationToken: '@GetAntiXsrfRequestToken()' };
    ajax.headers = { Authorization: localStorage.getItem('Token') };
}