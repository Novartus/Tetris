using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tetris.Blocks;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] blockTileImg = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Blue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Cyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Green.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Orange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Purple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Red.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Yellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Empty_Space.png", UriKind.Relative)),
        };

        private readonly ImageSource[] blockImg = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block_Design_Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block_Design_I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block_Design_J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block_Design_L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block_Design_O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block_Design_S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block_Design_T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block_Design_Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private readonly int maxDelay = 999;
        private readonly int minDelay = 74;
        private readonly int delayDecrease = 24;
        private State state = new State();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = GenerateGameCanvas(state.GameGrid);
        }

        private Image[,] GenerateGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int row = 0; row < grid.Rows; row++)
            {
                for (int column = 0; column < grid.Columns; column++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };
                    Canvas.SetTop(imageControl, (row - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, column * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[row, column] = imageControl;
                }
            }
            return imageControls;
        }


        private void DrawGrid(GameGrid grid)
        {
            for (int row = 0; row < grid.Rows; row++)
            {
                for (int column = 0; column < grid.Columns; column++)
                {
                    int ID = grid[row, column];
                    imageControls[row, column].Opacity = 1;
                    imageControls[row, column].Source = blockTileImg[ID];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilesPosition())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = blockTileImg[block.ID];
            }
        }

        private void DrawNextBlock(QueueBlock blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImg[next.ID];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImg[0];
            }
            else
            {
                HoldImage.Source = blockImg[heldBlock.ID];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = state.BlockDropDistance();

            foreach (Position p in block.TilesPosition())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = blockTileImg[block.ID];
            }
        }

        private void Draw(State state)
        {
            DrawGrid(state.GameGrid);
            DrawGhostBlock(state.CurrentSelectedBlock);
            DrawBlock(state.CurrentSelectedBlock);
            DrawNextBlock(state.BlockQueue);
            DrawHeldBlock(state.BlockOnHold);
            ScoreText.Text = $"Your Score: {state.Score}";
        }

        private void Window_Key_Down(object sender, KeyEventArgs e)
        {
            if (state.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    state.MoveBlockLeft();
                    break;
                case Key.Right:
                    state.MoveBlockRight();
                    break;
                case Key.Down:
                    state.MoveBlockDown();
                    break;
                case Key.Up:
                    state.RotateBlockClockWise();
                    break;
                case Key.Z:
                    state.RotateBlockCounterClockWise();
                    break;
                case Key.H:
                    state.HoldBlock();
                    break;
                case Key.Space:
                    state.BlockDropper();
                    break;
                default:
                    return;
            }

            Draw(state);
        }

        private async Task GameLoop()
        {
            Draw(state);

            while (!state.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (state.Score * delayDecrease));
                await Task.Delay(delay);
                state.MoveBlockDown();
                Draw(state);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Your Score: {state.Score}";
        }

        private async void Game_Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void Play_Again_Click(object sender, RoutedEventArgs e)
        {
            state = new State();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}
