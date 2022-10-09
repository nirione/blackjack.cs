using System;
using System.Collections.Generic;
	
		// blackjack card game (simplified) written by Jack Oldman in 2022 as a learning opportunity for coding in C#
		// game can possibly be improved by creating another class for Hand, which would consist of points, separate cards etc.
		// Copyright by jackoldman, version 1.0 released 9.10.2022
		
namespace BlackJack {

	class TheGame {
		public static bool init_session = true;		//defining session and game states
		public static bool init_game    = true;

		public static float wins =  0;		//defining statistical parameters
		public static float loses = 0;	
		public static float draws = 0;
		public static float folds = 0;
		public static float ratio = 0;
		public static float games = 0;
		
		private static readonly Random getrandom = new Random();
		
		public static void Main (string[] args) {

		    Console.WriteLine("Hello and welcome to Oldman\'s Casino!!");		// welcoming message
			Console.WriteLine();
			string[] figures = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"};		// defining figures and colors
			string[] colors = {"Hearts", "Diamonds", "Clubs", "Spades"};			
			
			while(TheGame.init_session) {
				games = wins + loses + draws;
				ratio = (wins / (games))*100;
				
				Console.Clear();	
				Console.WriteLine(">>>");
				Console.WriteLine("   New game!! ");
				if (games == 0)
				{ Console.WriteLine("Fresh start... Lets see what luck brings you today!!"); }
				else
				{ Console.WriteLine("Current run: {0} wins to {1} loses ({2:F2}% W/L ratio) with total games played {3} (including {4} draws and {5} folds)", wins, loses, ratio, games, draws, folds); }
								
				List<Card> deck        = new List<Card>();                              // creating empty lists of Card objects for later use
				List<Card> player_hand = new List<Card>();
				List<Card> dealer_hand = new List<Card>();
				
				deck = MakeDeck(figures, colors);                                       // creating a deck of cards using previously defined arrays of strings
				
				Console.WriteLine();
				Deal(deck, player_hand, dealer_hand);                                   // dealing the first 4 cards
				CardsInHand(player_hand); Console.WriteLine();                          // displaying the cards of the player
				Console.WriteLine("Points: {0}", PointCounter(player_hand));			// printing points scored by the player			
				Console.WriteLine();
				
				TheGame.init_game = true;												// initializing game
				
				while(TheGame.init_game) 												// while loop responsible for the continuous gameplay, refreshes everytime a game is terminated by the switch statement
					{game(deck, player_hand, dealer_hand);}								// creates the game
					
				if (TheGame.init_session == true)										// pauses the game for the player to read the results and clear the window 
				{
					Console.Write(" Press any key to proceed...");
					Console.ReadKey();
					Console.WriteLine();
					Console.Clear();	
				}
		    }	
		}
		
		public static void game (List<Card> aDeck, List<Card> aPlayer_hand, List<Card> aDealer_hand) {     	// main game function
			string user_choice;
			
			if ( PointCounter(aPlayer_hand) > 21) {															// checks if player exceeded the allowed 21 point limit, terminates the game if true and increments loses
				Console.WriteLine("You scored {0} and thus you lose...", PointCounter(aPlayer_hand));
				loses++;
				init_game = false;
			}
			else if (PointCounter(aPlayer_hand) == 21) {													// checks if player scored 21 points, terminates the game if true and increments wins
				Console.WriteLine("You scored {0} and thus you win!!", PointCounter(aPlayer_hand));
				wins++;
				init_game = false;				
			}			
			else {			
				Console.WriteLine("1 - hit | 2 - check | 3 - fold | e - quit game");						// displays possible choices for the player
				Console.Write(". . . ");
			}
			
			if (init_game == true) {																		// if statement allowing a game to occur
				user_choice = Console.ReadLine();	
						
				switch (user_choice) {																		// switch statement allowing the player's choice to influence the gameplay
					case "1":																				// case for player hit, deals a card to the player, calls DealerMove method, displays player's cards and points
						Console.Write("  >>Hitting...");
						Hit(aDeck, aPlayer_hand);
						DealerMove(aDeck, aDealer_hand);
						CardsInHand(aPlayer_hand); Console.WriteLine();
						Console.WriteLine("Score: {0} points.", PointCounter(aPlayer_hand));
						Console.WriteLine();
						break;
						
					case "2":																				// case for check, displays player's and dealer's cards and points, compares them and increments wins or losses respectively
						Console.Write("  >>Checking...");
						Console.WriteLine();
						Console.Write("You were dealt: ");
						CardsInHand(aPlayer_hand); Console.WriteLine();
						Console.Write("Dealer was dealt: ");
						CardsInHand(aDealer_hand); Console.WriteLine();
						Console.WriteLine("You scored {0} points to dealer's {1} points...", PointCounter(aPlayer_hand), PointCounter(aDealer_hand));
						if (PointCounter(aPlayer_hand) > PointCounter(aDealer_hand)) {
							Console.WriteLine("You win!!");
							wins++;
							init_game = false;	
						}
						else if (PointCounter(aPlayer_hand) < PointCounter(aDealer_hand) && (PointCounter(aDealer_hand) <= 21)) {		// checks if the player has less points than the dealer AND if the dealer has not exceeded the 21 point limit
							Console.WriteLine("You lose!!");
							loses++;
							init_game = false;	
						}						
						else if (PointCounter(aPlayer_hand) < PointCounter(aDealer_hand) && (PointCounter(aDealer_hand) > 21)) {		// checks if the player has less points than the dealer AND if the dealer has exceeded the 21 point limit
							Console.WriteLine("You win!!");
							wins++;
							init_game = false;	
						}
						else {
							Console.WriteLine("Its a draw!");
							draws++; init_game = false;
						}
						break;

					case "3":																				// case allowing the player to fold, increments folds and terminates the game
						Console.Write("   >>Folding...");
						folds++; TheGame.init_game = false;
						break;
					
					case "e":																				// case enabling the player to terminate the program through an 'are you sure' prompt
					Console.WriteLine("Are you sure you want to quit? [Y/N]");
					string prompt_quit = Console.ReadLine();
					
					if ((prompt_quit == "Y") || (prompt_quit == "y") || (prompt_quit == "e")) {
						Console.WriteLine("  Quitting game...");
						TheGame.init_game = false;
						TheGame.init_session = false;
						break;	
					}
					else { Console.WriteLine("Continuing..."); break; }
					
					case "q":																				// case forcing exit from the session (terminates the program)
						Console.WriteLine("  Quitting game...");
						TheGame.init_game = false;
						TheGame.init_session = false;
						break;	
						
					default:																				// default case removing the possibility of break if the player inputs a wrong case 
						Console.WriteLine("Oops! Wrong choice...");
						Console.WriteLine();
						Console.WriteLine("Current run: {0} wins to {1} loses ({2:F2} W/L ratio)", wins, loses, ratio);
						Console.Write("Current hand: "); CardsInHand(aPlayer_hand);
						Console.Write("scoring {0} points.", PointCounter(aPlayer_hand));
						Console.WriteLine();
						break;
				}
			}
		}

		public static void Hit (List<Card> aDeck, List<Card> aHand) {		// takes the deck and a hand as an input, generates a random card from the deck, adds it to the given hand and removes it from the deck
			int a = Shuffler(aDeck);
			int b = aHand.Count;
			aHand.Add(aDeck[a]);
			aDeck.Remove(aDeck[a]);			
		}
		
		public static void Deal (List<Card> aDeck, List<Card> aPlayer_hand, List<Card> aDealer_hand) {  // basic deal method, deals cards in the order Player-Dealer-Player-Dealer
			Console.Write("   Fresh deal: ");
			Hit(aDeck, aPlayer_hand);
			Hit(aDeck, aDealer_hand);
			Hit(aDeck, aPlayer_hand);
			Hit(aDeck, aDealer_hand);
		}

		public static int Shuffler (List<Card> aDeck) {  // generates a random number from 0 to X, where X is the number of cards in the deck, returns an integer
			lock(getrandom) // synchronize
			{ return getrandom.Next(0, aDeck.Count); }
		}

		public static int PointCounter (List<Card> aHand) {		// counts points in a given hand, returns an integer 
			int points, cards;
			points = 0;
			cards = aHand.Count - 1;
			
			while (cards >= 0) 
			{points+=aHand[cards].point; cards--;}

			if ( (HasAce(aHand) == true) && (points > 21) ) {points -= 10;}		// calls the HasAce method to check if the hand has an ace in it, subtracts 10 points from the hand (aces can count as either 11 or 1 points)
			return points;
		} 

		public static void CardsInHand (List<Card> aHand) {		// displays cards in a given hand
			List<string> card_names = new List<string>();
			int cards = aHand.Count;
			if (aHand.Count != 0) {
				for (int i = 0; i < cards; i++ )
				{ card_names.Add(aHand[i].name);}
			}

			for (int i = 0; i<cards; i++)
			{Console.Write(card_names[i]+", ");}
		}

		public static Boolean HasAce (List<Card> aHand) {		// checks if an Ace is present in a given hand, returns a Boolean
			Boolean hasace = false;
			int cards = aHand.Count;
			for (int i = 0; i < cards; i++)	{		// for loop which checks for an existing ace within a given hand, stops when meets a first ace				
				if (aHand[i].figure == "Ace" )
					{hasace = true; break;}
			}
			return hasace;
		}
		
		public static List<Card> MakeDeck (string[] aFigures, string[] aColors) {		// creates a deck of cards according to the given set of input arrays of strings, returns a list of Card objects
			List<Card> deck = new List<Card>();
			foreach (string clr in aColors) {foreach (string fig in aFigures ) {deck.Add(new Card(fig, clr)); } }
			return deck;
		}	
		
		public static void DealerMove (List<Card> aDeck, List<Card> aDealer_hand) {		// decides whether or not to hit for dealer, hits if dealer's points are below 15
			if (PointCounter(aDealer_hand) < 15)
				{ Hit(aDeck, aDealer_hand); }
		}
	}

	class Card {		// Card class definition for easy creation and later manipulation of Card objects
		public string figure, color, name;
		public int point;
		
		public Card (string aFigure, string aColor) {		// Card class constructor
			figure = aFigure;		
			color = aColor;
			if (aFigure.Length == 1) { point = Convert.ToInt32(aFigure);}		// converts the Figure string into points if length of the string is equal to 1
			else if (aFigure.Length == 3) { point = 11;}						// defines points for Ace
			else { point = 10;}													// defines points for Jack, Queen and King
			name = aFigure + " of " + aColor;									// creates name of the card as a concantenation of Figure and Color string
		}
	}
}
