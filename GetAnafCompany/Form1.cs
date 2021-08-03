using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace GetAnafCompany
{
    public partial class Form1 : Form
    {
        public String baza = "DataBaseName";
        public String host = "PcName";
        String AutomaticFile = "";
        String myStringCon = "";
        public Form1()
        {
            InitializeComponent();
            myStringCon = "SERVER=" + host + ";" +
                 "DATABASE=" + baza + ";" +
                 "UID=Username; PASSWORD=userPassword" + ";pooling=true;Allow Zero Datetime=True;Min Pool Size=1; Max Pool Size=100; default command timeout=120";

        }
        public void Automatic()
        {
            using (MySqlConnection con = new MySqlConnection(myStringCon))
            {
                MySqlCommand command = new MySqlCommand("select_Cod_Firme_Soc;", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String a = reader["cf"].ToString();
                        if (a.StartsWith("RO")){
                            a = a.Remove(0, 2);
                            SendRequest(a);
                        }
                        else
                        {
                            SendRequest(a);
                        }
                    }
                }
            }
        }
        public void SendRequest(String id)
        {
            try
            {
                Thread.Sleep(2000);
                var request = (HttpWebRequest)WebRequest.Create("https://webservicesp.anaf.ro/PlatitorTvaRest/api/v3/ws/tva");
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {

                        cui = id,
                        data = DateTime.Now.ToString("yyyy-MM-dd")
                    });

                    streamWriter.Write("[" + json + "]");
                }
                string fileLines;
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    fileLines = streamReader.ReadToEnd();
                }
                //write anser
                //File.WriteAllText("C:\\Dep\\"+id+".txt", fileLines);
                //read json
                string jsona = fileLines;
                var data = (JObject)JsonConvert.DeserializeObject(jsona);
                string cod = data["cod"].Value<string>();
                string message = data["message"].Value<string>();
                string cui = "";
                string dataa = "";
                string denumire = "";
                string adresa = "";
                string scpTVA = "";
                string data_inceput_ScpTVA = "";
                string data_sfarsit_ScpTVA = "";
                string data_anul_imp_ScpTVA = "";
                string mesaj_ScpTVA = "";
                string dataInceputTvaInc = "";
                string dataSfarsitTvaInc = "";
                string dataActualizareTvaInc = "";
                string dataPublicareTvaInc = "";
                string tipActTvaInc = "";
                string statusTvaIncasare = "";
                string dataInactivare = "";
                string dataReactivare = "";
                string dataPublicare = "";
                string dataRadiere = "";
                string statusInactivi = "";
                string dataInceputSplitTVA = "";
                string dataAnulareSplitTVA = "";
                string statusSplitTVA = "";
                if (message == "SUCCESS")
                {
                    string[] words = jsona.Split('[');
                    string[] jsonArray = words[1].Split(']');
                    var dataInner = (JObject)JsonConvert.DeserializeObject(jsonArray[0]);
                    cui = dataInner["cui"].Value<string>();
                    dataa = dataInner["data"].Value<string>();
                    denumire = dataInner["denumire"].Value<string>();
                    adresa = dataInner["adresa"].Value<string>();
                    scpTVA = dataInner["scpTVA"].Value<string>();
                    data_inceput_ScpTVA = dataInner["data_inceput_ScpTVA"].Value<string>();
                    data_sfarsit_ScpTVA = dataInner["data_sfarsit_ScpTVA"].Value<string>();
                    data_anul_imp_ScpTVA = dataInner["data_anul_imp_ScpTVA"].Value<string>();
                    mesaj_ScpTVA = dataInner["mesaj_ScpTVA"].Value<string>();
                    dataInceputTvaInc = dataInner["dataInceputTvaInc"].Value<string>();
                    dataSfarsitTvaInc = dataInner["dataSfarsitTvaInc"].Value<string>();
                    dataActualizareTvaInc = dataInner["dataActualizareTvaInc"].Value<string>();
                    dataPublicareTvaInc = dataInner["dataPublicareTvaInc"].Value<string>();
                    tipActTvaInc = dataInner["tipActTvaInc"].Value<string>();
                    statusTvaIncasare = dataInner["statusTvaIncasare"].Value<string>();
                    dataInactivare = dataInner["dataInactivare"].Value<string>();
                    dataReactivare = dataInner["dataReactivare"].Value<string>();
                    dataPublicare = dataInner["dataPublicare"].Value<string>();
                    dataRadiere = dataInner["dataRadiere"].Value<string>();
                    statusInactivi = dataInner["statusInactivi"].Value<string>();
                    dataInceputSplitTVA = dataInner["dataInceputSplitTVA"].Value<string>();
                    dataAnulareSplitTVA = dataInner["dataAnulareSplitTVA"].Value<string>();
                    statusSplitTVA = dataInner["statusSplitTVA"].Value<string>();
                }
                











                using (MySqlConnection con = new MySqlConnection(myStringCon))
                {
                    MySqlCommand command = new MySqlCommand("Insert_cod_firma_anaf_filtrat;", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add your parameters here if you need them
                    command.Parameters.Add(new MySqlParameter("CNP_i", id));
                    command.Parameters.Add(new MySqlParameter("City_i", cod));
                    command.Parameters.Add(new MySqlParameter("message_i", message));
                    command.Parameters.Add(new MySqlParameter("cui_i", cui));
                    command.Parameters.Add(new MySqlParameter("data_i", dataa));
                    command.Parameters.Add(new MySqlParameter("denumire_i", denumire));
                    command.Parameters.Add(new MySqlParameter("adresa_i", adresa));
                    command.Parameters.Add(new MySqlParameter("scpTVA_i", scpTVA));
                    command.Parameters.Add(new MySqlParameter("data_inceput_ScpTVA_i", data_inceput_ScpTVA));
                    command.Parameters.Add(new MySqlParameter("data_sfarsit_ScpTVA_i", data_sfarsit_ScpTVA));
                    command.Parameters.Add(new MySqlParameter("data_anul_imp_ScpTVA_i", data_anul_imp_ScpTVA));
                    command.Parameters.Add(new MySqlParameter("mesaj_ScpTVA_i", mesaj_ScpTVA));
                    command.Parameters.Add(new MySqlParameter("dataInceputTvaInc_i", dataInceputTvaInc));
                    command.Parameters.Add(new MySqlParameter("dataSfarsitTvaInc_i", dataSfarsitTvaInc));
                    command.Parameters.Add(new MySqlParameter("dataActualizareTvaInc_i", dataActualizareTvaInc));
                    command.Parameters.Add(new MySqlParameter("dataPublicareTvaInc_i", dataPublicareTvaInc));
                    command.Parameters.Add(new MySqlParameter("tipActTvaInc_i", tipActTvaInc));
                    command.Parameters.Add(new MySqlParameter("statusTvaIncasare_i", statusTvaIncasare));
                    command.Parameters.Add(new MySqlParameter("dataInactivare_i", dataInactivare));
                    command.Parameters.Add(new MySqlParameter("dataReactivare_i", dataReactivare));
                    command.Parameters.Add(new MySqlParameter("dataPublicare_i", dataPublicare));
                    command.Parameters.Add(new MySqlParameter("dataRadiere_i", dataRadiere));
                    command.Parameters.Add(new MySqlParameter("statusInactivi_i", statusInactivi));
                    command.Parameters.Add(new MySqlParameter("dataInceputSplitTVA_i", dataInceputSplitTVA));
                    command.Parameters.Add(new MySqlParameter("dataAnulareSplitTVA_i", dataAnulareSplitTVA));
                    command.Parameters.Add(new MySqlParameter("statusSplitTVA_i", statusSplitTVA));
                        con.Open();
                    command.ExecuteNonQuery();
                    //int result = (int)command.ExecuteScalar();

                }
            }
            catch (Exception e)
            {
                using (MySqlConnection con = new MySqlConnection(myStringCon))
                {
                    MySqlCommand command = new MySqlCommand("Insert_cod_firma_anaf;", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add your parameters here if you need them
                    command.Parameters.Add(new MySqlParameter("CNP_i", id));
                    command.Parameters.Add(new MySqlParameter("City_i", e.ToString()));
                    con.Open();
                    command.ExecuteNonQuery();
                    //int result = (int)command.ExecuteScalar();

                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://webservicesp.anaf.ro/PlatitorTvaRest/api/v3/ws/tva");
            request.ContentType = "application/json";
            request.Method = "POST";
            
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {

                    cui = textBox1.Text.ToString(),
                    data = DateTime.Now.ToString("yyyy-MM-dd")
                });

                streamWriter.Write("["+json+"]");
            }
           string fileLines;
            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                fileLines = streamReader.ReadToEnd();
            }
            //write anser
            File.WriteAllText("C:\\Dep\\filename.txt", fileLines);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Automatic();
        }
    }
}
