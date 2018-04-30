using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	public class Game
	{
		private GameBoard m_Board;
		private Player[] m_Players = new Player[2];
		private int m_PlayerTurn = 0;
		private GamePiece m_PieceToMove;
		private Point m_SquareToMoveTo;

		public string GetPlayerName(int i_PlayerNumber)
		{
				return m_Players[i_PlayerNumber].Name;
		}

		public char GetPlayerSymbol(int i_PlayerNumber)
		{
				return m_Players[i_PlayerNumber].GamePieceSymbol;
		}

		public int PlayerTurn
		{
			get
			{
				return m_PlayerTurn;
			}
		}

		public bool CanPlayerMove(out bool o_canEat)
		{
			bool canMove = false;
			bool canEat = false;
			o_canEat = false;
			foreach (GamePiece currentPiece in m_Players[m_PlayerTurn].GamePieces)
			{
				canMove = m_Board.hasLegalMoves(currentPiece, out canEat) || canMove;
				o_canEat = o_canEat || canEat;
			}

			return canMove;
		}

		public bool FirstMoveInTurn(Point i_Location, Point i_Destination, bool i_NeedsToEat,out bool o_HasEaten)
		{
			bool canMove = false;
			o_HasEaten = false;
			GamePiece eatenPiece = null;

			m_PieceToMove = m_Board.Board[i_Location.Y, i_Location.X];
			if (m_PieceToMove != null)
			{
				if (m_Players[m_PlayerTurn] == m_PieceToMove.Owner)
				{
					if (m_Board.isLegalFirstAction(m_PieceToMove, i_Destination, out o_HasEaten, out eatenPiece))
					{
						if (o_HasEaten == i_NeedsToEat)
						{
							canMove = true;
							movePiece(i_Destination);
							if (o_HasEaten)
							{
								eatPiece(eatenPiece);
							}
						}
					}
				}
			}
			return canMove;
		}

		public bool HasAnotherLegalMove()
		{
			return m_Board.hasLegalEatingMoves(m_PieceToMove);
		}

		public bool ContinuesMove(Point i_Destination)
		{
			bool canMove = false;

			if (m_Board.isLegalContinuationAction(m_PieceToMove, i_Destination, out GamePiece o_EatenPiece))
			{
				canMove = true;
				movePiece(i_Destination);
				eatPiece(o_EatenPiece);
			}
			return canMove;
		}

		public void CheckAndMakeKing()
		{
			if(m_PieceToMove.Owner == m_Players[0])
			{
				if(m_PieceToMove.Location.Y == m_Board.Size - 1)
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

		private void movePiece(Point i_Destination)
		{
			m_Board.Board[i_Destination.Y, i_Destination.X] = m_PieceToMove;
			m_Board.Board[m_PieceToMove.Location.Y, m_PieceToMove.Location.X] = null;
			m_PieceToMove.Location.UpdateCoordinates(i_Destination.X, i_Destination.Y);
		}

		private void eatPiece(GamePiece i_EatenPiece)
		{
			m_Players[otherPlayer()].RemoveGamePiece(i_EatenPiece);
			m_Board.Board[i_EatenPiece.Location.Y, i_EatenPiece.Location.X] = null;
		}

		public void EndTurn()
		{
			m_PlayerTurn = otherPlayer();
		}

		public int otherPlayer()
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

		public int BoardSize
		{
			get
			{
				return m_Board.Size;
			}
		}

		public void InitializePlayerOne(string i_Name)
		{
			m_Players[0] = new Player(i_Name, 'O', 'U', Player.eDirection.DOWN);
		}

		public void InitializePlayerTwo(string i_Name)
		{
			m_Players[1] = new Player(i_Name, 'X', 'K', Player.eDirection.UP);
		}

		public void InitializeBoard(int i_BoardSize)
		{
			m_Board = new GameBoard(i_BoardSize);
			m_Board.InitializeBoard(m_Players[0], m_Players[1]);
		}

		public char GetSymbol(Point i_Coordinates)
		{
			return m_Board.GetSymbol(i_Coordinates);
		}

		public Point GetCurrentPieceLocation()
		{
			return m_PieceToMove.Location;
		}
	}
}
