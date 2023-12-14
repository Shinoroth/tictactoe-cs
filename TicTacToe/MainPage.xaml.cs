namespace TicTacToe2
{
    enum Player {
        X,
        O,
        AI
    }

    public partial class MainPage : ContentPage {
        private Button[,] gameBoard;
        private int currentPlayer;
        private bool gameEnded;
        public MainPage()
        {
            InitializeComponent();

            gameBoard = new Button[,] {
                { btn_lu, btn_mu, btn_ru },
                { btn_ml, btn_mm, btn_rm },
                { btn_ll, btn_lm, btn_rl }
            };

            initializeGame();
        }

        void initializeGame() {
            for (int row = 0; row < 3; row++) {
                for (int col = 0; col < 3; col++) {
                    gameBoard[row, col].Text = " ";
                    gameBoard[row, col].IsEnabled = true;
                }
            }

            currentPlayer = 0; // 0 - X | 1 - O
            gameEnded = false;
        }

        void buttonClicked(object sender, EventArgs e) {
            Button button = (Button)sender;
            if (gameEnded || button.Text != " ") return;

            button.Text = ((currentPlayer == 0) ? "X" : "O");
            button.IsEnabled = false;

            if (checkForWinner()) {
                DisplayAlert("Game Over", $"Player {(currentPlayer == 0 ? "X" : "O")} wins!", "OK");
                gameEnded = true;
            }
            else if (isBoardFull()) {
                DisplayAlert("Game Over", "It's a draw!", "OK");
                gameEnded = true;
            }
            else {
                makeAIMove();
            }
            currentPlayer = 0;
        }

        private void newGameClicked(object sender, EventArgs e) {
            initializeGame();
        }

        bool checkForWinner() {

            for (int row = 0; row < 3; row++) {
                if (gameBoard[row, 0].Text == gameBoard[row, 1].Text && gameBoard[row, 1].Text == gameBoard[row, 2].Text
                    && !string.IsNullOrWhiteSpace(gameBoard[row, 0].Text))
                { return true; }
            }

            for (int col = 0; col < 3; col++) {
                if (gameBoard[0, col].Text == gameBoard[1, col].Text && gameBoard[1, col].Text == gameBoard[2, col].Text
                    && !string.IsNullOrWhiteSpace(gameBoard[0, col].Text))
                { return true; }
            }

            if (gameBoard[0, 0].Text == gameBoard[1, 1].Text && gameBoard[1, 1].Text == gameBoard[2, 2].Text
                && !string.IsNullOrWhiteSpace(gameBoard[0, 0].Text))
            { return true; }

            if (gameBoard[0, 2].Text == gameBoard[1, 1].Text && gameBoard[1, 1].Text == gameBoard[2, 0].Text
                && !string.IsNullOrWhiteSpace(gameBoard[0, 2].Text))
            { return true; }

            return false;
        }

        bool isBoardFull() {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (string.IsNullOrWhiteSpace(gameBoard[row, col].Text)) {
                        return false;
                    }
                }
            }
            return !checkForWinner();
        }

        void makeAIMove() {
            if (tryWinMove(out var winMove)) {
                applyMove(winMove, Player.AI);
            }
            else if(tryBlockMove(out var blockMove)) {
                applyMove(blockMove, Player.AI);
            }
            else { makeRandomMove(); }

            if (checkForWinner()) {
                DisplayAlert("Game Over", "AI wins!", "OK");
                gameEnded = true;
            }
            else if (isBoardFull()) {
                DisplayAlert("Game Over", "It's a draw!", "OK");
                gameEnded = true;
            }
        }

        bool tryWinMove(out Tuple<int, int> move) {
            for (int row = 0; row < 3; row++) {
                if (gameBoard[row, 0].Text == "O" && gameBoard[row, 1].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[row, 2].Text)) {
                    move = Tuple.Create(row, 2);
                    return true;
                }

                else if (gameBoard[row, 1].Text == "O" && gameBoard[row, 2].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[row, 0].Text)) {
                    move = Tuple.Create(row, 0);
                    return true;
                }

                else if (gameBoard[row, 0].Text == "O" && gameBoard[row, 2].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[row, 1].Text)) {
                    move = Tuple.Create(row, 1);
                    return true;

                }

            }

            // Check for winning move in columns

            for (int col = 0; col < 3; col++) {
                if (gameBoard[0, col].Text == "O" && gameBoard[1, col].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[2, col].Text)) {
                    move = Tuple.Create(2, col);
                    return true;
                }

                else if (gameBoard[1, col].Text == "O" && gameBoard[2, col].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[0, col].Text)) {
                    move = Tuple.Create(0, col);
                    return true;
                }

                else if (gameBoard[0, col].Text == "O" && gameBoard[2, col].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[1, col].Text)) {
                    move = Tuple.Create(1, col);
                    return true;
                }
            }

            // Check for winning move in diagonals

            if (gameBoard[0, 0].Text == "O" && gameBoard[1, 1].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[2, 2].Text)) {
                move = Tuple.Create(2, 2);
                return true;
            }

            else if (gameBoard[1, 1].Text == "O" && gameBoard[2, 2].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[0, 0].Text)) {
                move = Tuple.Create(0, 0);
                return true;
            }

            else if (gameBoard[0, 0].Text == "O" && gameBoard[2, 2].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[1, 1].Text)) {
                move = Tuple.Create(1, 1);
                return true;
            }

            if (gameBoard[0, 2].Text == "O" && gameBoard[1, 1].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[2, 0].Text)) {
                move = Tuple.Create(2, 0);
                return true;
            }

            else if (gameBoard[1, 1].Text == "O" && gameBoard[2, 0].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[0, 2].Text)) {
                move = Tuple.Create(0, 2);
                return true;
            }

            else if (gameBoard[0, 2].Text == "O" && gameBoard[2, 0].Text == "O" && string.IsNullOrWhiteSpace(gameBoard[1, 1].Text)) {
                move = Tuple.Create(1, 1);
                return true;
            }

            move = null;
            return false;
        }

        bool tryBlockMove(out Tuple<int, int> move) {
            // Check for blocking move in rows
            for (int row = 0; row < 3; row++) {
                if (gameBoard[row, 0].Text == "X" && gameBoard[row, 1].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[row, 2].Text)) {
                    move = Tuple.Create(row, 2);
                    return true;
                }
                else if (gameBoard[row, 1].Text == "X" && gameBoard[row, 2].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[row, 0].Text)) {
                    move = Tuple.Create(row, 0);
                    return true;
                }
                else if (gameBoard[row, 0].Text == "X" && gameBoard[row, 2].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[row, 1].Text)) {
                    move = Tuple.Create(row, 1);
                    return true;
                }
            }
            // Check for blocking move in columns
            for (int col = 0; col < 3; col++) {
                if (gameBoard[0, col].Text == "X" && gameBoard[1, col].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[2, col].Text)) {
                    move = Tuple.Create(2, col);
                    return true;
                }
                else if (gameBoard[1, col].Text == "X" && gameBoard[2, col].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[0, col].Text)) {
                    move = Tuple.Create(0, col);
                    return true;
                }
                else if (gameBoard[0, col].Text == "X" && gameBoard[2, col].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[1, col].Text)) {
                    move = Tuple.Create(1, col);
                    return true;
                }
            }

            // Check for blocking move in diagonals
            if (gameBoard[0, 0].Text == "X" && gameBoard[1, 1].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[2, 2].Text)) {
                move = Tuple.Create(2, 2);
                return true;
            }
            else if (gameBoard[1, 1].Text == "X" && gameBoard[2, 2].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[0, 0].Text)) {
                move = Tuple.Create(0, 0);
                return true;
            }
            else if (gameBoard[0, 0].Text == "X" && gameBoard[2, 2].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[1, 1].Text)) {
                move = Tuple.Create(1, 1);
                return true;
            }
            if (gameBoard[0, 2].Text == "X" && gameBoard[1, 1].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[2, 0].Text)) {
                move = Tuple.Create(2, 0);
                return true;
            }
            else if (gameBoard[1, 1].Text == "X" && gameBoard[2, 0].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[0, 2].Text)) {
                move = Tuple.Create(0, 2);
                return true;
            }
            else if (gameBoard[0, 2].Text == "X" && gameBoard[2, 0].Text == "X" && string.IsNullOrWhiteSpace(gameBoard[1, 1].Text)) {
                move = Tuple.Create(1, 1);
                return true;
            }
            move = null;
            return false;
        }

        void applyMove(Tuple<int, int> move, Player player) {
            gameBoard[move.Item1, move.Item2].Text = (player == Player.AI) ? "O" : "X";
            gameBoard[move.Item1, move.Item2].IsEnabled = false;
            currentPlayer = (player == Player.AI) ? 0 : 1;
        }

        void makeRandomMove() {
            List<Tuple<int, int>> emptyCells = new List<Tuple<int, int>>();

            for(int row = 0; row < 3; row++) {
                for(int col = 0; col < 3; col++) {
                    if (string.IsNullOrWhiteSpace(gameBoard[row, col].Text)) {
                        emptyCells.Add(Tuple.Create(row, col));
                    }
                }
            }

            if(emptyCells.Count > 0) {
                Random rand = new Random();
                int randomIndex = rand.Next(0, emptyCells.Count);
                applyMove(emptyCells[randomIndex], Player.AI);
            }
        }
    }
}