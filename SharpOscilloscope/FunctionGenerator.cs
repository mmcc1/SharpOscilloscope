using NAudio.Wave;
using SharpOscilloscopeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SharpOscilloscopeLib.AudioDeviceEnumerator;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SharpOscilloscope
{
    public partial class FunctionGenerator : Form
    {
        private bool running;
        private FuncGenerator generator;
        private WaveOutEvent waveOut;

        public FunctionGenerator()
        {
            InitializeComponent();
        }

        private void FunctionGenerator_Load(object sender, EventArgs e)
        {
            try
            {
                ADEnumerator audioDeviceEnumerator = new ADEnumerator();
                List<ADInfo> audioDeviceInfo = audioDeviceEnumerator.GetOutputDevices();

                foreach (var device in audioDeviceInfo)
                {
                    comboBox1.Items.Add($"{device.Index}: {device.Name} - {device.SampleRate} Hz, {device.BitDepth}-bit");
                }

                comboBox1.SelectedIndex = 0;
            }
            catch
            {

            }
        }

        //Generate
        private void button1_Click(object sender, EventArgs e)
        {
            StartStop();
        }

        private void StartStop()
        {
            if (!running)
            {
                running = true;

                FuncGenerator.WaveType waveType = FuncGenerator.WaveType.Sine;

                if (radioButton1.Checked)
                    waveType = FuncGenerator.WaveType.Sine;
                if (radioButton2.Checked)
                    waveType |= FuncGenerator.WaveType.Square;
                if (radioButton3.Checked)
                    waveType |= FuncGenerator.WaveType.Triangle;
                if (radioButton4.Checked)
                    waveType |= FuncGenerator.WaveType.Sawtooth;
                if (radioButton5.Checked)
                    waveType |= FuncGenerator.WaveType.Pulse;

                generator = new FuncGenerator();
                waveOut = new WaveOutEvent();
                waveOut.DeviceNumber = comboBox1.SelectedIndex;

                // Set to play a 1kHz sine wave
                if (waveType == FuncGenerator.WaveType.Pulse)
                    generator.SetWaveParameters(waveType, (float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value);
                else
                    generator.SetWaveParameters(waveType, (float)numericUpDown1.Value, (float)numericUpDown2.Value);

                waveOut.Init(generator);
                waveOut.Play();

                button1.Text = "&Stop";
            }
            else
            {
                running = false;
                waveOut.Stop();
                button1.Text = "&Generate";
            }
        }

        //Sine
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                    numericUpDown3.Enabled = false;
            }
        }

        //Square
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                    numericUpDown3.Enabled = false;
            }
        }

        //Triangle
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                    numericUpDown3.Enabled = false;
            }
        }

        //Sawtooth
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                    numericUpDown3.Enabled = false;
            }
        }

        //Pulse
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                    numericUpDown3.Enabled = true;
            }
        }

        private void FunctionGenerator_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (running)
                StartStop();
        }
    }
}
