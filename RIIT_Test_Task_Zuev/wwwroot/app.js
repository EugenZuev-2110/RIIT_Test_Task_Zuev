document.addEventListener("DOMContentLoaded", function () {

    // Строка локализации закомментирована для обеспечения стабильной автономной работы приложения
    // DevExpress.localization.locale("ru");

    // 1. ХРАНИЛИЩЕ ДАННЫХ (CustomStore) ДЛЯ СВЯЗИ С REST API ОБОРУДОВАНИЯ
    const equipmentStore = new DevExpress.data.CustomStore({
        key: "id",

        // Получение всего списка техники для отображения в таблице
        load: function () {
            return $.getJSON("/api/equipment");
        },

        // Отправка запроса на создание новой единицы техники
        insert: function (values) {
            return $.ajax({
                url: "/api/equipment",
                method: "POST",
                data: JSON.stringify(values),
                contentType: "application/json; charset=utf-8"
            }).fail(handleAjaxError);
        },

        // Отправка запроса на обновление существующей единицы техники
        update: function (key, values) {
            return $.ajax({
                url: "/api/equipment/" + key,
                method: "PUT",
                data: JSON.stringify(values),
                contentType: "application/json; charset=utf-8"
            }).fail(handleAjaxError);
        }
    });

    // Централизованная функция обработки серверных ошибок (включая лимит в 2000 записей)
    function handleAjaxError(xhr) {
        let errorMessage = "Произошла неизвестная ошибка при сохранении данных.";
        if (xhr.responseText) {
            try {
                errorMessage = xhr.responseText;
            } catch (e) { }
        }
        DevExpress.ui.notify(errorMessage, "error", 5000);
    }

    // 2. ИНИЦИАЛИЗАЦИЯ И НАСТРОЙКА КОМПОНЕНТА DXDATAGRID
    $("#equipmentGrid").dxDataGrid({
        dataSource: equipmentStore,
        showBorders: true,
        rowAlternationEnabled: true,

        // Сортировка по умолчанию включена для всех колонок таблицы
        sorting: {
            mode: "single"
        },

        // Конфигурация интерфейса редактирования (форма во всплывающем модальном окне)
        editing: {
            mode: "popup",
            allowAdding: true,    // Кнопка "+" в правом верхнем углу панели таблицы
            allowUpdating: true,  // Разрешает редактирование строк
            useIcons: true,       // Включает отображение иконок вместо текста на кнопках управления

            // Настройка всплывающего диалогового окна карточки техники
            popup: {
                title: "Карточка компьютерной техники",
                showTitle: true,
                width: 550,
                height: 420
            },

            // Состав и обязательность полей внутри формы редактирования
            form: {
                items: [
                    { dataField: "inventoryNumber", isRequired: true },
                    { dataField: "name", isRequired: true },
                    { dataField: "typeId", isRequired: true },
                    { dataField: "roomNumber", isRequired: true }
                ]
            }
        },

        // НАСТРОЙКА КОЛОНОК ТАБЛИЦЫ СОГЛАСНО ТЕХНИЧЕСКОМУ ЗАДАНИЮ
        columns: [
            {
                dataField: "inventoryNumber",
                caption: "Учетный номер",
                // Валидация: до 32 символов, строго латинские буквы и цифры
                validationRules: [
                    { type: "required", message: "Поле обязательно для заполнения" },
                    { type: "stringLength", max: 32, message: "Длина номера не должна превышать 32 символа" },
                    { type: "pattern", pattern: "^[a-zA-Z0-9]+$", message: "Разрешены только латинские буквы и цифры" }
                ]
            },
            {
                dataField: "name",
                caption: "Наименование",
                // Валидация: до 256 символов
                validationRules: [
                    { type: "required", message: "Поле обязательно для заполнения" },
                    { type: "stringLength", max: 256, message: "Длина наименования не должна превышать 256 символов" }
                ]
            },
            {
                dataField: "typeId",
                caption: "Тип техники",
                // Настройка выпадающего списка (Lookup) для связи со словарем типов техники
                lookup: {
                    dataSource: {
                        store: new DevExpress.data.CustomStore({
                            key: "id",

                            // Загрузка всего справочника типов для отображения выпадающего списка
                            load: function () {
                                return $.getJSON("/api/equipment/types");
                            },

                            // ИСПРАВЛЕНО: Получение конкретного типа техники по его ID 
                            // Метод необходим для корректной подстановки значения при открытии формы редактирования
                            byKey: function (key) {
                                return $.getJSON("/api/equipment/types").then(function (data) {
                                    return data.find(item => item.id === key);
                                });
                            }
                        })
                    },
                    valueExpr: "id",
                    displayExpr: "name"
                },
                validationRules: [{ type: "required", message: "Необходимо выбрать тип техники" }]
            },
            {
                dataField: "roomNumber",
                caption: "Комната размещения",
                dataType: "number",
                // Валидация: Обязательное целое число в диапазоне от 1 до 1000
                validationRules: [
                    { type: "required", message: "Поле обязательно для заполнения" },
                    { type: "range", min: 1, max: 1000, message: "Номер комнаты должен быть в диапазоне от 1 до 1000" }
                ]
            },
            {
                // Кнопка редактирования (карандаш) в конце каждой строки (Способ 1 открытия карточки)
                type: "buttons",
                width: 110,
                buttons: ["edit"]
            }
        ],

        // Событие двойного щелчка мыши по строке таблицы (Способ 2 открытия карточки)
        onRowDblClick: function (e) {
            if (e.rowType === "data") {
                e.component.editRow(e.rowIndex);
            }
        }
    });
});