using System;
using System.Collections.Generic;

	// figures are 2, 3, 4, 5, 6, 7, 8, 9, 10, Jack, Queen, King, Ace
	// colors are Spades, Clubs, Diamonds, Hearts
	// each card is a product of figure and color, eg 2 of Spades
	// each figure has its points: 2-10 respectively, J, Q and K are 10, Ace is either 1 or 11
	// Hence a class for cards has: Figure, Color, Point
	
	// each game has to start by randomizing the deck and then dealing cards player-dealer-player-dealer
	// possible choices for dealer (randomized): hit, hold
	// possible choices for player: hit, hold
	
	
namespace BlackJack{

	class TheGame{

		public static bool init_game = true;

		public static void Main (string[] args){

		    Console.WriteLine("Hello and welcome to Oldman\'s Casino!!");
			Console.WriteLine();
			string[] figures = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"};
			string[] colors = {"Hearts", "Diamonds", "Clubs", "Spades"};

			List<Card> deck = new List<Card>();
			List<Card> player_hand = new List<Card>();
			List<Card> dealer_hand = new List<Card>();

			foreach (string clr in colors) {foreach (string fig in figures ) {deck.Add(new Card(fig, clr)); } }

			Deal(deck, player_hand, dealer_hand);	
			while(TheGame.init_game)
			{ game(deck, player_hand, dealer_hand); } }	

		public static void game (List<Card> aDeck, List<Card> aPlayer_hand, List<Card> aDealer_hand){

			string user_choice;
			Console.WriteLine("Pick your choice: 1 - hit | 2 - hold | e - exit");
			Console.Write(". . . ");
			user_choice = Console.ReadLine();
			switch (user_choice) {

				case "1":
					Console.WriteLine(">>Hitting...");
					if (aDeck.Count == 0)
					{
						Console.WriteLine("Empty deck!"); 
						break;
					}
					else
					{ 
						Hit(aDeck, aPlayer_hand); 				
						CardsInHand(aPlayer_hand); Console.WriteLine();
						Console.WriteLine("Points of hand: {0}", PointCounter(aPlayer_hand));						
						break;
					}
					
				case "2":
					Console.WriteLine(">>Holding...");					
					CardsInHand(aPlayer_hand); Console.WriteLine();
					Hold(aPlayer_hand);
					break;
					
				case "e":
					Console.WriteLine(">>Exiting...");
					TheGame.init_game = false;
					break;
					
				default:
					Console.WriteLine("Oops! Wrong choice...");
					break;
			}
		}	

		public static void Hit (List<Card> aDeck, List<Card> aHand) {

			int a = Shuffler(aDeck);
			int b = aHand.Count;

			aHand.Add(aDeck[a]);
			aDeck.Remove(aDeck[a]);
		}
		
		public static void Hold (List<Card> aHand) {

			CardsInHand(aHand);
			Console.WriteLine();
			Console.WriteLine("Points of hand: {0}", PointCounter(aHand));
		}

		public static void Deal (List<Card> aDeck, List<Card> aPlayer_hand, List<Card> aDealer_hand) {

			Console.WriteLine(">>Dealing...");

			Hit(aDeck, aPlayer_hand);
			Hit(aDeck, aDealer_hand);
			Hit(aDeck, aPlayer_hand);
			Hit(aDeck, aDealer_hand);
		}

		public static int Shuffler (List<Card> aDeck) {

			Random getrand = new Random();
			return getrand.Next(0, aDeck.Count);
		}

		public static int PointCounter (List<Card> aHand) {
			
			Console.WriteLine(">>Counting...");
			
			int points, cards;
			points = 0;
			cards = aHand.Count - 1;
			
			while (cards >= 0) 
			{points+=aHand[cards].point; cards--;}

			if ( (HasAce(aHand) == true) && (points > 20) )
			{points -= 10;}
		
			return points;
		} 

		public static void CardsInHand (List<Card> aHand) {

			List<string> card_names = new List<string>();
			int cards = aHand.Count;
			if (aHand.Count != 0) {

				for (int i = 0; i < cards; i++ )
				{ card_names.Add(aHand[i].name);}
			}

			Console.Write("Cards in hand: ");	// to be removed??
			for (int i = 0; i<cards; i++)
			{Console.Write(card_names[i]+", ");}
		}

		public static Boolean HasAce (List<Card> aHand) {	
			
			Boolean hasace = false;
			int cards = aHand.Count;
			
			for (int i = 0; i < cards; i++)	{ 					
				if (aHand[i].figure == "Ace" )
					{hasace = true; break;}
			}

			return hasace;
		}
		
		public static int GameState (List<Card> aHand) { //checks points of the player's hand
			return 1;
		}	
	}

	class Card {

		public string figure, color, name;
		public int point;

		public Card (string aFigure, string aColor) {

			figure = aFigure;
			color = aColor;
			if (aFigure.Length == 1) { point = Convert.ToInt32(aFigure);}
			else if (aFigure.Length == 3) { point = 11;}
			else { point = 10;}
			name = aFigure + " of " + aColor;
		}
	}
}