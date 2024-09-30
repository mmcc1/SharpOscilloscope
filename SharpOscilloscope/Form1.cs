/*
 * Sharp Oscilloscope - A windows 2 channel oscilloscope which uses
 * audio inputs.
 * 
 * Copyright (C) 2024  Mark McCarron
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, see
 * <https://www.gnu.org/licenses/>.
 */

using SharpOscilloscopeLib;
using SharpOscilloscopeScope;

namespace SharpOscilloscope
{
    public partial class Form1 : Form
    {
        private int channel1DeviceIndex;
        private int channel2DeviceIndex;
        private bool running;
        private AudioCapture audioCapture;
        private AudioCapture audioCapture2;
        bool manualStart = false;
        FftProcessor fftProcessor;
        FifoFloatBuffer fifoBuffer;
        bool fftChannel1 = true;

        public Form1()
        {
            InitializeComponent();

            checkBox1.Checked = true;
            checkBox1_CheckedChanged(null, null);
            checkBox2.Checked = false;
            checkBox2_CheckedChanged(null, null);

            comboBox1.SelectedIndex = 3;  //Time
            comboBox1_SelectedIndexChanged(null, null);
            comboBox2.SelectedIndex = 4; //Amplitude
            comboBox2_SelectedIndexChanged(null, null);

            //Channel 1
            comboBox3.SelectedIndex = 0; //Trigger mode
            comboBox3_SelectedIndexChanged(null, null);
            comboBox4.SelectedIndex = 0; //Trigger type

            //Channel 2
            comboBox6.SelectedIndex = 0; //Trigger mode
            comboBox6_SelectedIndexChanged(null, null);
            comboBox5.SelectedIndex = 0; //Trigger type

            button4.Enabled = false;
            button5.Enabled = false;

            //Status
            toolStripStatusLabel1.Text = "Stopped...";

            comboBox4.Enabled = false;
            comboBox5.Enabled = false;

            //Channel 1 input boxes
            textBox1.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            button2.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;

            //Channel 2 input boxes
            textBox2.Enabled = false;
            textBox10.Enabled = false;
            textBox9.Enabled = false;
            textBox8.Enabled = false;
            textBox7.Enabled = false;
            button3.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;

            fftProcessor = new FftProcessor(4096);
            fifoBuffer = new FifoFloatBuffer();
            radioButton1.Checked = true;
        }

        //Run/Stop button
        private void button1_Click(object sender, EventArgs e)
        {
            if (!running)
            {
                SetupAudioCapture();
                running = true;
                manualStart = true;
                button1.Text = "&Stop";
                //Status
                toolStripStatusLabel1.Text = "Running...";
            }
            else
            {
                running = false;
                audioCapture.StopRecordingChannel1();
                audioCapture2.StopRecordingChannel2();
                signalDisplayControl1.ResetSingleChannel1();

                manualStart = false;
                button1.Text = "&Run";
                //Status
                toolStripStatusLabel1.Text = "Stopped...";
            }
        }

        //Start while changing time
        private void Start()
        {
            if (manualStart)
            {
                SetupAudioCapture();
                running = true;
                button1.Text = "&Stop";
            }
        }

        //Stop while changing time
        private void Stop()
        {
            if (running)
            {
                running = false;
                audioCapture.StopRecordingChannel1();
                audioCapture2.StopRecordingChannel2();
                signalDisplayControl1.ResetSingleChannel1();

                button1.Text = "&Run";
            }
        }

        //Start audio capture
        private void SetupAudioCapture()
        {
            // Initialize audio capture with a callback to process audio data
            audioCapture = new AudioCapture();
            audioCapture.AudioCaptureChannel1(channel1DeviceIndex, 48000, 16, ProcessAudioDataChannel1);
            audioCapture.StartRecordingChannel1();

            audioCapture2 = new AudioCapture();
            audioCapture2.AudioCaptureChannel2(channel2DeviceIndex, 48000, 16, ProcessAudioDataChannel2);
            audioCapture2.StartRecordingChannel2();
        }

        // This will be called when new audio data is available for Channel 1
        private void ProcessAudioDataChannel1(float[] channel1Data, float[] channel1DummyData)
        {
            if (running && fftChannel1)
            {
                fifoBuffer.AppendBlock(channel1Data);


                float[] fftbuffer = fftProcessor.ProcessFft(fifoBuffer.GetBufferContents());
                fftDisplayControl1.UpdateFFTData(fftbuffer);
            }

            signalDisplayControl1.UpdateChannel1Data(channel1Data);
        }

        // This will be called when new audio data is available for Channel 2
        private void ProcessAudioDataChannel2(float[] channel2Data, float[] channel2DummyData)
        {
            if (running && !fftChannel1)
            {
                fifoBuffer.AppendBlock(channel2Data);


                float[] fftbuffer = fftProcessor.ProcessFft(fifoBuffer.GetBufferContents());
                fftDisplayControl1.UpdateFFTData(fftbuffer);
            }

            signalDisplayControl1.UpdateChannel2Data(channel2Data);
        }

        //Edit - Settings
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Settings settings = new Settings();
                settings.channel1Device = channel1DeviceIndex;
                settings.channel2Device = channel2DeviceIndex;

                if (settings.ShowDialog() == DialogResult.OK)
                {
                    channel1DeviceIndex = settings.channel1Device;
                    channel2DeviceIndex = settings.channel2Device;
                }
            }
            catch
            {

            }
        }

        //Time Scale
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: //1ms
                    {
                        Stop();
                        signalDisplayControl1.SetTimeScale(0.001f);
                        signalDisplayControl1.SetIntervalChannel1(3000);
                        signalDisplayControl1.SetIntervalChannel2(3000);
                        Start();
                        break;
                    }
                case 1: //10ms
                    {
                        Stop();
                        signalDisplayControl1.SetTimeScale(0.010f);
                        signalDisplayControl1.SetIntervalChannel1(3000);
                        signalDisplayControl1.SetIntervalChannel2(3000);
                        Start();
                        break;
                    }
                case 2: //50ms
                    {
                        Stop();
                        signalDisplayControl1.SetTimeScale(0.050f);
                        signalDisplayControl1.SetIntervalChannel1(3000);
                        signalDisplayControl1.SetIntervalChannel2(3000);
                        Start();
                        break;
                    }
                case 3: //100ms
                    {
                        Stop();
                        signalDisplayControl1.SetTimeScale(0.100f);
                        signalDisplayControl1.SetIntervalChannel1(3000);
                        signalDisplayControl1.SetIntervalChannel2(3000);
                        Start();
                        break;
                    }
                case 4: //500ms
                    {
                        Stop();
                        signalDisplayControl1.SetTimeScale(0.500f);
                        signalDisplayControl1.SetIntervalChannel1(3000);
                        signalDisplayControl1.SetIntervalChannel2(3000);
                        Start();
                        break;
                    }
                case 5: //1sec
                    {
                        Stop();
                        signalDisplayControl1.SetTimeScale(1.000f);
                        signalDisplayControl1.SetIntervalChannel1(3000);
                        signalDisplayControl1.SetIntervalChannel2(3000);
                        Start();
                        break;
                    }
                case 6: //5sec
                    {
                        Stop();
                        signalDisplayControl1.SetTimeScale(5.000f);
                        signalDisplayControl1.SetIntervalChannel1(8000);
                        signalDisplayControl1.SetIntervalChannel2(8000);
                        Start();
                        break;
                    }
                case 7: //10sec
                    {
                        Stop();
                        signalDisplayControl1.SetTimeScale(10.000f);
                        signalDisplayControl1.SetIntervalChannel1(13000);
                        signalDisplayControl1.SetIntervalChannel2(13000);
                        Start();
                        break;
                    }
                default:
                    break;
            }
        }

        //Voltage scale
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0: //1mV
                    {
                        signalDisplayControl1.SetAmplitudeScale(0.001f);
                        break;
                    }
                case 1: //10mV
                    {
                        signalDisplayControl1.SetAmplitudeScale(0.010f);
                        break;
                    }
                case 2: //100mV
                    {
                        signalDisplayControl1.SetAmplitudeScale(0.100f);
                        break;
                    }
                case 3: //1V
                    {
                        signalDisplayControl1.SetAmplitudeScale(1.000f);
                        break;
                    }
                case 4: //2V
                    {
                        signalDisplayControl1.SetAmplitudeScale(2.000f);
                        break;
                    }
                case 5: //5V
                    {
                        signalDisplayControl1.SetAmplitudeScale(5.000f);
                        break;
                    }
                case 6: //10V
                    {
                        signalDisplayControl1.SetAmplitudeScale(10.000f);
                        break;
                    }
                default:
                    break;
            }
        }

        //Channel 1 - Mode
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0: //None
                    {
                        signalDisplayControl1.SetModeChannel1(0);
                        button4.Enabled = false;
                        comboBox4.Enabled = false;

                        textBox1.Enabled = false;
                        textBox3.Enabled = false;
                        textBox4.Enabled = false;
                        textBox5.Enabled = false;
                        textBox6.Enabled = false;
                        button2.Enabled = false;
                        button6.Enabled = false;
                        button7.Enabled = false;

                        break;
                    }
                case 1: //Auto
                    {
                        signalDisplayControl1.SetModeChannel1(1);
                        button4.Enabled = false;
                        comboBox4.Enabled = true;

                        comboBox4_SelectedIndexChanged(null, null);

                        break;
                    }
                case 2: //Normal
                    {
                        signalDisplayControl1.SetModeChannel1(2);
                        button4.Enabled = false;
                        comboBox4.Enabled = true;

                        comboBox4_SelectedIndexChanged(null, null);

                        break;
                    }
                case 3: //Single
                    {
                        signalDisplayControl1.SetModeChannel1(3);
                        button4.Enabled = true;
                        comboBox4.Enabled = true;

                        comboBox4_SelectedIndexChanged(null, null);

                        break;
                    }
                default:
                    break;
            }
        }

        //Channel 1 - Type
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox4.SelectedIndex)
            {
                case 0: //RisingEdge
                    {
                        signalDisplayControl1.SetTriggerTypeChannel1(0);

                        textBox1.Enabled = true;
                        textBox3.Enabled = false;
                        textBox4.Enabled = false;
                        textBox5.Enabled = false;
                        textBox6.Enabled = false;
                        button2.Enabled = true;
                        button6.Enabled = false;
                        button7.Enabled = false;

                        break;
                    }
                case 1: //FallingEdge
                    {
                        signalDisplayControl1.SetTriggerTypeChannel1(1);

                        textBox1.Enabled = true;
                        textBox3.Enabled = false;
                        textBox4.Enabled = false;
                        textBox5.Enabled = false;
                        textBox6.Enabled = false;
                        button2.Enabled = true;
                        button6.Enabled = false;
                        button7.Enabled = false;
                        break;
                    }
                case 2: //Level
                    {
                        signalDisplayControl1.SetTriggerTypeChannel1(2);

                        textBox1.Enabled = true;
                        textBox3.Enabled = false;
                        textBox4.Enabled = false;
                        textBox5.Enabled = false;
                        textBox6.Enabled = false;
                        button2.Enabled = true;
                        button6.Enabled = false;
                        button7.Enabled = false;
                        break;
                    }
                case 3: //Pulse
                    {
                        signalDisplayControl1.SetTriggerTypeChannel1(3);

                        textBox1.Enabled = true;
                        textBox3.Enabled = true;
                        textBox4.Enabled = true;
                        textBox5.Enabled = false;
                        textBox6.Enabled = false;
                        button2.Enabled = true;
                        button6.Enabled = true;
                        button7.Enabled = false;
                        break;
                    }
                case 4: //Slope
                    {
                        signalDisplayControl1.SetTriggerTypeChannel1(4);

                        textBox1.Enabled = false;
                        textBox3.Enabled = false;
                        textBox4.Enabled = false;
                        textBox5.Enabled = true;
                        textBox6.Enabled = true;
                        button2.Enabled = false;
                        button6.Enabled = false;
                        button7.Enabled = true;
                        break;
                    }
                default:
                    break;
            }
        }

        //Channel 1 - Trigger level
        private void button2_Click(object sender, EventArgs e)
        {
            float level = 0.3f;
            if (float.TryParse(textBox1.Text, out level))
                signalDisplayControl1.SetLevelChannel1(level);
            else
                MessageBox.Show("Numeric values only", "Error");
        }

        //Show/Hide Channel 1
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                signalDisplayControl1.ToggleLeftChannelDisplay(true);
            else
                signalDisplayControl1.ToggleLeftChannelDisplay(false);
        }

        //Show/Hide Channel 2
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                signalDisplayControl1.ToggleRightChannelDisplay(true);
            else
                signalDisplayControl1.ToggleRightChannelDisplay(false);
        }

        //File - Close
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (audioCapture != null)
                {
                    audioCapture.StopRecordingChannel1();
                    audioCapture2.StopRecordingChannel2();
                }
            }
            catch (Exception ex)
            {

            }

            Application.Exit();
        }

        //Channel 2 - Mode
        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox6.SelectedIndex)
            {
                case 0: //None
                    {
                        signalDisplayControl1.SetModeChannel2(0);
                        button5.Enabled = false;
                        comboBox5.Enabled = false;

                        //Channel 2 input boxes
                        textBox2.Enabled = false;
                        textBox10.Enabled = false;
                        textBox9.Enabled = false;
                        textBox8.Enabled = false;
                        textBox7.Enabled = false;
                        button3.Enabled = false;
                        button8.Enabled = false;
                        button9.Enabled = false;

                        break;
                    }
                case 1: //Auto
                    {
                        signalDisplayControl1.SetModeChannel2(1);
                        button5.Enabled = false;
                        comboBox5.Enabled = true;

                        comboBox5_SelectedIndexChanged(null, null);
                        break;
                    }
                case 2: //Normal
                    {
                        signalDisplayControl1.SetModeChannel2(2);
                        button5.Enabled = false;
                        comboBox5.Enabled = true;

                        comboBox5_SelectedIndexChanged(null, null);
                        break;
                    }
                case 3: //Single
                    {
                        signalDisplayControl1.SetModeChannel2(3);
                        button5.Enabled = true;
                        comboBox5.Enabled = true;

                        comboBox5_SelectedIndexChanged(null, null);
                        break;
                    }
                default:
                    break;
            }
        }

        //Channel 2 - Type
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox5.SelectedIndex)
            {
                case 0: //RisingEdge
                    {
                        signalDisplayControl1.SetTriggerTypeChannel2(0);

                        //Channel 2 input boxes
                        textBox2.Enabled = true;
                        textBox10.Enabled = false;
                        textBox9.Enabled = false;
                        textBox8.Enabled = false;
                        textBox7.Enabled = false;
                        button3.Enabled = true;
                        button8.Enabled = false;
                        button9.Enabled = false;
                        break;
                    }
                case 1: //FallingEdge
                    {
                        signalDisplayControl1.SetTriggerTypeChannel2(1);

                        //Channel 2 input boxes
                        textBox2.Enabled = true;
                        textBox10.Enabled = false;
                        textBox9.Enabled = false;
                        textBox8.Enabled = false;
                        textBox7.Enabled = false;
                        button3.Enabled = true;
                        button8.Enabled = false;
                        button9.Enabled = false;
                        break;
                    }
                case 2: //Level
                    {
                        signalDisplayControl1.SetTriggerTypeChannel2(2);

                        //Channel 2 input boxes
                        textBox2.Enabled = true;
                        textBox10.Enabled = false;
                        textBox9.Enabled = false;
                        textBox8.Enabled = false;
                        textBox7.Enabled = false;
                        button3.Enabled = true;
                        button8.Enabled = false;
                        button9.Enabled = false;
                        break;
                    }
                case 3: //Pulse
                    {
                        signalDisplayControl1.SetTriggerTypeChannel2(3);

                        //Channel 2 input boxes
                        textBox2.Enabled = true;
                        textBox10.Enabled = true;
                        textBox9.Enabled = true;
                        textBox8.Enabled = false;
                        textBox7.Enabled = false;
                        button3.Enabled = true;
                        button8.Enabled = true;
                        button9.Enabled = false;
                        break;
                    }
                case 4: //Slope
                    {
                        signalDisplayControl1.SetTriggerTypeChannel2(4);

                        //Channel 2 input boxes
                        textBox2.Enabled = false;
                        textBox10.Enabled = false;
                        textBox9.Enabled = false;
                        textBox8.Enabled = true;
                        textBox7.Enabled = true;
                        button3.Enabled = false;
                        button8.Enabled = false;
                        button9.Enabled = true;
                        break;
                    }
                default:
                    break;
            }
        }

        //Channel 2 - Triggrt level
        private void button3_Click(object sender, EventArgs e)
        {
            float level = 0.3f;
            if (float.TryParse(textBox2.Text, out level))
                signalDisplayControl1.SetLevelChannel2(level);
            else
                MessageBox.Show("Numeric values only", "Error");
        }

        //Channel 1 - Single Trigger
        private void button4_Click(object sender, EventArgs e)
        {
            signalDisplayControl1.ResetSingleChannel1();
        }

        //Channel 2 - Single Trigger
        private void button5_Click(object sender, EventArgs e)
        {
            signalDisplayControl1.ResetSingleChannel2();
        }

        //Channel 1 - Pulse duration
        private void button6_Click(object sender, EventArgs e)
        {
            double durationmin = 0.0;
            double durationmax = 0.0;

            if (!double.TryParse(textBox3.Text, out durationmin))
                MessageBox.Show("Minimum value must be numeric.", "Error");

            if (!double.TryParse(textBox4.Text, out durationmax))
                MessageBox.Show("Maximum value must be numeric.", "Error");

            if (durationmin < durationmax && durationmin >= 1)
            {
                signalDisplayControl1.SetMinPulseDurationChannel1(durationmin);
                signalDisplayControl1.SetMaxPulseDurationChannel1(durationmax);
            }
            else
                MessageBox.Show("Minimum value must be greater than or equal to one and maximum value must exceed minimum value.", "Error");
        }

        //Channel 1 - slope values
        private void button7_Click(object sender, EventArgs e)
        {
            float threshold = 0.0f;
            float tbs = 0.0f;

            if (!float.TryParse(textBox6.Text, out threshold))
                MessageBox.Show("Minimum value must be numeric.", "Error");

            if (!float.TryParse(textBox5.Text, out tbs))
                MessageBox.Show("Maximum value must be numeric.", "Error");

            if (threshold > 0 && tbs > 0)
            {
                signalDisplayControl1.SetSlopeThresholdChannel1(threshold);
                signalDisplayControl1.SetTimeBetweenSamplesChannel1(tbs);
            }
        }

        //Channel 2 - Pulse duration
        private void button8_Click(object sender, EventArgs e)
        {
            double durationmin = 0.0;
            double durationmax = 0.0;

            if (!double.TryParse(textBox10.Text, out durationmin))
                MessageBox.Show("Minimum value must be numeric.", "Error");

            if (!double.TryParse(textBox9.Text, out durationmax))
                MessageBox.Show("Maximum value must be numeric.", "Error");

            if (durationmin < durationmax && durationmin >= 1)
            {
                signalDisplayControl1.SetMinPulseDurationChannel2(durationmin);
                signalDisplayControl1.SetMaxPulseDurationChannel2(durationmax);
            }
            else
                MessageBox.Show("Minimum value must be greater than or equal to one and maximum value must exceed minimum value.", "Error");
        }

        //Channel 2 - slope values
        private void button9_Click(object sender, EventArgs e)
        {
            float threshold = 0.0f;
            float tbs = 0.0f;

            if (!float.TryParse(textBox8.Text, out threshold))
                MessageBox.Show("Minimum value must be numeric.", "Error");

            if (!float.TryParse(textBox7.Text, out tbs))
                MessageBox.Show("Maximum value must be numeric.", "Error");

            if (threshold > 0 && tbs > 0)
            {
                signalDisplayControl1.SetSlopeThresholdChannel2(threshold);
                signalDisplayControl1.SetTimeBetweenSamplesChannel2(tbs);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                    fftChannel1 = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                    fftChannel1 = false;
            }
        }
    }
}
