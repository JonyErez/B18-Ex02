using System;
using System.Collections.Generic;
using System.Text;
using B18_Ex02_Data;
using B18_Ex02_Interface;

namespace B18_Ex02_Game_Controller
{
	public class GameController
	{
		private		ConsoleInterface	m_View = new ConsoleInterface();
		private		Game				m_Model = new Game();
		private		bool				m_PlayAnotherRound = false;

		public		void	IsAnotherRound()
		{
			m_PlayAnotherRound = m_View.AskForAnotherRound();
			if(m_PlayAnotherRound)
			{
				resetGameData();
			}
		}

		private		void	resetGameData()
		{
			m_Model.ResetGameData();
			m_View.LastAction = null;
		}

		public		bool	PlayAnotherRound
		{
			get
			{
				return m_PlayAnotherRound;
			}
		}

		public		void	PlayCurrentTurn()
		{
			int playerSelectedMove;
			bool canPlayerQuit = canCurrentPlayerQuit();

			m_Model.IsGameOver = doesCurrentPlayerHaveMoves();
			if (!m_Model.IsGameOver)
			{
				m_View.TurnInformation(
					m_Model.GetPlayerName(m_Model.CurrentPlayerTurn), 
					m_Model.GetPlayerSymbol(m_Model.CurrentPlayerTurn), 
					m_Model.GetPlayerName(m_Model.OtherPlayer()), 
					m_Model.GetPlayerSymbol(m_Model.OtherPlayer()));
				decideActionForCurrentTurn(canPlayerQuit, out playerSelectedMove);
				if (!m_Model.DidPlayerQuit)
				{
					makePlayerMove(playerSelectedMove);
					while (m_Model.WasPieceEaten && m_Model.FindPlayersContinuationMoves() && !m_Model.DidPlayerQuit)
					{
						m_View.TurnInformation(
							m_Model.GetPlayerName(m_Model.CurrentPlayerTurn), 
							m_Model.GetPlayerSymbol(m_Model.CurrentPlayerTurn), 
							m_Model.GetPlayerName(m_Model.CurrentPlayerTurn), 
							m_Model.GetPlayerSymbol(m_Model.CurrentPlayerTurn));
						canPlayerQuit = canCurrentPlayerQuit();
						decideActionForCurrentTurn(canPlayerQuit, out playerSelectedMove);
						if (!m_Model.DidPlayerQuit)
						{
							makePlayerMove(playerSelectedMove);
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

		private		void	makePlayerMove(int i_PlayerSelectedMove)
		{
			m_Model.MakePlayerMove(i_PlayerSelectedMove);
			m_Model.CheckAndMakeKing();
			PrintBoard();
		}

		private		bool	canCurrentPlayerQuit()
		{
			return ((int)m_Model.CalculatePlayerScore(m_Model.CurrentPlayerTurn) - (int)m_Model.CalculatePlayerScore(m_Model.OtherPlayer())) < 0;
		}

		private		void	decideActionForCurrentTurn(bool i_CanPlayerQuit, out int o_PlayerSelectedMove)
		{
			Random selectedAction = new Random();
			if(!m_Model.IsCurrentPlayerComputer())
			{
				m_Model.DidPlayerQuit = m_View.GetPlayerInput(m_Model.GetCurrentMoves(), i_CanPlayerQuit, out o_PlayerSelectedMove);
			}
			else
			{
				o_PlayerSelectedMove = selectedAction.Next(0, m_Model.GetCurrentMoves().Count - 1);
				m_View.LastAction = m_Model.GetCurrentMoves()[o_PlayerSelectedMove];
			}
		}

		public		bool	IsGameOver()
		{
			return m_Model.IsGameOver;
		}

		private		bool	checkIfTie()
		{
			return !m_Model.DidPlayerQuit && !m_Model.FindPlayersFirstMoves(m_Model.OtherPlayer());
		}

		public		void	GameOver()
		{
			if(checkIfTie())
			{
				m_View.PrintTie();
			}
			else
			{ 
				uint winnerScore;
				winnerScore = m_Model.CalculatePlayerScore(m_Model.OtherPlayer()) - m_Model.CalculatePlayerScore(m_Model.CurrentPlayerTurn);
				m_Model.SetPlayerScore(winnerScore, m_Model.OtherPlayer());
				m_View.PrintWinner(m_Model.GetPlayerName(m_Model.OtherPlayer()), m_Model.GetPlayerSymbol(m_Model.OtherPlayer()), winnerScore);
			}

			m_View.PrintGameOver(
				m_Model.GetPlayerName(m_Model.CurrentPlayerTurn), 
				m_Model.GetPlayerScore(m_Model.CurrentPlayerTurn),
				m_Model.GetPlayerName(m_Model.OtherPlayer()), 
				m_Model.GetPlayerScore(m_Model.OtherPlayer()));
		}

		private		bool	doesCurrentPlayerHaveMoves()
		{
			bool doesCurrentPlayerHaveMoves;
			doesCurrentPlayerHaveMoves = !doesCurrentPlayerHaveLegalMoves() || !doesCurrentPlayerHavePiecesLeft();
			return doesCurrentPlayerHaveMoves;
		}

		private		bool	doesCurrentPlayerHavePiecesLeft()
		{
			bool doesPiecesLeft = true;
			if(m_Model.GetPlayerNumberOfPieces(m_Model.CurrentPlayerTurn) == 0)
			{
				doesPiecesLeft = false;
			}

			return doesPiecesLeft;
		}

		private		bool	doesCurrentPlayerHaveLegalMoves()
		{
			bool doesPlayerHaveLegalMoves = true;
			if(!m_Model.FindPlayersFirstMoves(m_Model.CurrentPlayerTurn))
			{
				doesPlayerHaveLegalMoves = false;
			}

			return doesPlayerHaveLegalMoves;
		}

		public		void	InitializeGame()
		{
			int		boardSize;
			bool	vsComputer;

			System.Console.Title = "Checkers Game - By Niv Dunay and Jonathan Erez";
			InitializePlayerOne();
			boardSize = m_View.AskGameBoardSize();
			vsComputer = m_View.AskHowManyPlayers() == 1;
			InitializePlayerTwo(vsComputer);
			InitializeBoard(boardSize);
			InitializeViewBoard();
		}

		private		void	InitializePlayerOne()
		{
			m_Model.InitializePlayerOne(m_View.AskPlayerName(m_Model.CurrentPlayerTurn));
		}

		private		void	InitializePlayerTwo(bool isComputer)
		{
			if (!isComputer)
			{
				m_Model.InitializePlayerTwo(m_View.AskPlayerName(m_Model.OtherPlayer()), isComputer);
			}
			else
			{
				m_Model.InitializePlayerTwo("CheckersAI", isComputer);
			}
		}

		private		void	InitializeBoard(int gameBoardSize)
		{
			m_Model.InitializeBoard(gameBoardSize);
		}

		private		void	InitializeViewBoard()
		{
			m_View.GameBoard = new char[m_Model.BoardSize, m_Model.BoardSize];
			updateBoard();
		}

		private		void	updateBoard()
		{
			for (int row = 0; row < m_View.GameBoardSize; row++)
			{
				for (int col = 0; col < m_View.GameBoardSize; col++)
				{
					m_View.GameBoard[row, col] = m_Model.GetSymbol(new Point(row, col));
				}
			}
		}

		public		void	PrintBoard()
		{
			updateBoard();
			m_View.ClearScreen();
			m_View.PrintBoard();
		}

		public		void	EndGame()
		{
			m_View.EndGame();
		}
	}
}
