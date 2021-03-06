$(function () {
    new Vue({
        el: '#view',
        data: {
            activeMenu: '角色管理',
            menus: menus,
            module: '角色管理',
            moduleDescription: '',
            breadcrumb: [{
                name: '角色管理',
                href: '#'
            }],
            els: [],
            page: app.getUrlParam('page') || 1,
            size: app.getUrlParam('size') || 16,
            total: 0
        },
        created: function () {
            loadView(this);
        },
        methods: {
            remove: function (id) {
                const that = this;
                swal({
                    title: "确定要删除此角色吗?",
                    type: "warning",
                    text: '删除角色会把用户角色级连删除',
                    showCancelButton: true
                }, function () {
                    app.delete("/api/role/" + id, function () {
                       loadView(that);
                    });
                });
            }
        }
    });

    function loadView(vue) {
        const url = '/api/role?page=' + vue.$data.page + '&size=' + vue.$data.size;
        app.get(url, function (result) {
            vue.$data.els = result.data.result;
            vue.$data.total = result.data.total;
            vue.$data.page = result.data.page;
            vue.$data.size = result.data.size;

            app.ui.initPagination('#pagination', result.data, function (page) {
                window.location.href = '/api/role?page=' + page + '&size=' + vue.$data.size;
            });
        });
    }
});

