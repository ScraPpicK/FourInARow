using System;

namespace FourInARow
{
    public class GameField
    {
        public int[,] Fields { get; set; }

        public GameField()
        {
            Fields = new int[Const.columnsCount, Const.rowsCount];
            GameLogic.InitField(this.Fields);
        } 

        /// <summary>
        /// Adds the move to the field.
        /// </summary>
        /// <param name="column">Index of the column player want to move</param>
        /// <param name="row">Index of the row player want to move</param>
        /// <param name="playerIndex">Index of the player want to move</param>
        /// <returns></returns>
        public void AddMove(int column, int row, int playerIndex)
        {
            if(GameLogic.CanMove(column, ref row, this.Fields))
            {
                Fields[column, row] = playerIndex;
            }
        }
    }
}
