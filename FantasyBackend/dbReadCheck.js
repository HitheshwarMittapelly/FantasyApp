var doStuff = require('./app.js');
var arraySort = require('array-sort');
let matchid = "csk vs rcb";
var firebase = require('firebase').initializeApp({
    serviceAccount: "./hitesh_fantasyApp.json",
    databaseURL: "https://fantasyapp-7e292.firebaseio.com/"
});
function RetrieveTeamFromUsers(){
    let userPlayers=[] ;
    let scoreCalculatedPlayers = doStuff();
    
    
    let dashBoardEntries = []
    var ref = firebase.database().ref().child("users");
    ref.once('value',function(snapshot){
        snapshot.forEach(function(user){
            let dashBoardUser = {
                userID : "",
                name : "",
                totalScore: 0
        
            };
            var totalScore = user.child("totalPoints");
            var retrievedTotalScore = 0;
            
            if(totalScore.exists()){
            
                retrievedTotalScore = totalScore.val();
                dashBoardUser.totalScore = retrievedTotalScore;
            }
            var team = user.child("matches").child(matchid).child("matchTeam");
            team.forEach(function(playerInfo){
                var name = user.child("info").child("displayName").val();
                dashBoardUser.name = name;
                dashBoardUser.userID = user.key.toString();
                let playerPID = playerInfo.child("pid").val();
                //console.log(playerInfo.val());
                for(var player of scoreCalculatedPlayers){
                    if(playerPID == player.pid){
                        var logger = firebase.database().ref("users").child(user.key.toString()).child("matches").child(matchid).child("matchTeam");
                        let playerPoints = player.points;
                        playerPoints = Math.round(playerPoints);
                        //console.log(playerPoints);
                        logger.child(playerInfo.key.toString()).child("points").set(playerPoints);
                        //console.log(player.points +"--"+player.name+ "--"+user);
                        dashBoardUser.totalScore +=playerPoints;
                    }
                }
                
               
                
            });
            var scoreUpdater = firebase.database().ref("users").child(user.key.toString());
            scoreUpdater.child("totalPoints").set(dashBoardUser.totalScore);
            dashBoardEntries.push(dashBoardUser); 
        });
        setTimeout(function(){
            
        ref.off(); 
        createDashBoard(dashBoardEntries);
        
        },2000);
        
    });

    

}

function createDashBoard(dashBoardEntries){
    var dashBoardUpdater = firebase.database().ref();
    var result = arraySort(dashBoardEntries, ['totalScore'],{reverse: true});
    var index = 0;
    for(var res of result){
        dashBoardUpdater.child("Dashboard").child(index.toString()).child("name").set(res.name);
        dashBoardUpdater.child("Dashboard").child(index.toString()).child("points").set(res.totalScore);
        dashBoardUpdater.child("Dashboard").child(index.toString()).child("userid").set(res.userID);
        index++;
    }
    setTimeout(function(){
            
     
        firebase.database().goOffline();
        
        },2000);
}
RetrieveTeamFromUsers();



