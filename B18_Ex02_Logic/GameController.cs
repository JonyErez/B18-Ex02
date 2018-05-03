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

			if (m_Model.FindPlayersFirstMoves())
			{
				m_View.TurnInformation(m_Model.GetPlayerName(m_Model.WhosPlayersTurn), m_Model.GetPlayerSymbol(m_Model.WhosPlayersTurn), m_Model.GetPlayerName(m_Model.otherPlayer()), m_Model.GetPlayerSymbol(m_Model.otherPlayer()));
				m_Model.MakePlayerMove(m_View.getPlayerInput(m_Model.GetCurrentMoves()));
				afterMoveActions();
				while (m_Model.FindPlayersContinuationMoves())
				{
					m_View.TurnInformation(m_Model.GetPlayerName(m_Model.WhosPlayersTurn), m_Model.GetPlayerSymbol(m_Model.WhosPlayersTurn), m_Model.GetPlayerName(m_Model.otherPlayer()), m_Model.GetPlayerSymbol(m_Model.otherPlayer()));
					m_Model.MakePlayerMove(m_View.getPlayerInput(m_Model.GetCurrentMoves()));
					afterMoveActions();
				}
			}
			m_Model.EndTurn();
				
		}

		private void afterMoveActions()
		{
			m_Model.CheckAndMakeKing();
			UpdateBoard();
			PrintBoard();
		}

		private void getPlayerInputForFirstTurn(out Point o_Source, out Point o_Destination)
		{
			m_View.TurnInformation(m_Model.GetPlayerName(m_Model.WhosPlayersTurn), m_Model.GetPlayerSymbol(m_Model.WhosPlayersTurn),
									m_Model.GetPlayerName(m_Model.otherPlayer()), m_Model.GetPlayerSymbol(m_Model.otherPlayer()));
			string playerInput = Console.ReadLine();
			char maxCapital = (char)(m_Model.BoardSize + 'A' - 1);
			char maxLittle = (char)(m_Model.BoardSize + 'a' - 1);
			string regex = string.Format("^[A-{0}][a-{1}]>[A-{0}][a-{1}]$", maxCapital, maxLittle);
			System.Text.RegularExpressions.Regex checkInput = new System.Text.RegularExpressions.Regex(regex);
			while (!checkInput.IsMatch(playerInput))
			{
				m_View.PrintError(ConsoleInterface.eErrors.InvalidMoveInput);
				playerInput = Console.ReadLine();
			}
			stringToLocations(playerInput, out o_Source, out o_Destination);
		}

		private void getPlayerInputForContinuousTurns(Point i_Location, out Point o_Destination)
		{
			m_View.TurnInformation(m_Model.GetPlayerName(m_Model.WhosPlayersTurn), m_Model.GetPlayerSymbol(m_Model.WhosPlayersTurn),
									m_Model.GetPlayerName(m_Model.WhosPlayersTurn), m_Model.GetPlayerSymbol(m_Model.WhosPlayersTurn));
			string playerInput = Console.ReadLine();
			char maxCapital = (char)(m_Model.BoardSize + 'A' - 1);
			char maxLittle = (char)(m_Model.BoardSize + 'a' - 1);
			string regex = string.Format("^{0}>[A-{1}][a-{2}]$", locationToString(i_Location), maxCapital, maxLittle);
			System.Text.RegularExpressions.Regex checkInput = new System.Text.RegularExpressions.Regex(regex);
			while (!checkInput.IsMatch(playerInput))
			{
				m_View.PrintError(ConsoleInterface.eErrors.InvalidMoveInput);
				playerInput = Console.ReadLine();
			}
			stringToLocations(playerInput, out i_Location, out o_Destination);
		}

		private void stringToLocations(string i_PlayerInput, out Point o_Source, out Point o_Destination)
		{
			o_Source = new Point(i_PlayerInput[0] - 'A', i_PlayerInput[1] - 'a');
			o_Destination = new Point(i_PlayerInput[3] - 'A', i_PlayerInput[4] - 'a');
		}

		public void InitializeGame()
		{
			int boardSize;
			bool vsComputer;
			InitializePlayerOne();
			boardSize = m_View.AskGameBoardSize();
			vsComputer = m_View.AskHowManyPlayers() == 1;
			InitializePlayerTwo(vsComputer);
			InitializeBoard(boardSize);
			InitializeViewBoard();
		}

		private void InitializePlayerOne()
		{
			m_Model.InitializePlayerOne(m_View.askPlayerName(m_Model.WhosPlayersTurn));
		}

		private void InitializePlayerTwo(bool isComputer)
		{
			if (!isComputer)
			{
				m_Model.InitializePlayerTwo(m_View.askPlayerName(m_Model.otherPlayer()), isComputer);
			}
			else
			{
				m_Model.InitializePlayerTwo("CheckersAI", isComputer);
			}
		}

		private void InitializeBoard(int gameBoardSize)
		{
			m_Model.InitializeBoard(gameBoardSize);
		}

		private void InitializeViewBoard()
		{
			m_View.GameBoard = new char[m_Model.BoardSize,m_Model.BoardSize];
			UpdateBoard();
		}

		public void UpdateBoard()
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

		public bool IsGameOver()
		{
			bool isGameOver = false;
			if (m_Model.GetCurrentPlayerNumberOfPieces() == 0)
			{
				isGameOver = true;
			}

			return isGameOver;
		}
	}
}
