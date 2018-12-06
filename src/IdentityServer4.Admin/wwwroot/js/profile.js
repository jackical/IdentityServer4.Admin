$(function () {
    new Vue({
        el: '#view',
        data: {
            activeMenu: '个人信息',
            menus: menus,
            module: '个人信息',
            moduleDescription: '',
            breadcrumb: [{
                name: '个人信息',
                href: '#'
            }],
            userName: '',
            email: '',
            phoneNumber: '',
            oldPassword: '',
            newPassword: ''
        },
        created: function () {
            loadView(this);
        },
        methods: {
            update: function () {
                app.put("/api/user/" + app.getPathPart(window.location.href, 1) + '/profile', {
                    email: this.$data.email,
                    phoneNumber: this.$data.phoneNumber
                }, function () {
                    swal({
                        title: "更新成功",
                        type: "success",
                        showCancelButton: false
                    });
                }, function (result) {
                    swal({
                        title: "更新失败",
                        type: "error",
                        text: result.msg,
                        showCancelButton: false
                    });
                });
            },
            changePassword:function () {
                app.put("/api/user/" + app.getPathPart(window.location.href, 1) + '/password', {
                    oldPassword: this.$data.oldPassword,
                    newPassword: this.$data.newPassword
                }, function () {
                    swal({
                        title: "更新成功",
                        type: "success",
                        showCancelButton: false
                    });
                }, function (result) {
                    swal({
                        title: "更新失败",
                        type: "error",
                        text: result.msg,
                        showCancelButton: false
                    });
                });
            }
        }
    });

    function loadView(vue) {
        const url = '/api/user/' + app.getPathPart(window.location.href, 1) + '/profile';
        app.get(url, function (result) {
            vue.$data.userName = result.data.userName;
            vue.$data.email = result.data.email;
            vue.$data.phoneNumber = result.data.phoneNumber;
        });
    }
});

