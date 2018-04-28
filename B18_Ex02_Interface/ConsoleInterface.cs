using System;
using System.Collections.Generic;
using System.Text;
using Ex02.ConsoleUtils;
using B18_Ex02_Data;

namespace B18_Ex02_Interface
{
	/*
	public class Program
	{
		public static void Main()
		{
			ConsoleInterface test = new ConsoleInterface();
			char[,] board = new char[8, 8] {
				{' ','O',' ','O',' ','O',' ', 'O'},
				{'O',' ','O',' ','O',' ','O', ' '},
				{' ','O',' ','O',' ','O',' ', 'O'},
				{' ',' ',' ',' ',' ',' ',' ',' ' },
				{' ',' ',' ',' ',' ',' ',' ',' ' },
				{'X',' ','X',' ','X',' ','X',' ' },
				{' ','X',' ','X',' ','X',' ','X' },
				{'X',' ','X',' ','X',' ', 'X',' '}
			};
			test.PrintBoard(board);
			Console.ReadLine();
		}
	}
	*/

	public class ConsoleInterface
	{
		private char[,] m_GameBoard;
		private string m_LastAction;

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
	}
}