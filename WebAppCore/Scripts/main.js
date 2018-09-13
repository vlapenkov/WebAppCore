
// es6
class Vehicle {

    constructor(name, type) {
        this.name = name;
        this.type = type;
    }

    getName() {
        return this.name;
    }

    getType() {
        return this.type;
    }

}


// Write your Javascript code.
let x = '123';
const greetings = (name) => {
    return `hello ${name}`;
}
greetings('test');

var $ = require('jquery');
$(function () {
    console.log('jquery loaded')
    let car = new Vehicle('Tesla', 'car');
    console.log(car.getName()); // Tesla
    console.log(car.getType()); // car
})