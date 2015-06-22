using System;
using System.Timers;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace FourInARow
{
    public partial class GameScreen : Form
    {
#region const
        private const int xLoc = 12;
        private const int yLoc = 41;
        private const int fieldWidth = 82;
        private const int fieldHeight = 73;
        private const int distBetwFields = 6;
        private const int secondInterval = 1000;
        private const int coinWidthNHeight = 50;
        private const int resultsCount = 4;
#endregion

        System.Timers.Timer gameTimer;
        DateTime timerTime = new DateTime();
        Panel[,] UIGameField;
        GameField logicGameField;
        Color[] players;
        int currentPlayer;
        bool isItStartOfTheGame;

        public List<Result> results;

        public delegate void OpenMainWindow();
        public delegate void CloseAllWindows();
        public delegate void EndOfTheGame();

        public event OpenMainWindow goOpenMainWindow;
        public event CloseAllWindows weNeedToStop;
        public event EndOfTheGame firstPlayerWins;
        public event EndOfTheGame secondPlayerWins;
        public event EndOfTheGame draw;

        public GameScreen()
        {
            currentPlayer = -1;
            logicGameField = new GameField();
            isItStartOfTheGame = true;
            InitGameField();
            InitPlayers();
            InitializeComponent();
            timerLabel.Text = "0:0";
            this.FormClosed += CloseTheForm;      
            GetResults();
        }

        /// <summary>
        /// Gets results from the file.
        /// </summary>
        private void GetResults()
        {
            results = new List<Result>();
            FileStream streamForAccesToFile = File.Open("BestTime.txt", FileMode.OpenOrCreate);
            StreamReader streamForReadResults = new StreamReader(streamForAccesToFile);
            string[] tmp;
            for (int i = 0; i < resultsCount; i++)
            {
                if (!streamForReadResults.EndOfStream)
                {
                    tmp = streamForReadResults.ReadLine().Split('-');
                    results.Add(new Result(tmp[0] + "-", Convert.ToDateTime(tmp[1])));
                }
                else
                {
                    streamForReadResults.Close();
                    return;
                }
            }
        }

        /// <summary>
        /// Event that occurs when form is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseTheForm(object sender, FormClosedEventArgs e)
        {
            weNeedToStop();
        }

        private void InitPlayers()
        {
            players = new Color[2];
            players[0] = Color.Red;
            players[1] = Color.Blue;

            currentPlayer = 0;
        }

        private void InitTimer()
        {         
            gameTimer = new System.Timers.Timer();
            timerTime = DateTime.Today;
            gameTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            gameTimer.Interval = secondInterval;
            gameTimer.Enabled = true;          
        }

        /// <summary>
        /// Timer ticks.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object o, ElapsedEventArgs e)
        {
            timerTime = timerTime.AddSeconds(1);
            timerLabel.Text = timerTime.Minute.ToString() + ":" + timerTime.Second.ToString();
        }

        private void InitGameField()
        {
            UIGameField = new Panel[Const.columnsCount, Const.rowsCount];

            for(int i = 0; i < Const.columnsCount; i++)
                for(int j = 0; j < Const.rowsCount; j++)
                {
                    var newPanel = new Panel();
                    newPanel.Width = fieldWidth;
                    newPanel.Height = fieldHeight;

                    newPanel.Location = new Point(xLoc + distBetwFields * i + i * fieldWidth, yLoc + distBetwFields * j + j * fieldHeight);
                    newPanel.BackColor = Color.White;
                    newPanel.BorderStyle = BorderStyle.FixedSingle;
                    newPanel.Click += field_Click;

                    UIGameField[i, j] = newPanel;
                    this.Controls.Add(newPanel);
                }
        }

        /// <summary>
        /// Event that occurs when panel is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void field_Click(object sender, EventArgs e)
        {         
            int column;
            int row;

            if (isItStartOfTheGame)
            {
                isItStartOfTheGame = false;
                InitTimer();
            }

            FindAField((sender as Panel), out column, out row);

            if (GameLogic.CanMove(column, ref row, logicGameField.Fields))
            {
                logicGameField.AddMove(column, row, currentPlayer);

                PaintACoint(UIGameField[column, row], currentPlayer);

                switch (GameLogic.IfGameEnd(column, row, currentPlayer, logicGameField.Fields))
                {
                    case -1:
                        if (Convert.ToBoolean(currentPlayer))
                        {
                            currentPlayer = Convert.ToInt16(false);
                            turnLabel.Text = "Player " + (currentPlayer + 1).ToString() + "`s turn";
                        }
                        else 
                        { 
                            currentPlayer = Convert.ToInt16(true);
                            turnLabel.Text = "Player " + (currentPlayer + 1).ToString() + "`s turn";
                        }
                        return;
                    case 0:
                        {
                            results.Add(new Result("First player-", Convert.ToDateTime(timerLabel.Text)));
                            results.SortList();
                            StreamWriter sw = new StreamWriter("BestTime.txt");
                            for (int i = 0; i < results.Count; i++)
                                sw.WriteLine(results[i].player.ToString() + results[i].result.ToString());
                            sw.Close();
                            firstPlayerWins();
                            GetResults();
                            isItStartOfTheGame = true;
                            gameTimer.Stop();
                            break;
                        }

                    case 1:
                        {
                            results.Add(new Result("Second player-", Convert.ToDateTime(timerLabel.Text)));
                            results.SortList();
                            var sr = new StreamWriter("BestTime.txt");
                            for (int i = 0; i < results.Count; i++)
                                sr.WriteLine(results[i].player.ToString() + results[i].result.ToString());
                            sr.Close();
                            secondPlayerWins();
                            GetResults();
                            isItStartOfTheGame = true;
                            gameTimer.Stop();
                            break;
                        }
                    case 2:
                        {
                            draw();
                            isItStartOfTheGame = true;
                            gameTimer.Stop();
                            break;
                        }
                }          
            }
        }

        private void PaintACoint(object sender, int player)
        {
            if (player != -1)
            {
                Graphics g = (sender as Panel).CreateGraphics();
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(new SolidBrush(players[player]), (fieldWidth - coinWidthNHeight) / 2, (fieldHeight - coinWidthNHeight) / 2,
                    coinWidthNHeight, coinWidthNHeight);
            }
        }

        /// <summary>
        /// Finds the field that was clicked in the array.
        /// </summary>
        /// <param name="field">Game field.</param>
        /// <param name="column">Index of the column.</param>
        /// <param name="row">Index of the row.</param>
        private void FindAField(Panel field, out int column, out int row)
        {
            column = -1;
            row = -1;

            for (int i = 0; i < Const.columnsCount; i++)
                for (int j = 0; j < Const.rowsCount; j++)
                    if (UIGameField[i, j] == field)
                    {
                        column = i;
                        row = j;
                        return;
                    }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            goOpenMainWindow();
            isItStartOfTheGame = true;
            if(gameTimer != null)
                gameTimer.Stop();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            restartGame();
            isItStartOfTheGame = true;
            if(gameTimer != null)
                gameTimer.Stop();
        }

        public void restartGame()
        {   
            isItStartOfTheGame = true;
            if(gameTimer != null)
                gameTimer.Stop();

            turnLabel.Text = "Player 1`s turn";
            timerLabel.Text = "0:0";

            for (int i = 0; i < Const.columnsCount; i++)
                for (int j = 0; j < Const.rowsCount; j++)
                    this.Controls.Remove(UIGameField[i, j]);

            logicGameField = new GameField();
            InitGameField();
            InitPlayers();

            Graphics g = CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle rec = new Rectangle();
            PaintEventArgs es = new PaintEventArgs(g, rec);
            this.OnPaint(es);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (UIGameField != null)
                for (int i = 0; i < Const.columnsCount; i++)
                    for (int j = 0; j < Const.rowsCount; j++)
                        PaintACoint(UIGameField[i, j], logicGameField.Fields[i, j]);
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {

        }      
    }

    /// <summary>
    /// Extention class for list of results.
    /// </summary>
    public static class ListExtentions
    {
        /// <summary>
        /// Sort list of results.
        /// </summary>
        /// <param name="results">Sorted list of results.</param>
        /// <returns></returns>
        public static List<Result> SortList(this List<Result> results)
        {
            for (int i = 0; i < results.Count - 1; i++)
            {
                for (int j = i + 1; j < results.Count; j++)
                {
                    if (results[i].result > results[j].result)
                    {
                        var tmp = results[i];
                        results[i] = results[j];
                        results[j] = tmp;
                    }
                }
            }
            return results;
        }
    }
    public struct Result
    {
        public string player { get; set; }
        public DateTime result { get; set; }

        public Result(string pl, DateTime res)
            : this()
        {
            player = pl;
            result = res;
        }
    }
}
