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

		public static bool init_session = true;
		public static bool init_game = true;
		public static float wins = 0; 
		public static float loses = 0;		
		private static readonly Random getrandom = new Random();
		
		public static void Main (string[] args){

		    Console.WriteLine("Hello and welcome to Oldman\'s Casino!!");
			Console.WriteLine();
			string[] figures = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"};
			string[] colors = {"Hearts", "Diamonds", "Clubs", "Spades"};
			
			
			while(TheGame.init_session)
			{			
				float ratio = wins / (wins+loses);
				
				Console.WriteLine("   __________");
				Console.Write("   New game!! ");
				if ((wins == 0) && (loses == 0))
				{ Console.WriteLine("Fresh start... Lets see what luck brings you today!!"); }
				else
				{ Console.WriteLine(" >>Current run: {0} wins to {1} loses ({2:F2} W/L ratio)", wins, loses, ratio); }
								
				List<Card> deck = new List<Card>();
				List<Card> player_hand = new List<Card>();
				List<Card> dealer_hand = new List<Card>();
				
				deck = MakeDeck(figures, colors);
				
				Console.WriteLine("   __________");
				Deal(deck, player_hand, dealer_hand);	
				CardsInHand(player_hand); Console.WriteLine();
				Console.WriteLine("Points: {0}", PointCounter(player_hand));						
				Console.WriteLine("   __________");
				
				TheGame.init_game = true;
				
				while(TheGame.init_game) 
					{game(deck, player_hand, dealer_hand);}
				if (TheGame.init_session == true)
				{
					Console.Write("Press any key to proceed...");
					Console.ReadKey();
					Console.WriteLine();
				}
		    }	
		}
		
		public static void game (List<Card> aDeck, List<Card> aPlayer_hand, List<Card> aDealer_hand)
		{

			string user_choice;
						
			if ( PointCounter(aPlayer_hand) > 21)
			{
				CardsInHand(aPlayer_hand);
				Console.WriteLine("You scored {0} and thus you lose...", PointCounter(aPlayer_hand));
				loses++;
				init_game = false;
			}
			else if (PointCounter(aPlayer_hand) == 21)
			{			

				CardsInHand(aPlayer_hand);
				Console.WriteLine("You scored {0} and thus you win!!", PointCounter(aPlayer_hand));
				wins++;
				init_game = false;				
			}
			
			if (init_game == true) {
				Console.WriteLine("1 - hit | 2 - check | 3 - deal | e - quit game");
				Console.Write(". . . ");
				user_choice = Console.ReadLine();		
			
				switch (user_choice) {

					case "1":
						Console.Write("  >>Hitting...");
						Hit(aDeck, aPlayer_hand); 				
						CardsInHand(aPlayer_hand); Console.WriteLine();
						Console.WriteLine("Current points: {0}", PointCounter(aPlayer_hand));	
						break;
						
					case "2":
						Console.Write("  >>Checking...");
						Console.WriteLine();
						Console.WriteLine("You were dealt:");
						CardsInHand(aPlayer_hand);
						Console.WriteLine("Dealer was dealt:");
						CardsInHand(aDealer_hand);
						Console.WriteLine("You scored: {0}", PointCounter(aPlayer_hand));
						Console.WriteLine("Dealer scored: {0}", PointCounter(aDealer_hand));
						if (PointCounter(aPlayer_hand) > PointCounter(aDealer_hand)) {
							Console.WriteLine("You win!!");
							wins++;
							init_game = false;	
						}
						else if (PointCounter(aPlayer_hand) < PointCounter(aDealer_hand)) {
							Console.WriteLine("You lose!!");
							loses++;
							init_game = false;	
						}
						else {
							Console.WriteLine("Its a draw!");
							init_game = false;
						}
						break;

					case "3":
						Console.Write("  >>New deal...");
						TheGame.init_game = false;
						break;
					
					case "e":
						Console.WriteLine("Quitting game...");
						TheGame.init_game = false;
						TheGame.init_session = false;
						break;	
						
					default:
						Console.WriteLine("Oops! Wrong choice...");
						break;
				}
			}
		}

		public static void Hit (List<Card> aDeck, List<Card> aHand) {
			int a = Shuffler(aDeck);
			int b = aHand.Count;
			aHand.Add(aDeck[a]);
			aDeck.Remove(aDeck[a]);
		}
		

		public static void Deal (List<Card> aDeck, List<Card> aPlayer_hand, List<Card> aDealer_hand) {
			Console.Write("  Fresh deal: ");
			Hit(aDeck, aPlayer_hand);
			Hit(aDeck, aDealer_hand);
			Hit(aDeck, aPlayer_hand);
			Hit(aDeck, aDealer_hand);
		}

		public static int Shuffler (List<Card> aDeck)
		{
			lock(getrandom) // synchronize
			{
				return getrandom.Next(0, aDeck.Count);
			}
		}

		public static int PointCounter (List<Card> aHand) {
			int points, cards;
			points = 0;
			cards = aHand.Count - 1;
			
			while (cards >= 0) 
			{points+=aHand[cards].point; cards--;}

			if ( (HasAce(aHand) == true) && (points > 21) )
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

			for (int i = 0; i<cards; i++)
			{Console.Write(card_names[i]+", ");}
			Console.WriteLine();
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
		
		public static List<Card> MakeDeck (string[] aFigures, string[] aColors) {
			List<Card> deck = new List<Card>();
			foreach (string clr in aColors) {foreach (string fig in aFigures ) {deck.Add(new Card(fig, clr)); } }
			return deck;
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
