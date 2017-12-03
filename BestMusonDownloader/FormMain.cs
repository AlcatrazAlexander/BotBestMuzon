using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace BestMusonDownloader
{
    public partial class FormMain : Form
    {
        private enum ActionKind
        {
            LoadMainPage,
            LoadDownloadPage,
            LoadBusy,
            StopLoad
        }

        private struct SongInfo
        {
            public string url;
            public string name;
            public string authorName;

            public string FullName => $"{authorName} - {name}";

            public string GetNameFromUrl()
            {
                int start = url.LastIndexOf('/') + 1;
                string name = url.Substring(start).Replace(".html", "");
                start = name.IndexOf('-') + 1;
                name = name.Substring(start) + ".mp3";

                return name;
            }

            public override string ToString()
            {
                return FullName;
            }
        }

        private string mainPage = "http://best-muzon.fm/rock_pesni/page/";
        private List<SongInfo> songs = new List<SongInfo>();
        private string directory = "Best-muzon rock";
        private ActionKind actionKind = ActionKind.LoadMainPage;
        private KnownColor[] knownColors;
        private int knownColorIndex = 0;

        private Color GetNextKnownColor()
        {
            if (knownColorIndex >= knownColors.Length)
                knownColorIndex = 0;
            
            Color nextColor = Color.FromKnownColor(knownColors[knownColorIndex]);
            knownColorIndex++;

            Console.WriteLine("Current known color: " + nextColor.ToString());
            Console.WriteLine("Next known color: " + Color.FromKnownColor(knownColors[knownColorIndex % knownColors.Length]).ToString());
            
            return nextColor;
        }

        public FormMain()
        {
            InitializeComponent();

            Directory.CreateDirectory(directory);

            webBrowser.ScriptErrorsSuppressed = true;
            prBarSong.UsePercentage = true;
            prBarSong.UseText = true;
            listBoxSongs.ScrollAlwaysVisible = true;

            int colorCount = 174 - (7 + 27);
            knownColors = new KnownColor[colorCount];
            Array array = Enum.GetValues(typeof(KnownColor));
            Array.Copy(array, 27, knownColors, 0, colorCount);
        }

        private void SetMainPage(int number)
        {
            nextMainPage = number;
            numericUpDownMainPage.Value = number;
            currMainPage = mainPage + number.ToString() + "/";
        }

        private void GoToMainPage() => webBrowser.Navigate(currMainPage);

        private void GoToDownloadPage(int index)
        {
            currDownloadPage = songs[index].url;
            webBrowser.Navigate(currDownloadPage);
        }

        private Task DownloadFile(string link, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                prBarSong.Text = "";
                oldBytesReceived = 0;
                oldDateTimeBytesReceived = DateTime.Now;
                client.DownloadProgressChanged += Client_DownloadProgressChanged;

                Task task = client.DownloadFileTaskAsync(link, fileName);

                return task;
            }
        }

        private long oldBytesReceived;
        private DateTime oldDateTimeBytesReceived;

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DateTime now = DateTime.Now;
            TimeSpan tsCheck = now - oldDateTimeBytesReceived;

            if (tsCheck.TotalSeconds >= 0.5)
            {
                long progDiff = e.BytesReceived - oldBytesReceived;

                double speed = progDiff / tsCheck.TotalSeconds;
                double speedKByte = speed / 1_000;
                double speedMByte = speed / 1_000_000;
                string speedStr = "";

                if (double.IsInfinity(speed) || double.IsNaN(speed) || speed < 1)
                    speedStr = "(- байт/сек)";
                else if (speedMByte > 1)
                    speedStr = $"({speedMByte.ToString("F2")} Мбайт/сек)";
                else if (speedKByte > 1)
                    speedStr = $"({speedKByte.ToString("F0")} Кбайт/сек)";
                else
                    speedStr = $"({speed.ToString("F0")} байт/сек)";

                prBarSong.Text = speedStr;

                oldBytesReceived = e.BytesReceived;
                oldDateTimeBytesReceived = now;
            }

            prBarSong.Value = e.ProgressPercentage;
        }

        private void SetText(string text)
        {
            //if (InvokeRequired)
            //{
            //    BeginInvoke((Action<string>)SetText, text);
            //    return;
            //}

            textBoxInfo.AppendText(text + Environment.NewLine);
            textBoxInfo.ScrollToCaret();
        }

        private string GetDownloadLink()
        {
            //string link = null;

            return webBrowser.Document.Links.OfType<HtmlElement>().
                Select(he => he.GetAttribute("href")).
                Where(he => he.Contains("http://best-muzon.fm/dl/")).
                FirstOrDefault();

            //HtmlElement elem = webBrowser1.Document.GetElementsByTagName("h2")[1];
            //link = elem.GetAttribute("href");

            //int index = elem.InnerHtml.IndexOf(".mp3") + 4;

            //link = elem.InnerHtml.Substring(9, index - 9);

            //Console.WriteLine();
            //foreach (HtmlElement item in webBrowser1.Document.Links)
            //{
            //    string href = item.GetAttribute("href");
            //    if (href.Contains("http://best-muzon.fm/dl/"))
            //    {
            //        return href;
            //    }
            //}

            //return link;
        }
        
        private void FillDownloadUrls()
        {
            //Links download_button
            var collection = webBrowser.Document.Links;
            songs.Clear();

            foreach (HtmlElement item in collection.
                OfType<HtmlElement>().
                Where(he =>
                {
                    string outerHtml = he.OuterHtml;

                    return 
                        he.InnerHtml != "Скачать" &&
                        outerHtml != null &&
                        outerHtml.Contains("rock_pesni") &&
                        outerHtml.Contains(".html");
                }))
            {
                SongInfo song = new SongInfo();
                song.url = item.GetAttribute("href");
                song.name = item.InnerText;

                songs.Add(song);
            }

            int songIndex = 0;

            foreach (HtmlElement item in collection.
                OfType<HtmlElement>().
                Where(he => 
                {
                    string outerHtml = he.OuterHtml;
                    return 
                        outerHtml != null && 
                        outerHtml.Contains("/singer/") &&
                        he.InnerText.Length > 1;
                }).
                Take(songs.Count))
            {
                SongInfo song = songs[songIndex];
                song.authorName = item.InnerText;
                songs[songIndex++] = song;
            }
        }
        
        private string currMainPage;
        private string currDownloadPage;
        private int nextSong = -1;
        private int nextMainPage = -1;
        private DateTime startLoadPageTime;

        private async void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (actionKind == ActionKind.LoadBusy) return;
            else if (actionKind == ActionKind.StopLoad)
            {
                webBrowser.DocumentCompleted -= WebBrowser_DocumentCompleted;
                SetControlsEnabled(true);
                return;
            }

            switch (actionKind)
            {
                case ActionKind.LoadMainPage:
                    if (currMainPage.Contains(e.Url.AbsoluteUri))
                    {
                        if (nextSong == -1)
                        {
                            startLoadPageTime = DateTime.Now;

                            FillDownloadUrls();

                            if (songs.Count == 0)
                            {
                                actionKind = ActionKind.LoadBusy;
                                webBrowser.DocumentCompleted -= WebBrowser_DocumentCompleted;
                                SetControlsEnabled(true);
                                break;
                            }

                            if (textBoxInfo.TextLength > 0)
                                textBoxInfo.AppendText("\r\n\r\n");

                            prBarSong.Value = 0;
                            prBarAllSongs.Value = 0;
                            prBarAllSongs.Maximum = songs.Count;

                            listBoxSongs.Items.Clear();
                            listBoxSongs.Items.AddRange(songs.Select(s => s.FullName).ToArray());

                            actionKind = ActionKind.LoadDownloadPage;

                            listBoxSongs.SelectedIndex = 0;
                            nextSong = 0;
                            GoToDownloadPage(nextSong);
                        }
                    }
                    break;
                case ActionKind.LoadDownloadPage:
                    if (currDownloadPage.Contains(e.Url.AbsoluteUri))
                    {
                        actionKind = ActionKind.LoadBusy;
                        SongInfo song = songs[nextSong];

                        string songName = song.FullName;

                        char[] invalid = Path.GetInvalidFileNameChars();

                        var check = songName.Where(c => invalid.Contains(c)).ToList();

                        if (check.Count > 0)
                        {
                            for (int i = 0; i < check.Count; i++)
                            {
                                char c = check[i];

                                songName = songName.Replace(c, '_');
                            }
                        }

                        string name = Path.Combine(directory, songName + ".mp3");

                        if (File.Exists(name))
                        {
                            string result = $"[{nextSong + 1}] [{DateTime.Now}] [{song.FullName}] [Existed]\r\n";

                            SetText(result);
                            Console.WriteLine(result);
                        }
                        else
                        {
                            string status = null;
                            string link = GetDownloadLink();

                            if (link == null)
                            {
                                nextSong--;
                            }
                            else
                            {
                                try
                                {
                                    DateTime startSongLoadTime = DateTime.Now;
                                    Task task = DownloadFile(link, name);
                                    await task; // main page - 12

                                    if (task.IsFaulted) status = "Faulted";
                                    else if (task.IsCanceled) status = "Canceled";
                                    else status = "Completed";

                                    TimeSpan songLoadTime = DateTime.Now - startSongLoadTime;
                                    string info =
                                        $"[{nextSong + 1}] [{DateTime.Now}] [{song.FullName}] [{status}]\r\n" +
                                        $"Load song time: {songLoadTime.ToString()}\r\n";

                                    SetText(info);
                                    Console.WriteLine(info);
                                }
                                catch (Exception exc)
                                {
                                    SetText(exc.Message + Environment.NewLine);
                                    nextSong--;

                                    if (status == null || status != "Completed")
                                    {
                                        if (File.Exists(name))
                                        {
                                            try
                                            {
                                                File.Delete(name);
                                            }
                                            catch (Exception exc2)
                                            {
                                                SetText(exc2.Message + Environment.NewLine);
                                            }
                                        }
                                    }
                                }
                            }

                            if (status == null || status != "Completed")
                            {
                                int maxLoop = 200;
                                int loop = 0;

                                while (!ConnectionAvailable("http://www.google.com/"))
                                {
                                    await Task.Delay(1000);

                                    loop++;
                                    if (loop > maxLoop) actionKind = ActionKind.StopLoad;
                                    if (actionKind == ActionKind.StopLoad) break;
                                }
                            }
                        }

                        nextSong++;

                        prBarSong.Value = 0;
                        prBarAllSongs.Value = nextSong;

                        if (nextSong >= songs.Count)
                        {
                            TimeSpan loadPageTime = DateTime.Now - startLoadPageTime;
                            SetText($"Load page time: {loadPageTime.ToString()}");

                            nextSong = -1;
                            actionKind = ActionKind.LoadMainPage;

                            SetMainPage(++nextMainPage);
                            GoToMainPage();
                            break;
                        }

                        prBarSong.ForeColor = GetNextKnownColor();
                        listBoxSongs.SelectedIndex = nextSong;
                        int topIndex = nextSong - 2;
                        if (topIndex < 0) topIndex = 0;
                        listBoxSongs.TopIndex = topIndex;

                        if (actionKind != ActionKind.StopLoad)
                        {
                            actionKind = ActionKind.LoadDownloadPage;
                            GoToDownloadPage(nextSong);
                        }
                        else
                        {
                            webBrowser.DocumentCompleted -= WebBrowser_DocumentCompleted;
                            SetControlsEnabled(true);
                        }
                    }
                    break;
            }
            //Console.WriteLine(e.Url);
        }

        private void ButtonStartBot_Click(object sender, EventArgs e)
        {
            SetControlsEnabled(false);
            prBarSong.Value = 0;
            prBarSong.Maximum = 100;
            actionKind = ActionKind.LoadMainPage;

            nextSong = -1;
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
            GoToMainPage();
        }

        private void ButtonSetMainPage_Click(object sender, EventArgs e)
        {
            SetMainPage((int)numericUpDownMainPage.Value);
            GoToMainPage();
        }

        private void SetControlsEnabled(bool enabled)
        {
            buttonBotStart.Enabled = enabled;
            buttonSetMainPage.Enabled = enabled;
            numericUpDownMainPage.Enabled = enabled;
            buttonStop.Enabled = !enabled;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            actionKind = ActionKind.StopLoad;
            //buttonStop.Enabled = false;
        }

        public bool ConnectionAvailable(string strServer)
        {
            try
            {
                HttpWebRequest reqFP = (HttpWebRequest)WebRequest.Create(strServer);

                HttpWebResponse rspFP = (HttpWebResponse)reqFP.GetResponse();
                if (HttpStatusCode.OK == rspFP.StatusCode)
                {
                    // HTTP = 200 - Интернет безусловно есть! 
                    rspFP.Close();
                    return true;
                }
                else
                {
                    // сервер вернул отрицательный ответ, возможно что инета нет
                    rspFP.Close();
                    return false;
                }
            }
            catch (WebException)
            {
                // Ошибка, значит интернета у нас нет. Плачем :'(
                return false;
            }
        }

    }
}
