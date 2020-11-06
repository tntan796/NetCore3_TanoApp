var productCategory = function () {
    this.initialize = function () {
        registerEvent();
        loadData();
    }

    function loadData() {
        $.ajax({
            method: 'get',
            url: '/admin/productcategory/getall',
            dataType: 'json',
            success: function (response) {
                var data = [];
                response.forEach(item => {
                    data.push({
                        id: item.id,
                        text: item.name,
                        parentId: item.parentId,
                        sortOrder: item.sortOrder
                    });
                });
                var treeArr = tano.unflattern(data);
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });

                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onDrop: function (target, source, point) {
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });
                            // Update to database
                            $.ajax({
                                url: '/admin/productcategory/updateParentId',
                                type: 'post',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function (response) {
                                    customNotify("Update parent Id faile", types.success);
                                    loadData();
                                },
                                error: function (error) {
                                    customNotify("Update parent Id faile", types.danger);
                                    console.log(error);
                                }
                            });

                        } else if (point == 'top' || point == 'bottom') {
                            $.ajax({
                                url: '/admin/productcategory/reorder',
                                type: 'post',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function (response) {
                                    customNotify("Order successful", types.success);
                                    loadData();
                                },
                                error: function (error) {
                                    customNotify("Re order fail", types.danger);
                                    console.log(error);
                                }
                            });
                        }
                    },

                });
            },
            error: function (error) {

            }
        })
    }

    function registerEvent() {

    }
}