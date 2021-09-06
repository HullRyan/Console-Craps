// Made by Ryan Hull
// ITCS 3112 - 051

using System;

namespace Craps
{
    /*       Initial instructions
         * Player rolls 2 die
         * Sum of die faces are calculated
         * First throw:
         * If 7 or 11, Game won
         * If 2, 3, 12, Game lost
         * If else, sum is "players point"
         * Player rolls again until:
         * Rerolls "players point", player wins
         * Rolls 7, player loses
         * 
         * Player starts with 100 chips
         * Player asked for wager for each round
         * On win, player wins double wager
         * On loss, player loses full wager
         * 
         * Game is played until:
         * Player quits
         * All players chips are used
         * 
         * Display final chips on exit
    */

    class Craps //Contains main function, which crates a Craps game object and starts the game.
    {
        static void Main(string[] args)
        {
            CrapsGame game = new CrapsGame();
            game.startGame();
        }
    }

    class Dice //Dice object, holds die face and roll function
    {
        public int face = 0;            //Simulates face of die
        Random random = new Random();   //Random object to simulate roll

        public int roll()               //Roll function, returns random int betweem 1-6
        {
            this.face = random.Next(1, 7);
            return this.face;
        }
    }

    class Player //Player object, holds player info ie. name and chip count
    {
        public void startPlayer()   //Initializes player and fields
        {
            Console.WriteLine("Please input player name: ");
            playerName = Console.ReadLine();
            chips = startingChips;
            wager = 0;
        }

        string playerName;              //Players name
        int chips;                      //Current chip count
        int wager;                      //Current wager
        int resetCount;                 //Tracks how many times player resets
        int roundsWon;                  //Tracks how many rounds the player has won
        public int startingChips = 100; //Starting chip count

        public void bet()       //Gets bet from user
        {
            Console.WriteLine("Chips remaining: " + chips); //Displays remaining chips
            Console.WriteLine("Please input your wager " +
                "for upcoming round: ");

            while (true)    //TryParse to get correct int from the string returned from read line
            {               //Found on microsoft docs https://docs.microsoft.com/en-us/dotnet/api/system.int32.tryparse?view=net-5.0
                if (int.TryParse(Console.ReadLine(), out wager))
                {
                    if (wager > 0 && wager <= chips)
                    {
                        break;
                    }
                }
                Console.WriteLine("Sorry, please input a wager" +
                    " above 0 and below your remaining chips: ");
            }
        }

        public void win()       //Adds win to counter and adds wager to chip count
        {
            roundsWon++;
            chips += wager;
        }

        public void loss()      //Removes wager from chip count
        {
            chips -= wager;
        }

        public void resetChips()//Resets chip count to starting amount and adds a reset to the counter
        {
            chips = startingChips;
            resetCount++;
        }

        public int getChips()   //Getter for chip count
        {
            return chips;
        }

        public int getResets()  //Getter for reset count
        {
            return resetCount;
        }

        public string getName() //Getter for player name
        {
            return playerName;
        }

        public int getWins()    //Getter for rounds won
        {
            return roundsWon;
        }
    }

    class Round //Round object, holds round count and players point
    {
        static int Rounds;  //Holds round count across instances
        int playersPoint;   //Holds round count per instance
        public Round()  //Adds round to counter and displays the current count
        {
            Rounds++;

            Console.WriteLine("Current Round: " + Rounds);
        }
        public static int getRounds()   //Getter for round count
        {
            return Rounds;
        }

        public void setPlayersPoint(int point)  //Takes roll sum and sets players point
        {
            playersPoint = point;
        }

        public int getPlayersPoint()    //Getter for players point
        {
            return playersPoint;
        }
    }

    class CrapsGame //Craps Game object, conatins game logic, and instances of other objects
    {

        bool playing;   //Boolean for game loop

        Player player = new Player();   //Player instance
        Dice dieOne = new Dice();   //1st die instance
        Dice dieTwo = new Dice();   //2nd die instance
        public void startGame() //Starting game function, holds game loop
        {

            intro();    //Displays header and rules options

            player.startPlayer();   //Initializes player

            playing = true;

            while (playing)
            {
                display();  //Header
                Round round = new Round();  //New round made each iteration
                player.bet();   //Handles players bet for round
                dieOne.roll();  //Rolls 1st die
                dieTwo.roll();  //Rolls 2nd die
                showFaces();    //Shows faces of die
                checkRolls(round);  //Compares the faces to the rules of craps

            }

        }

        void showFaces() //Displays die faces to player
        {
            Console.WriteLine("\nFirst Die: " + dieOne.face);
            Console.WriteLine("Second Die: " + dieTwo.face);
            Console.WriteLine("You rolled a: " + (dieTwo.face + dieOne.face));
        }

        void checkRolls(Round round)    //Handles logic for first roll of a round
        {
            switch (dieOne.face + dieTwo.face)
            {
                case 7:
                    Console.WriteLine("Round Won!");
                    addWin();
                    break;
                case 11:
                    Console.WriteLine("Round Won!");
                    addWin();
                    break;
                case 2:
                    Console.WriteLine("Round Loss!");
                    addLoss();
                    break;
                case 3:
                    Console.WriteLine("Round Loss!");
                    addLoss();
                    break;
                case 12:
                    Console.WriteLine("Round Loss!");
                    addLoss();
                    break;
                default:    //If player didn't win or lose, player must reroll for players point 
                    round.setPlayersPoint(dieOne.face + dieTwo.face);
                    Console.WriteLine("Players point: " + round.getPlayersPoint());
                    reRoll(round);  //Calls function with loop until win or loss for round
                    break;



            }
        }

        void reRoll(Round round)    //Handles loop for rerolling until win/loss
        {
            while (((dieOne.face + dieTwo.face) != round.getPlayersPoint()) || (dieOne.face + dieTwo.face) != 7)
            {
                Console.Write("\nNo point! Must roll a " + round.getPlayersPoint() + " to win round.\n" +   //Displays players point
                    "       Press Enter to reroll. ");
                Console.ReadLine();
                dieOne.roll();
                dieTwo.roll();
                showFaces();
                if ((dieOne.face + dieTwo.face) == round.getPlayersPoint()) //Win if rolls players point
                {
                    Console.WriteLine("Round Won!");
                    addWin();
                    break;
                }
                else if ((dieOne.face + dieTwo.face) == 7)  //Loss if rolls 7
                {
                    Console.WriteLine("Round Loss!");
                    addLoss();
                    break;
                }
            }
        }

        void addWin()   //Adds win to player object and checks if player wants to continue
        {
            player.win();
            checkContinue();
        }
        void addLoss()  //Handles loss
        {
            player.loss();  //Adds loss to player
            if (player.getChips() == 0) //If player is out of chips, asks to reset chip count
            {
                Console.WriteLine("Out of chips! Game Over!");
                Console.WriteLine("Reset chips? y/n");
                if ((Console.ReadLine()) == "y")
                {
                    player.resetChips();
                }
                else
                {
                    gameover();
                }
            }
            else    //Checks if player wants to continue
            {
                checkContinue();
            }
        }

        void gameover() //Displays player and round info, and ends main game loop
        {
            playing = false;
            display();
            Console.WriteLine("\nGame quit, thank you for playing!");
            Console.WriteLine("Player: " + player.getName());
            Console.WriteLine("Final Chips: " + player.getChips());
            Console.WriteLine("Rounds played: " + Round.getRounds());
            Console.WriteLine("Rounds Won: " + player.getWins());
            Console.WriteLine("Chip Resets: " + player.getResets() + "\n\n");
            Console.WriteLine("Art inspired from https://www.asciiart.eu/");


        }

        void checkContinue()    //Checks if players want to continue on loss
        {
            Console.WriteLine("Another Round? y/n");
            if ((Console.ReadLine().ToLower().Equals("n")))
            {
                gameover();
            }
        }

        void display()  //Displays the Craps header
        {
            Console.Clear();
            Console.WriteLine(@"  ____ ");
            Console.WriteLine(@" /\' .\       ___                         _____");
            Console.WriteLine(@"/: \___\     / __\ __ __ _ _ __  ___     / .  /\");
            Console.WriteLine(@"\' / . /    / / | '__/ _` | '_ \/ __|   /____/..\");
            Console.WriteLine(@" \/___/    / /___ | | (_| | |_) \__ \   \'  '\  /");
            Console.WriteLine(@"           \____/_|  \__,_| .__/|___/    \'__'\/");
            Console.WriteLine(@"                          |_|     By Ryan Hull  ");
        }

        void displayRules() //Displays the rules of Craps according to instruction
        {
            Console.WriteLine(
                @"             ____        _          " + "\n" +
                @"            |  _ \ _   _| | ___ ___ " + "\n" +
                @"            | |_) | | | | |/ _ \__ |" + "\n" +
                @"            |  _ <| |_| | |  __\__ \" + "\n" +
                @"            |_| \_\\__,_|_|\___|___/");
            Console.WriteLine("\nCraps is a simple chance dice game");
            Console.WriteLine("where a player rolls 2 die.");
            Console.WriteLine("On the first roll of the die,");
            Console.WriteLine("The player wins if they roll a 7, or 11");
            Console.WriteLine("But, they lose if they roll a 2, 3, or 12");
            Console.WriteLine("If the sum is 4, 5, 6, 8, 9, or 10 on the first throw,");
            Console.WriteLine(@"then that sum is known as the player’s “point”");
            Console.WriteLine(@"To win, you must reroll until you hit the “point”");
            Console.WriteLine("where your sum of die is equal to the point.");
            Console.WriteLine("However, if the player rolls a 7 before making");
            Console.WriteLine("the point they lose the round.");
            Console.WriteLine("\n    Press enter to start a game");
            Console.ReadLine();
        }

        void intro()    //Checks if player wants to see rules or continue to game
        {
            display();
            Console.WriteLine("\nPress enter to start, " +
                "or enter 'r' to view Rules");
            if (Console.ReadLine().ToLower() == "r")
            {
                displayRules();
            }
        }
    }
}

