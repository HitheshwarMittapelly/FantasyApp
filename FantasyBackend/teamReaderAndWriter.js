const wkFilePath = './WK.csv';
const bwlFilePath = './BWL.csv';
const batFilePath = './BAT.csv';
const allFilePath = './ALL.csv';
const cskFilePath = './CSK.csv';
const rcbFilePath = './RCB.csv';
const srhFilePath = './SRH.csv';
const miFilePath = './MI.csv';
const kkrFilePath = './KKR.csv';
const kxipFilePath = './KXIP.csv';
const rrFilePath = './RR.csv';
const dcFilePath = './DC.csv';
var files = [];

const csvtojson = require('csvtojson');
var firebase = require('firebase').initializeApp({
    serviceAccount: "./hitesh_fantasyApp.json",
    databaseURL: "https://fantasyapp-7e292.firebaseio.com/"
});
var ref = firebase.database().ref();
filePath = dcFilePath;
csvtojson().fromFile(filePath).then((jsonObj)=>{
    var name = filePath.toString().replace('.csv','');
    name = name.replace('./','');
    console.log(name);
    
    ref.child("Squads").child(name.toString()).set(jsonObj);
});

