$(function () {
    new Vue({
        el: '#view',
        data: {
            activeMenu: '用户管理',
            menus: menus,
            module: '修改密码',
            moduleDescription: '',
            breadcrumb: [{
                name: '用户管理',
                href: '/user'
            }, {
                name: '修改密码',
                href: '#'
            }],
            newPassword: ''
        },
        methods: {
            update: function () {
                app.put("/api/user/" + app.getPathPart(window.location.href, 1) + '/changepassword', this.$data, function () {
                    window.location.href = '/user';
                }, function (result) {
                    swal('更新失败', result.msg, 'error');
                });
            }
        }
    });
});

