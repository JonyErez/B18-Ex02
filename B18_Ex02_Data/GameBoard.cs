﻿using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class GameBoard
	{
		private readonly int m_BoardSize;
		private GamePiece[,] m_Board;

		public GameBoard(int i_BoardSize)
		{
			m_BoardSize = i_BoardSize;
			m_Board = new GamePiece[i_BoardSize, i_BoardSize];
		}

		public bool isLegalFirstAction(GamePiece i_Piece, Point i_NewLocation, out bool o_HasEaten, out GamePiece o_EatenPiece)
		{
			bool isLegalMove = false;
			bool isLegalEat = false;
			o_EatenPiece = null;
			o_HasEaten = false;

			if (i_Piece.IsKing)
			{
				isLegalMove = isAllowedMove(i_Piece.Location, i_Piece.Owner.ReverseDirection, i_NewLocation);
				isLegalEat = isAllowedEat(i_Piece, i_Piece.Owner.ReverseDirection, i_NewLocation, out o_EatenPiece);
			}
			isLegalMove = isLegalMove || isAllowedMove(i_Piece.Location, i_Piece.Owner.Direction, i_NewLocation);
			isLegalEat = isLegalEat || isAllowedEat(i_Piece, i_Piece.Owner.Direction, i_NewLocation, out o_EatenPiece);
			o_HasEaten = isLegalEat;
			return isLegalEat || isLegalMove;
		}

		public bool isLegalContinuationAction(GamePiece i_Piece, Point i_NewLocation, out GamePiece o_EatenPiece)
		{
			bool isLegalEat = false;
			o_EatenPiece = null;

			if (i_Piece.IsKing)
			{
				isLegalEat = isAllowedEat(i_Piece, i_Piece.Owner.ReverseDirection, i_NewLocation, out o_EatenPiece);
			}
			isLegalEat = isLegalEat || isAllowedEat(i_Piece, i_Piece.Owner.Direction, i_NewLocation, out o_EatenPiece);
			return isLegalEat;
		}

		private bool isAllowedMove(Point i_CurrentLocation, Player.eDirection i_VerticalDirection, Point i_NewLocation)
		{
			bool isAllowed = false;
			if (isLegalBoardLocation(i_NewLocation.Y, i_NewLocation.X))
			{
				if (checkSquareOwnership(i_NewLocation.Y, i_NewLocation.X) == null)
				{
					Point checkIfAllowed = new Point(i_CurrentLocation.X, i_CurrentLocation.Y);
					checkIfAllowed.Y += (int)i_VerticalDirection;
					if (checkIfAllowed.Y == i_NewLocation.Y)
					{
						isAllowed = checkIfAllowed.X + (int)Player.eDirection.LEFT == i_NewLocation.X || checkIfAllowed.X + (int)Player.eDirection.RIGHT == i_NewLocation.X;
					}
				}
			}
			return isAllowed;
		}

		private bool isAllowedEat(GamePiece i_Piece, Player.eDirection i_VerticalDirection, Point i_NewLocation, out GamePiece o_EatenPiece)
		{
			bool isAllowed = false;
			o_EatenPiece = null;

			if (isLegalBoardLocation(i_NewLocation.Y, i_NewLocation.X))
			{
				if (checkSquareOwnership(i_NewLocation.Y, i_NewLocation.X) == null)
				{
					int rowDiff = i_NewLocation.Y - i_Piece.Location.Y;
					int colDiff = i_NewLocation.X - i_Piece.Location.X;
					if (Math.Abs(rowDiff) == 2 && Math.Abs(colDiff) == 2)
					{
						rowDiff /= 2;
						if (rowDiff == (int)i_Piece.Owner.Direction)
						{
							colDiff /= 2;
							int eatenSquareRow = i_Piece.Location.Y + rowDiff;
							int eatenSquareCol = i_Piece.Location.X + colDiff;
							if (checkSquareOwnership(eatenSquareRow, eatenSquareCol) != null)
							{
								if (checkSquareOwnership(eatenSquareRow, eatenSquareCol) != i_Piece.Owner)
								{
									o_EatenPiece = m_Board[eatenSquareRow, eatenSquareCol];
									isAllowed = true;
								}
							}
						}
					}
				}
			}
			return isAllowed;
		}

		public bool hasLegalEatingMoves(GamePiece i_Piece)
		{
			bool hasLegalEatingMove = false;
			if (i_Piece.IsKing)
			{
				hasLegalEatingMove = checkManEatingMoves(i_Piece, i_Piece.Owner.ReverseDirection);
			}
			hasLegalEatingMove = hasLegalEatingMove || checkManEatingMoves(i_Piece, i_Piece.Owner.Direction);
			return hasLegalEatingMove;
		}

		public bool hasLegalMoves(GamePiece i_Piece, out bool o_CanEat)
		{
			bool hasLegalMove = false;
			bool canKingEat = false;
			bool canManEat = false;
			if(i_Piece.IsKing)
			{
				hasLegalMove = checkManMoves(i_Piece, i_Piece.Owner.ReverseDirection, out canKingEat);
			}
			hasLegalMove = hasLegalMove || checkManMoves(i_Piece, i_Piece.Owner.Direction, out canManEat);
			o_CanEat = canManEat || canKingEat;
			return hasLegalMove;

		}

		private bool checkManMoves(GamePiece i_Piece, Player.eDirection i_VerticalDirection, out bool o_CanEat)
		{
			bool canMove = false;
			canMove = o_CanEat = checkManEatingMoves(i_Piece, i_VerticalDirection);
			canMove = canMove || checkManNormalMoves(i_Piece, i_VerticalDirection);
			return canMove;
		}

		private bool checkManEatingMoves(GamePiece i_Piece, Player.eDirection i_VerticalDirection)
		{
			return checkEat(i_Piece, i_VerticalDirection, Player.eDirection.LEFT) ||
					checkEat(i_Piece, i_VerticalDirection, Player.eDirection.RIGHT);
		}

		private bool checkEat(GamePiece i_Piece, Player.eDirection i_VerticalDirection, Player.eDirection i_HorizontalDirection)
		{
			int pieceToEatRow = i_Piece.Location.Y + (int)i_VerticalDirection;
			int pieceToEatCol = i_Piece.Location.X + (int)i_HorizontalDirection;
			bool canEat = false;
			if (isLegalBoardLocation(pieceToEatRow, pieceToEatCol))
			{
				Player leftSquareOwner = checkSquareOwnership(pieceToEatRow, pieceToEatCol);
				if (leftSquareOwner != null && leftSquareOwner != i_Piece.Owner)
				{
					int squareToJumpToRow = pieceToEatRow + (int)i_VerticalDirection;
					int squareToJumpToCol = pieceToEatCol + (int)i_HorizontalDirection;
					if (isLegalBoardLocation(squareToJumpToRow, squareToJumpToCol))
					{
						if (checkSquareOwnership(squareToJumpToRow, squareToJumpToCol) == null)
						{
							canEat = true;
						}
					}
				}
			}
			return canEat;
		}

		private bool checkManNormalMoves(GamePiece i_Piece, Player.eDirection i_VerticalDirection)
		{
			return checkMove(i_Piece.Location, i_VerticalDirection, Player.eDirection.RIGHT) || 
					checkMove(i_Piece.Location, i_VerticalDirection, Player.eDirection.LEFT);
		}

		private bool checkMove(Point i_Location, Player.eDirection i_VerticalDirection, Player.eDirection i_HorizontalDirection)
		{
			int row = i_Location.Y + (int)i_VerticalDirection;
			int col = i_Location.X + (int)i_HorizontalDirection;

			return isLegalBoardLocation(row,col) && checkSquareOwnership(row, col) == null;
		}

		private Player checkSquareOwnership(int i_Row, int i_Col)
		{
			Player owner = null;
			if (m_Board[i_Row, i_Col] != null)
			{
				owner = m_Board[i_Row, i_Col].Owner;
			}
			return owner;
		}

		private bool isLegalBoardLocation(int i_Row, int i_Col)
		{
			return !(i_Col < 0 || i_Col >= m_BoardSize || i_Row < 0 || i_Row >= m_BoardSize);
		}

		public void InitializeBoard(Player i_PlayerOne, Player i_PlayerTwo)
		{
			int howManyRows = (m_BoardSize / 2) - 1;
			initializePlayerPieces(i_PlayerOne, 0, howManyRows);
			initializePlayerPieces(i_PlayerTwo, (m_BoardSize / 2) + 1, howManyRows);
		}

		private void initializePlayerPieces(Player i_Player, int i_StartingRow, int i_HowManyRows)
		{
			for(int currentRow = i_StartingRow; currentRow < i_StartingRow + i_HowManyRows; currentRow++)
			{
				int currentCol = currentRow % 2 == 0 ? 1 : 0;
				for(; currentCol < m_BoardSize; currentCol += 2)
				{
					m_Board[currentRow, currentCol] = new GamePiece(i_Player, new Point(currentCol, currentRow));
				}
			}
		}

		public int Size
		{
			get
			{
				return m_BoardSize;
			}
		}

		public GamePiece[,] Board
		{
			get
			{
				return m_Board;
			}
		}

		public char GetSymbol(Point i_Coordinates)
		{
			char symbol = ' ';
			if (m_Board[i_Coordinates.X, i_Coordinates.Y] != null)
			{
				symbol = m_Board[i_Coordinates.X, i_Coordinates.Y].Symbol;
			}
			return symbol;
		}
	}
}
