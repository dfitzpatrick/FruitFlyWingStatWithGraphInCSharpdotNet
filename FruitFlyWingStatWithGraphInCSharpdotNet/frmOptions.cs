using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FruitFlyWingStatWithGraphInCSharpdotNet
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void frmSetChartAxis_Load(object sender, EventArgs e)
        {
            chkAutoScaling.Checked = Global.AUTO_SCALING;
            txtXAxisMinimum.Text = Global.X_AXIS_MINIMUM.ToString();
            txtXAxisMaximum.Text = Global.X_AXIS_MAXIMUM.ToString();
            txtXAxisInterval.Text = Global.X_AXIS_INTERVAL.ToString();
            txtYAxisMinimum.Text = Global.Y_AXIS_MINIMUM.ToString();
            txtYAxisMaximum.Text = Global.Y_AXIS_MAXIMUM.ToString();
            txtYAxisInterval.Text = Global.Y_AXIS_INTERVAL.ToString();
            txtLineWidth.Text = Global.LINE_WIDTH.ToString();
            txtPointSize.Text = Global.POINT_SIZE.ToString();
            txtLineColor.BackColor = Global.LINE_COLOR;
            txtPointColor.BackColor = Global.POINT_COLOR;
            chkShowDataLabels.Checked = Global.SHOW_DATA_LABELS;
            chkShowDataLabelsToolTips.Checked = Global.SHOW_DATA_LABELS_AS_TOOLTIP;
            txtDataLabelColor.BackColor = Global.DATA_LABEL_COLOR;
            chkShowXAxisTitle.Checked = Global.SHOW_X_AXIS_TITLE;
            txtXAxisTitle.Text = Global.X_AXIS_TITLE;
            chkShowYAxisTitle.Checked = Global.SHOW_Y_AXIS_TITLE;
            txtYAxisTitle.Text = Global.Y_AXIS_TITLE;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Global.AUTO_SCALING = chkAutoScaling.Checked;
            Global.X_AXIS_MINIMUM = Convert.ToDouble(txtXAxisMinimum.Text);
            Global.X_AXIS_MAXIMUM = Convert.ToDouble(txtXAxisMaximum.Text);
            Global.X_AXIS_INTERVAL = Convert.ToDouble(txtXAxisInterval.Text);
            Global.Y_AXIS_MINIMUM = Convert.ToDouble(txtYAxisMinimum.Text);
            Global.Y_AXIS_MAXIMUM = Convert.ToDouble(txtYAxisMaximum.Text);
            Global.Y_AXIS_INTERVAL = Convert.ToDouble(txtYAxisInterval.Text);
            Global.LINE_COLOR = txtLineColor.BackColor;
            Global.POINT_COLOR = txtPointColor.BackColor;
            Global.LINE_WIDTH = Convert.ToInt32(txtLineWidth.Text);
            Global.POINT_SIZE = Convert.ToInt32(txtPointSize.Text);
            Global.SHOW_DATA_LABELS = chkShowDataLabels.Checked;
            Global.SHOW_DATA_LABELS_AS_TOOLTIP = chkShowDataLabelsToolTips.Checked;
            Global.DATA_LABEL_COLOR = txtDataLabelColor.BackColor;
            Global.SHOW_X_AXIS_TITLE = chkShowXAxisTitle.Checked;
            Global.X_AXIS_TITLE = txtXAxisTitle.Text;
            Global.SHOW_Y_AXIS_TITLE = chkShowYAxisTitle.Checked;
            Global.Y_AXIS_TITLE = txtYAxisTitle.Text;
            this.Close();
        }

        private void txtLineColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                txtLineColor.BackColor = colorDialog1.Color;
        }

        private void txtPointColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                txtPointColor.BackColor = colorDialog1.Color;
        }

        private void chkAutoScaling_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoScaling.Checked)
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        private void txtDataLabelColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                txtDataLabelColor.BackColor = colorDialog1.Color;
        }

        private void chkShowXAxisTitle_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowXAxisTitle.Checked)
                txtXAxisTitle.Enabled = true;
            else
                txtXAxisTitle.Enabled = false;
        }

        private void chkShowYAxisTitle_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowYAxisTitle.Checked)
                txtYAxisTitle.Enabled = true;
            else
                txtYAxisTitle.Enabled = false;
        }
    }
}
