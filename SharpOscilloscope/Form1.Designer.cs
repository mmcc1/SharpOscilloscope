namespace SharpOscilloscope
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            splitContainer1 = new SplitContainer();
            groupBox4 = new GroupBox();
            button9 = new Button();
            button8 = new Button();
            label13 = new Label();
            textBox7 = new TextBox();
            label14 = new Label();
            textBox8 = new TextBox();
            label15 = new Label();
            textBox9 = new TextBox();
            label16 = new Label();
            textBox10 = new TextBox();
            button5 = new Button();
            button3 = new Button();
            label6 = new Label();
            textBox2 = new TextBox();
            label7 = new Label();
            comboBox5 = new ComboBox();
            comboBox6 = new ComboBox();
            label8 = new Label();
            groupBox3 = new GroupBox();
            checkBox2 = new CheckBox();
            checkBox1 = new CheckBox();
            groupBox2 = new GroupBox();
            button7 = new Button();
            button6 = new Button();
            label11 = new Label();
            textBox5 = new TextBox();
            label12 = new Label();
            textBox6 = new TextBox();
            label10 = new Label();
            textBox4 = new TextBox();
            label9 = new Label();
            textBox3 = new TextBox();
            button4 = new Button();
            button2 = new Button();
            label5 = new Label();
            textBox1 = new TextBox();
            label4 = new Label();
            comboBox4 = new ComboBox();
            comboBox3 = new ComboBox();
            label3 = new Label();
            groupBox1 = new GroupBox();
            comboBox2 = new ComboBox();
            comboBox1 = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            button1 = new Button();
            signalDisplayControl1 = new SharpOscilloscopeScope.SignalDisplayControl();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1309, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { closeToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new Size(103, 22);
            closeToolStripMenuItem.Text = "&Close";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { settingsToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(125, 22);
            settingsToolStripMenuItem.Text = "&Settings...";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 870);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1309, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(118, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBox4);
            splitContainer1.Panel1.Controls.Add(groupBox3);
            splitContainer1.Panel1.Controls.Add(groupBox2);
            splitContainer1.Panel1.Controls.Add(groupBox1);
            splitContainer1.Panel1.Controls.Add(button1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(signalDisplayControl1);
            splitContainer1.Size = new Size(1309, 846);
            splitContainer1.SplitterDistance = 317;
            splitContainer1.TabIndex = 2;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(button9);
            groupBox4.Controls.Add(button8);
            groupBox4.Controls.Add(label13);
            groupBox4.Controls.Add(textBox7);
            groupBox4.Controls.Add(label14);
            groupBox4.Controls.Add(textBox8);
            groupBox4.Controls.Add(label15);
            groupBox4.Controls.Add(textBox9);
            groupBox4.Controls.Add(label16);
            groupBox4.Controls.Add(textBox10);
            groupBox4.Controls.Add(button5);
            groupBox4.Controls.Add(button3);
            groupBox4.Controls.Add(label6);
            groupBox4.Controls.Add(textBox2);
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(comboBox5);
            groupBox4.Controls.Add(comboBox6);
            groupBox4.Controls.Add(label8);
            groupBox4.Location = new Point(12, 234);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(298, 222);
            groupBox4.TabIndex = 13;
            groupBox4.TabStop = false;
            groupBox4.Text = "Channel 2";
            // 
            // button9
            // 
            button9.Location = new Point(205, 190);
            button9.Name = "button9";
            button9.Size = new Size(75, 23);
            button9.TabIndex = 25;
            button9.Text = "Set";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button8
            // 
            button8.Location = new Point(205, 132);
            button8.Name = "button8";
            button8.Size = new Size(75, 23);
            button8.TabIndex = 24;
            button8.Text = "Set";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(6, 194);
            label13.Name = "label13";
            label13.Size = new Size(70, 15);
            label13.TabIndex = 23;
            label13.Text = "Sample Gap";
            // 
            // textBox7
            // 
            textBox7.Location = new Point(89, 191);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(107, 23);
            textBox7.TabIndex = 22;
            textBox7.Text = "0.001";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(6, 165);
            label14.Name = "label14";
            label14.Size = new Size(59, 15);
            label14.TabIndex = 21;
            label14.Text = "Threshold";
            // 
            // textBox8
            // 
            textBox8.Location = new Point(89, 162);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(107, 23);
            textBox8.TabIndex = 20;
            textBox8.Text = "0.5";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(6, 136);
            label15.Name = "label15";
            label15.Size = new Size(79, 15);
            label15.TabIndex = 19;
            label15.Text = "Max Duration";
            // 
            // textBox9
            // 
            textBox9.Location = new Point(89, 133);
            textBox9.Name = "textBox9";
            textBox9.Size = new Size(107, 23);
            textBox9.TabIndex = 18;
            textBox9.Text = "1000";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(6, 107);
            label16.Name = "label16";
            label16.Size = new Size(77, 15);
            label16.TabIndex = 17;
            label16.Text = "Min Duration";
            // 
            // textBox10
            // 
            textBox10.Location = new Point(89, 104);
            textBox10.Name = "textBox10";
            textBox10.Size = new Size(107, 23);
            textBox10.TabIndex = 16;
            textBox10.Text = "1";
            // 
            // button5
            // 
            button5.Location = new Point(205, 15);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 7;
            button5.Text = "T&rigger";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button3
            // 
            button3.Location = new Point(205, 75);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 6;
            button3.Text = "Set";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 78);
            label6.Name = "label6";
            label6.Size = new Size(34, 15);
            label6.TabIndex = 5;
            label6.Text = "Level";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(89, 75);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(110, 23);
            textBox2.TabIndex = 4;
            textBox2.Text = "0.3";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 48);
            label7.Name = "label7";
            label7.Size = new Size(31, 15);
            label7.TabIndex = 3;
            label7.Text = "Type";
            // 
            // comboBox5
            // 
            comboBox5.FormattingEnabled = true;
            comboBox5.Items.AddRange(new object[] { "Rising Edge", "Falling Edge", "Level", "Pulse", "Slope" });
            comboBox5.Location = new Point(89, 45);
            comboBox5.Name = "comboBox5";
            comboBox5.Size = new Size(107, 23);
            comboBox5.TabIndex = 2;
            comboBox5.SelectedIndexChanged += comboBox5_SelectedIndexChanged;
            // 
            // comboBox6
            // 
            comboBox6.FormattingEnabled = true;
            comboBox6.Items.AddRange(new object[] { "None", "Auto", "Normal", "Single" });
            comboBox6.Location = new Point(89, 16);
            comboBox6.Name = "comboBox6";
            comboBox6.Size = new Size(107, 23);
            comboBox6.TabIndex = 1;
            comboBox6.SelectedIndexChanged += comboBox6_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 19);
            label8.Name = "label8";
            label8.Size = new Size(38, 15);
            label8.TabIndex = 0;
            label8.Text = "Mode";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(checkBox2);
            groupBox3.Controls.Add(checkBox1);
            groupBox3.Location = new Point(12, 558);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(298, 55);
            groupBox3.TabIndex = 13;
            groupBox3.TabStop = false;
            groupBox3.Text = "Display";
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(123, 22);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(111, 19);
            checkBox2.TabIndex = 4;
            checkBox2.Text = "Show Channel 2";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(6, 22);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(111, 19);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "Show Channel 1";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button7);
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(textBox5);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(textBox6);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(textBox4);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(textBox3);
            groupBox2.Controls.Add(button4);
            groupBox2.Controls.Add(button2);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(textBox1);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(comboBox4);
            groupBox2.Controls.Add(comboBox3);
            groupBox2.Controls.Add(label3);
            groupBox2.Location = new Point(12, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(298, 225);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "Channel 1";
            // 
            // button7
            // 
            button7.Location = new Point(205, 190);
            button7.Name = "button7";
            button7.Size = new Size(75, 23);
            button7.TabIndex = 17;
            button7.Text = "Set";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button6
            // 
            button6.Location = new Point(205, 133);
            button6.Name = "button6";
            button6.Size = new Size(75, 23);
            button6.TabIndex = 16;
            button6.Text = "Set";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(6, 194);
            label11.Name = "label11";
            label11.Size = new Size(70, 15);
            label11.TabIndex = 15;
            label11.Text = "Sample Gap";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(89, 191);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(107, 23);
            textBox5.TabIndex = 14;
            textBox5.Text = "0.001";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(6, 165);
            label12.Name = "label12";
            label12.Size = new Size(59, 15);
            label12.TabIndex = 13;
            label12.Text = "Threshold";
            // 
            // textBox6
            // 
            textBox6.Location = new Point(89, 162);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(107, 23);
            textBox6.TabIndex = 12;
            textBox6.Text = "0.5";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 136);
            label10.Name = "label10";
            label10.Size = new Size(79, 15);
            label10.TabIndex = 11;
            label10.Text = "Max Duration";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(89, 133);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(107, 23);
            textBox4.TabIndex = 10;
            textBox4.Text = "1000";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 107);
            label9.Name = "label9";
            label9.Size = new Size(77, 15);
            label9.TabIndex = 9;
            label9.Text = "Min Duration";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(89, 104);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(107, 23);
            textBox3.TabIndex = 8;
            textBox3.Text = "1";
            // 
            // button4
            // 
            button4.Location = new Point(205, 15);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 7;
            button4.Text = "&Trigger";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button2
            // 
            button2.Location = new Point(205, 75);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 6;
            button2.Text = "Set";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 75);
            label5.Name = "label5";
            label5.Size = new Size(34, 15);
            label5.TabIndex = 5;
            label5.Text = "Level";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(89, 75);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(110, 23);
            textBox1.TabIndex = 4;
            textBox1.Text = "0.3";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 48);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 3;
            label4.Text = "Type";
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "Rising Edge", "Falling Edge", "Level", "Pulse", "Slope" });
            comboBox4.Location = new Point(89, 45);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(107, 23);
            comboBox4.TabIndex = 2;
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "None", "Auto", "Normal", "Single" });
            comboBox3.Location = new Point(89, 16);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(107, 23);
            comboBox3.TabIndex = 1;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 19);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 0;
            label3.Text = "Mode";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 462);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(298, 90);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Scaling";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "1mV", "10mV", "100mV", "1V", "2V", "5V", "10V" });
            comboBox2.Location = new Point(89, 51);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(110, 23);
            comboBox2.TabIndex = 14;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "1ms", "10ms", "50ms", "100ms", "500ms", "1sec", "5sec", "10sec" });
            comboBox1.Location = new Point(89, 22);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(110, 23);
            comboBox1.TabIndex = 13;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 54);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 12;
            label2.Text = "Amplitude";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 25);
            label1.Name = "label1";
            label1.Size = new Size(33, 15);
            label1.TabIndex = 11;
            label1.Text = "Time";
            // 
            // button1
            // 
            button1.Location = new Point(235, 619);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "&Run";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // signalDisplayControl1
            // 
            signalDisplayControl1.BackColor = Color.Black;
            signalDisplayControl1.Dock = DockStyle.Fill;
            signalDisplayControl1.Location = new Point(0, 0);
            signalDisplayControl1.Name = "signalDisplayControl1";
            signalDisplayControl1.Size = new Size(988, 846);
            signalDisplayControl1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1309, 892);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Sharp Oscilloscope";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private SplitContainer splitContainer1;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private Button button1;
        private SharpOscilloscopeScope.SignalDisplayControl signalDisplayControl1;
        private GroupBox groupBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox1;
        private Label label2;
        private Label label1;
        private GroupBox groupBox2;
        private ComboBox comboBox4;
        private ComboBox comboBox3;
        private Label label3;
        private Label label5;
        private TextBox textBox1;
        private Label label4;
        private Button button2;
        private GroupBox groupBox3;
        private CheckBox checkBox2;
        private CheckBox checkBox1;
        private GroupBox groupBox4;
        private Button button3;
        private Label label6;
        private TextBox textBox2;
        private Label label7;
        private ComboBox comboBox5;
        private ComboBox comboBox6;
        private Label label8;
        private Button button4;
        private Button button5;
        private Label label9;
        private TextBox textBox3;
        private Label label10;
        private TextBox textBox4;
        private Label label11;
        private TextBox textBox5;
        private Label label12;
        private TextBox textBox6;
        private Button button9;
        private Button button8;
        private Label label13;
        private TextBox textBox7;
        private Label label14;
        private TextBox textBox8;
        private Label label15;
        private TextBox textBox9;
        private Label label16;
        private TextBox textBox10;
        private Button button7;
        private Button button6;
    }
}
