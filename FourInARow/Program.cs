using System;
using System.Windows.Forms;

namespace FourInARow
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static MainMenuScreen newMainScreen;
        static GameScreen gameScreen;
        static WiningForm winForm;
        static BestTimeScreen resultScreen;
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InitMainMenuScreen();
            InitGameScreen();
            InitWinForm();
            InitResultScreen();
            Application.Run(newMainScreen);
        }

        /// <summary>
        /// Initialize main menu screen.
        /// </summary>
        static void InitMainMenuScreen()
        {
            newMainScreen = new MainMenuScreen();
            newMainScreen.GoOn += RunGameScreen;
            newMainScreen.weNeedToGo += RunResultScreen;
            newMainScreen.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        /// <summary>
        /// Initialize game screen.
        /// </summary>
        static void InitGameScreen()
        {
            gameScreen = new GameScreen();
            gameScreen.FormBorderStyle = FormBorderStyle.FixedDialog;
            gameScreen.goOpenMainWindow += RunMainScreen;
            gameScreen.weNeedToStop += CloseApp;
            gameScreen.firstPlayerWins += FirstPlayerWins;
            gameScreen.secondPlayerWins += SecondPlayerWinds;
            gameScreen.draw += Draw;
        }

        /// <summary>
        /// Initialize winning screen.
        /// </summary>
        static void InitWinForm()
        {
            winForm = new WiningForm();
            winForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            winForm.newGame += RunGameScreen;
            winForm.weNeedGoToMainMenu += RunMainScreen;
        }

        /// <summary>
        /// Initialize result screen.
        /// </summary>
        static void InitResultScreen()
        {
            resultScreen = new BestTimeScreen(gameScreen.results);
            resultScreen.thisIsTheEnd += CloseApp;
            resultScreen.backToMainMenu += RunMainScreen;
        }

        /// <summary>
        /// Shows result screen.
        /// </summary>
        static void RunResultScreen()
        {
            newMainScreen.Hide();
            resultScreen.Show();
        }

        /// <summary>
        /// Shows game screen.
        /// </summary>
        static void RunGameScreen()
        {
            newMainScreen.Hide();
            gameScreen.restartGame();
            gameScreen.Enabled = true;
            if(gameScreen.Visible == false)
                gameScreen.Show();
            winForm.Hide();
        }

        /// <summary>
        /// Shows main screen.
        /// </summary>
        static void RunMainScreen()
        {
            resultScreen.Hide();
            gameScreen.Hide();
            winForm.Hide();
            newMainScreen.Show();           
        }

        /// <summary>
        /// Shows winning form.
        /// </summary>
        static void FirstPlayerWins()
        {
            winForm.Text = "First Player Wins!";
            winForm.Show();
            gameScreen.Enabled = false;
            InitResultScreen();
        }

        /// <summary>
        /// Shows winning form.
        /// </summary>
        private static void SecondPlayerWinds()
        {
            winForm.Text = "Second Player Wins!";
            winForm.Show();
            gameScreen.Enabled = false;
            InitResultScreen();
        }

        /// <summary>
        /// Shows winning form.
        /// </summary>
        private static void Draw()
        {
            winForm.Text = "Draw!";
            winForm.Show();
            gameScreen.Enabled = false;
        }

        /// <summary>
        /// Closes application.
        /// </summary>
        static void CloseApp()
        {
            Application.Exit();
        }
    }
}
