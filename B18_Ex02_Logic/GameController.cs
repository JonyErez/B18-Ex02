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
	}
}
