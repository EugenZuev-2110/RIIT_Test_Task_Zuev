$(function () {
    DevExpress.localization.locale("ru");

    const equipmentStore = new DevExpress.data.CustomStore({
        key: "id",

        load: function () {
            return $.getJSON("/api/equipment");
        },

        insert: function (values) {
            return $.ajax({
                url: "/api/equipment",
                method: "POST",
                data: JSON.stringify(values),
                contentType: "application/json; charset=utf-8"
            }).fail(handleAjaxError);
        },

        update: function (key, values) {
            return $.ajax({
                url: "/api/equipment/" + key,
                method: "PUT",
                data: JSON.stringify(values),
                contentType: "application/json; charset=utf-8"
            }).fail(handleAjaxError);
        }
    });

    function handleAjaxError(xhr) {
        let errorMessage = "Произошла неизвестная ошибка при сохранении.";
        if (xhr.responseText) {
            try {
                errorMessage = xhr.responseText;
            } catch (e) { }
        }
        DevExpress.ui.notify(errorMessage, "error", 5000);
    }

    $("#equipmentGrid").dxDataGrid({
        dataSource: equipmentStore,
        showBorders: true,
        rowAlternationEnabled: true,

        sorting: {
            mode: "single"
        },

        editing: {
            mode: "popup",
            allowAdding: true,
            allowUpdating: true,
            useIcons: true,

            popup: {
                title: "Карточка компьютерной техники",
                showTitle: true,
                width: 550,
                height: 420,
                toolbarItems: [
                    { shortcut: "save", location: "after" },
                    { shortcut: "cancel", location: "after" }
                ]
            },

            form: {
                items: [
                    { dataField: "inventoryNumber", isRequired: true },
                    { dataField: "name", isRequired: true },
                    { dataField: "typeId", isRequired: true },
                    { dataField: "roomNumber", isRequired: true }
                ]
            }
        },

        columns: [
            {
                dataField: "inventoryNumber",
                caption: "Учетный номер",
                validationRules: [
                    { type: "required", message: "Поле обязательно" },
                    { type: "stringLength", max: 32, message: "Максимум 32 символа" },
                    { type: "pattern", pattern: "^[a-zA-Z0-9]+$", message: "Разрешены только латинские буквы и цифры" }
                ]
            },
            {
                dataField: "name",
                caption: "Наименование",
                validationRules: [
                    { type: "required", message: "Поле обязательно" },
                    { type: "stringLength", max: 256, message: "Максимум 256 символов" }
                ]
            },
            {
                dataField: "typeId",
                caption: "Тип техники",
                lookup: {
                    dataSource: {
                        store: new DevExpress.data.CustomStore({
                            key: "id",
                            load: function () {
                                return $.getJSON("/api/equipment/types");
                            }
                        })
                    },
                    valueExpr: "id",
                    displayExpr: "name"
                },
                validationRules: [{ type: "required", message: "Выберите тип техники" }]
            },
            {
                dataField: "roomNumber",
                caption: "Комната размещения",
                dataType: "number",
                validationRules: [
                    { type: "required", message: "Поле обязательно" },
                    { type: "range", min: 1, max: 1000, message: "Диапазон от 1 до 1000" }
                ]
            },
            {
                type: "buttons",
                width: 110,
                buttons: ["edit"]
            }
        ],

        onRowDblClick: function (e) {
            if (e.rowType === "data") {
                e.component.editRow(e.rowIndex);
            }
        }
    });
});
