class Test {
    private x: number = 1;
    constructor() { this.x = 2; }

    public getX()  { return this.x }
}

let t = new Test();
console.log(t.getX());
var x = 1;
let y = 2;