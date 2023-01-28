declare global {
    interface String {
        format(...parameters: any): string;
    }
    
    interface Number {
        toTime(): string;
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

Number.prototype.toTime = function () {
    "use strict";
    return new Date(this.valueOf()).toLocaleString();
}

export { }
