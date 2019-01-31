using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ClassLibrarySnakeFram;
using WpfAppTest.Models;
using ClassLibrarySocketClietn;


namespace WpfAppTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
  
        public MainWindow()
        {
            
            InitializeComponent();

        }
 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up) game.HandleKey(Direction.Up);
            if (e.Key == Key.Left) game.HandleKey(Direction.Left);
            if (e.Key == Key.Down) game.HandleKey(Direction.Down);
            if (e.Key == Key.Right) game.HandleKey(Direction.Right);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PortGame.Focusable = false;
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 150);
            dispatcherTimer.Start();

            game = new Game();
        }

        public void AddEllips(int x, int y, int sizeFigure)
        {
            Ellipse ellipse1 = new Ellipse() { Height = sizeFigure, Width = sizeFigure };
            ellipse1.Stroke = Brushes.Blue;
            ellipse1.Name = "e" + x + y;
            Canvas.SetLeft(ellipse1, x * sizeFigure);
            Canvas.SetTop(ellipse1, y * sizeFigure);
            MyCanvas.Children.Add(ellipse1);
        }
        private void MoveSnake()
        {
            List<ListPoint> listPoint = new List<ListPoint>();
            
            MyCanvas.Children.Clear();
            listPoint=game.MoveSnake();
            if (game.statusGame == false) EndGame();
            else
            {
                foreach (var p in listPoint)
                {
                    AddEllips(p.X, p.Y, p.SizeFigure);
                }
               
            }
        }
        private void EndGame()
        {
            dispatcherTimer.Stop();
           // game.


        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            Game.StartServer();
        }
    }
}
