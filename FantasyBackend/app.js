var fs = require('fs');
const doStuff = ()=> {
    let playerInfo = []
    var hasTeamOneBatFirst = false;
    fs.readFile('data.json', 'utf8', function(err, contents) {
    const data = JSON.parse(contents);
    let starPlayer1PID = "321777";
    let starPlayer2PID = "49758";
    let manOftheMatchPID = data.data['man-of-the-match'].pid;
    teamOne = data.data.team[0].players;
    for(var player of teamOne){
        let currentPid = player.pid;
        const output = {
            pid : currentPid,
            name : player.name
        }
        if(currentPid == starPlayer1PID || currentPid == starPlayer2PID){
            output.starPlayer = 1;
        }
        else{
            output.starPlayer = 0;
        }
        if(manOftheMatchPID == currentPid){
            output.mom = 1;
        }else{
            output.mom = 0;
        }
        let teamOneBatting;
        if(hasTeamOneBatFirst){
            teamOneBatting = data.data.batting[0].scores;
        }
        else{
            teamOneBatting = data.data.batting[1].scores;
        }

        for(var batter of teamOneBatting){
            if(batter.pid == currentPid){
                output.runs = batter.R;
                output.fours = batter['4s'];
                output.sixes = batter['6s'];
                output.StrikeRate = batter.SR;
            }
        }
        let teamOneBowling;
        if(hasTeamOneBatFirst){
            teamOneBowling = data.data.bowling[1].scores;
        }
        else{
            teamOneBowling = data.data.bowling[0].scores;
        }
        for(var bowler of teamOneBowling){
            if(bowler.pid == currentPid){
                output.economy = bowler.Econ;
                output.wickets = bowler.W;
                output.maidens = bowler.M;
            }
        }
        let teamOneFielding;
        if(hasTeamOneBatFirst){
            teamOneFielding = data.data.fielding[1].scores;
        }
        else{
            teamOneFielding = data.data.fielding[0].scores;
        }
        for(var fielder of teamOneFielding){
            if(fielder.pid == currentPid){
                output.catches = fielder.catch;
                output.runouts = fielder.runout;
                output.stumped = fielder.stumped;
            }
        }
        var playerPoints = CalculateScores(output);
        output.points = playerPoints;
        playerInfo.push(output);
    }
    output = undefined;
    teamTwo = data.data.team[1].players;
    for(var player of teamTwo){
        let currentPid = player.pid;
        const output = {
            pid : currentPid,
            name : player.name
        }
        if(currentPid == starPlayer1PID || currentPid == starPlayer2PID){
            output.starPlayer = 1;
        }
        else{
            output.starPlayer = 0;
        }
        if(manOftheMatchPID == currentPid){
            output.mom = 1;
        }else{
            output.mom = 0;
        }
        let teamTwoBatting;
        if(hasTeamOneBatFirst){
            teamTwoBatting = data.data.batting[1].scores;
        }
        else{
            teamTwoBatting = data.data.batting[0].scores;
        }

        for(var batter of teamTwoBatting){
            if(batter.pid == currentPid){
                output.runs = batter.R;
                output.fours = batter['4s'];
                output.sixes = batter['6s'];
                output.StrikeRate = batter.SR;
            }
        }
        let teamTwoBowling;
        if(hasTeamOneBatFirst){
            teamTwoBowling = data.data.bowling[0].scores;
        }
        else{
            teamTwoBowling = data.data.bowling[1].scores;
        }
        for(var bowler of teamTwoBowling){
            if(bowler.pid == currentPid){
                output.economy = bowler.Econ;
                output.wickets = bowler.W;
                output.maidens = bowler.M;
            }
        }
        let teamTwoFielding;
        if(hasTeamOneBatFirst){
            teamTwoFielding = data.data.fielding[0].scores;
        }
        else{
            teamTwoFielding = data.data.fielding[1].scores;
        }
        for(var fielder of teamTwoFielding){
            if(fielder.pid == currentPid){
                output.catches = fielder.catch;
                output.runouts = fielder.runout;
                output.stumped = fielder.stumped;
            }
        }
        let playerPoints = CalculateScores(output);
        output.points = playerPoints;
        
        playerInfo.push(output);
    }
    
    

   
    });
    
    return playerInfo;
    
}
const CalculateScores = (output)=>{
    let points = 0;

    //Man of the match
    if(output.mom == 1){
        points+= 50;
    }
    //runs and strike rate
    if(output.runs != undefined){
        if(output.runs == 0){
            points -= 10;
        }
        let fifties = Math.floor(output.runs/50);
        points += fifties * 10;
        
        
        points += output.runs * output.StrikeRate/100;
        
        //Fours and Sixes
        points += output.fours * 2 ;
        points += output.sixes * 4 ;
    }
    if(output.wickets!=undefined){
        //Wickets and Maidens
        points += output.wickets * 20;
        points += output.maidens * 10;
        //Economy
        if(output.economy >= 1 && output.economy <=1.99){
            points += 50;
        }
        else if(output.economy >= 2 && output.economy <=2.99){
            points += 30;
        }
        else if(output.economy >= 3 && output.economy <=3.99){
            points += 25;
        }
        else if(output.economy >= 4 && output.economy <=5.99){
            points += 20;
        }
        else if(output.economy >= 6 && output.economy <=6.99){
            points += 10;
        }
        else if(output.economy >= 8 && output.economy <=8.99){
            points -= 5;
        }
        else if(output.economy >= 9 && output.economy <=9.99){
            points -= 15;
        }
        else if(output.economy >= 10 && output.economy <=10.99){
            points -= 25;
        }
        else if(output.economy >= 11 && output.economy <=12.99){
            points -= 30;
        }
        else if(output.economy >= 13 && output.economy <=14.99){
            points -= 40;
        }
        else if(output.economy >=15){
            points -= 50;
        }
    }
    if(output.catches != undefined){
        //catches runouts and stumps
        let fieldPoints = output.catches + output.runouts + output.stumped;
        points += fieldPoints * 10;
    }
    //Star player
    if(output.starPlayer == 1){
        points = points * 2;
    }
    
    return points;
}
doStuff();
module.exports = doStuff;