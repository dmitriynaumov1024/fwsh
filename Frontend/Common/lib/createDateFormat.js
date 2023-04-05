function createDateFormat (localeKey) {
    try {
        return new Intl.DateTimeFormat(localeKey, {
            day: "numeric",
            month: "long",
            year: "numeric"
        })
    }
    catch (error) {
        return {
            format(date) {
                return date.toDateString()
            }
        }
    }
}

export { createDateFormat }
