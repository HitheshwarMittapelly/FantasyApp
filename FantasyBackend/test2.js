var arraySort = require('array-sort');
function helloWorld(){
    let object ={
        name:"",
        score : 0
    }
    object.name = "hello";
    object.score = 5;
    let scores = [];
    scores.push(object);
    let obj = {};
    obj.name = "bro";
    obj.score = 10;
    scores.push(obj);
    let bbj = {};
    bbj.name = "bud";
    bbj.score = 6;
    scores.push(bbj);
    

    var result = arraySort(scores, ['score']);
    console.log(result);
}
helloWorld();