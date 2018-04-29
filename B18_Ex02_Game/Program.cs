using System;
using System.Collections.Generic;
using System.Text;
using B18_Ex02_Game_Controller;

namespace B18_Ex02_Game
{
	public class Program
	{
		public static void Main()
		{
			RunGame();
			Console.ReadLine();
		}

		public static void RunGame()
		{
			GameController game = new GameController();
			game.InitializePlayers();
			game.InitializeBoard();
			game.InitializeViewBoard();
			game.PrintBoard();
			game.PlayCurrentTurn();
			
		}
	}
}
