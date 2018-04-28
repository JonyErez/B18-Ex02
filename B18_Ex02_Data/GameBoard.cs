using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class GameBoard
	{
		private int m_BoardSize;
		private GamePiece[,] m_Board;

		public int Size
		{
			set
			{
				m_BoardSize = value;
			}

			get
			{
				return m_BoardSize;
			}
		}

		public char GetSymbol(Point i_Coordinates)
		{
			char symbol = ' ';
			if (m_Board[i_Coordinates.X, i_Coordinates.Y] != null)
			{
				symbol = m_Board[i_Coordinates.X, i_Coordinates.Y].Symbol;
			}
			return symbol;
		}
	}
}
