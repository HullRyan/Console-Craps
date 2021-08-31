using System;

namespace Craps
{
    /* 
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

    class Craps
    {
        static void Main(string[] args)
        {
            CrapsGame game = new CrapsGame();
            game.startGame();
        }
    }

    class Dice
    {
        public int face = 0;
        Random random = new Random();

        public int roll()
        {
            this.face = random.Next(1, 7);
            return this.face;
        }
        public int returnFace()
        {
            return this.face;
        }
    }

    class Player
    {
        public Player()
        {
            Console.WriteLine("Please input player name: ");
            playerName = Console.ReadLine();
            chips = startingChips;
            wager = 0;
        }

        string playerName;
        int chips;
        int wager;
        int resetCount;
        const int startingChips = 100;

        public void bet()
        {
            Console.WriteLine("Chips remaining: " + chips);
            Console.WriteLine("Please input your wager for upcomong round: ");
            wager = Int32.Parse(Console.ReadLine());
            while (wager <= 0)
            {
                Console.WriteLine("Sorry, please input a wager above 0 and below your remaining chips: ");
                wager = Int32.Parse(Console.ReadLine());
            }
            while (wager > chips)
            {
                Console.WriteLine("Sorry, please input a wager above 0 and below your remaining chips: ");
                wager = Int32.Parse(Console.ReadLine());
            }
        }

        public void win()
        {
            chips += wager;
        }

        public void loss()
        {
            chips -= wager;
        }

        public int getChips()
        {
            return chips;
        }

        public void resetChips()
        {
            chips = startingChips;
            resetCount++;
        }
        public int getResets()
        {
            return resetCount;
        }
        public string getName()
        {
            return playerName;
        }
    }

    class Round
    {
        static int Rounds;
        static int RoundsWon;
        int playersPoint;
        public Round()
        {
            Rounds++;

            Console.WriteLine("Current Round: " + Rounds);
        }

        public static void addWin()
        {
            RoundsWon++;
        }
        public static int getWins()
        {
            return RoundsWon;
        }
        public static int getRounds()
        {
            return Rounds;
        }

        public void setPlayersPoint(int point)
        {
            playersPoint = point;
        }

        public int getPlayersPoint()
        {
            return playersPoint;
        }
    }

    class CrapsGame
    {
        bool playing;

        Player player = new Player();
        Dice dieOne = new Dice();
        Dice dieTwo = new Dice();
        public void startGame()
        {
            playing = true;

            while (playing)
            {
                Round round = new Round();
                player.bet();
                dieOne.roll();
                dieTwo.roll();
                showFaces();
                checkRolls(round);

            }

        }

        void showFaces()
        {
            Console.WriteLine("First Die: " + dieOne.face);
            Console.WriteLine("Second Die: " + dieTwo.face);
            Console.WriteLine("You rolled a: " + (dieTwo.face + dieOne.face));
        }

        void checkRolls(Round round)
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
                default:
                    round.setPlayersPoint(dieOne.face + dieTwo.face);
                    Console.WriteLine("Players point: " + round.getPlayersPoint());
                    reRoll(round);
                    break;



            }
        }

        void reRoll(Round round)
        {
            while (((dieOne.face + dieTwo.face) != round.getPlayersPoint()) || (dieOne.face + dieTwo.face) != 7)
            {
                Console.WriteLine("No point! must reroll again.");
                Console.Read();
                dieOne.roll();
                dieTwo.roll();
                showFaces();
                if ((dieOne.face + dieTwo.face) == round.getPlayersPoint())
                {
                    Console.WriteLine("Round Won!");
                    addWin();
                    break;
                }
                else if ((dieOne.face + dieTwo.face) == 7)
                {
                    Console.WriteLine("Round Loss!");
                    addLoss();
                    break;
                }
                else
                {

                }
            }
        }

        void addWin()
        {
            Round.addWin();
            player.win();
            checkContinue();
        }
        void addLoss()
        {
            player.loss();
            if (player.getChips() == 0)
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
            else
            {
                checkContinue();
            }
        }

        void gameover()
        {
            playing = false;
            Console.WriteLine("Game quit, thank you for playing!");
            Console.WriteLine("Player: " + player.getName());
            Console.WriteLine("Final Chips: " + player.getChips());
            Console.WriteLine("Rounds played: " + Round.getRounds());
            Console.WriteLine("Rounds Won: " + Round.getWins());
            Console.WriteLine("Chip Resets: " + player.getResets());


        }

        void checkContinue()
        {
            Console.WriteLine("Another Round? y/n");
            if ((Console.ReadLine().ToLower().Equals("n")))
            {
                gameover();
            }
            else
            {
                Console.WriteLine("Starting Round " + Round.getRounds());
            }
        }
    }
}

