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

        public FormMain()
        {
            InitializeComponent();
            webBrowser.ScriptErrorsSuppressed = true;

            Directory.CreateDirectory(directory);
            prBarSong.UsePercentage = true;
            listBoxSongs.ScrollAlwaysVisible = true;
        }

        private void SetMainPage(int number)
        {
            nextMainPage = number;
            numericUpDown1.Value = number;
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
                client.DownloadProgressChanged += Client_DownloadProgressChanged;

                Task task = client.DownloadFileTaskAsync(link, fileName);

                return task;
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
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

                        string name = Path.Combine(directory, song.FullName + ".mp3");

                        if (File.Exists(name))
                        {
                            string result = $"[{DateTime.Now}] [{song.FullName}] [Existed]\r\n";

                            SetText(result);
                            Console.WriteLine(result);
                        }
                        else
                        {
                            DateTime startSongLoadTime = DateTime.Now;
                            string link = GetDownloadLink();

                            Task task = DownloadFile(link, name);
                            await task;

                            string status;
                            if (task.IsFaulted) status = "Faulted";
                            else if (task.IsCanceled) status = "Canceled";
                            else status = "Completed";

                            TimeSpan songLoadTime = DateTime.Now - startSongLoadTime;
                            string info =
                                $"[{DateTime.Now}] [{song.FullName}] [{status}]\r\n" +
                                $"Load song time: {songLoadTime.ToString()}\r\n";

                            SetText(info);
                            Console.WriteLine(info);
                        }

                        prBarSong.Value = 0;
                        prBarAllSongs.Value++;

                        nextSong++;

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

                        listBoxSongs.SelectedIndex = nextSong;
                        int topIndex = nextSong - 2;
                        if (topIndex < 0) topIndex = 0;
                        listBoxSongs.TopIndex = topIndex;

                        actionKind = ActionKind.LoadDownloadPage;
                        GoToDownloadPage(nextSong);
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
            SetMainPage((int)numericUpDown1.Value);
            GoToMainPage();
        }

        private void SetControlsEnabled(bool enabled)
        {
            buttonBotStart.Enabled = enabled;
            buttonSetMainPage.Enabled = enabled;
            numericUpDown1.Enabled = enabled;
            buttonStop.Enabled = !enabled;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            actionKind = ActionKind.StopLoad;
            buttonStop.Enabled = false;
        }
    }
}
