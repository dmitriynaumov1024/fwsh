import base from "./base.js"

import { nestedObjectMerge } from "../utils/index.js"

import { createDateFormat } from "../lib/createDateFormat.js"

const dateFormat = createDateFormat("uk-UA")

export default nestedObjectMerge (base, {
    key: "uk-UA",
    displayName: "Українська",
    dateFormat: {
        format: dateFormat.format
    },
    yesNo: {
        [true]: "Так",
        [false]: "Ні"
    },
    badFields: {
        before: "Перевірте правильність введення таких полів: ",
        after: "."
    },
    action: {
        back: "Назад",
        next: "Далі",
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
        makeOrder: "Зробити замовлення",
        assignWorker: "Призначити робітника"
    },
    common: {
        id: "id",
        other: "Інше",
        list: "Список",
        archive: "Архів",
        catalog: "Каталог",
        page: "Сторінка",
        pleaseTryAgain: "Будь ласка, спробуйте ще раз",
        somethingWrong: "Виникла проблема",
        seeConsole: "Дивіться детальний опис проблеми в консолі", 
        createdAt: "Створено",
        daysSinceCreated: "Днів з моменту створення",
        language: "мова",
        notSelected: "Не вибрано",
        notAssigned: "Не призначено",
    },
    setVisible: {
        [true]: "Сховати",
        [false]: "Відобразити",
    },
    setVisibleResult: {
        [false]: "Сховано від перегляду",
        [true]: "Доступно для перегляду"    
    },
    setActive: {
        [true]: "Активувати",
        [false]: "Деактивувати"
    },
    setActiveResult: {
        [true]: "Активно",
        [false]: "Не активно"
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
        description: "На жаль, нам не вдалося завантажити дані, або ця сторінка не існує.",
        before: "",
        after: "не знайдено"
    },
    alreadyExists: {
        before: "Такий",
        after: "вже існує"
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
    error: {
        title: "Виникла невідома помилка",
        description: "Сталася помилка. Дивіться опис проблеми в консолі."
    },
    saveFailed: {
        title: "Не вдалося зберегти зміни",
        description: "Не вдалося зберегти зміни. Скоріш за все, це сталося через проблеми на сервері. Дивіться опис проблеми в консолі"
    },
    tryLater: {
        title: "Поперед батька в пекло",
        description: "Запитувана функція зараз недоступна. Будь ласка, спробуйте пізніше",
        before: "Спробуйте ще раз",
    },
    photoLimitExceeded: {
        title: "Перевищено ліміт фото",
        description: "Перевищено максимально допустиму кількість прикріплених фото.",
        getDescription(n) {
            return `Перевищено максимально допустиму кількість прикріплених фото (${n}).`
        }
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
        successDescription: "Ваш обліковий запис був щойно створений. Тепер Ви можете увійти в систему.",
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
        create: "Новий покупець"
    },
    manager: {
        single: "Менеджер",
        plural: "Менеджери",
        create: "Новий менеджер"
    },
    worker: {
        single: "Робітник",
        plural: "Робітники",
        create: "Новий робітник"
    },
    supplier: {
        single: "Постачальник",
        plural: "Постачальники",
        create: "Новий постачальник"
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
        isOrganization: "Представник організації",
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
        received: "Отримано",
        paid: "Оплачено",
        receivedpaid: "Отримано та оплачено"
    },
    notification: {
        single: "Сповіщення",
        plural: "Сповіщення",
        new: "Нове сповіщення",
        readAll: "Відмітити все як прочитане",
    },
    myOrder: {
        single: "Моє замовлення",
        plural: "Мої замовлення"
    },
    myTask: {
        single: "Моя задача",
        plural: "Мої задачі"
    },
    order: {
        single: "Замовлення",
        plural: "Замовлення",
        status: "Стан замовлення",
        isActive: "Активне",
        price: "Вартість",
        payment: "Сплачено",
        customer: "Покупець",
        createdAt: "Створено",
        startedAt: "Початок робіт",
        finishedAt: "Завершено",
        receivedAt: "Отримано",
        notifications: "Сповіщення",
        details: "Деталі замовлення"
    },
    productionOrder: {
        single: "Виробниче замовлення",
        plural: "Виробничі замовлення",
        create: "Нове виробниче замовлення",
        quantity: "Кількість",
        pricePerOne: "Ціна за одиницю",
        priceTotal: "Сумарна вартість",
        design: "Дизайн",
        fabric: "Тканина",
        decor: "Декор"
    },
    repairOrder: {
        single: "Ремонтне замовлення",
        plural: "Ремонтні замовлення",
        create: "Нове ремонтне замовлення",
        price: "Вартість робіт",
        prepayment: "Внесена передплата",
        description: "Опис замовлення"
    },
    measureUnits: {
        [null]: "невідомо",
        unknown: "невідомо",
        mm: "мм",
        cm: "см",
        m: "м",
        m2: "кв.м",
        m3: "куб.м",
        L: "л",
        Kg: "кг",
        g: "г"
    },
    slotNames: {
        [null]: "Невідомо",
        decor: "Декор",
        fabric: "Тканина",
    },
    resource: {
        single: "Ресурс",
        plural: "Ресурси",
        type: "Тип",
        slotName: "Слот",
        name: "Найменування",
        externalId: "Ідентифікатор",
        description: "Опис",
        price: "Ціна",
        pricePer: "Ціна за 1",
        pricePerUnit: "Ціна за одиницю",
        measureUnit: "Одиниці виміру",
        precision: "Точність, десяткових знаків",
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
        hasSupplyOrder: "Буде поповнено найближчим часом",
        createdAt: "Створено",
    },
    resourceQuantity: {
        single: "Кількість ресурсу",
        plural: "Кількість ресурсів",
        expectQuantity: ""
    },
    resourceUsage: {
        single: "Використання ресурсу",
        plural: "Використання ресурсів",
        expectQuantity: "Заплановане використання",
        actualQuantity: "Фактичне використання"
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
    decor: {
        single: "Декораційний матеріал",
        plural: "Декораційні матеріали",
        create: "Створити новий декор.матеріал"
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
        priceStartsAt: "від",
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
        [null]: "Будь-який",
        unknown: "Невідомо",
        ottoman: "Тахта",
        minisofa: "Міні-диван",
        sofa: "Диван",
        corner: "Куток",
        armchair: "Крісло",
        pouffe: "Пуф"
    },
    taskPrototype: {
        single: "Шаблон задачі",
        plural: "Шаблони задач",
        description: "Опис",
        precedence: "Порядок виконання"
    },
    task: {
        single: "Задача",
        plural: "Задачі",
        status: "Статус",
        isActive: "Активно",
        createdAt: "Створено",
        startedAt: "Початок робіт",
        finishedAt: "Завершено",
        workType: "Тип роботи",
        role: "Роль",
        payment: "Оплата",
        description: "Опис",
        resourceUsage: "Використання ресурсів",
        instructions: "Інструкції",
        notAssigned: "Не призначено"
    },
    productionTask: {
        single: "Виробнича задача",
        plural: "Виробничі задачі",
    },
    repairTask: {
        single: "Ремонтна задача",
        plural: "Ремонтні задачі",
    },
    furniture: {
        single: "Предмет меблів",
        plural: "Меблі"
    },
    supplyOrder: {
        single: "Закупка ресурсів",
        plural: "Закупки ресурсів",
        externalId: "Ідентифікатор",
        expectPrice: "Очікувана ціна",
        expectPricePerUnit: "Очікувана ціна",
        expectQuantity: "Очікувана кількість",
        actualPrice: "Актуальна ціна",
        actualPricePerUnit: "Актуальна ціна",
        actualQuantity: "Актуальна кількість",
        submitOrder: "Надіслати замовлення",
        closeOrder: "Закрити замовлення"
    },
    paycheck: {
        single: "Заробітна плата",
        plural: "Заробітні чеки",
        create: "Оформити заробітну плату",
        created: "Заробітний чек створений",
        pay: "Підтвердити оплату",
        paid: "Заробітна плата виплачена",
        amount: "Сума",
        isPaid: "Оплачено",
        intervalStart: "Початок періоду",
        intervalEnd: "Кінець періоду",
        createdAt: "Створено"
    },
    paycheckBonus: {
        single: "Премія",
        plural: "Премії",
        create: "Оформити премію",
        created: "Премія створена",
    }
})
