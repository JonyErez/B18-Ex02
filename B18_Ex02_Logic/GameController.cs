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

		public void PlayCurrentTurn()
		{
			Point source, destination;
			bool needsToEat;
			bool hasEaten;
			if (m_Model.CanPlayerMove(out needsToEat))
			{
				getPlayerInputForFirstTurn(out source, out destination);
				while (!m_Model.FirstMoveInTurn(source, destination, needsToEat, out hasEaten))
				{
					PrintBoard();
					m_View.PrintError(ConsoleInterface.eErrors.InvalidInput);
					getPlayerInputForFirstTurn(out source, out destination);
				}
				m_View.LastAction = string.Format("{0}>{1}", locationToString(source), locationToString(destination));
				afterMoveActions();
				if (hasEaten)
				{
					while (m_Model.HasAnotherLegalMove())
					{
						getPlayerInputForContinuousTurns(m_Model.GetCurrentPieceLocation(), out destination);
						while (!m_Model.ContinuesMove(destination))
						{
							PrintBoard();
							m_View.PrintError(ConsoleInterface.eErrors.InvalidInput);
							getPlayerInputForContinuousTurns(m_Model.GetCurrentPieceLocation(), out destination);
						}
						m_View.LastAction = string.Format("{0}>{1}", locationToString(source), locationToString(destination));
						afterMoveActions();
					}
				}
			}
			m_Model.EndTurn();
		}

		private void afterMoveActions()
		{
			m_Model.CheckAndMakeKing();
			UpdateViewBoard();
			PrintBoard();
		}

		private void getPlayerInputForFirstTurn(out Point o_Source, out Point o_Destination)
		{
			m_View.TurnInformation(m_Model.GetPlayerName(m_Model.PlayerTurn), m_Model.GetPlayerSymbol(m_Model.PlayerTurn),
									m_Model.GetPlayerName(m_Model.otherPlayer()), m_Model.GetPlayerSymbol(m_Model.otherPlayer()));
			string playerInput = Console.ReadLine();
			char maxCapital = (char)(m_Model.BoardSize + 'A' - 1);
			char maxLittle = (char)(m_Model.BoardSize + 'a' - 1);
			string regex = string.Format("^[A-{0}][a-{1}]>[A-{0}][a-{1}]$", maxCapital, maxLittle);
			System.Text.RegularExpressions.Regex checkInput = new System.Text.RegularExpressions.Regex(regex);
			while (!checkInput.IsMatch(playerInput))
			{
				Console.WriteLine("Please enter a valid input: ");
				playerInput = Console.ReadLine();
			}
			stringToLocations(playerInput, out o_Source, out o_Destination);
		}

		private void getPlayerInputForContinuousTurns(Point i_Location, out Point o_Destination)
		{
			m_View.TurnInformation(m_Model.GetPlayerName(m_Model.PlayerTurn), m_Model.GetPlayerSymbol(m_Model.PlayerTurn),
									m_Model.GetPlayerName(m_Model.PlayerTurn), m_Model.GetPlayerSymbol(m_Model.PlayerTurn));
			string playerInput = Console.ReadLine();
			char maxCapital = (char)(m_Model.BoardSize + 'A' - 1);
			char maxLittle = (char)(m_Model.BoardSize + 'a' - 1);
			string regex = string.Format("^{0}>[A-{1}][a-{2}]$", locationToString(i_Location), maxCapital, maxLittle);
			System.Text.RegularExpressions.Regex checkInput = new System.Text.RegularExpressions.Regex(regex);
			while (!checkInput.IsMatch(playerInput))
			{
				Console.WriteLine("Please enter a valid input: ");
				playerInput = Console.ReadLine();
			}
			stringToLocations(playerInput, out i_Location, out o_Destination);
		}

		private void stringToLocations(string i_PlayerInput, out Point o_Source, out Point o_Destination)
		{
			o_Source = new Point(i_PlayerInput[0] - 'A', i_PlayerInput[1] - 'a');
			o_Destination = new Point(i_PlayerInput[3] - 'A', i_PlayerInput[4] - 'a');
		}

		public void InitializePlayers()
		{
			m_Model.InitializePlayerOne(m_View.askPlayerName(m_Model.PlayerTurn));
			m_Model.InitializePlayerTwo(m_View.askPlayerName(m_Model.otherPlayer()));
		}

		public void InitializeBoard()
		{
			m_Model.InitializeBoard(m_View.AskGameBoardSize());
		}

		public void InitializeViewBoard()
		{
			m_View.GameBoard = new char[m_Model.BoardSize,m_Model.BoardSize];
			UpdateViewBoard();
		}

		public void UpdateViewBoard()
		{
			for (int row = 0; row < m_View.GameBoardSize; row++)
			{
				for (int col = 0; col < m_View.GameBoardSize; col++)
				{
					m_View.GameBoard[row, col] = m_Model.GetSymbol(new Point(row, col));
				}
			}
		}

		public void PrintBoard()
		{
			m_View.ClearScreen();
			m_View.PrintBoard();
		}

		private string locationToString(Point i_Location)
		{
			return string.Format("{0}{1}", (char)(i_Location.X + 'A'), (char)(i_Location.Y + 'a'));
		}
	}
}
