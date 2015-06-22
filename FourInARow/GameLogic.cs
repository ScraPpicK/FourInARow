using System;

namespace FourInARow
{
    /// <summary>
    /// Static class for game logic
    /// </summary>
    public static class GameLogic
    {
        private const int draw = 2;
        private const int isNotEndOfTheGame = -1;
        private const int fourInARow = 4;
  
        /// <summary>
        /// Initialize game field.
        /// </summary>
        /// <param name="field">Field to initialize.</param>
        /// <returns></returns>
        public static int[,] InitField(int[,] field)
        {
            for (int i = 0; i < Const.columnsCount; i++)
                for (int j = 0; j < Const.rowsCount; j++)
                    field[i, j] = Const.emptyFieldIndex;

            return field;
        }

        /// <summary>
        /// Returns a boolean value if a player can do the move.
        /// </summary>
        /// <param name="column">Index of the column player want to move.</param>
        /// <param name="row">Index of the row player want to move.</param>
        /// <returns></returns>
        public static bool CanMove(int column, ref int row, int[,] fields)
        {
            if (fields[column, row] == Const.emptyFieldIndex)
                if (row == Const.firstAvailableRowIndex)
                    return true;
                else if (fields[column, row + 1] != Const.emptyFieldIndex)
                        return true;

            for (int i = row; i < Const.rowsCount - 1; i++)
                if (fields[column, i] == Const.emptyFieldIndex && fields[column, i + 1] != Const.emptyFieldIndex)
                {
                    row = i;
                    return true;
                }
                else if (fields[column, i] == Const.emptyFieldIndex && fields[column, Const.rowsCount - 1] == Const.emptyFieldIndex
                    && i == Const.rowsCount - 2)
                {
                    row = 5;
                    return true;
                }

                return false;
        }

        /// <summary>
        /// Returns index of the game state.
        /// </summary>
        /// <param name="column">Index of the column of last move.</param>
        /// <param name="row">Index of the row of last move.</param>
        /// <param name="playerIndex">Index of the player, who did last move.</param>
        /// <param name="fields">Game field.</param>
        /// <returns></returns>
        public static int IfGameEnd(int column, int row, int playerIndex, int[,] fields)
        {
            int i = 0;
            for (; i < Const.columnsCount; i++)
                for (int j = 0; j < Const.rowsCount; j++)
                    if (fields[i, j] == Const.emptyFieldIndex)
                        goto NoDraw;

            return draw;

        NoDraw:
            if (VerticalCheck(column, playerIndex, fields))
                return playerIndex;

            if (GorizontalCheck(row, playerIndex, fields))
                return playerIndex;

            if (DiagonalCheck(column, row, playerIndex, fields))
                return playerIndex;

            return isNotEndOfTheGame;
        }

        /// <summary>
        /// Checks if vertical line of the field contains 4 in a row coins of the last player, who did the move.
        /// </summary>
        /// <param name="row">Index of the row of last move.</param>
        /// <param name="playerIndex">Index of the player, who did the move.</param>
        /// <param name="fields">Game field.</param>
        /// <returns></returns>
        private static bool VerticalCheck(int column, int playerIndex, int[,] fields)
        {
            int inARow = 0;
            int coinCount = 0;
            for (int i = 0; i < Const.rowsCount; i++)
                if (coinCount == fourInARow)
                    return true;
                else
                    if (fields[column, i] == playerIndex && inARow == i)
                    {
                        coinCount++;
                        inARow++;
                    }
                    else
                    {
                        coinCount = 0;
                        inARow = i + 1;
                    }
            if (coinCount == fourInARow)
                return true;

            return false;    
        }

        /// <summary>
        /// Checks if gorizontal line of the field contains 4 in a row coins of the last player, who did the move.
        /// </summary>
        /// <param name="column">Index of the column of last move.</param>
        /// <param name="playerIndex">Index of the player, who did the move.</param>
        /// <param name="fields">Game field.</param>
        /// <returns></returns>
        private static bool GorizontalCheck(int row, int playerIndex, int[,] fields)
        {
            int inARow = 0;
            int coinCount = 0;

            for (int i = 0; i < Const.columnsCount; i++)
                if (coinCount == fourInARow)
                    return true;
                else
                    if (fields[i, row] == playerIndex && inARow == i)
                    {
                        coinCount++;
                        inARow++;
                    }
                    else
                    {
                        coinCount = 0;
                        inARow = i + 1;
                    }

            if (coinCount == fourInARow)
                return true;
            return false;
        }

        /// <summary>
        /// Checks if diagonal lines of the field contains 4 in a row coins of the last player, who did the move.
        /// </summary>
        /// <param name="column">Index of the column of last move.</param>
        /// <param name="row">Index of the row of last move.</param>
        /// <param name="playerIndex">Index of the player, who did the move.</param>
        /// <param name="fields">Game field.</param>
        /// <returns></returns>
        private static bool DiagonalCheck(int column, int row, int playerIndex, int[,] fields)
        {
            int inARow = 0;
            int coinCount = 0;
            int columnIndex = column;
            int rowIndex = row;
           
            while(columnIndex != 0 && rowIndex != 0)
            {
                columnIndex--;
                rowIndex--;
            }

            int i = columnIndex;
            int j = rowIndex;

            if (columnIndex == 0)
                goto Column;

            if (rowIndex == 0)
                goto Row;

        Column:
            while (i < Const.columnsCount && j < Const.rowsCount)
                if (coinCount == fourInARow)
                    return true;
                else
                    CheckFromTopToBot(ref i, ref j, playerIndex, ref inARow, ref coinCount, fields);

            if (coinCount == fourInARow)
                return true;

            i = columnIndex;
            j = rowIndex;

            coinCount = 0;
            inARow = 0;

            while (i < Const.columnsCount && j < Const.rowsCount)
                if (coinCount == fourInARow)
                    return true;
                else CheckFromBotToTopForColumn(ref i, ref j, playerIndex, ref inARow, ref coinCount, fields);

            if (coinCount == fourInARow)
                return true;

            goto NotFourInARow;

        Row:
            while(i < Const.columnsCount && j < Const.rowsCount)
                if (coinCount == fourInARow)
                    return true;
                else
                    CheckFromTopToBot(ref i, ref j, playerIndex, ref inARow, ref coinCount, fields);

            if (coinCount == fourInARow)
                return true;

            columnIndex = column;
            rowIndex = row;

            while(columnIndex != 0 && rowIndex != Const.rowsCount - 1)
            {
                columnIndex--;
                rowIndex++;
            }

            i = columnIndex;
            j = rowIndex;

            inARow = i;
            coinCount = 0;

            while (i < Const.columnsCount && j < Const.rowsCount)
                if (coinCount == fourInARow)
                    return true;
                else
                    CheckFromBotToTopForRow(ref i, ref j, playerIndex, ref inARow, ref coinCount, fields);

            if (coinCount == fourInARow)
                return true;

        NotFourInARow:
            return false;
        }

        /// <summary>
        /// Checks how many coins in a row is in the diagonal.
        /// </summary>
        /// <param name="i">Index of the column.</param>
        /// <param name="j">Index of the row.</param>
        /// <param name="playerIndex">Index of the player, who did the move.</param>
        /// <param name="inARow">Index of the row to check.</param>
        /// <param name="coinCount">Count of coins in a row.</param>
        /// <param name="fields">Game field.</param>
        private static void CheckFromTopToBot(ref int i, ref int j, int playerIndex, ref int inARow, ref int coinCount, int[,] fields)
        {
            try
            {
                if (fields[i, j] == playerIndex && inARow == i)
                {
                    i++;
                    j++;
                    inARow++;
                    coinCount++;
                }
                else
                {
                    i++;
                    j++;
                    inARow = i;
                    coinCount = 0;
                }
            }
            catch (IndexOutOfRangeException) 
            { 
                i = Const.columnsCount; 
                j = Const.rowsCount; 
                return; 
            }
        }

        private static void CheckFromBotToTopForColumn(ref int i, ref int j, int playerIndex, ref int inARow, ref int coinCount, int[,] fields)
        {
            try
            {
                if (fields[i, j] == playerIndex && inARow == i)
                {
                    i--;
                    j++;
                    inARow++;
                    coinCount++;
                }
                else
                {
                    i--;
                    j++;
                    inARow = j;
                    coinCount = 0;
                }
            }
            catch (IndexOutOfRangeException)
            {
                i = Const.columnsCount;
                j = Const.rowsCount;
                return;
            }
        }

        private static void CheckFromBotToTopForRow(ref int i, ref int j, int playerIndex, ref int inARow, ref int coinCount, int[,] fields)
        {
            try
            {
                if (fields[i, j] == playerIndex && inARow == i)
                {
                    i++;
                    j--;
                    inARow++;
                    coinCount++;
                }
                else
                {
                    i++;
                    j--;
                    inARow = i;
                    coinCount = 0;
                }
            }
            catch (IndexOutOfRangeException)
            {
                i = Const.columnsCount;
                j = Const.rowsCount;
                return;
            }
        }
    }
}
