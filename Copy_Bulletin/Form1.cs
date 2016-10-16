using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Copy_Bulletin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //mongo_db = new mdb();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private WebBrowser web_connecter = new WebBrowser();
        private string file_place;
        private mdb mongo_db;
        private static void system_record(ref RichTextBox box, string words, Color color)
        {
            box.Select(box.TextLength, 0);
            box.SelectionColor = Color.Black;
            box.AppendText("[" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "] \r\n");
            box.Select(box.TextLength, 0);
            box.SelectionColor = color;
            box.AppendText(words);
        }

        private void start_button_Click_1(object sender, EventArgs e)
        {
            file_place = file_textBox.Text;
            if (string.IsNullOrWhiteSpace(file_place))
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = "C:";
                ofd.Filter = "Text Files (*.txt)|*.txt";//|All Files (*.*)|*.*";
                ofd.Title = "請開啟文字檔案";
                ofd.CheckFileExists = true;				  //檔案 不存在顯示錯誤訊息
                ofd.CheckPathExists = true;                 //路徑 不存在顯示錯誤訊息
                if (ofd.ShowDialog(this) == DialogResult.Cancel)
                    return;
                file_textBox.Text = ofd.FileName;
            }
            else if (file_place == "clear")
            {
                display_textBox.Clear();
                file_textBox.Clear();
                return;
            }
            else if (File.Exists(file_place))
            {
                system_record(ref display_textBox, "Start load\r\n", Color.Blue);

                StreamReader input_txt = new StreamReader(file_place);
                string target_url = input_txt.ReadLine();
                input_txt.Close();

                int signn = html_request(ref target_url);
                if (signn  == 1)
                { 
                    //success
                    system_record(ref display_textBox, "End load\r\n", Color.Green);
                }
                else
                {
                    //fail
                    system_record(ref display_textBox, "Return not ok or empty\r\n", Color.Red);
                }
            }
            else
            {
                system_record(ref display_textBox, "ERROR : file is not exists\r\n", Color.Red);
            }
        }

        private int html_request(ref string target_url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(target_url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Encoding encoding = Encoding.Default; 
                switch (response.CharacterSet.ToLower()) 
                {
                    case "gbk":
                        encoding = Encoding.GetEncoding("GBK");
                        break;
                    case "gb2312":
                        encoding = Encoding.GetEncoding("GB2312");
                        break;
                    case "utf-8":
                        encoding = Encoding.UTF8;
                        break;
                    case "iso-8859-1":
                        encoding = Encoding.GetEncoding("Big5");    //GB2312                
                        break;
                    case "big5":
                        encoding = Encoding.GetEncoding("Big5");
                        break;
                    default:
                        encoding = Encoding.UTF8;
                        break;
                }

                Stream receiveStream = response.GetResponseStream();
                if (response.CharacterSet == null)
                {
                    system_record(ref display_textBox, "ERROR : Get NULL CharacterSet\r\n", Color.Red);
                    response.Close();
                    return 0;
                }

                StreamReader readStream = new StreamReader(receiveStream, encoding);
                document_completed(readStream.ReadToEnd());

                response.Close();
                readStream.Close();
                receiveStream.Close();
            }
            else
            {
                return 0;
            }

            return 1;
        }

        private void document_completed(string RowWebContent)
        {
            remove_non_breaking_space(ref RowWebContent);
            remove_comment(ref RowWebContent);
            remove_javascript(ref RowWebContent);
            /*remove_htmltag(ref RowWebContent);
            remove_enter_space(ref RowWebContent);*/

            string activitys;

            activitys = get_tbody(RowWebContent);

            BsonDocument dataa = new BsonDocument{
                {"Title","all activitys"},
                {"Body",activitys},
            };
            //mongo_db.insertt(ref dataa);
           
            Form2 obj = new Form2(activitys);//把Form1的值給到Form2
            obj.ShowDialog(this);

            if (obj.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter output_txt = new StreamWriter(file_place, true, Encoding.UTF8);
                output_txt.WriteLine("#########################################################");
                output_txt.WriteLine(activitys);
                output_txt.Close();
                system_record(ref display_textBox, "Recorded\r\n", Color.Blue);
            }
            else if (obj.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            { }
            else
            { }

        }
        
        private string get_tbody(string pure_data)
        {
            /*first time deal*/
            MatchCollection v = Regex.Matches(pure_data, @"(<tr>\s*(<td.*>.*<\/td>\s*){2}<\/tr>\s*?){3}");
            List<string> tmp = new List<string>();
            string outt = "";
            if (v.Count > 0)
            {
                foreach (Match m in v)
                {
                    tmp.Add(m.Groups[0].ToString());
                }
            }
            else
                return "";

            /*second time deal*/
            List<string> tmp1 = new List<string>();
            foreach (string tdata in tmp)
            {
                MatchCollection v1 = Regex.Matches(tdata, "<td.*?alt=\"(.*?)\"[\\s\\S]*?<td.*?href=\"(.*?)\".*?title=\"(.*?)\"[\\s\\S]*?<td.*?<\\/td>[\\s\\S]*?<td.*?>(.*?)<\\/td>[\\s\\S]*?<td.*?<\\/td>[\\s\\S]*?<td.*?>(.*?)<\\/td>");
                if (v1.Count > 0)
                {
                    outt += ("類別 : " + v1[0].Groups[1].ToString() + "\r\n");
                    outt += ("網址 : " + v1[0].Groups[2].ToString() + "\r\n");
                    outt += ("名稱 : " + v1[0].Groups[3].ToString() + "\r\n");
                    outt += ("時間 : " + v1[0].Groups[4].ToString() + "\r\n");
                    outt += ("地點 : " + v1[0].Groups[5].ToString() + "\r\n");

                    outt += ("\r\n" + "\r\n");
                }
            }

            return outt;
        }

        private void remove_non_breaking_space(ref string pure_data)
        {
            pure_data = Regex.Replace(pure_data, @"&nbsp;", "");
        }

        private void remove_comment(ref string pure_data)
        {
            pure_data = Regex.Replace(pure_data, @"<!--[\s\S]*?-->", "");
        }

        private void remove_javascript(ref string pure_data)
        {
            pure_data = Regex.Replace(pure_data, @"<script[\d\D]*?>[\d\D]*?</script>", "");
            pure_data = Regex.Replace(pure_data, @"<noscript[\d\D]*?>[\d\D]*?</noscript>", "");
        }

        private void remove_htmltag(ref string pure_data)
        {
            pure_data = Regex.Replace(pure_data, @"<[^>]*>", "");
        }

        private void remove_enter_space(ref string pure_data)
        {
            pure_data = Regex.Replace(pure_data, @"\n[\n\s\t\f]*", "\n");
        }


    }

    public class mdb
    {
        public string connectionString;
        private MongoClient _client;
        private MongoServer _server;
        private MongoDatabase db;
        private MongoCollection _collection;
        public mdb()
        {
            connectionString = "mongodb://localhost:27017";
            _client = new MongoClient(connectionString);
            _server = _client.GetServer();
            _server.Connect();
            db = _server.GetDatabase("test");
            _collection = db.GetCollection("nctu_bot");
        }

        public int connectt(string connectString, string data_base, string collectionn)
        {
            _server.Disconnect();
            _client = new MongoClient(connectionString);
            _server = _client.GetServer();
            _server.Connect();
            db = _server.GetDatabase(data_base);
            _collection = db.GetCollection(collectionn);
            return 1;
        }

        public int insertt(ref BsonDocument input)
        {
            try
            {
                _collection.Insert(input);
            }
            catch (MongoWriteConcernException ex)
            {
                return 0;
            }
            return 1;
        }

        ~mdb()
        {
            _server.Disconnect();
        }

    }
}




/*
           string connectionString = "mongodb://localhost:27017";
           MongoClient _client = new MongoClient(connectionString);
           MongoServer _server = _client.GetServer();
           _server.Connect();
           MongoDatabase db = _server.GetDatabase("test");
           MongoCollection _collection = db.GetCollection("nctu_bot");
           BsonDocument inserData1 = new BsonDocument{
               {"Title","c# "},
               {"Body",RowWebContent},
           };
           _collection.Insert(inserData1);
           _server.Disconnect();
           */
