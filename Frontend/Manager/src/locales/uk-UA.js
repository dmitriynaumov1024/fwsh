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
        send: "Надіслати",
        submit: "Готово",
        create: "Створити",
        addPhotos: "Додати фото",
        reset: "Скасувати зміни",
        clear: "Очистити",
        edit: "Редагувати",
        save: "Зберегти",
        confirm: "Підтвердити",
        cancel: "Скасувати",
        delete: "Видалити",
        signup: "Зареєструватися",
        login: "Увійти",
        logout: "Вийти",
        details: "Докладніше"
    },
    common: {
        id: "id",
        other: "Інше",
        list: "Список",
        archive: "Архів",
        page: "Сторінка",
        pleaseTryAgain: "Будь ласка, спробуйте ще раз",
        somethingWrong: "Виникла проблема",
        seeConsole: "Дивіться детальний опис проблеми в консолі", 
        createdAt: "Створено",
        daysSinceCreated: "Днів з моменту створення"
    },
    noDataYet: {
        title: "Тут ще нічого немає",
        description: "Але це нормально, на початку таке трапляється!"
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
    index: {
        title: "Панель менеджера - головна сторінка",
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
    roles: {
        unknown: "Невідомо",
        customer: "Покупець",
        worker: "Робітник",
        management: "Управління",
        carpentry: "Столяр",
        sewing: "Швець",
        assembly: "Збиральник",
        upholstery: "Оббивщик"
    },
    person: {
        single: "Людина",
        plural: "Люди",
    },
    customer: {
        single: "Покупець",
        plural: "Покупці",
    },
    manager: {
        single: "Менеджер",
        plural: "Менеджери",
    },
    worker: {
        single: "Робітник",
        plural: "Робітники",
    },
    supplier: {
        single: "Постачальник",
        plural: "Постачальники",
    },
    profile: {
        myProfile: "Мій профіль",
        myProductionOrders: "Мої замовлення",
        myRepairOrders: "Ремонт меблів",
        surname: "Прізвище",
        name: "Ім'я",
        patronym: "По батькові",
        phone: "Номер телефону",
        email: "E-mail",
        orgName: "Назва організації",
        roles: "Ролі",
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
        new: "Нове сповіщення"
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
        quantity: "Кількість",
        pricePerOne: "Ціна за одиницю",
        priceTotal: "Сумарна вартість",
    },
    repairOrder: {
        single: "Ремонтне замовлення",
        plural: "Ремонтні замовлення",
        price: "Вартість робіт",
        prepayment: "Внесена передплата",
        description: "Опис замовлення"
    },
    measureUnits: {
        unknown: "невідомо",
        m: "м",
        m2: "кв.м",
        m3: "куб.м",
        L: "л",
        Kg: "кг",
        g: "г"
    },
    resource: {
        single: "Ресурс",
        plural: "Ресурси",
        name: "Найменування",
        externalId: "Ідентифікатор",
        description: "Опис",
        price: "Ціна",
        pricePer: "Ціна за 1",
        pricePerUnit: "Ціна за одиницю",
        measureUnit: "Одиниці виміру",
        quantity: "Кількість",
        inStock: "В наявності",
        normalStock: "Нормальна кількість",
        refillPeriodDays: "Період поповнення, днів",
        lastCheckedAt: "Перевірено",
        lastRefilledAt: "Поповнено",
        createdAt: "Створено",
    },
    part: {
        single: "Деталь",
        plural: "Деталі",
    },
    material: {
        single: "Матеріал",
        plural: "Матеріали",
    },
    fabric: {
        catalog: "Каталог тканин",
        single: "Тканина",
        plural: "Тканини"
    },
    fabricType: {
        single: "Тип тканини",
        plural: "Типи тканин",
        name: "Найменування",
        description: "Опис"
    },
    color: {
        single: "Колір",
        plural: "Кольори",
        name: "Найменування",
        rgbCode: "RGB-код"
    },
    blueprint: {
        single:"Прототип",
        plural: "Прототипи",
    },    
    design: {
        notFound: "Не вдалося знайти дизайн меблів з заданим id або ключем",
        creationMessage: "Новий дизайн меблів був створений!",
        create: "Створити дизайн",
        list: "Список дизайнів",   
        catalog: "Каталог дизайнів",
        single: "Дизайн меблів",
        plural: "Дизайни меблів",
        nameKey: "Іменний код",
        displayName: "Найменування",
        type: "Тип",
        price: "Базова ціна",
        description: "Опис",
        isVisible: "Відображається в каталозі",
        isTransformable: "Трансформується",
        dimCompact: "Розміри в складеному стані (ШxДхВ)",
        dimExpanded: "Розміри в розгорнутому стані (ШxДхВ)",
        length: "Довжина",
        width: "Ширина",
        height: "Висота",
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
