// base for all locales
// it contains all essential methods
//
export default {
    dateFormat: {
        format(date) {
            return date.toLocaleDateString(this.key)
        }
    },
    formatBadFieldsImpl (badFields, selector, wrap) {
        if (!badFields instanceof Array) badFields = Object.keys(badFields)
        if (selector instanceof Function) 
            badFields = badFields.map(key => selector(this)[key]).filter(key => key != undefined)
        return `${wrap.before} ${badFields.map(field => field.toLowerCase()).join(", ")} ${wrap.after}`
    },
    formatBadFields (badFields, selector) {
        return this.formatBadFieldsImpl (badFields, selector, this.badFields)
    },
    formatNotFound (badFields, selector) {
        return this.formatBadFieldsImpl (badFields, selector, this.notFound)
    },
    formatAlreadyExists (badFields, selector) {
        return this.formatBadFieldsImpl (badFields, selector, this.alreadyExists)
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
