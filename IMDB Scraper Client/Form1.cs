using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Collections;

namespace IMDB_Scraper_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "http://www.omdbapi.com/?t=" + textBox1.Text.Trim() + "&y=" + textBox3.Text +"&apikey=49b58569";
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                JavaScriptSerializer oJS = new JavaScriptSerializer();
                ScraperProperties obj = new ScraperProperties();
                obj = oJS.Deserialize<ScraperProperties>(json);
                if (obj.Response == "True")
                {
                    button2.PerformClick();
                    textBox4.Text = obj.Poster;
                    listBox1.Items.Add(textBox1.Text);
                    textBox6.Text = obj.Plot;
                    pictureBox1.ImageLocation = textBox4.Text;
                    DataTable dt = new DataTable();
                    dt = (DataTable)dataGridView1.DataSource;
                    string c1 = obj.imdbID;
                    string c2 = obj.imdbRating;
                    string c3 = obj.Title;
                    string c4 = obj.Year;
                    string c5 = obj.Runtime;
                    string c6 = obj.Genre;
                    string c7 = obj.Director;
                    string c8 = obj.Writer;
                    string c9 = obj.Actors;
                    string c10 = obj.Language;
                    string c11 = obj.Awards;
                    string[] row = { c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11 };
                    dataGridView1.Rows.Add(row);
                }
                else
                {
                    MessageBox.Show("Make sure you have entered both the Title and Year correctly!", "Movie couldn't be found...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            textBox6.Text = "";
            dataGridView1.Rows.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(Properties.Settings.Default.Recent.ToArray());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Recent = new ArrayList(listBox1.Items);
            Properties.Settings.Default.Save();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.SelectedItem.ToString();
            button1.PerformClick();
        }
    }
}
