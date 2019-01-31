using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.Models
{
    public struct GameInfo
    {
        public int GameID;
     
        public GameInfo(NameValueCollection list)
        {
            GameID=int.Parse(list["GameID"]);
        }
    }
}
