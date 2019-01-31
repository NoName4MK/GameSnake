using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ClassLibrarySnakeFram;
using ClassLibrarySocketClietn;
using WpfAppTest;


namespace WpfAppTest.Models
{
    public struct ListPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int SizeFigure { get; set; }

        public ListPoint(int x, int y , int size)
        {
            X = x;
            Y = y;
            SizeFigure = size;
        }

    }

    public class Game
    {
        public Boolean statusGame = true;
        public const string host = "";
        private const int sizeFigure = 10;
        private Snake snake;
        private Walls walls;
        private static Player player;
        private BlockingCollection<Figure> blockingCollection;
        public bool loop=true;


        private struct ElementPlayer
        {
            public string name;
            public int value;
            public ElementPlayer(string name, int value)
            {
                this.name = name;
                this.value = value;
            }
          
        }


        public Game()
        {
            
            ClassLibrarySnakeFram.Point p = new ClassLibrarySnakeFram.Point(5, 5);
            snake = new Snake(p, 2, Direction.Down);
            walls = new Walls(50, 50);
            player = new Player(0, snake.pList);
            Client();
        }

        private async void Client()
        {
            await Task.Run(() => SocketClientStart());
        }

        private void SocketClientStart()
        {
            Figure figure1 = new Figure();
            try
            {
                SocketClientAsync socketClient = new SocketClientAsync();
                while (loop)
                {

                    blockingCollection = new BlockingCollection<Figure>();
                    figure1 = ReceiveReg(socketClient.StartClient(String.Format("id:{0}{1}<EOF>", player.Id, player.GetListPoint())));
                    blockingCollection.TryAdd(figure1);
                    Thread.Sleep(80);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка:" + e.ToString());
                //MessageBox.Show("Требуется ввести имя", "Ошибка при вводе имени", MessageBoxButton.OK, MessageBoxImage.Error);                
            }
            finally
            {
                loop = false;
            }

        }

        private List<ListPoint> DrawWPF(List<ClassLibrarySnakeFram.Point> pList)
        {
            List<ListPoint> listPoint = new List<ListPoint>();
            foreach (var p in pList)
            {
                listPoint.Add(new ListPoint(p.x, p.y, sizeFigure));
            }
            return listPoint;
        }

        public List<ListPoint> MoveSnake()
        {
            //Thread.Sleep(300);
            if (walls.IsHit(snake) || snake.IsHitTail()) //смотрим препятствия и проверяем не столкнулись ли с собой.
            {
                statusGame = false;
            }
            if (walls.IsHitFuture(snake.GetNextPoint()))
            {
                snake.HandleKey();
            }
            snake.Move();
            //player.pList = new List<ClassLibrarySnakeFram.Point>(snake.pList);
            player.pList = snake.pList.ToList();
            //Figure figure1 = new Figure(player.pList);
            Figure figure1 = new Figure();
            figure1.pList = player.pList.ToList();

            foreach (var p in blockingCollection)
            {
                figure1.pList.AddRange(p.pList);
            }
            return DrawWPF(figure1.Draw());
        }

        public void HandleKey(Direction direction)
        {
            snake.HandleKey(direction);
        }

        static private Figure ReceiveReg(string strreg)
        {
            List<ElementPlayer> elems = new List<ElementPlayer>();
            Figure figureRReceiveReg = new Figure();
            List<ClassLibrarySnakeFram.Point> points = new List<ClassLibrarySnakeFram.Point>();

            strreg = strreg.Substring(0, strreg.Length);
            //(\w+)\:"?([^,"})]*)?
            string pattern = @"(\w+)\:""?([^,""})]*)?";
            foreach (Match m in Regex.Matches(strreg, pattern))
                if (m.Groups.Count == 3)
                    elems.Add(new ElementPlayer(m.Groups[1].Value, int.Parse(m.Groups[2].Value)));
     
            int id = 0;
            int x = 0;
            int y = 0;
            bool first=true;

            foreach (ElementPlayer el in elems)
            {
                if (el.name == "id")
                { 
                    if ((el.value != id) & (id != 0))
                    {
                        if (first)
                        { 
                            player.Id = id;
                            first = false;
                            points.Clear();
                        }
                    }
                    id = el.value;
                }
                if (el.name == "x")
                    x = el.value;
                if (el.name == "y")
                {
                    y = el.value;
                    points.Add(new ClassLibrarySnakeFram.Point(x,y));
                }                
            }
            if (first)
            { 
                player.Id = id;
                points.Clear();
            }
            figureRReceiveReg.pList = points;
            return figureRReceiveReg;
        }
    }
}
