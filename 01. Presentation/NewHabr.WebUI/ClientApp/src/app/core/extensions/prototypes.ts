declare global {
    interface String {
        format(...parameters: any): string;
    }
}

String.prototype.format = function (parameters) {
    "use strict";
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
            ? args[number]
            : match
    })
}

export { }
