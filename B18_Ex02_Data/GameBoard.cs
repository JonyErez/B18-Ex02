﻿using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02_Data
{
	internal class GameBoard
	{
		private readonly int r_BoardSize;
		private GamePiece[,] m_Board;

		public	GameBoard(int i_BoardSize)
		{
			r_BoardSize = i_BoardSize;
			m_Board = new GamePiece[i_BoardSize, i_BoardSize];
		}

		public	List<PieceMove>	FindPossibleSteppingForwardMoves(GamePiece i_GamePiece)
		{
			List<PieceMove> possibleSteppingForwardMoves = new List<PieceMove>(2);

			if(i_GamePiece.IsKing)
			{
				possibleSteppingForwardMoves.AddRange(findSteppingForwardMoves(i_GamePiece, i_GamePiece.Owner.ReverseDirection));
			}

			possibleSteppingForwardMoves.AddRange(findSteppingForwardMoves(i_GamePiece, i_GamePiece.Owner.Direction));

			return possibleSteppingForwardMoves;
		}

		private	List<PieceMove>	findSteppingForwardMoves(GamePiece i_GamePiece, Player.eDirection i_VerticalDirection)
		{
			List<PieceMove> currentPieceSteppingForward = new List<PieceMove>(2);
			PieceMove currentPieceSteppingLeft = findSpecificSteppingMove(i_GamePiece.Location, i_VerticalDirection, Player.eDirection.LEFT);
			PieceMove currentPieceSteppingRight = findSpecificSteppingMove(i_GamePiece.Location, i_VerticalDirection, Player.eDirection.RIGHT);

			if(currentPieceSteppingLeft != null)
			{
				currentPieceSteppingForward.Add(currentPieceSteppingLeft);
			}

			if (currentPieceSteppingRight != null)
			{
				currentPieceSteppingForward.Add(currentPieceSteppingRight);
			}

			return currentPieceSteppingForward;
		}

		private	PieceMove		findSpecificSteppingMove(Point i_Location, Player.eDirection i_VerticalDirection, Player.eDirection i_HorizontalDirection)
		{
			PieceMove steppingMove = null;
			int row = i_Location.Y + (int)i_VerticalDirection;
			int col = i_Location.X + (int)i_HorizontalDirection;

			if(isCoordinateInBoard(row, col) && getSquareOwnership(row, col) == null)
			{
				Point destination = new Point(col, row);
				steppingMove = new PieceMove(i_Location, destination, false);
			}

			return steppingMove;
		}

		public	List<PieceMove>	FindPossibleEatingMoves(GamePiece i_GamePiece)
		{
			List<PieceMove> possibleEatingMoves = new List<PieceMove>(2);

			if(i_GamePiece.IsKing)
			{
				possibleEatingMoves.AddRange(findEatingMoves(i_GamePiece, i_GamePiece.Owner.ReverseDirection));
			}

			possibleEatingMoves.AddRange(findEatingMoves(i_GamePiece, i_GamePiece.Owner.Direction));

			return possibleEatingMoves;
		}

		private List<PieceMove> findEatingMoves(GamePiece i_Piece, Player.eDirection i_VerticalDirection)
		{
			List<PieceMove> currentPieceEatingMoves = new List<PieceMove>(2);
			PieceMove currentPieceEatingLeft = findSpecificEatingMoves(i_Piece, i_VerticalDirection, Player.eDirection.LEFT);
			PieceMove currentPieceEatingRight = findSpecificEatingMoves(i_Piece, i_VerticalDirection, Player.eDirection.RIGHT);

			if (currentPieceEatingLeft != null)
			{
				currentPieceEatingMoves.Add(currentPieceEatingLeft);
			}

			if (currentPieceEatingRight != null)
			{
				currentPieceEatingMoves.Add(currentPieceEatingRight);
			}

			return currentPieceEatingMoves;
		}

		private	PieceMove		findSpecificEatingMoves(GamePiece i_Piece, Player.eDirection i_VerticalDirection, Player.eDirection i_HorizontalDirection)
		{
			PieceMove EatingMove = null;
			int pieceToEatRow = i_Piece.Location.Y + (int)i_VerticalDirection;
			int pieceToEatCol = i_Piece.Location.X + (int)i_HorizontalDirection;
			if (isCoordinateInBoard(pieceToEatRow, pieceToEatCol))
			{
				Player leftSquareOwner = getSquareOwnership(pieceToEatRow, pieceToEatCol);
				if (leftSquareOwner != null && leftSquareOwner != i_Piece.Owner)
				{
					int squareToJumpToRow = pieceToEatRow + (int)i_VerticalDirection;
					int squareToJumpToCol = pieceToEatCol + (int)i_HorizontalDirection;
					if (isCoordinateInBoard(squareToJumpToRow, squareToJumpToCol))
					{
						if (getSquareOwnership(squareToJumpToRow, squareToJumpToCol) == null)
						{
							Point destination = new Point(squareToJumpToCol, squareToJumpToRow);
							EatingMove = new PieceMove(i_Piece.Location, destination, true);
						}
					}
				}
			}

			return EatingMove;
		}

		public	GamePiece		FindEatenPiece(PieceMove i_EatingMove)
		{
			Point eatenPieceLocation;
			Point difference = i_EatingMove.Destination - i_EatingMove.Location;
			difference = difference / 2;
			eatenPieceLocation = i_EatingMove.Location + difference;
			return m_Board[eatenPieceLocation.Y, eatenPieceLocation.X];
		}

		private Player			getSquareOwnership(int i_Row, int i_Col)
		{
			Player owner = null;

			if (m_Board[i_Row, i_Col] != null)
			{
				owner = m_Board[i_Row, i_Col].Owner;
			}

			return owner;
		}

		private bool			isCoordinateInBoard(int i_Row, int i_Col)
		{
			return !(i_Col < 0 || i_Col >= r_BoardSize || i_Row < 0 || i_Row >= r_BoardSize);
		}

		public	void			InitializeBoard(Player i_PlayerOne, Player i_PlayerTwo)
		{
			int howManyRows = (r_BoardSize / 2) - 1;
			initializePlayerPieces(i_PlayerOne, 0, howManyRows);
			initializePlayerPieces(i_PlayerTwo, (r_BoardSize / 2) + 1, howManyRows);
		}

		private void			initializePlayerPieces(Player i_Player, int i_StartingRow, int i_HowManyRows)
		{
			for(int currentRow = i_StartingRow; currentRow < i_StartingRow + i_HowManyRows; currentRow++)
			{
				int currentCol = currentRow % 2 == 0 ? 1 : 0;
				for(; currentCol < r_BoardSize; currentCol += 2)
				{
					m_Board[currentRow, currentCol] = new GamePiece(i_Player, new Point(currentCol, currentRow));
				}
			}
		}

		public	int				Size
		{
			get
			{
				return r_BoardSize;
			}
		}

		public	GamePiece[,]	Board
		{
			get
			{
				return m_Board;
			}
		}

		public	char			GetSymbol(Point i_Coordinates)
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
