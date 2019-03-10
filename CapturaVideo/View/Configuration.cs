﻿using CapturaVideo.Model;
using CapturaVideo.Model.Enums;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CapturaVideo
{
    public partial class ConfigurationForm : Form
    {
        public bool save { get; set; }
        public ConfigurationForm()
        {
            InitializeComponent();

            //load options inputs
            InitFonts();

            //load config interface
            txt_dir.Text = Configuration.Data.PathSaveVideo;
            bar_timer.Value = Configuration.Data.TimeInterval;
            bar_timer.Enabled = Configuration.Data.EnableInterval;
            bar_frame_rate.Value = Configuration.Data.FrameRate;
            bar_bit_rate.Value = Configuration.Data.BitRate / 1000;
            chk_enable_timer.Checked = Configuration.Data.EnableInterval;
            chk_web_difusion.Checked = Configuration.Data.EnableServer;
            chk_enable_compress.Checked = Configuration.Data.EnableCompressVideo;
            chk_date_time.Checked = Configuration.Data.ViewDateTime;
            cmb_font.Text = Configuration.Data.Font.Size.ToString();
            SetAlign(Configuration.Data.LegendAlign);
        }

        #region Submit
        private void btn_ok_config_Click(object sender, EventArgs e)
        {
            //aply values
            Configuration.Data.PathSaveVideo = GetPath(txt_dir.Text);
            Configuration.Data.TimeInterval = bar_timer.Value;
            Configuration.Data.FrameRate = bar_frame_rate.Value;
            Configuration.Data.BitRate = bar_bit_rate.Value * 1000;
            Configuration.Data.EnableInterval = chk_enable_timer.Checked;
            Configuration.Data.EnableServer = chk_web_difusion.Checked;
            Configuration.Data.EnableCompressVideo = chk_enable_compress.Checked;
            Configuration.Data.ViewDateTime = chk_date_time.Checked;
            Configuration.Data.Font = new Font(Configuration.Data.Font.FontFamily, Convert.ToInt32(cmb_font.Text));
            Configuration.Data.LegendAlign = GetAlign();

            //finish
            save = true;
            Close();
        }

        private void btn_cancel_config_Click(object sender, EventArgs e)
        {
            save = false;
            Close();
        }
        #endregion

        #region Search
        private string GetPath(string txt)
        {
            return txt.Last() != '\\' ? txt + @"\" : txt;
        }
        private void btn_search_dir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog search = new FolderBrowserDialog();
            search.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            if (search.ShowDialog() == DialogResult.OK)
                txt_dir.Text = search.SelectedPath;
        }
        #endregion

        #region Timer
        private void chk_enable_timer_DockChanged(object sender, EventArgs e)
        {
            //enable controls
            bar_timer.Enabled = chk_enable_timer.Checked;
            lbl_interval.Enabled = chk_enable_timer.Checked;

            //aplly values
            if(!chk_enable_timer.Checked)
                bar_timer.Value = Configuration.Data.TimeInterval;
        }
        #endregion

        #region Legend
        private void chk_date_time_CheckedChanged(object sender, EventArgs e)
        {
            //checkboxes
            chk_top_left.Enabled = chk_date_time.Checked;
            chk_top_right.Enabled = chk_date_time.Checked;
            chk_botton_left.Enabled = chk_date_time.Checked;
            chk_botton_right.Enabled = chk_date_time.Checked;

            //labels
            lbl_align.Enabled = chk_date_time.Checked;
            lbl_font.Enabled = chk_date_time.Checked;

            //combobox
            cmb_font.Enabled = chk_date_time.Checked;

            //aplly values
            if (!chk_date_time.Checked){
                cmb_font.Text = Configuration.Data.Font.Size.ToString();
                SetAlign(Configuration.Data.LegendAlign);
            }
        }
        #endregion

        #region Legend Align
        public void SetAlign(ELegendAlign align)
        {
            chk_top_left.Checked = align == ELegendAlign.TopLeft;
            chk_top_right.Checked = align == ELegendAlign.TopRight;
            chk_botton_left.Checked = align == ELegendAlign.BottonLeft;
            chk_botton_right.Checked = align == ELegendAlign.BottonRight;
        }
        public ELegendAlign GetAlign()
        {
            if (chk_top_left.Checked)
                return ELegendAlign.TopLeft;
            if (chk_top_right.Checked)
                return ELegendAlign.TopRight;
            if (chk_botton_left.Checked)
                return ELegendAlign.BottonLeft;

            return ELegendAlign.BottonRight;
        }
        private void chk_top_left_Click(object sender, EventArgs e)
        {
            SetAlign(ELegendAlign.TopLeft);
        }
        private void chk_top_right_Click(object sender, EventArgs e)
        {
            SetAlign(ELegendAlign.TopRight);
        }
        private void chk_botton_right_Click(object sender, EventArgs e)
        {
            SetAlign(ELegendAlign.BottonRight);
        }
        private void chk_botton_left_Click(object sender, EventArgs e)
        {
            SetAlign(ELegendAlign.BottonLeft);
        }
        #endregion

        #region Legend Font
        private void InitFonts()
        {
            foreach (var size in Consts.FONT_SIZE)
                cmb_font.Items.Add(size.ToString());
        }
        #endregion

        #region Bar Control
        private void bar_timer_ValueChanged(object sender, EventArgs e)
        {
            TrackBarInterval(bar_timer);
            lbl_interval.Text = $"Tempo de vídeo [{bar_timer.Value} min]:";
        }
        private void bar_frame_rate_ValueChanged(object sender, EventArgs e)
        {
            TrackBarInterval(bar_frame_rate);
            lbl_frame_rate.Text = $"Frame rate [{bar_frame_rate.Value} FPS]";
        }
        private void bar_bit_rate_ValueChanged(object sender, EventArgs e)
        {
            TrackBarInterval(bar_bit_rate);
            lbl_bit_rate.Text = $"Taxa de bits [{bar_bit_rate.Value} Kbps]";
        }
        private void TrackBarInterval(TrackBar bar)
        {
            if ((bool)(bar.Tag ?? false)) return;
            var value = bar.Value;
            var samll = bar.LargeChange;

            if (bar.Value % samll != 0)
            {
                value = (value / samll) * samll;
                bar.Tag = true;
                bar.Value = value;
                bar.Tag = false;
            }
        }

        #endregion
    }
}
