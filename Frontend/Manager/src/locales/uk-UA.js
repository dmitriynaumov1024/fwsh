import { createDateFormat } from "@common"

const dateFormat = createDateFormat("uk-UA")

export default {
    key: "uk-UA",
    displayName: "Українська",
    action: {
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
        list: "Список",
        archive: "Архів",
        page: "Сторінка",
        pleaseTryAgain: "Будь ласка, спробуйте ще раз",
        somethingWrong: "Виникла проблема",
        seeConsole: "Дивіться детальний опис проблеми в консолі", 
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
    formatBadFields (badFields, selector) {
        if (!badFields instanceof Array) badFields = Object.keys(badFields)
        if (selector instanceof Function) 
            badFields = badFields.map(key => selector(this)[key]).filter(key => key != undefined)
        return "Будь ласка, перевірте правильність введення таких полів: " + badFields.map(field => field.toLowerCase()).join(", ") + "."
    },
    formatDate (date) {
        date = new Date(date)
        return dateFormat.format(date)
    }
}