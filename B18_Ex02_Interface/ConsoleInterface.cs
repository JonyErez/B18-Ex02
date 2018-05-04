﻿using System;
using System.Collections.Generic;
using System.Text;
using Ex02.ConsoleUtils;
using B18_Ex02_Data;

namespace B18_Ex02_Interface
{
	public class ConsoleInterface
	{
		private char[,] m_GameBoard;
		private string m_LastAction = null;

		public bool AskForAnotherRound()
		{
			char playerInputForAnotherRound;

			do
			{
				Console.WriteLine(
	String.Format(@"Do you wish to play another round (Y\N)? "));

				while (!char.TryParse(Console.ReadLine(), out playerInputForAnotherRound))
				{
					PrintError(eErrors.InvalidInput);
					Console.WriteLine(
		String.Format(@"Do you wish to play another round (Y\N)? "));

				}
			} while (!(char.ToUpper(playerInputForAnotherRound) == 'Y') && !(char.ToUpper(playerInputForAnotherRound) == 'N'));

			return char.ToUpper(playerInputForAnotherRound) == 'Y';
		}

		public void PrintWinner(string i_Winner, char i_WinnerSymbol, uint i_WinnerScore)
		{
			Console.WriteLine(String.Format(@"{0} ({1}) Won! with the score of {2}.", i_Winner, i_WinnerSymbol, i_WinnerScore));
		}

		public void PrintTie()
		{
			Console.WriteLine("Its a tie!");
		}

		public void PrintGameOver(string i_PlayerOneName, uint i_PlayerOneScore, string i_PlayerTwoName, uint i_PlayerTwoScore)
		{
			Console.WriteLine(
String.Format(@"The current scores are:
{0}'s score is: {1}
{2}'s score is: {3}", i_PlayerOneName, i_PlayerOneScore, i_PlayerTwoName, i_PlayerTwoScore));
		}

		public void TurnInformation(string i_CurrentPlayerName, char i_CurrentPlayerSymbol, string i_PreviousPlayerName, char i_PreviousPlayerSymbol)
		{
			if(m_LastAction != null)
			{
				Console.WriteLine("{0}'s move was ({1}): {2}", i_PreviousPlayerName, i_PreviousPlayerSymbol, m_LastAction);
			}
			Console.Write("{0}'s turn ({1}): ", i_CurrentPlayerName, i_CurrentPlayerSymbol);
		}

		public string askPlayerName(int i_PlayerNumber)
		{
			Console.WriteLine("Please enter player {0}s name: ", i_PlayerNumber == 0 ? "one" : "two");
			return Console.ReadLine();
		}

		public int AskGameBoardSize()
		{
			int gameBoardSize;
			bool legalValue;

			Console.WriteLine("Please enter the game board size (6, 8, 10): ");
			legalValue = int.TryParse(Console.ReadLine(), out gameBoardSize);
			while (!legalValue || (gameBoardSize != 6 && gameBoardSize != 8 && gameBoardSize != 10))
			{
				PrintError(ConsoleInterface.eErrors.InvalidBoardInput);
				legalValue = int.TryParse(Console.ReadLine(), out gameBoardSize);
			}
			return gameBoardSize;
		}

		public void ClearScreen()
		{
			Ex02.ConsoleUtils.Screen.Clear();
		}

		public char[,] GameBoard
		{
			get
			{
				return m_GameBoard;
			}

			set
			{
				m_GameBoard = value;
			}
		}

		public int GameBoardSize
		{
			get
			{
				return m_GameBoard.GetLength(0);
			}
		}

		public string LastAction
		{
			get
			{
				return m_LastAction;
			}
			
			set
			{
				m_LastAction = value;
			}
		}

		public int AskHowManyPlayers()
		{
			int amountOfPlayers = 0;
			Console.Write("How many players will be playing? (1 or 2): ");
			while (!int.TryParse(Console.ReadLine(), out amountOfPlayers))
			{
				if (amountOfPlayers != 1 && amountOfPlayers != 2)
				{
					PrintError(eErrors.InvalidAmountOfPlayers);
				}
			}
			return amountOfPlayers;
		}

		public void PrintBoard()
		{
			int boardSize = m_GameBoard.GetLength(0);
			printTopRowIndicators(boardSize);
			printSeperatorLine(boardSize);
			for (int currentRow = 0; currentRow < boardSize; currentRow++)
			{
				printCurrentBoardRow(currentRow, boardSize);
				printSeperatorLine(boardSize);
			}

		}

		private void printTopRowIndicators(int i_Size)
		{
			for (int i = 0; i < i_Size; i++)
			{
				Console.Write("   {0}", (char)('A' + i));
			}

			Console.Write(System.Environment.NewLine);

		}

		private void printSeperatorLine(int boardSize)
		{
			Console.Write(" ");
			for (int i = 0; i < boardSize; i++)
			{
				Console.Write("====");
			}
			Console.Write("=");
			Console.Write(System.Environment.NewLine);

		}

		private void printCurrentBoardRow(int i_CurrentRow, int i_Size)
		{
			StringBuilder currentRow = new StringBuilder();
			currentRow.Append((char)('a' + i_CurrentRow));
			for (int i = 0; i < i_Size; i++)
			{
				currentRow.Append(string.Format("| {0} ", m_GameBoard[i_CurrentRow, i]));
			}
			currentRow.Append('|');
			Console.WriteLine(currentRow);
		}

		public bool getPlayerInput(List<string> i_LegalInputs, bool i_CanPlayerQuit, out int o_PlayerSelectedMove)
		{
			string playerInput;
			bool didPlayerQuit = false;

			o_PlayerSelectedMove = 0;
			playerInput = Console.ReadLine();
			while(!i_LegalInputs.Contains(playerInput) && (playerInput != "Q" || !i_CanPlayerQuit))
			{
				PrintError(eErrors.InvalidMoveInput);
				playerInput = Console.ReadLine();
			}

			didPlayerQuit = playerInput == "Q";
			if(!didPlayerQuit)
			{
				m_LastAction = playerInput;
				o_PlayerSelectedMove = i_LegalInputs.IndexOf(playerInput);
			}

			return didPlayerQuit;
		}

		public enum eErrors { InvalidBoardInput = 1, InvalidMoveInput = 2, InvalidPieceMove = 3, InvalidAmountOfPlayers = 4, InvalidInput = 5}

		public void PrintError(eErrors i_Error)
		{
			switch (i_Error)
			{
				case eErrors.InvalidBoardInput:
Console.WriteLine(string.Format(@"Invalid board input!
Please enter a valid game board size(6, 8, 10): "));
					break;
				case eErrors.InvalidMoveInput:
Console.WriteLine(string.Format(@"Invalid move input!
Please enter a valid input in the following format: COLrow>COLrow"));
					break;
				case eErrors.InvalidPieceMove:
					Console.WriteLine("Invalid piece move!");
					break;
				case eErrors.InvalidAmountOfPlayers:
					Console.WriteLine(@"Invalid number of players!
Please enter a valid number of players (1 or 2): ");
					break;
				case eErrors.InvalidInput:
					Console.WriteLine("Invalid input!");
					break;
			}
		}
	}
}