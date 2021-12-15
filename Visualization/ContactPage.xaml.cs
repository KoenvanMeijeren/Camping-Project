using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for ContactPage.xaml
    /// </summary>
    public partial class ContactPage : Page
    {
        public ContactPage()
        {
            InitializeComponent();

/*            try
            {
                string DistanceApiUrl = ConfigurationManager.AppSettings["DistanceApi"];
                string myKey = ConfigurationManager.AppSettings["ApiKey"];
                string url = DistanceApiUrl + txtOrigins.Text + "&destinations=" + txtDestinations.Text"&mode=driving&sensor=false&language=en-EN&units=imperial&Key=" + myKey;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader sreader = new StreamReader(dataStream);
                string responsereader = sreader.ReadToEnd();
                response.Close();
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlTextReader(new StringReader(responsereader)));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables["element"].Rows[0]["status"].ToString() == "OK")
                    {
                        objDistance.durtion = Convert.ToString(ds.Tables["duration"].Rows[0]["text"].ToString().Trim());
                        objDistance.distance = Convert.ToDouble(ds.Tables["distance"].Rows[0]["text"].ToString().Replace("mi", "").Trim());
                    }
                }
                txtDuration.Text = objDistance.durtion;
                txtDistance.Text = Convert.ToString(objDistance.distance);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in calculating Distance!" + ex.Message);
            }*/
        }
    }
}
