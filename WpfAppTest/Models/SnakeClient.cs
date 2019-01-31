using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfAppTest.Models
{
    public class SnakeClient
    {
        public string host { get; private set; }
        private int GarrentGameID;
        public SnakeClient(string host)
        {
            this.host = host;
        }
        public GameInfo GetCurrentGame()
        {
            GameInfo game = new GameInfo(ParseJeson(CallServer()));
            GarrentGameID = game.GameID;
            return game;
        }
        public GameInfo SendMove(string move)
        {
            string json = CallServer(GarrentGameID + "/" + move);
            GameInfo game = new GameInfo(ParseJeson(json));
            return game;
        }
        //public  void GetCurrentGame()
        //{
        //    Console.WriteLine(CallServer());
        //    //foreach( string key in ParseJeson(CallServer()))
        //}
        private string CallServer( string param ="")
        {
            //request server
            return "request server";
        }
        private NameValueCollection ParseJeson(string json)
        {
            NameValueCollection list = new NameValueCollection();
            string pattern = @"""(\w+)\"":""?([,""}]"")?";
            foreach (Match m in Regex.Matches(json, pattern))
                if (m.Groups.Count == 3)
                    list[m.Groups[1].Value] = m.Groups[2].Value;
            return list;
        }

            

    }
}
