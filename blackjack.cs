using System;
using System.Collections.Generic;

	// figures are 2, 3, 4, 5, 6, 7, 8, 9, 10, Jack, Queen, King, Ace
	// colors are Spades, Clubs, Diamonds, Hearts
	// each card is a product of figure and color, eg 2 of Spades
	// each figure has its points: 2-10 respectively, J, Q and K are 10, Ace is either 1 or 11
	// Hence a class for cards has: Figure, Color, Point

	// deck is composed as an array of cards
	// deck order has to be randomized each deal
	
	// each game has to start by randomizing the deck and then dealing cards player-dealer-player-dealer
	// each dealt card has to be removed from the deck
	// player doesnt know dealers hand
	// possible choices for dealer (randomized): hit, hold
	// possible choices for player: hit, hold
	
	
namespace BlackJack

{
	class TheGame
	{
		public static bool init_game = true;

		public static void Main (string[] args)
		{
		    Console.WriteLine("Hello and welcome to Oldman\'s Casino!!");
			Console.WriteLine();
			string[] figures = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"};
			string[] colors = {"Hearts", "Diamonds", "Clubs", "Spades"};

			List<Card> deck = new List<Card>();
			List<Card> player_hand = new List<Card>();
			List<Card> dealer_hand = new List<Card>();

			foreach (string clr in colors)
			{foreach (string fig in figures)
					{deck.Add(new Card(fig, clr));}
			}
			
			Console.WriteLine("Dealing...");
			Deal(deck, player_hand, dealer_hand);	
			while(TheGame.init_game)
			{ game(deck, player_hand, dealer_hand); }	


		}

		public static void game (List<Card> aDeck, List<Card> aPlayer_hand, List<Card> aDealer_hand)
		{
			string user_choice;
			Console.WriteLine("Pick your choice: 1 - hit | 2 - hold | e - exit");
			Console.Write(". . . ");
			user_choice = Console.ReadLine();
			switch (user_choice)
			{
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
						Console.WriteLine("Points of hand: {0}", PointCounter(aPlayer_hand));						
						break;
					}
					
				case "2":
					Console.WriteLine(">>Holding...");

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

		public static void Hit (List<Card> aDeck, List<Card> aHand)
		{
			int a = Shuffler(aDeck);
			int b = aHand.Count;

			aHand.Add(aDeck[a]);
			aDeck.Remove(aDeck[a]);
			Console.WriteLine("New card in hand: {0}", aHand[b].name);
			CardsInHand(aHand);
			Console.WriteLine();

			Console.WriteLine("end of hit");
		}
		
		public static void Hold (List<Card> aHand)
		{
			CardsInHand(aHand);
			Console.WriteLine();
			Console.WriteLine("Points of hand: {0}", PointCounter(aHand));
			Console.WriteLine("end of hold");
			
		}

		public static void Deal (List<Card> aDeck, List<Card> aPlayer_hand, List<Card> aDealer_hand)
		{
			Console.WriteLine(">>Dealing...");
			
			Console.WriteLine("Dealing player...");
			Hit(aDeck, aPlayer_hand);
			
			Console.WriteLine("Dealing player...");
			Hit(aDeck, aDealer_hand);
			
			Console.WriteLine("Dealing player...");
			Hit(aDeck, aPlayer_hand);
			
			Console.WriteLine("Dealing player...");
			Hit(aDeck, aDealer_hand);		

			Console.WriteLine("end of deal");
		}

		public static int Shuffler (List<Card> aDeck)
		{
			Random getrand = new Random();
			return getrand.Next(0, aDeck.Count);
		}

		public static int PointCounter (List<Card> aHand)
		{
			int points, cards;
			points = 0;
			cards = aHand.Count - 1;
			while (cards >= 0) 
			{points+=aHand[cards].point; cards--;}
			HasAce();
			return points;
		} 

		public static void CardsInHand (List<Card> aHand)
		{
			List<string> card_names = new List<string>();
			int cards = aHand.Count;
			if (aHand.Count != 0)
			{
				for (int i = 0; i < cards; i++ )
				{ card_names.Add(aHand[i].name);}
			}
			else{ Console.WriteLine("empty hand"); }
		
			Console.Write("Cards in hand: ");	
			for (int i = 0; i<cards; i++)
			{Console.Write(card_names[i]+", ");}
		}

		public static void HasAce () //  List<Card> aHand make it return true if a given hand has an ace ; alternatively return points -10 <=> (HasAce ^ points>21); should be part of point counter function
		{
			Console.WriteLine("Has Ace?");
		}


	}

	class Card
	{
		public string figure, color, name;
		public int point;

		public Card (string aFigure, string aColor)
		{
			figure = aFigure;
			color = aColor;
			if (aFigure.Length == 1) { point = Convert.ToInt32(aFigure);}
			else if (aFigure.Length == 3) { point = 11;}
			else { point = 10;}
			name = aFigure + " of " + aColor;
		}

	}

}

