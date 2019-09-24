let matchid = "csk vs rcb";
var firebase = require('firebase').initializeApp({
    serviceAccount: "./hitesh_fantasyApp.json",
    databaseURL: "https://fantasyapp-7e292.firebaseio.com/"
});
function UpdateFlag(){
   
    let userIDs =[];
    let players = [];
    
    let userTeams = []
    var ref = firebase.database().ref().child("users");
    ref.once('value',function(snapshot){
        var inde = 0;
        snapshot.forEach(function(user){
            if(user.exists){
                let userTeamsInfo = {
                    team: [],
                    userID:undefined,
                };
                
                var team = user.child('team');
                if(team.exists){
                    userIDs.push(user.key.toString());
                    team.forEach(function(player){
                        players.push(player.val());
                        userTeamsInfo.team.push(player.val());
                        //console.log(player.val());
                    });
                    
                    userTeamsInfo.userID = user.key.toString();
                    userTeams[inde] = userTeamsInfo;
               
                    inde++
                }
                

               
            }
            
         });
         userTeams.forEach(function(t){
            console.log(t.userID);
        });
        LogMatchTeam(userTeams);
        LogFlag(userIDs);
    });
    
    //firebase.database().goOffline();
}
function LogMatchTeam(userTeams){
    var logger = firebase.database().ref().child("users");
        
    
        for(var j=0; j<userTeams.length; j++){
            
               
                logger.child(userTeams[j].userID).child("matches").child(matchid).child("matchPoints").set(0);
                var ind = 0;
                for(var p of userTeams[j].team){
                    //console.log(p);
                    logger.child(userTeams[j].userID).child("matches").child(matchid).child("matchTeam").child("player"+ind.toString()).child("pid").set(p);
                    logger.child(userTeams[j].userID).child("matches").child(matchid).child("matchTeam").child("player"+ind.toString()).child("points").set(0);
                    ind++;
                }
             
        }

}
function LogFlag(userIDs){
    var updater = firebase.database().ref().child("users");
    // updater.child("vRaKhbRH26QeFlQtCI1DgexfXZ23").child("canUpdateTeam").set("false");
    for(var userID of userIDs){
        console.log(userID);
        updater.child(userID).child("canUpdateTeam").set("false");
    }
}
let userIDs = UpdateFlag();

setTimeout(function(){
    firebase.database().goOffline();
},20000)

