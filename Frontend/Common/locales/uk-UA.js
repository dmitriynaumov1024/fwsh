import base from "./base.js"

import { nestedObjectMerge } from "../utils/index.js"

import { createDateFormat } from "../lib/createDateFormat.js"

const dateFormat = createDateFormat("uk-UA")

export default nestedObjectMerge (base, {
    key: "uk-UA",
    displayName: "Українська",
    dateFormat: dateFormat,
    yesNo: {
        [true]: "Так",
        [false]: "Ні"
    },
    badFields: {
        before: "Перевірте правильність введення таких полів: ",
        after: "."
    },
    action: {
        send: "Надіслати",
        submit: "Готово",
        create: "Створити",
        update: "Оновити",
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
        details: "Докладніше",
        makeOrder: "Зробити замовлення"
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
        daysSinceCreated: "Днів з моменту створення",
        language: "мова"
    },
    selectLanguage: {
        title: "Мова сайту"
    },
    changesSaved: {
        title: "Успіх",
        description: "Всі зміни збережено."
    },
    loading: {
        title: "Завантажуємо",
        description: "Зачекайте, будь ласка. Завантаження даних може зайняти певний час. "+
            "Якщо через 10 секунд нічого не з'явиться, перезавантажте сторінку з CTRL+F5, "+
            "перевірте своє мережеве з'єднання або зв'яжіться з адміністратором сайту."
    },
    badRequest: {
        title: "Некоректний запит",
        description: "Будь ласка, перевірте правильність введених параметрів запиту та спробуйте ще раз."
    },
    unauthorized: {
        title: "Ви не авторизувалися",
        description: "Ви намагалися переглянути ресурси, що доступні лише авторизованим користувачам. "+
            "Будь ласка, увійдіть в обліковий запис або зареєструйтеся."
    },
    notFound: {
        title: "Тут нічого немає",
        description: "На жаль, нам не вдалося завантажити дані, або ця сторінка не існує."
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
        title: "workshop",
        nav: {
            catalog: "Каталог",
            profile: "Профіль",
            login: "Вхід",
            signup: "Реєстрація" 
        }
    },
    footer: { 
        siteName: "fwsh" 
    },
    index: {
        title: "fwsh",
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
    noSignup: {
        title: "Реєстрація недоступна",
        description: "Реєстрація на даний момент недоступна. Якщо Ви впевнені, що це помилка, "+
            "зв'яжіться з адміністратором сайту. Якщо Ви вже маєте обліковий запис, увійдіть в нього."
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
        usedQuantity: "Використано",
        actualQuantity: "Актуальна кількість",
        inStock: "В наявності",
        normalStock: "Нормальна кількість",
        refillPeriodDays: "Період поповнення, днів",
        lastCheckedAt: "Перевірено",
        lastRefilledAt: "Поповнено",
        needsRefill: "Треба поповнити запас якнайшвидше",
        isTimeToRefill: "Прийшов час поповнювати",
        createdAt: "Створено",
    },
    part: {
        single: "Деталь",
        plural: "Деталі",
        create: "Нова деталь",
    },
    material: {
        single: "Матеріал",
        plural: "Матеріали",
        create: "Новий матеріал"
    },
    fabric: {
        catalog: "Каталог тканин",
        single: "Тканина",
        plural: "Тканини",
        create: "Нова тканина"
    },
    fabricType: {
        single: "Тип тканини",
        plural: "Типи тканин",
        create: "Новий тип тканини",
        name: "Найменування",
        description: "Опис"
    },
    color: {
        single: "Колір",
        plural: "Кольори",
        create: "Новий колір",
        edit: "Редагувати колір",
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
    taskPrototype: {
        single: "Шаблон задачі",
        plural: "Шаблони задач",
    },
    task: {
        single: "Задача",
        plural: "Задачі",
        status: "Статус",
        createdAt: "Створено",
        startedAt: "Початок робіт",
        finishedAt: "Завершено",
        workType: "Тип роботи",
        role: "Роль",
        resourceUsage: "Використання ресурсів",
        instructions: "Інструкції",
    },
    productionTask: {
        single: "Виробнича задача",
        plural: "Виробничі задачі",
    },
    repairTask: {
        single: "Ремонтна задача",
        plural: "Ремонтні задачі",
    }
})
