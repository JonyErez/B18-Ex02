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
			while (true)
			{
				game.UpdateBoard();
				game.PrintBoard();
				game.PlayCurrentTurn(); 
			}
			//Needs to be continue

			//Console.WriteLine("GAME IS OVER");

		}
	}
}
