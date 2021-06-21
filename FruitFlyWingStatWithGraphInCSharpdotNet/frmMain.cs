using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FruitFlyWingStatWithGraphInCSharpdotNet.data;

namespace FruitFlyWingStatWithGraphInCSharpdotNet
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            cmbGender.SelectedIndex = 0;
            
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) //show folder browser dialog
            {
                txtFolderPath.Text = folderBrowserDialog1.SelectedPath; //show selected path on the txtFolderPath text box
                this.LoadFiles(); //call the loadfiles function to load all files in the cmbFiles combo box
            }
        }

        private void LoadFiles(bool ClearRemovedFilesList=true)
        {
            if (ClearRemovedFilesList)
            {
                lstRemovedFiles.Items.Clear(); //Clear Removed List
            }
            //Clear Measurement Outputs
            txtLength.Text = "";
            txtPerimeter.Text = "";
            txtArea.Text = "";
            //Clear Chart points
            chart1.Series[0].Points.Clear();
            cmbFiles.Items.Clear(); //first removing all items from combo box
            if (!string.IsNullOrEmpty(txtFolderPath.Text))
            {                
                string FolderPath = Path.Combine(txtFolderPath.Text, cmbGender.SelectedItem.ToString().ToLower()); //Create Folder Path with user selected folder and the male/female folder
                if (Directory.Exists(FolderPath))
                {
                    string[] CSVFiles = Directory.GetFiles(FolderPath, "*.csv"); //get all CSV files from the folder path
                    //load these csv files to the cmbFiles combo box                    
                    foreach (string file in CSVFiles)
                    {
                        int bsli = file.LastIndexOf(@"\"); //bsli=Back Slash Last Index
                        string filename = file.Substring(bsli + 1, file.Length - (bsli + 1));
                        cmbFiles.Items.Add(filename);
                    }
                    //select first item from combo box
                    if (cmbFiles.Items.Count > 0) //if at there is one item
                    {
                        cmbFiles.SelectedIndex = 0;
                        this.LoadChartData();
                    }
                }                
            }
        }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadFiles(false);
        }

        private void cmbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadChartData();
        }

        private void LoadChartData(bool ShowFolderSelectMessage=true)
        {            
            if (string.IsNullOrEmpty(txtFolderPath.Text))
            {
                if (ShowFolderSelectMessage)
                    MessageBox.Show("Please select folder first.", "Select Folder");
            }
            else
            {
                string FilePath = Path.Combine(txtFolderPath.Text, cmbGender.SelectedItem.ToString().ToLower(), cmbFiles.SelectedItem.ToString());
                float[] x = new float[38];
                float[] y = new float[38];
                ReadValues(FilePath, x, y);

                //Loading Chart

                //Setting Axis
                if (!Global.AUTO_SCALING)
                {                    
                    chart1.ChartAreas[0].AxisX.Minimum = Global.X_AXIS_MINIMUM;
                    chart1.ChartAreas[0].AxisX.Maximum = Global.X_AXIS_MAXIMUM;
                    chart1.ChartAreas[0].AxisX.Interval = Global.X_AXIS_INTERVAL;
                    chart1.ChartAreas[0].AxisY.Minimum = Global.Y_AXIS_MINIMUM;
                    chart1.ChartAreas[0].AxisY.Maximum = Global.Y_AXIS_MAXIMUM;
                    chart1.ChartAreas[0].AxisY.Interval = Global.Y_AXIS_INTERVAL;
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.Minimum = Double.NaN;
                    chart1.ChartAreas[0].AxisX.Maximum = Double.NaN;
                    chart1.ChartAreas[0].AxisX.Interval = Double.NaN;
                    chart1.ChartAreas[0].AxisY.Minimum = Double.NaN;
                    chart1.ChartAreas[0].AxisY.Maximum = Double.NaN;
                    chart1.ChartAreas[0].AxisY.Interval = Double.NaN;
                    chart1.ChartAreas[0].RecalculateAxesScale();
                }

                //Setting Axis Titles
                if (Global.SHOW_X_AXIS_TITLE)
                {
                    chart1.ChartAreas[0].AxisX.Title = Global.X_AXIS_TITLE;
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.Title = "";
                }
                if (Global.SHOW_Y_AXIS_TITLE)
                {
                    chart1.ChartAreas[0].AxisY.Title = Global.Y_AXIS_TITLE;
                }
                else
                {
                    chart1.ChartAreas[0].AxisY.Title = "";
                }

                //Setting Colors
                chart1.Series[0].Color = Global.LINE_COLOR;
                chart1.Series[0].MarkerColor = Global.POINT_COLOR;

                //Setting Widths
                chart1.Series[0].BorderWidth = Global.LINE_WIDTH;
                chart1.Series[0].MarkerSize = Global.POINT_SIZE;

                //Adding Data Points                
                chart1.Series[0].Points.Clear();
                int i;
                for (i = 0; i <= 36; i++)
                {
                    chart1.Series[0].Points.AddXY(x[i], y[i]);
                    if (Global.SHOW_DATA_LABELS)
                    {
                        chart1.Series[0].Points[i].Label = GetDataLabel(i + 1, x[i], y[i]);
                        chart1.Series[0].Points[i].LabelForeColor = Global.DATA_LABEL_COLOR;
                    }
                    if (Global.SHOW_DATA_LABELS_AS_TOOLTIP)
                    {
                        chart1.Series[0].Points[i].ToolTip = GetDataLabel(i + 1, x[i], y[i]);
                    }
                }

                //Show 3 Measurements
                txtLength.Text = this.GetWingLength(x, y).ToString();
                txtPerimeter.Text = this.GetWingPerimeter(x, y).ToString();
                txtArea.Text = this.GetWingArea(x, y).ToString();
            }
        }

        string GetDataLabel(int serial, float x, float y)
        {
            return (serial).ToString() + " (x:" + x + ", y:" + y + ")";
        }

        void ReadValues(string FileNameWithPath, float[] x, float[] y)
        {
            TextFieldParser tfp = new TextFieldParser(FileNameWithPath);
            tfp.Delimiters = new string[] { "," };
            tfp.TextFieldType = FieldType.Delimited;
            tfp.ReadLine(); //skip header
            while (!tfp.EndOfData)
            {
                string[] fields = tfp.ReadFields();
                int i;
                for (i = 0; i <= 36; i++)
                {
                    x[i] = Convert.ToSingle(fields[i * 2]);
                    y[i] = Convert.ToSingle(fields[i * 2 + 1]);
                }
            }
        }

        float GetWingLength(float[] x, float[] y)
        {
            float wl = 0;
            wl = Convert.ToSingle(Math.Sqrt(Math.Pow(x[16] - x[0], 2) + Math.Pow(y[16] - y[0], 2)));
            return wl;
        }

        float GetWingPerimeter(float[] x, float[] y)
        {
            float wp = 0;
            x[37] = x[0];
            y[37] = y[0];
            int p;
            for (p = 0; p <= 36; p++)
            {
                float tempDist = 0;
                tempDist = Convert.ToSingle(Math.Sqrt(Math.Pow(x[p + 1] - x[p], 2) + Math.Pow(y[p + 1] - y[p], 2)));
                wp += tempDist;
            }
            return wp;
        }

        float GetWingArea(float[] x, float[] y)
        {
            float wa = 0;
            float midX = Convert.ToSingle((x[16] - x[0]) / 2.0);
            float midY = Convert.ToSingle((y[16] - y[0]) / 2.0);
            int p;
            for (p = 0; p <= 36; p++)
            {
                float outLen = Convert.ToSingle(Math.Sqrt(Math.Pow(x[p + 1] - x[p], 2) + Math.Pow(y[p + 1] - y[p], 2)));
                float base1 = Convert.ToSingle(Math.Sqrt(Math.Pow(x[p] - midX, 2) + Math.Pow(y[p] - midY, 2)));
                float base2 = Convert.ToSingle(Math.Sqrt(Math.Pow(x[p + 1] - midX, 2) + Math.Pow(y[p + 1] - midY, 2)));
                float heron = Convert.ToSingle((outLen + base1 + base2) / 2.0);
                wa = wa + Convert.ToSingle(Math.Sqrt(heron * (heron - outLen) * (heron - base1) * (heron - base2)));
            }
            return wa;
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            frmOptions frm = new frmOptions();
            frm.ShowDialog();
            this.LoadChartData(false);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cmbFiles.SelectedItem != null && !string.IsNullOrEmpty(cmbFiles.SelectedItem.ToString()))
            {
                if (MessageBox.Show ("Are you sure to Remove this file from Output?", "Remove File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!lstRemovedFiles.Items.Contains(cmbFiles.SelectedItem.ToString()))
                    {
                        lstRemovedFiles.Items.Add(cmbFiles.SelectedItem.ToString());
                    }
                }                
            }
        }

        private void unRemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstRemovedFiles.SelectedItems.Count > 0)
            {
                lstRemovedFiles.Items.RemoveAt(lstRemovedFiles.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Please select a file to UnRemove");
            }
        }

        private void btnWriteDatFiles_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty (txtFolderPath.Text))
            {
                string WorkingFolderPath = txtFolderPath.Text;
                int mfiles, ffiles;
                mfiles = this.GetFilesCount("male");
                ffiles = this.GetFilesCount("female");                
                
                //Output Arrays
                float[] MLength = new float[mfiles];
                float[] MPerimeter = new float[mfiles];
                float[] MArea = new float[mfiles];
                string[] MFilesWithPath = new string[mfiles];
                string[] MFileNames = new string[mfiles];

                float[] FLength = new float[ffiles];
                float[] FPerimeter = new float[ffiles];
                float[] FArea = new float[ffiles];
                string[] FFilesWithPath = new string[mfiles];
                string[] FFileNames = new string[mfiles];


                //Male Files
                string FolderPath = Path.Combine(WorkingFolderPath, "male");
                if (Directory.Exists(FolderPath))
                {
                    string[] CSVFiles = Directory.GetFiles(FolderPath, "*.csv"); //get all CSV files from the folder path                                                                                                    
                    int fn = 0;
                    foreach (string file in CSVFiles)
                    {
                        int bsli = file.LastIndexOf(@"\"); //bsli=Back Slash Last Index
                        string filename = file.Substring(bsli + 1, file.Length - (bsli + 1));
                        if (!lstRemovedFiles.Items.Contains(filename))                        
                        {
                            string FilePath = Path.Combine(FolderPath, filename);
                            float[] x = new float[38];
                            float[] y = new float[38];
                            ReadValues(FilePath, x, y);
                            MLength[fn] = GetWingLength(x, y);
                            MPerimeter[fn] = GetWingPerimeter(x, y);
                            MArea[fn] = GetWingArea(x, y);
                            MFilesWithPath[fn] = file;
                            MFileNames[fn] = filename;
                            fn++;
                        }
                    }
                }

                //Female Files
                FolderPath = Path.Combine(WorkingFolderPath, "female");
                if (Directory.Exists(FolderPath))
                {
                    string[] CSVFiles = Directory.GetFiles(FolderPath, "*.csv"); //get all CSV files from the folder path                                                                                                    
                    int fn = 0;
                    foreach (string file in CSVFiles)
                    {
                        int bsli = file.LastIndexOf(@"\"); //bsli=Back Slash Last Index
                        string filename = file.Substring(bsli + 1, file.Length - (bsli + 1));
                        if (!lstRemovedFiles.Items.Contains(filename))
                        {
                            string FilePath = Path.Combine(FolderPath, filename);
                            float[] x = new float[38];
                            float[] y = new float[38];
                            ReadValues(FilePath, x, y);
                            FLength[fn] = GetWingLength(x, y);
                            FPerimeter[fn] = GetWingPerimeter(x, y);
                            FArea[fn] = GetWingArea(x, y);
                            FFilesWithPath[fn] = file;
                            FFileNames[fn] = filename;
                            fn++;
                        }
                    }
                }


                //Writing output to file
                //Male Output
                string mOutStr = GetOutputString(MLength, MPerimeter, MArea, MFilesWithPath, MFileNames);
                string OutputFileNameWithPath = Path.Combine(WorkingFolderPath, "maleout.dat");
                File.WriteAllText(OutputFileNameWithPath, mOutStr);

                //Female Output
                string fOutStr = GetOutputString(FLength, FPerimeter, FArea, FFilesWithPath, FFileNames);                
                OutputFileNameWithPath = Path.Combine(WorkingFolderPath, "femout.dat");
                File.WriteAllText(OutputFileNameWithPath, fOutStr);

                MessageBox.Show("Files written successfully.");
            }
            else
            {
                MessageBox.Show("Please select an appropriate data folder.");
            }
        }

        int GetFilesCount(string TypeOfFiles)
        {
            int fcount = 0;
            string FolderPath = Path.Combine(txtFolderPath.Text, TypeOfFiles); //Create Folder Path with user selected folder and the male/female folder
            if (Directory.Exists(FolderPath))
            {
                string[] CSVFiles = Directory.GetFiles(FolderPath, "*.csv"); //get all CSV files from the folder path
                                                                             //load these csv files to the cmbFiles combo box                    
                foreach (string file in CSVFiles)
                {
                    int bsli = file.LastIndexOf(@"\"); //bsli=Back Slash Last Index
                    string filename = file.Substring(bsli + 1, file.Length - (bsli + 1));
                    if (!lstRemovedFiles.Items.Contains(filename))
                        fcount++;
                }                
            }
            return fcount;
        }

        string GetOutputString(float[] Length, float[] Perimeter, float[] Area, string[] FileNamesWithPath, string[] FileNames)
        {
            string MStrOutput = String.Format("{0,-10} {1,-10} {2,-10} {3,-10} {4,-25} {5,-25} {6,-10}", "filenum", "length", "peri", "area", "file date time", "current date time", "file name");
            MStrOutput += Environment.NewLine;
            int i;
            for (i = 0; i <= Length.GetUpperBound(0); i++)
            {
                DateTime FileCreationDate = File.GetCreationTime(FileNamesWithPath[i]);
                MStrOutput += String.Format("{0,-10} {1,-10} {2,-10} {3,-10} {4,-25} {5,-25} {6,-10}", (i + 1), Length[i], Perimeter[i], Area[i], FileCreationDate.ToString(), DateTime.Now.ToString(), FileNames[i]);
                MStrOutput += Environment.NewLine;
            }
            return MStrOutput;
        }
    }
}
