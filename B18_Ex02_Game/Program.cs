using System;
using System.Collections.Generic;
using System.Text;
using B18_Ex02_Game_Controller;

namespace B18_Ex02_Game
{
	public class Program
	{
		public	static	void	Main()
		{
			RunGame();
		}

		public	static	void	RunGame()
		{
			GameController checkersGame = new GameController();

			checkersGame.InitializeGame();
			do
			{
				do
				{
					checkersGame.PrintBoard();
					checkersGame.PlayCurrentTurn();
				}
				while (!checkersGame.IsGameOver());
				checkersGame.GameOver();
				checkersGame.IsAnotherRound();
			}
			while (checkersGame.PlayAnotherRound);
			checkersGame.EndGame();
		}
	}
}
