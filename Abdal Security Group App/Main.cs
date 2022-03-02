using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocksSharp;
using SocksSharp.Proxy;

namespace Abdal_Security_Group_App
{
    public partial class Main : Telerik.WinControls.UI.RadForm
    {
        private bool proxy_file_selected = false;

        private string proxy_file_path = "";
        private string[] proxy_file_line = new string[] { };
        private Int32 total_proxy_check = 0;
        private bool stop_proxy_check = false;

        // Abdal Sound Player
        private AbdalSoundPlayer ab_player = new AbdalSoundPlayer();
        private string abdal_app_name = Assembly.GetExecutingAssembly().GetName().ToString().Split(',')[0];

        private string abdal_app_name_for_url = Assembly.GetExecutingAssembly().GetName().ToString().Split(',')[0]
            .ToLower().Replace(' ', '-');


        public Main()
        {
            InitializeComponent();

            //change form title
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = abdal_app_name + " " + version.Major + "." + version.Minor;

            // Call Global Chilkat Unlock
            Abdal_Security_Group_App.GlobalUnlockChilkat GlobalUnlock =
                new Abdal_Security_Group_App.GlobalUnlockChilkat();
            GlobalUnlock.unlock();
        }


        #region Dragable Form Start

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr) (HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        #endregion

        private void EncryptToggleSwitch_ValueChanged(object sender, EventArgs e)
        {
        }

        private void DecryptToggleSwitch_ValueChanged(object sender, EventArgs e)
        {
        }


        private void radLabelElement4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://abdalagency.ir/");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            stop_proxy_check = false;
            listBox_bad_proxy.Items.Clear();
            listBox_good_proxy.Items.Clear();

            if (radDropDownList1.Text == "SOCKS5")
            {
                if (backgroundWorker_Socks5.IsBusy != true)
                {
                    ab_player.sPlayer("start");
                    backgroundWorker_Socks5.RunWorkerAsync();
                }
            }
            else if (radDropDownList1.Text == "SOCKS4")
            {
                if (backgroundWorker_Socks4.IsBusy != true)
                {
                    ab_player.sPlayer("start");
                    backgroundWorker_Socks4.RunWorkerAsync();
                }
            }
            else if (radDropDownList1.Text == "SOCKS4a")
            {
                if (backgroundWorker_Socks4a.IsBusy != true)
                {
                    ab_player.sPlayer("start");
                    backgroundWorker_Socks4a.RunWorkerAsync();
                }
            }
            else if (radDropDownList1.Text == "HTTP")
            {
                if (backgroundWorker_http.IsBusy != true)
                {
                    ab_player.sPlayer("start");
                    backgroundWorker_http.RunWorkerAsync();
                }
            }
            else
            {
                ab_player.sPlayer("error");

                string MessageBoxTitle = "Abdal Proxy Checker";
                string MessageBoxContent = "Please firset select the proxy type";

                DialogResult dialogResult = MessageBox.Show(MessageBoxContent, MessageBoxTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            openFileDialog_proxy_file.Reset();
            openFileDialog_proxy_file.FileName = "";
            if (e.Cancelled == true)
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = "Canceled Process By User!";
                this.desk_alert.Show();
                ab_player.sPlayer("cancel");
            }
            else if (e.Error != null)
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = e.Error.Message;
                this.desk_alert.Show();


                ab_player.sPlayer("error");
            }
            else
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = "Done!";
                this.desk_alert.Show();

                ab_player.sPlayer("done");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void radMenu1_Click(object sender, EventArgs e)
        {
        }

        private void radMenuItem1_Click_1(object sender, EventArgs e)
        {
            Abdal_Security_Group_App.about about_form = new Abdal_Security_Group_App.about();
            about_form.Show();
            about_form.TopMost = true;
        }

        private void radMenuItem5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://gitlab.com/abdal-security-group/" + abdal_app_name_for_url);
        }

        private void radMenuItem4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/abdal-security-group/" + abdal_app_name_for_url);
        }

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://donate.abdalagency.ir/");
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog_proxy_file.AddExtension = false;
                openFileDialog_proxy_file.Title = "Abdal Proxy Checker- Get Proxy File";
                openFileDialog_proxy_file.DefaultExt = "txt";
                openFileDialog_proxy_file.Filter = "txt File |*.txt";
                openFileDialog_proxy_file.FileName = "";
                openFileDialog_proxy_file.CheckFileExists = true;
                openFileDialog_proxy_file.CheckPathExists = true;
                DialogResult result = openFileDialog_proxy_file.ShowDialog();

                if (result == DialogResult.OK)
                {
                    proxy_file_selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region proxyCheckerSocks5

        private static async Task<bool> proxyCheckerSocks5(string ip_addr, Int32 port_addr)
        {
            try
            {
                var settings = new ProxySettings();
                settings.Host = ip_addr;
                settings.Port = port_addr;
                var proxyClientHandler = new ProxyClientHandler<Socks5>(settings);

                var httpClient = new HttpClient(proxyClientHandler);

                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:90.0) Gecko/20100101 Firefox/90.0");
                var response = await httpClient.GetAsync("https://google.com");
                // MessageBox.Show(response.StatusCode.ToString());


                return true;
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());

                return false;
            }
        }

        #endregion


        #region proxyCheckerHTTP

        private bool proxyCheckerHttp(string ip_addr, Int32 port_addr)
        {
            try
            {
                bool success = false;
                Chilkat.Socket socket = new Chilkat.Socket();

                string url = "https://google.com";
                socket.HttpProxyHostname = ip_addr;
                socket.HttpProxyPort = port_addr;
                string hostname = url;
                bool ssl = true;
                int maxWaitMillisec = 3000;
                success = socket.Connect(hostname, 443, ssl, maxWaitMillisec);
                socket.Close(1000);
                return success;
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());

                return false;
            }
        }

        #endregion

        #region proxyCheckerHTTP_ex

        private bool proxyCheckerHttp_ex(string ip_addr, Int32 port_addr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://google.com");
                request.Timeout = 1000;
                WebProxy myproxy = new WebProxy(ip_addr + ":" + port_addr.ToString(), false);
                request.Proxy = myproxy;
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());

                return false;
            }
        }

        #endregion


        #region proxyCheckerSocks4

        private static async Task<bool> proxyCheckerSocks4(string ip_addr, Int32 port_addr)
        {
            try
            {
                var settings = new ProxySettings();
                settings.Host = ip_addr;
                settings.Port = port_addr;
                var proxyClientHandler = new ProxyClientHandler<Socks4>(settings);

                var httpClient = new HttpClient(proxyClientHandler);

                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:90.0) Gecko/20100101 Firefox/90.0");
                var response = await httpClient.GetAsync("https://google.com");
                // MessageBox.Show(response.StatusCode.ToString());


                return true;
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());

                return false;
            }
        }

        #endregion


        #region proxyCheckerSocks4a

        private static async Task<bool> proxyCheckerSocks4a(string ip_addr, Int32 port_addr)
        {
            try
            {
                var settings = new ProxySettings();
                settings.Host = ip_addr;
                settings.Port = port_addr;
                var proxyClientHandler = new ProxyClientHandler<Socks4a>(settings);

                var httpClient = new HttpClient(proxyClientHandler);

                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:90.0) Gecko/20100101 Firefox/90.0");
                var response = await httpClient.GetAsync("https://google.com");
                // MessageBox.Show(response.StatusCode.ToString());


                return true;
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());

                return false;
            }
        }

        #endregion


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Timer Start

            var timer_stop_watch = new Stopwatch();
            radLabel_Time_Cracking_Taken.Text = "Time Taken : Work in progress";

            timer_stop_watch.Start();

            #endregion


            int counter_for_progress = 0;
            this.proxy_file_path = openFileDialog_proxy_file.FileName;

            try
            {
                List<Task> tasks_proxy_reader = new List<Task>();

                tasks_proxy_reader.Add(Task.Factory.StartNew(() =>
                {
                    #region Stream Proxy Reader

                    var proxy_file_lines_counter = File.ReadAllLines(proxy_file_path).Count();
                    radLabel_total_proxy.Text = "Total Proxy : " + proxy_file_lines_counter.ToString();


                    using (StreamReader stream_proxy = new StreamReader(proxy_file_path))
                    {
                        radProgressBar1.Minimum = 0;
                        radProgressBar1.Maximum = proxy_file_lines_counter;

                        String proxy_single_line;
                        while ((proxy_single_line = stream_proxy.ReadLine()) != null)
                        {
                            if (stop_proxy_check)
                            {
                                break;
                            }

                            counter_for_progress += 1;
                            radProgressBar1.Value2 = counter_for_progress;


                            String[] proxy_explode = proxy_single_line.Split(":".ToArray());
                            var result = proxyCheckerSocks5(proxy_explode[0], Int32.Parse(proxy_explode[1]));

                            total_proxy_check = Convert.ToInt32(listBox_bad_proxy.Items.Count) +
                                                Convert.ToInt32(listBox_good_proxy.Items.Count);
                            radLabel_total_proxy_check.Text =
                                "Total Proxy Check : " + total_proxy_check.ToString();

                            if (result.Result == true)
                            {
                                ab_player.sPlayer("find");

                                #region Add to Good Proxy

                                listBox_good_proxy.Items.Add(proxy_explode[0] + ":" + Int32.Parse(proxy_explode[1]));
                                int visibleItems = listBox_good_proxy.ClientSize.Height /
                                                   listBox_good_proxy.ItemHeight;
                                listBox_good_proxy.TopIndex =
                                    Math.Max(listBox_good_proxy.Items.Count - visibleItems + 1, 0);

                                #endregion
                            }
                            else
                            {
                                #region Add to Bad Proxy

                                listBox_bad_proxy.Items.Add(proxy_explode[0] + ":" + Int32.Parse(proxy_explode[1]));
                                int visibleItems = listBox_bad_proxy.ClientSize.Height /
                                                   listBox_bad_proxy.ItemHeight;
                                listBox_bad_proxy.TopIndex =
                                    Math.Max(listBox_bad_proxy.Items.Count - visibleItems + 1, 0);

                                #endregion
                            }
                        }
                    }

                    #endregion
                }));

                Task.WaitAll(tasks_proxy_reader.ToArray());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                throw;
            }

            #region Stop Timer

            timer_stop_watch.Stop();
            TimeSpan time_cracking_taken = timer_stop_watch.Elapsed;

            radLabel_Time_Cracking_Taken.Text = "Time Taken : " + time_cracking_taken.ToString(@"m\:ss\.fff");

            #endregion
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.FileName = "Good proxy.txt";
            string[] list_all_good_proxy = listBox_good_proxy.Items.OfType<string>().ToArray();

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, string.Join("\n", list_all_good_proxy));
            }
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            stop_proxy_check = true;
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.FileName = "Bad proxy.txt";
            string[] list_all_bad_proxy = listBox_bad_proxy.Items.OfType<string>().ToArray();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, string.Join("\n", list_all_bad_proxy));
            }
        }

        private void backgroundWorker_Socks4_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Timer Start

            var timer_stop_watch = new Stopwatch();
            radLabel_Time_Cracking_Taken.Text = "Time Taken : Work in progress";

            timer_stop_watch.Start();

            #endregion


            int counter_for_progress = 0;
            this.proxy_file_path = openFileDialog_proxy_file.FileName;

            try
            {
                List<Task> tasks_proxy_reader = new List<Task>();

                tasks_proxy_reader.Add(Task.Factory.StartNew(() =>
                {
                    #region Stream Proxy Reader

                    var proxy_file_lines_counter = File.ReadAllLines(proxy_file_path).Count();
                    radLabel_total_proxy.Text = "Total Proxy : " + proxy_file_lines_counter.ToString();


                    using (StreamReader stream_proxy = new StreamReader(proxy_file_path))
                    {
                        radProgressBar1.Minimum = 0;
                        radProgressBar1.Maximum = proxy_file_lines_counter;

                        String proxy_single_line;
                        while ((proxy_single_line = stream_proxy.ReadLine()) != null)
                        {
                            if (stop_proxy_check)
                            {
                                break;
                            }

                            counter_for_progress += 1;
                            radProgressBar1.Value2 = counter_for_progress;


                            String[] proxy_explode = proxy_single_line.Split(":".ToArray());
                            var result = proxyCheckerSocks4(proxy_explode[0], Int32.Parse(proxy_explode[1]));

                            total_proxy_check = Convert.ToInt32(listBox_bad_proxy.Items.Count) +
                                                Convert.ToInt32(listBox_good_proxy.Items.Count);
                            radLabel_total_proxy_check.Text =
                                "Total Proxy Check : " + total_proxy_check.ToString();

                            if (result.Result == true)
                            {
                                ab_player.sPlayer("find");

                                #region Add to Good Proxy

                                listBox_good_proxy.Items.Add(proxy_explode[0] + ":" + Int32.Parse(proxy_explode[1]));
                                int visibleItems = listBox_good_proxy.ClientSize.Height /
                                                   listBox_good_proxy.ItemHeight;
                                listBox_good_proxy.TopIndex =
                                    Math.Max(listBox_good_proxy.Items.Count - visibleItems + 1, 0);

                                #endregion
                            }
                            else
                            {
                                #region Add to Bad Proxy

                                listBox_bad_proxy.Items.Add(proxy_explode[0] + ":" + Int32.Parse(proxy_explode[1]));
                                int visibleItems = listBox_bad_proxy.ClientSize.Height /
                                                   listBox_bad_proxy.ItemHeight;
                                listBox_bad_proxy.TopIndex =
                                    Math.Max(listBox_bad_proxy.Items.Count - visibleItems + 1, 0);

                                #endregion
                            }
                        }
                    }

                    #endregion
                }));

                Task.WaitAll(tasks_proxy_reader.ToArray());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                throw;
            }

            #region Stop Timer

            timer_stop_watch.Stop();
            TimeSpan time_cracking_taken = timer_stop_watch.Elapsed;

            radLabel_Time_Cracking_Taken.Text = "Time Taken : " + time_cracking_taken.ToString(@"m\:ss\.fff");

            #endregion
        }

        private void backgroundWorker_Socks4a_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Timer Start

            var timer_stop_watch = new Stopwatch();
            radLabel_Time_Cracking_Taken.Text = "Time Taken : Work in progress";

            timer_stop_watch.Start();

            #endregion


            int counter_for_progress = 0;
            this.proxy_file_path = openFileDialog_proxy_file.FileName;

            try
            {
                List<Task> tasks_proxy_reader = new List<Task>();

                tasks_proxy_reader.Add(Task.Factory.StartNew(() =>
                {
                    #region Stream Proxy Reader

                    var proxy_file_lines_counter = File.ReadAllLines(proxy_file_path).Count();
                    radLabel_total_proxy.Text = "Total Proxy : " + proxy_file_lines_counter.ToString();


                    using (StreamReader stream_proxy = new StreamReader(proxy_file_path))
                    {
                        radProgressBar1.Minimum = 0;
                        radProgressBar1.Maximum = proxy_file_lines_counter;

                        String proxy_single_line;
                        while ((proxy_single_line = stream_proxy.ReadLine()) != null)
                        {
                            if (stop_proxy_check)
                            {
                                break;
                            }

                            counter_for_progress += 1;
                            radProgressBar1.Value2 = counter_for_progress;


                            String[] proxy_explode = proxy_single_line.Split(":".ToArray());
                            var result = proxyCheckerSocks4a(proxy_explode[0], Int32.Parse(proxy_explode[1]));

                            total_proxy_check = Convert.ToInt32(listBox_bad_proxy.Items.Count) +
                                                Convert.ToInt32(listBox_good_proxy.Items.Count);
                            radLabel_total_proxy_check.Text =
                                "Total Proxy Check : " + total_proxy_check.ToString();

                            if (result.Result == true)
                            {
                                ab_player.sPlayer("find");

                                #region Add to Good Proxy

                                listBox_good_proxy.Items.Add(proxy_explode[0] + ":" + Int32.Parse(proxy_explode[1]));
                                int visibleItems = listBox_good_proxy.ClientSize.Height /
                                                   listBox_good_proxy.ItemHeight;
                                listBox_good_proxy.TopIndex =
                                    Math.Max(listBox_good_proxy.Items.Count - visibleItems + 1, 0);

                                #endregion
                            }
                            else
                            {
                                #region Add to Bad Proxy

                                listBox_bad_proxy.Items.Add(proxy_explode[0] + ":" + Int32.Parse(proxy_explode[1]));
                                int visibleItems = listBox_bad_proxy.ClientSize.Height /
                                                   listBox_bad_proxy.ItemHeight;
                                listBox_bad_proxy.TopIndex =
                                    Math.Max(listBox_bad_proxy.Items.Count - visibleItems + 1, 0);

                                #endregion
                            }
                        }
                    }

                    #endregion
                }));

                Task.WaitAll(tasks_proxy_reader.ToArray());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                throw;
            }

            #region Stop Timer

            timer_stop_watch.Stop();
            TimeSpan time_cracking_taken = timer_stop_watch.Elapsed;

            radLabel_Time_Cracking_Taken.Text = "Time Taken : " + time_cracking_taken.ToString(@"m\:ss\.fff");

            #endregion
        }

        private void backgroundWorker_Socks4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            openFileDialog_proxy_file.Reset();
            openFileDialog_proxy_file.FileName = "";
            if (e.Cancelled == true)
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = "Canceled Process By User!";
                this.desk_alert.Show();
                ab_player.sPlayer("cancel");
            }
            else if (e.Error != null)
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = e.Error.Message;
                this.desk_alert.Show();


                ab_player.sPlayer("error");
            }
            else
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = "Done!";
                this.desk_alert.Show();

                ab_player.sPlayer("done");
            }
        }

        private void backgroundWorker_Socks4a_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            openFileDialog_proxy_file.Reset();
            openFileDialog_proxy_file.FileName = "";
            if (e.Cancelled == true)
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = "Canceled Process By User!";
                this.desk_alert.Show();
                ab_player.sPlayer("cancel");
            }
            else if (e.Error != null)
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = e.Error.Message;
                this.desk_alert.Show();


                ab_player.sPlayer("error");
            }
            else
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = "Done!";
                this.desk_alert.Show();

                ab_player.sPlayer("done");
            }
        }

        private void backgroundWorker_http_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Timer Start

            var timer_stop_watch = new Stopwatch();
            radLabel_Time_Cracking_Taken.Text = "Time Taken : Work in progress";

            timer_stop_watch.Start();

            #endregion


            int counter_for_progress = 0;
            this.proxy_file_path = openFileDialog_proxy_file.FileName;

            try
            {
                List<Task> tasks_proxy_reader = new List<Task>();

                tasks_proxy_reader.Add(Task.Factory.StartNew(() =>
                {
                    #region Stream Proxy Reader

                    var proxy_file_lines_counter = File.ReadAllLines(proxy_file_path).Count();
                    radLabel_total_proxy.Text = "Total Proxy : " + proxy_file_lines_counter.ToString();


                    using (StreamReader stream_proxy = new StreamReader(proxy_file_path))
                    {
                        radProgressBar1.Minimum = 0;
                        radProgressBar1.Maximum = proxy_file_lines_counter;

                        String proxy_single_line;
                        while ((proxy_single_line = stream_proxy.ReadLine()) != null)
                        {
                            if (stop_proxy_check)
                            {
                                break;
                            }

                            counter_for_progress += 1;
                            radProgressBar1.Value2 = counter_for_progress;


                            String[] proxy_explode = proxy_single_line.Split(":".ToArray());
                            var result = proxyCheckerHttp(proxy_explode[0], Int32.Parse(proxy_explode[1]));

                            total_proxy_check = Convert.ToInt32(listBox_bad_proxy.Items.Count) +
                                                Convert.ToInt32(listBox_good_proxy.Items.Count);
                            radLabel_total_proxy_check.Text =
                                "Total Proxy Check : " + total_proxy_check.ToString();

                            if (result == true)
                            {
                                ab_player.sPlayer("find");

                                #region Add to Good Proxy

                                listBox_good_proxy.Items.Add(proxy_explode[0] + ":" + Int32.Parse(proxy_explode[1]));
                                int visibleItems = listBox_good_proxy.ClientSize.Height /
                                                   listBox_good_proxy.ItemHeight;
                                listBox_good_proxy.TopIndex =
                                    Math.Max(listBox_good_proxy.Items.Count - visibleItems + 1, 0);

                                #endregion
                            }
                            else
                            {
                                #region Add to Bad Proxy

                                listBox_bad_proxy.Items.Add(proxy_explode[0] + ":" + Int32.Parse(proxy_explode[1]));
                                int visibleItems = listBox_bad_proxy.ClientSize.Height /
                                                   listBox_bad_proxy.ItemHeight;
                                listBox_bad_proxy.TopIndex =
                                    Math.Max(listBox_bad_proxy.Items.Count - visibleItems + 1, 0);

                                #endregion
                            }
                        }
                    }

                    #endregion
                }));

                Task.WaitAll(tasks_proxy_reader.ToArray());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                throw;
            }

            #region Stop Timer

            timer_stop_watch.Stop();
            TimeSpan time_cracking_taken = timer_stop_watch.Elapsed;

            radLabel_Time_Cracking_Taken.Text = "Time Taken : " + time_cracking_taken.ToString(@"m\:ss\.fff");

            #endregion
        }

        private void backgroundWorker_http_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            openFileDialog_proxy_file.Reset();
            openFileDialog_proxy_file.FileName = "";
            if (e.Cancelled == true)
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = "Canceled Process By User!";
                this.desk_alert.Show();
                ab_player.sPlayer("cancel");
            }
            else if (e.Error != null)
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = e.Error.Message;
                this.desk_alert.Show();


                ab_player.sPlayer("error");
            }
            else
            {
                this.desk_alert.CaptionText = abdal_app_name;
                this.desk_alert.ContentText = "Done!";
                this.desk_alert.Show();

                ab_player.sPlayer("done");
            }
        }
    }
}