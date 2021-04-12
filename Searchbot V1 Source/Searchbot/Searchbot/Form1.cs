using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

using System.Data.SqlClient;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Searchbot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {



        }
        public string[] Spaceless;
        private void button_click(object sender, EventArgs e)
        {
            this.richTextBox2.AppendText("\r\n");
            this.richTextBox2.AppendText("\r\n");
            this.richTextBox2.AppendText("User:" + this.richTextBox1.Text);
            responce();
        }
        public void responce()
        {
            this.richTextBox2.AppendText("\r\n");
            this.richTextBox2.AppendText("\r\n");
            this.richTextBox2.AppendText("SearchBot: Processing request. One momment please...");
            parser();
        }
        public void parser()
        {
            this.richTextBox2.AppendText("\r\n");
            
            
            //preparing the text to be most properly split into peices for the queary
            // removes common punctuations and turns them into spaces so that words are in simple forms to be put into SQL
            string Raw = this.richTextBox1.Text.ToLower();
            string punctless1 = Raw.Replace(".", " ");
            string punctless2 = punctless1.Replace("?", " ");
            string punctless3 = punctless2.Replace("'", " ");
            string punctless4 = punctless3.Replace("!", " ");
            string punctless5 = punctless4.Replace("\"", " ");
            string punctless6 = punctless5.Replace("\\", " ");
            string punctless7 = punctless6.Replace("/", " ");
            string punctless8 = punctless7.Replace("(", " ");
            string punctless9 = punctless8.Replace(")", " ");
            string punctless10 = punctless9.Replace(",", " ");
            string punctless11 = punctless10.Replace(":", " ");
            string punctless12 = punctless11.Replace(";", " ");
            string punctless13 = punctless12.Replace("-", " ");


            Spaceless = punctless13.Split(' ');
           
            this.sqlgenerator(Spaceless);
        }

        public void sqlgenerator(String[] parsedtxt)
        {
            // format parsed text to fit query specifications
            string formatedParsedtext = "";
            this.richTextBox2.AppendText("\r\n");
            foreach (string word in parsedtxt)
            {
                formatedParsedtext = formatedParsedtext + "'";
                formatedParsedtext = formatedParsedtext + word;
                formatedParsedtext = formatedParsedtext + "'";
                if (word != parsedtxt.Last())
                {
                    formatedParsedtext = formatedParsedtext + ",";
                }

            }



            //this connection is on a SQL express local server so in
            //order for this to be used in the hypothetical final version it would need to be converted to a hosted database.
            //but for the sake of the initial testing it will be local


            //connection and queary setup
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=LAPTOP-KDKHLBLU\SQLEXPRESS;Initial Catalog=SearchBotDB;Integrated Security=true";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand command;
            SqlDataReader reader;
            string query;

            query = "Select * From MainTable Where Name In (" + formatedParsedtext + ")";

            command = new SqlCommand(query, cnn);
            reader = command.ExecuteReader();

            // handles the presentation of the returned database values
            bool isReturnedValue = false;
            while (reader.Read())
            {
                
                if (reader.GetValue(0).ToString() != null)
                {

                    this.richTextBox2.AppendText("Searchbot: " + reader.GetValue(0).ToString() + reader.GetValue(1).ToString());
                    this.richTextBox2.AppendText("\r\n");
                    isReturnedValue = true;

                }


            }
            //this is for the case that the search returns no results
            if (isReturnedValue == false)
            {
                this.richTextBox2.AppendText("Searchbot:Your question did not return any page results. \r\nPlease check your spelling or try a new question. ");
            }
            //clears the initial textbox for ease of use.
            this.richTextBox1.Clear();

            //closes all opened connections
            reader.Close();
            command.Dispose();
            cnn.Close();
        }



    }
}
