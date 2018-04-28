using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	public class Game
	{
		private GameBoard m_Board;
		private Player[] m_Players = new Player[2];

		public int BoardSize
		{
			get
			{
				return m_Board.Size;
			}
		}

		public char GetSymbol(Point i_Coordinates)
		{
			return m_Board.GetSymbol(i_Coordinates);
		}
	}
}
