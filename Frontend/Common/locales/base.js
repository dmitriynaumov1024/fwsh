// base for all locales
// it contains all essential methods
//
export default {
    dateFormat: {
        format(date) {
            return date.toLocaleDateString(this.key)
        }
    },
    formatBadFields (badFields, selector) {
        if (!badFields instanceof Array) badFields = Object.keys(badFields)
        if (selector instanceof Function) 
            badFields = badFields.map(key => selector(this)[key]).filter(key => key != undefined)
        return `${this.badFields.before} ${badFields.map(field => field.toLowerCase()).join(", ")} ${this.badFields.after}`
    },
    formatDate (dateTimeString) {
        let date = new Date(dateTimeString)
        return this.dateFormat.format(date)
    },
    formatTime (dateTimeString) {
        let date = new Date(dateTimeString)
        return date.toLocaleTimeString(this.key)
    },
    formatDateTime (dateTimeString) {
        let date = new Date(dateTimeString)
        return `${this.dateFormat.format(date)} ${date.toLocaleTimeString(this.key)}`
    }
}
