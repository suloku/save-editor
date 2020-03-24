using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SaveEditor.Core;

namespace SaveEditor.UI
{
    public partial class MainWindow : Form
    {
        private SRAM SRAM_;

        private SaveFile currentSaveFile_;

        public MainWindow()
        {
            InitializeComponent();

            saveCombo.Items.Clear();
            saveCombo.Items.Add("File 1");
            saveCombo.Items.Add("File 2");
            saveCombo.Items.Add("File 3");
            saveCombo.SelectedIndex = 0;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadRom();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadRom()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "GBA Save Files|*.sav|All Files|*.*",
                Title = "Select TMC Save File"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                SRAM_ = new SRAM(ofd.FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            /*
            if (SRAM.Instance.version.Equals(RegionVersion.None))
            {
                MessageBox.Show("Invalid TMC save file. Please open a valid save.", "Invalid save", MessageBoxButtons.OK);
                statusText.Text = "Unable to determine save file.";
                return;
            }
            */
            currentSaveFile_ = SRAM_.GetSaveFile(0);
            LoadData();
        }

        private void LoadData()
        {
            saveCombo.Enabled = true;
            fileNameTextBox.Enabled = true;
            LightArrowBut.Enabled = true;

            fileNameTextBox.Text = currentSaveFile_.GetName();

            statusText.Text = "Loaded: " + SRAM.Instance.path;
        }

        private void Save()
        {
            if (SRAM_ != null)
            {
                currentSaveFile_.WriteName(fileNameTextBox.Text);

                SRAM_.SaveFile(currentSaveFile_, saveCombo.SelectedIndex);
            }
            else
            {
                statusText.Text = "Save failed: No save file loaded";
            }
        }

        private void saveCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SRAM_ != null)
            {
                // check to see if data changed, then load based on new index.
                currentSaveFile_ = SRAM_.GetSaveFile(saveCombo.SelectedIndex);
                LoadData();
            }
           
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("Closing");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentSaveFile_.EnableArrows();
            MessageBox.Show("Light Arrows Flag enabled");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Light Arrows are miss-able. If Link reaches Cloud Tops (via the whirlwind atop Veil Falls) having not rid him of the ghost, Gregal will still be sick and the Light Arrows permanently out of reach. To get the Light Arrows and the reward of 100 Mysterious Shells, Link must warp to the Tower of Winds and rid Gregal of his ghost before riding the whirlwind to Cloud Tops.\n\n" +
                "This will enable the flag set when Link saves Gregal, allowing to get the rewards even if Link has already reached Cloud Tops.");
        }
    }
}
