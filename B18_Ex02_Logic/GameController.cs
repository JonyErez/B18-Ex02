using System;
using System.Collections.Generic;
using System.Text;
using B18_Ex02_Data;
using B18_Ex02_Interface;

namespace B18_Ex02_Game_Controller
{
	public class GameController
	{
		private ConsoleInterface m_View = new ConsoleInterface();
		private Game m_Model = new Game();

		public void InitializeViewBoard()
		{
			m_View.GameBoard = new char[m_Model.BoardSize,m_Model.BoardSize];
			for (int row=0; row<m_Model.BoardSize; row++)
			{
				for (int col=0; col<m_Model.BoardSize; col++)
				{
					m_View.GameBoard[row, col] = m_Model.GetSymbol(new Point(row, col));
				}
			}
		}

	}
}
