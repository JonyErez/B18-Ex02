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

			game.InitializeGame();
			do
			{
				do
				{
					game.PrintBoard();
					game.PlayCurrentTurn();
				} while (!game.IsGameOver());
				game.GameOver();
				game.IsAnotherRound();
			} while (game.PlayAnotherRound);

		}
	}
}
