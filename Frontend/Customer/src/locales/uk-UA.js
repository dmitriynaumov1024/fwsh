import { createDateFormat } from "@common"

const dateFormat = createDateFormat("uk-UA")

export default {
    key: "uk-UA",
    displayName: "Українська",
    yesNo: {
        [true]: "Так",
        [false]: "Ні"
    },
    action: {
        edit: "Редагувати",
        save: "Зберегти",
        confirm: "Підтвердити",
        cancel: "Скасувати",
        delete: "Видалити",
        signup: "Зареєструватися",
        login: "Увійти",
        logout: "Вийти",
        details: "Докладніше",
        makeOrder: "Замовити"
    },
    common: {
        catalog: "Каталог",
        list: "Список",
        archive: "Архів",
        page: "Сторінка",
        pleaseTryAgain: "Будь ласка, спробуйте ще раз",
        somethingWrong: "Виникла проблема",
        seeConsole: "Дивіться детальний опис проблеми в консолі", 
        loading: "Завантажується! Будь ласка, зачекайте...",
    },
    noDataYet: {
        title: "Тут ще нічого немає",
        description: "Але нічого страшного, на початку таке трапляється."
    },
    noData: {
        title: "Тут нічого немає",
        description: "На жаль, нам не вдалося завантажити дані, або ця сторінка не існує."
    },
    noRoute: {
        title: "Помилка 404",
        description: "Скоріш за все, Ви намагалися переглянути неіснуючу сторінку"
    },
    header: {
        nav: {
            catalog: "Каталог",
            profile: "Профіль",
            login: "Вхід",
            signup: "Реєстрація" 
        }
    },
    login: {
        welcomeBack: "Ласкаво просимо!",
        haveNoAccount: "Немає облікового запису?"
    },
    signup: {
        title: "Реєстрація",
        success: "Реєстрація пройшла успішно!",
        successDescription: "Ваш обліковий запис покупця був щойно створений. Тепер Ви можете увійти в систему та замовити меблі!",
        goToLogin: "Увійти в обл.запис",
        iAmOrganization: "Я представляю організацію",
        alreadyHaveAccount: "Вже маєте обліковий запис?"
    },
    profile: {
        myProfile: "Мій профіль",
        myProductionOrders: "Мої замовлення",
        myRepairOrders: "Ремонт меблів",
        surname: "Прізвище",
        name: "Ім'я",
        patronym: "По батькові",
        phone: "Телефон",
        email: "E-mail",
        orgName: "Назва організації",
        discount: "Знижка",
        createdAt: "Дата реєстрації",
        password: "Пароль",
        newPassword: "Новий пароль"
    },
    photo: {
        single: "Фото",
        plural: "Фото"
    },
    status: {
        unknown: "Невизначений",
        submitted: "Опубліковано",
        assigned: "Призначено",
        delayed: "Затримується",
        working: "Робота почалася",
        finished: "Завершено",
        impossible: "Неможливо",
        rejected: "Відхилено",
        receivedpaid: "Отримано та оплачено"
    },
    notification: {
        single: "Сповіщення",
        plural: "Сповіщення",
        new: "Нове сповіщення",
        readAll: "Відмітити все як прочитане"
    },
    order: {
        single: "Замовлення",
        plural: "Замовлення",
        status: "Стан замовлення",
        customer: "Покупець",
        createdAt: "Створено",
        startedAt: "Початок робіт",
        finishedAt: "Завершено",
        receivedAt: "Отримано",
        notifications: "Сповіщення"
    },
    productionOrder: {
        single: "Виробниче замовлення",
        plural: "Виробничі замовлення",
        details: "Деталі замовлення",
        quantity: "Кількість",
        pricePerOne: "Ціна за одиницю",
        priceTotal: "Сумарна вартість",
        design: "Дизайн",
        fabric: "Тканина",
        decorMaterial: "Матеріал декорації"
    },
    repairOrder: {
        single: "Ремонтне замовлення",
        plural: "Ремонтні замовлення",
        price: "Вартість робіт",
        prepayment: "Внесена передплата",
        description: "Опис замовлення"
    },
    fabric: {
        catalog: "Каталог тканин",
        single: "Тканина",
        plural: "Тканини",
    },
    design: {
        catalog: "Каталог дизайнів",
        single: "Дизайн меблів",
        plural: "Дизайни меблів",
        displayName: "Найменування",
        type: "Тип",
        price: "Базова ціна",
        description: "Опис",
        isTransformable: "Трансформується",
        dimCompact: "Розміри в складеному стані (ДxШxВ)",
        dimExpanded: "Розміри в розгорнутому стані (ДxШxВ)",
        createdAt: "Created at"
    },
    furnitureTypes: {
        unknown: "Невідомо",
        ottoman: "Тахта",
        sofa: "Диван",
        corner: "Куток",
        armchair: "Крісло",
        pouffe: "Пуф"
    },
    formatBadFields (badFields, selector) {
        if (!badFields instanceof Array) badFields = Object.keys(badFields)
        if (selector instanceof Function) 
            badFields = badFields.map(key => selector(this)[key]).filter(key => key != undefined)
        return "Будь ласка, перевірте правильність введення таких полів: " + badFields.map(field => field.toLowerCase()).join(", ") + "."
    },
    formatDate (dateTimeString) {
        let date = new Date(dateTimeString)
        return dateFormat.format(date)
    },
    formatTime (dateTimeString) {
        let date = new Date(dateTimeString)
        return date.toLocaleTimeString(this.key)
    },
    formatDateTime (dateTimeString) {
        let date = new Date(dateTimeString)
        return dateFormat.format(date) + " " + date.toLocaleTimeString(this.key)
    }
}
