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
using System.Diagnostics;
using System.IO.Compression;

namespace AutoMod
{

    public partial class Form3 : Form
    {

        static readonly string SavePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        // public string zipPath = Path.Combine(SavePath, ".minecraft/mods/mods.zip");
        // public string extractPath = Path.Combine(SavePath, ".minecraft/mods/");
        public bool overwriteFiles = true;

        public Form3()
        {
            InitializeComponent();
            label1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.ProgressPercentage + "% Completed";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Text = "Downloading...";
            button1.Visible = false;
            label1.Visible = true;
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFileCompleted += DownloadCompleted;
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new System.Uri("https://github.com/NinjaCheetah/automod-mods/releases/latest/download/mods.zip"),
                    // Param2 = Path to save
                    Path.Combine(SavePath, ".minecraft/mods/mods.zip")
                );
            }
            
        }
        public void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string zipPath = Path.Combine(SavePath, ".minecraft/mods/mods.zip");
            string extractPath = Path.Combine(SavePath, ".minecraft/mods/");

            // Normalizes the path.
            extractPath = Path.GetFullPath(extractPath);

            // Ensures that the last character on the extraction path
            // is the directory separator char.
            // Without this, a malicious zip file could try to traverse outside of the expected
            // extraction path.
            if (!extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                extractPath += Path.DirectorySeparatorChar;

            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                    {
                        // Gets the full path to ensure that relative segments are removed.
                        string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

                        // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                        // are case-insensitive.
                        if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                            entry.ExtractToFile(destinationPath, overwriteFiles);
                    }
                    else if (entry.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        // Gets the full path to ensure that relative segments are removed.
                        string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

                        // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                        // are case-insensitive.
                        if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                            entry.ExtractToFile(destinationPath, overwriteFiles);
                    }
                }
            }
            // ZipFile.ExtractToDirectory(zipPath, extractPath);
            label1.Text = "Done!";
            button2.Visible = true;
            button3.Visible = true;
            File.Delete(Path.Combine(SavePath, ".minecraft/mods/mods.zip"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Visible = false;
        }
    }
}
