using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	public class Game
	{
		private	const	int				k_KingScore = 4;
		private	const	int				k_PawnScore = 1;

		private			GameBoard		m_Board;
		private			Player[]		m_Players = new Player[2];
		private			int				m_PlayerTurn = 0;
		private			GamePiece		m_PieceToMove = null;
		private			List<PieceMove> m_CurrentTurnPossibleMoves = new List<PieceMove>(2);
		private			bool			m_WasPieceEaten = false;
		private			bool			m_IsGameOver = false;
		private			bool			m_didPlayerQuit = false;

		public	bool			IsCurrentPlayerComputer()
		{
			return m_Players[m_PlayerTurn].IsComputer;
		}

		private	void			clearPlayerPieces(Player i_Player)
		{
			foreach(GamePiece currentPiece in i_Player.GamePieces)
			{
				m_Board.Board[currentPiece.Location.Y, currentPiece.Location.X] = null;
			}

			i_Player.GamePieces.Clear();
		}

		public	void			ResetGameData()
		{
			clearBoard();
			resetGameFlags();
			m_Board.InitializeBoard(m_Players[0], m_Players[1]);
		}

		private	void			clearBoard()
		{
			clearPlayerPieces(m_Players[0]);
			clearPlayerPieces(m_Players[1]);
		}

		private	void			resetGameFlags()
		{
			m_PlayerTurn = 0;
			m_IsGameOver = false;
			m_didPlayerQuit = false;
			m_WasPieceEaten = false;
		}

		public	bool			DidPlayerQuit
		{
			get
			{
				return m_didPlayerQuit;
			}

			set
			{
				m_didPlayerQuit = value;
			}
		}

		public	bool			IsGameOver
		{
			get
			{
				return m_IsGameOver;
			}

			set
			{
				m_IsGameOver = value;
			}
		}

		public	List<string>	GetCurrentMoves()
		{
			List<string> currentMoves = new List<string>(m_CurrentTurnPossibleMoves.Count);
			foreach (PieceMove currentMove in m_CurrentTurnPossibleMoves)
			{
				currentMoves.Add(string.Format("{0}>{1}", locationToString(currentMove.Location), locationToString(currentMove.Destination)));
			}

			return currentMoves;
		}

		public	int				CurrentPlayerTurn
		{
			get
			{
				return m_PlayerTurn;
			}
		}
	
		public	bool			WasPieceEaten
		{
			get
			{
				return m_WasPieceEaten;
			}

			set
			{
				m_WasPieceEaten = value;
			}
		}

		private	string			locationToString(Point i_Location)
		{
			return string.Format("{0}{1}", (char)(i_Location.X + 'A'), (char)(i_Location.Y + 'a'));
		}

		public	string			GetPlayerName(int i_PlayerNumber)
		{
				return m_Players[i_PlayerNumber].Name;
		}

		public	char			GetPlayerSymbol(int i_PlayerNumber)
		{
				return m_Players[i_PlayerNumber].GamePieceSymbol;
		}

		public	bool			FindPlayersFirstMoves(int i_PlayerNumber)
		{
			m_CurrentTurnPossibleMoves.Clear();
			foreach (GamePiece currentPiece in m_Players[i_PlayerNumber].GamePieces)
			{
				m_CurrentTurnPossibleMoves.AddRange(m_Board.findPossibleEatingMoves(currentPiece));
			}

			if(m_CurrentTurnPossibleMoves.Count == 0)
			{
				foreach (GamePiece currentPiece in m_Players[i_PlayerNumber].GamePieces)
				{
					m_CurrentTurnPossibleMoves.AddRange(m_Board.findPossibleSteppingForwardMoves(currentPiece));
				}
			}

			return m_CurrentTurnPossibleMoves.Count != 0;
		}

		public	bool			FindPlayersContinuationMoves()
		{
			m_CurrentTurnPossibleMoves.Clear();
			m_CurrentTurnPossibleMoves.AddRange(m_Board.findPossibleEatingMoves(m_PieceToMove));

			return m_CurrentTurnPossibleMoves.Count != 0;
		}

		public	void			MakePlayerMove(int i_PieceMoveIndex)
		{
			if (m_PieceToMove == null)
			{
				m_PieceToMove = m_Board.Board[m_CurrentTurnPossibleMoves[i_PieceMoveIndex].Location.Y, m_CurrentTurnPossibleMoves[i_PieceMoveIndex].Location.X];
			}

			if (m_CurrentTurnPossibleMoves[i_PieceMoveIndex].DoesEat)
			{
				eatPiece(m_Board.findEatenPiece(m_CurrentTurnPossibleMoves[i_PieceMoveIndex]));
				m_WasPieceEaten = true;
			}

			movePiece(m_CurrentTurnPossibleMoves[i_PieceMoveIndex].Destination);
		}

		public	void			CheckAndMakeKing()
		{
			if (m_PieceToMove != null)
			{
				if (m_PieceToMove.Owner == m_Players[0])
				{
					if (m_PieceToMove.Location.Y == m_Board.Size - 1)
					{
						m_PieceToMove.MakeKing();
					}
				}
				else
				{
					if (m_PieceToMove.Location.Y == 0)
					{
						m_PieceToMove.MakeKing();
					}
				}
			}
		}

		private	void			movePiece(Point i_Destination)
		{
			m_Board.Board[i_Destination.Y, i_Destination.X] = m_PieceToMove;
			m_Board.Board[m_PieceToMove.Location.Y, m_PieceToMove.Location.X] = null;
			m_PieceToMove.Location.UpdateCoordinates(i_Destination.X, i_Destination.Y);
		}

		private void			eatPiece(GamePiece i_EatenPiece)
		{
			m_Players[OtherPlayer()].RemoveGamePiece(i_EatenPiece);
			m_Board.Board[i_EatenPiece.Location.Y, i_EatenPiece.Location.X] = null;
		}

		public	void			EndTurn()
		{
			m_WasPieceEaten = false;
			m_PieceToMove = null;
			m_PlayerTurn = OtherPlayer();
		}

		public	int				OtherPlayer()
		{
			int secondPlayer;
			if (m_PlayerTurn == 0)
			{
				secondPlayer = 1;
			}
			else
			{
				secondPlayer = 0;
			}

			return secondPlayer;
		}

		public	int				BoardSize
		{
			get
			{
				return m_Board.Size;
			}
		}

		public	void			InitializePlayerOne(string i_Name)
		{
			m_Players[0] = new Player(i_Name, 'O', 'U', Player.eDirection.DOWN, false);
		}

		public	void			InitializePlayerTwo(string i_Name, bool i_IsComputer)
		{
			m_Players[1] = new Player(i_Name, 'X', 'K', Player.eDirection.UP, i_IsComputer);
		}

		public	void			InitializeBoard(int i_BoardSize)
		{
			m_Board = new GameBoard(i_BoardSize);
			m_Board.InitializeBoard(m_Players[0], m_Players[1]);
		}

		public	char			GetSymbol(Point i_Coordinates)
		{
			return m_Board.GetSymbol(i_Coordinates);
		}

		public	uint			GetPlayerNumberOfPieces(int PlayerNumber)
		{
			return (uint)m_Players[PlayerNumber].GamePieces.Count;
		}

		public	void			SetPlayerScore(uint i_Score, int PlayerNumber)
		{
			m_Players[PlayerNumber].Score += i_Score;
		}

		public	uint			GetPlayerScore(int PlayerNumber)
		{
			return m_Players[PlayerNumber].Score;
		}

		public	uint			CalculatePlayerScore(int PlayerNumber)
		{
			uint playerScore = 0;

			foreach (GamePiece currentPiece in m_Players[PlayerNumber].GamePieces)
			{
				if (currentPiece.IsKing)
				{
					playerScore += k_KingScore;
				}
				else
				{
					playerScore += k_PawnScore;
				}
			}

			return playerScore;
		}
	}
}
