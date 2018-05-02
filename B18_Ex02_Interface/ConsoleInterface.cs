using System;
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

		public void TurnInformation(string i_CurrentPlayerName, char i_CurrentPlayerSymbol, string i_PreviousPlayerName, char i_PreviousPlayerSymbol)
		{
			if(m_LastAction != null)
			{
				Console.WriteLine("{0}'s move was ({1}): {2}", i_PreviousPlayerName, i_PreviousPlayerSymbol, m_LastAction);
			}
			Console.WriteLine("{0}'s turn ({1}): ", i_CurrentPlayerName, i_CurrentPlayerSymbol);
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

		public string getPlayerInput(List<string> i_LegalInputs)
		{
			string playerInput;

			playerInput = Console.ReadLine();
			while(!i_LegalInputs.Contains(playerInput))
			{
				PrintError(eErrors.InvalidMoveInput);
				playerInput = Console.ReadLine();
			}

			return playerInput;
		}

		public enum eErrors { InvalidBoardInput = 1, InvalidMoveInput = 2, InvalidPieceMove = 3}

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
			}
		}
	}
}