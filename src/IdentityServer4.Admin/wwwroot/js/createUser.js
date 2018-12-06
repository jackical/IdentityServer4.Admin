$(function () {
    new Vue({
        el: '#view',
        data: {
            activeMenu: '用户管理',
            menus: menus,
            module: '创建用户',
            moduleDescription: '',
            breadcrumb: [{
                name: '用户管理',
                href: '/user'
            }, {
                name: '创建',
                href: '#'
            }],
            userName: '',
            email: '',
            password: '',
            phoneNumber: ''
        },
        methods: {
            create: function () {
                app.post('/api/user', this.$data, function () {
                    window.location.href = '/user';
                });
            }
        }
    });
});