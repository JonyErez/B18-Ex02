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
		private bool m_PlayAnotherRound = false;

		public void IsAnotherRound()
		{
			m_PlayAnotherRound = m_View.AskForAnotherRound();
			if(m_PlayAnotherRound)
			{
				resetGameData();
			}
		}

		private void resetGameData()
		{
			m_Model.ResetGameData();
			m_View.LastAction = null;
		}

		public bool PlayAnotherRound
		{
			get
			{
				return m_PlayAnotherRound;
			}
		}

		public void PlayCurrentTurn()
		{
			int playerSelectedMove;
			bool canPlayerQuit = m_Model.GetPlayerScore(m_Model.PlayerTurn) - m_Model.GetPlayerScore(m_Model.otherPlayer()) < 0;
			m_Model.IsGameOver = doesCurrentPlayerHaveMoves();
			if (!m_Model.IsGameOver)
			{
				m_View.TurnInformation(m_Model.GetPlayerName(m_Model.PlayerTurn), m_Model.GetPlayerSymbol(m_Model.PlayerTurn), m_Model.GetPlayerName(m_Model.otherPlayer()), m_Model.GetPlayerSymbol(m_Model.otherPlayer()));
				m_Model.DidPlayerQuit = m_View.getPlayerInput(m_Model.GetCurrentMoves(), canPlayerQuit, out playerSelectedMove);
				if (!m_Model.DidPlayerQuit)
				{
					m_Model.MakePlayerMove(playerSelectedMove);
					afterMoveActions();
					while (m_Model.WasPieceEaten && m_Model.FindPlayersContinuationMoves() && !m_Model.DidPlayerQuit)
					{
						m_View.TurnInformation(m_Model.GetPlayerName(m_Model.PlayerTurn), m_Model.GetPlayerSymbol(m_Model.PlayerTurn), m_Model.GetPlayerName(m_Model.PlayerTurn), m_Model.GetPlayerSymbol(m_Model.PlayerTurn));
						m_Model.DidPlayerQuit = m_View.getPlayerInput(m_Model.GetCurrentMoves(), canPlayerQuit, out playerSelectedMove);
						if (!m_Model.DidPlayerQuit)
						{
							m_Model.MakePlayerMove(playerSelectedMove);
							afterMoveActions();
						}
					}

					if (!m_Model.DidPlayerQuit)
					{
						m_Model.EndTurn();
					}
				}
			}

			m_Model.IsGameOver = m_Model.DidPlayerQuit || m_Model.IsGameOver;	
		}

		public bool IsGameOver()
		{
			return m_Model.IsGameOver;
		}

		private bool checkIfTie()
		{
			return !m_Model.DidPlayerQuit && !m_Model.FindPlayersFirstMoves(m_Model.otherPlayer());
		}

		public void GameOver()
		{
			if(checkIfTie())
			{
				m_View.PrintTie();
			}
			else
			{ 
				uint winnerScore;
				winnerScore = m_Model.CalculatePlayerScore(m_Model.otherPlayer()) - m_Model.CalculatePlayerScore(m_Model.PlayerTurn);
				m_Model.SetPlayerScore(winnerScore, m_Model.otherPlayer());
				m_View.PrintWinner(m_Model.GetPlayerName(m_Model.otherPlayer()), m_Model.GetPlayerSymbol(m_Model.otherPlayer()), winnerScore);
			}

			m_View.PrintGameOver(
				m_Model.GetPlayerName(m_Model.PlayerTurn), 
				m_Model.GetPlayerScore(m_Model.PlayerTurn),
				m_Model.GetPlayerName(m_Model.otherPlayer()), 
				m_Model.GetPlayerScore(m_Model.otherPlayer()));
		}

		private bool doesCurrentPlayerHaveMoves()
		{
			bool doesCurrentPlayerHaveMoves;
			doesCurrentPlayerHaveMoves = !doesCurrentPlayerHaveLegalMoves() || !doesCurrentPlayerHavePiecesLeft();
			return doesCurrentPlayerHaveMoves;
		}

		private bool doesCurrentPlayerHavePiecesLeft()
		{
			bool doesPiecesLeft = true;
			if(m_Model.GetPlayerNumberOfPieces(m_Model.PlayerTurn) == 0)
			{
				doesPiecesLeft = false;
			}

			return doesPiecesLeft;
		}

		private bool doesCurrentPlayerHaveLegalMoves()
		{
			bool doesPlayerHaveLegalMoves = true;
			if(!m_Model.FindPlayersFirstMoves(m_Model.PlayerTurn))
			{
				doesPlayerHaveLegalMoves = false;
			}

			return doesPlayerHaveLegalMoves;
		}

		private void afterMoveActions()
		{
			m_Model.CheckAndMakeKing();
			PrintBoard();
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
			m_Model.InitializePlayerOne(m_View.askPlayerName(m_Model.PlayerTurn));
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
			m_View.GameBoard = new char[m_Model.BoardSize, m_Model.BoardSize];
			updateBoard();
		}

		private void updateBoard()
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
			updateBoard();
			m_View.ClearScreen();
			m_View.PrintBoard();
		}

		private string locationToString(Point i_Location)
		{
			return string.Format("{0}{1}", (char)(i_Location.X + 'A'), (char)(i_Location.Y + 'a'));
		}
	}
}
