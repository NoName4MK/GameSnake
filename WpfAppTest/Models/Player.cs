using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrarySnakeFram;

namespace WpfAppTest.Models
{
    class Player:Figure
    {

        public int Id { get; set; }

        public Player(int id, List<ClassLibrarySnakeFram.Point> points)
        {
            Id = id;
            pList = points;
        }
        public string GetListPoint()
        {
            string str = null;
            foreach (ClassLibrarySnakeFram.Point point in pList)
            {
                str += String.Format(",x:{0},y:{1}", point.x, point.y);
            }
            return str;
        }
    }

}
