using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class GameBoard
	{
		private uint m_BoardSize;
		private GamePiece[,] m_Board;

		public uint Size
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
	}
}
