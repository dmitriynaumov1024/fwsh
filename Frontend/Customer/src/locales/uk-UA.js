import { createDateFormat } from "Common"

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
        logout: "Вийти",
        details: "Докладніше"
    },
    header: {
        nav: {
            catalog: "Каталог",
            profile: "Профіль",
            login: "Вхід",
            signup: "Реєстрація" 
        }
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
        orgName: "Організація",
        discount: "Знижка",
        createdAt: "Дата реєстрації",
        password: "Пароль",
        newPassword: "Новий пароль"
    },
    formatDate(date) {
        date = new Date(date)
        return dateFormat.format(date)
    }
}
