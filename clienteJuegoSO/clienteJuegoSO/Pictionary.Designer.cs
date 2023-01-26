
namespace clienteJuegoSO
{
    partial class Pictionary
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pictionary));
            this.Mensaje_Pictionary_btn = new System.Windows.Forms.Button();
            this.Respuesta_Pictionary = new System.Windows.Forms.TextBox();
            this.pictionaryChat = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.j1PictionaryLbl = new System.Windows.Forms.Label();
            this.j2PictionaryLbl = new System.Windows.Forms.Label();
            this.puntosJ4PictLbl = new System.Windows.Forms.Label();
            this.j3PictionaryLbl = new System.Windows.Forms.Label();
            this.puntosJ3PictLbl = new System.Windows.Forms.Label();
            this.j4PictionaryLbl = new System.Windows.Forms.Label();
            this.puntosJ2PictLbl = new System.Windows.Forms.Label();
            this.puntosJ1PictLbl = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PictionaryAdvisoryLbl = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.timePictionaryLbl = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.prompt_Lbl = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.clearButton = new System.Windows.Forms.PictureBox();
            this.boxPurple = new System.Windows.Forms.PictureBox();
            this.boxBlue = new System.Windows.Forms.PictureBox();
            this.boxYellow = new System.Windows.Forms.PictureBox();
            this.boxBlack = new System.Windows.Forms.PictureBox();
            this.boxOrange = new System.Windows.Forms.PictureBox();
            this.boxGreen = new System.Windows.Forms.PictureBox();
            this.boxRed = new System.Windows.Forms.PictureBox();
            this.timerPictionary = new System.Windows.Forms.Timer(this.components);
            this.timerPictionaryPuntos = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.AdvisoryBackground = new System.Windows.Forms.PictureBox();
            this.groupBox5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clearButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxPurple)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxYellow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxBlack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxOrange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdvisoryBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // Mensaje_Pictionary_btn
            // 
            this.Mensaje_Pictionary_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Mensaje_Pictionary_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Mensaje_Pictionary_btn.Location = new System.Drawing.Point(1385, 775);
            this.Mensaje_Pictionary_btn.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Mensaje_Pictionary_btn.Name = "Mensaje_Pictionary_btn";
            this.Mensaje_Pictionary_btn.Size = new System.Drawing.Size(68, 50);
            this.Mensaje_Pictionary_btn.TabIndex = 45;
            this.Mensaje_Pictionary_btn.Text = "➤";
            this.Mensaje_Pictionary_btn.UseVisualStyleBackColor = true;
            this.Mensaje_Pictionary_btn.Click += new System.EventHandler(this.Mensaje_Pictionary_btn_Click);
            // 
            // Respuesta_Pictionary
            // 
            this.Respuesta_Pictionary.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Respuesta_Pictionary.Location = new System.Drawing.Point(1133, 775);
            this.Respuesta_Pictionary.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Respuesta_Pictionary.MaxLength = 100;
            this.Respuesta_Pictionary.Name = "Respuesta_Pictionary";
            this.Respuesta_Pictionary.Size = new System.Drawing.Size(244, 44);
            this.Respuesta_Pictionary.TabIndex = 44;
            this.Respuesta_Pictionary.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Respuesta_Pictionary_KeyDown);
            // 
            // pictionaryChat
            // 
            this.pictionaryChat.AcceptsReturn = true;
            this.pictionaryChat.AcceptsTab = true;
            this.pictionaryChat.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.pictionaryChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pictionaryChat.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pictionaryChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.pictionaryChat.Location = new System.Drawing.Point(1133, 40);
            this.pictionaryChat.Margin = new System.Windows.Forms.Padding(4);
            this.pictionaryChat.Multiline = true;
            this.pictionaryChat.Name = "pictionaryChat";
            this.pictionaryChat.ReadOnly = true;
            this.pictionaryChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.pictionaryChat.ShortcutsEnabled = false;
            this.pictionaryChat.Size = new System.Drawing.Size(320, 713);
            this.pictionaryChat.TabIndex = 47;
            this.pictionaryChat.WordWrap = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.j1PictionaryLbl);
            this.groupBox5.Controls.Add(this.j2PictionaryLbl);
            this.groupBox5.Controls.Add(this.puntosJ4PictLbl);
            this.groupBox5.Controls.Add(this.j3PictionaryLbl);
            this.groupBox5.Controls.Add(this.puntosJ3PictLbl);
            this.groupBox5.Controls.Add(this.j4PictionaryLbl);
            this.groupBox5.Controls.Add(this.puntosJ2PictLbl);
            this.groupBox5.Controls.Add(this.puntosJ1PictLbl);
            this.groupBox5.Location = new System.Drawing.Point(29, 675);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(1074, 158);
            this.groupBox5.TabIndex = 46;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Puntos";
            // 
            // j1PictionaryLbl
            // 
            this.j1PictionaryLbl.AutoSize = true;
            this.j1PictionaryLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.j1PictionaryLbl.Location = new System.Drawing.Point(32, 42);
            this.j1PictionaryLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.j1PictionaryLbl.Name = "j1PictionaryLbl";
            this.j1PictionaryLbl.Size = new System.Drawing.Size(110, 26);
            this.j1PictionaryLbl.TabIndex = 30;
            this.j1PictionaryLbl.Text = "Jugador1";
            // 
            // j2PictionaryLbl
            // 
            this.j2PictionaryLbl.AutoSize = true;
            this.j2PictionaryLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.j2PictionaryLbl.Location = new System.Drawing.Point(210, 42);
            this.j2PictionaryLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.j2PictionaryLbl.Name = "j2PictionaryLbl";
            this.j2PictionaryLbl.Size = new System.Drawing.Size(110, 26);
            this.j2PictionaryLbl.TabIndex = 31;
            this.j2PictionaryLbl.Text = "Jugador2";
            // 
            // puntosJ4PictLbl
            // 
            this.puntosJ4PictLbl.AutoSize = true;
            this.puntosJ4PictLbl.Location = new System.Drawing.Point(620, 90);
            this.puntosJ4PictLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.puntosJ4PictLbl.Name = "puntosJ4PictLbl";
            this.puntosJ4PictLbl.Size = new System.Drawing.Size(24, 25);
            this.puntosJ4PictLbl.TabIndex = 37;
            this.puntosJ4PictLbl.Text = "0";
            // 
            // j3PictionaryLbl
            // 
            this.j3PictionaryLbl.AutoSize = true;
            this.j3PictionaryLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.j3PictionaryLbl.Location = new System.Drawing.Point(392, 42);
            this.j3PictionaryLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.j3PictionaryLbl.Name = "j3PictionaryLbl";
            this.j3PictionaryLbl.Size = new System.Drawing.Size(110, 26);
            this.j3PictionaryLbl.TabIndex = 32;
            this.j3PictionaryLbl.Text = "Jugador3";
            // 
            // puntosJ3PictLbl
            // 
            this.puntosJ3PictLbl.AutoSize = true;
            this.puntosJ3PictLbl.Location = new System.Drawing.Point(434, 90);
            this.puntosJ3PictLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.puntosJ3PictLbl.Name = "puntosJ3PictLbl";
            this.puntosJ3PictLbl.Size = new System.Drawing.Size(24, 25);
            this.puntosJ3PictLbl.TabIndex = 36;
            this.puntosJ3PictLbl.Text = "0";
            // 
            // j4PictionaryLbl
            // 
            this.j4PictionaryLbl.AutoSize = true;
            this.j4PictionaryLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.j4PictionaryLbl.Location = new System.Drawing.Point(572, 42);
            this.j4PictionaryLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.j4PictionaryLbl.Name = "j4PictionaryLbl";
            this.j4PictionaryLbl.Size = new System.Drawing.Size(110, 26);
            this.j4PictionaryLbl.TabIndex = 33;
            this.j4PictionaryLbl.Text = "Jugador4";
            // 
            // puntosJ2PictLbl
            // 
            this.puntosJ2PictLbl.AutoSize = true;
            this.puntosJ2PictLbl.Location = new System.Drawing.Point(256, 90);
            this.puntosJ2PictLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.puntosJ2PictLbl.Name = "puntosJ2PictLbl";
            this.puntosJ2PictLbl.Size = new System.Drawing.Size(24, 25);
            this.puntosJ2PictLbl.TabIndex = 35;
            this.puntosJ2PictLbl.Text = "0";
            // 
            // puntosJ1PictLbl
            // 
            this.puntosJ1PictLbl.AutoSize = true;
            this.puntosJ1PictLbl.Location = new System.Drawing.Point(76, 90);
            this.puntosJ1PictLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.puntosJ1PictLbl.Name = "puntosJ1PictLbl";
            this.puntosJ1PictLbl.Size = new System.Drawing.Size(24, 25);
            this.puntosJ1PictLbl.TabIndex = 34;
            this.puntosJ1PictLbl.Text = "0";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(29, 115);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1074, 541);
            this.panel1.TabIndex = 43;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // PictionaryAdvisoryLbl
            // 
            this.PictionaryAdvisoryLbl.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.PictionaryAdvisoryLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PictionaryAdvisoryLbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.PictionaryAdvisoryLbl.Location = new System.Drawing.Point(130, 245);
            this.PictionaryAdvisoryLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PictionaryAdvisoryLbl.Name = "PictionaryAdvisoryLbl";
            this.PictionaryAdvisoryLbl.Size = new System.Drawing.Size(856, 272);
            this.PictionaryAdvisoryLbl.TabIndex = 44;
            this.PictionaryAdvisoryLbl.Text = "label1";
            this.PictionaryAdvisoryLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Gainsboro;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.timePictionaryLbl);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.prompt_Lbl);
            this.panel3.Location = new System.Drawing.Point(29, 26);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(510, 90);
            this.panel3.TabIndex = 42;
            // 
            // timePictionaryLbl
            // 
            this.timePictionaryLbl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.timePictionaryLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timePictionaryLbl.Location = new System.Drawing.Point(393, 1);
            this.timePictionaryLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.timePictionaryLbl.Name = "timePictionaryLbl";
            this.timePictionaryLbl.Size = new System.Drawing.Size(116, 88);
            this.timePictionaryLbl.TabIndex = 38;
            this.timePictionaryLbl.Text = "100";
            this.timePictionaryLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(6, 6);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(240, 29);
            this.label9.TabIndex = 41;
            this.label9.Text = "Tienes que dibujar:";
            this.label9.Visible = false;
            // 
            // prompt_Lbl
            // 
            this.prompt_Lbl.AutoSize = true;
            this.prompt_Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.125F, System.Drawing.FontStyle.Bold);
            this.prompt_Lbl.Location = new System.Drawing.Point(6, 40);
            this.prompt_Lbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.prompt_Lbl.Name = "prompt_Lbl";
            this.prompt_Lbl.Size = new System.Drawing.Size(224, 44);
            this.prompt_Lbl.TabIndex = 39;
            this.prompt_Lbl.Text = "SOLUCION";
            this.prompt_Lbl.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.clearButton);
            this.panel2.Controls.Add(this.boxPurple);
            this.panel2.Controls.Add(this.boxBlue);
            this.panel2.Controls.Add(this.boxYellow);
            this.panel2.Controls.Add(this.boxBlack);
            this.panel2.Controls.Add(this.boxOrange);
            this.panel2.Controls.Add(this.boxGreen);
            this.panel2.Controls.Add(this.boxRed);
            this.panel2.Location = new System.Drawing.Point(538, 26);
            this.panel2.Margin = new System.Windows.Forms.Padding(6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(565, 90);
            this.panel2.TabIndex = 1;
            // 
            // clearButton
            // 
            this.clearButton.BackColor = System.Drawing.Color.Black;
            this.clearButton.Image = global::clienteJuegoSO.Properties.Resources.erase;
            this.clearButton.Location = new System.Drawing.Point(490, 15);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(60, 60);
            this.clearButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.clearButton.TabIndex = 45;
            this.clearButton.TabStop = false;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // boxPurple
            // 
            this.boxPurple.BackColor = System.Drawing.Color.DarkViolet;
            this.boxPurple.Location = new System.Drawing.Point(346, 15);
            this.boxPurple.Margin = new System.Windows.Forms.Padding(6);
            this.boxPurple.Name = "boxPurple";
            this.boxPurple.Size = new System.Drawing.Size(60, 60);
            this.boxPurple.TabIndex = 4;
            this.boxPurple.TabStop = false;
            this.boxPurple.Click += new System.EventHandler(this.boxColor_Click);
            // 
            // boxBlue
            // 
            this.boxBlue.BackColor = System.Drawing.Color.Blue;
            this.boxBlue.Location = new System.Drawing.Point(280, 15);
            this.boxBlue.Margin = new System.Windows.Forms.Padding(6);
            this.boxBlue.Name = "boxBlue";
            this.boxBlue.Size = new System.Drawing.Size(60, 60);
            this.boxBlue.TabIndex = 3;
            this.boxBlue.TabStop = false;
            this.boxBlue.Click += new System.EventHandler(this.boxColor_Click);
            // 
            // boxYellow
            // 
            this.boxYellow.BackColor = System.Drawing.Color.Gold;
            this.boxYellow.Location = new System.Drawing.Point(148, 15);
            this.boxYellow.Margin = new System.Windows.Forms.Padding(6);
            this.boxYellow.Name = "boxYellow";
            this.boxYellow.Size = new System.Drawing.Size(60, 60);
            this.boxYellow.TabIndex = 2;
            this.boxYellow.TabStop = false;
            this.boxYellow.Click += new System.EventHandler(this.boxColor_Click);
            // 
            // boxBlack
            // 
            this.boxBlack.BackColor = System.Drawing.Color.Black;
            this.boxBlack.Location = new System.Drawing.Point(412, 15);
            this.boxBlack.Margin = new System.Windows.Forms.Padding(6);
            this.boxBlack.Name = "boxBlack";
            this.boxBlack.Size = new System.Drawing.Size(60, 60);
            this.boxBlack.TabIndex = 1;
            this.boxBlack.TabStop = false;
            this.boxBlack.Click += new System.EventHandler(this.boxColor_Click);
            // 
            // boxOrange
            // 
            this.boxOrange.BackColor = System.Drawing.Color.DarkOrange;
            this.boxOrange.Location = new System.Drawing.Point(82, 15);
            this.boxOrange.Margin = new System.Windows.Forms.Padding(6);
            this.boxOrange.Name = "boxOrange";
            this.boxOrange.Size = new System.Drawing.Size(60, 60);
            this.boxOrange.TabIndex = 1;
            this.boxOrange.TabStop = false;
            this.boxOrange.Click += new System.EventHandler(this.boxColor_Click);
            // 
            // boxGreen
            // 
            this.boxGreen.BackColor = System.Drawing.Color.ForestGreen;
            this.boxGreen.Location = new System.Drawing.Point(214, 15);
            this.boxGreen.Margin = new System.Windows.Forms.Padding(6);
            this.boxGreen.Name = "boxGreen";
            this.boxGreen.Size = new System.Drawing.Size(60, 60);
            this.boxGreen.TabIndex = 1;
            this.boxGreen.TabStop = false;
            this.boxGreen.Click += new System.EventHandler(this.boxColor_Click);
            // 
            // boxRed
            // 
            this.boxRed.BackColor = System.Drawing.Color.Red;
            this.boxRed.Location = new System.Drawing.Point(16, 15);
            this.boxRed.Margin = new System.Windows.Forms.Padding(6);
            this.boxRed.Name = "boxRed";
            this.boxRed.Size = new System.Drawing.Size(60, 60);
            this.boxRed.TabIndex = 0;
            this.boxRed.TabStop = false;
            this.boxRed.Click += new System.EventHandler(this.boxColor_Click);
            // 
            // timerPictionary
            // 
            this.timerPictionary.Tick += new System.EventHandler(this.timerPictionary_Tick);
            // 
            // timerPictionaryPuntos
            // 
            this.timerPictionaryPuntos.Interval = 1000;
            this.timerPictionaryPuntos.Tick += new System.EventHandler(this.timerPictionaryPuntos_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.pictureBox1.Location = new System.Drawing.Point(1123, 25);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(344, 806);
            this.pictureBox1.TabIndex = 48;
            this.pictureBox1.TabStop = false;
            // 
            // AdvisoryBackground
            // 
            this.AdvisoryBackground.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.AdvisoryBackground.Location = new System.Drawing.Point(87, 199);
            this.AdvisoryBackground.Margin = new System.Windows.Forms.Padding(6);
            this.AdvisoryBackground.Name = "AdvisoryBackground";
            this.AdvisoryBackground.Size = new System.Drawing.Size(948, 369);
            this.AdvisoryBackground.TabIndex = 43;
            this.AdvisoryBackground.TabStop = false;
            // 
            // Pictionary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1547, 865);
            this.ControlBox = false;
            this.Controls.Add(this.PictionaryAdvisoryLbl);
            this.Controls.Add(this.Mensaje_Pictionary_btn);
            this.Controls.Add(this.AdvisoryBackground);
            this.Controls.Add(this.Respuesta_Pictionary);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pictionaryChat);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Pictionary";
            this.Text = "Pictionary";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clearButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxPurple)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxYellow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxBlack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxOrange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdvisoryBackground)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Mensaje_Pictionary_btn;
        private System.Windows.Forms.TextBox Respuesta_Pictionary;
        private System.Windows.Forms.TextBox pictionaryChat;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label j1PictionaryLbl;
        private System.Windows.Forms.Label j2PictionaryLbl;
        private System.Windows.Forms.Label puntosJ4PictLbl;
        private System.Windows.Forms.Label j3PictionaryLbl;
        private System.Windows.Forms.Label puntosJ3PictLbl;
        private System.Windows.Forms.Label j4PictionaryLbl;
        private System.Windows.Forms.Label puntosJ2PictLbl;
        private System.Windows.Forms.Label puntosJ1PictLbl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label timePictionaryLbl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label prompt_Lbl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox boxPurple;
        private System.Windows.Forms.PictureBox boxBlue;
        private System.Windows.Forms.PictureBox boxYellow;
        private System.Windows.Forms.PictureBox boxBlack;
        private System.Windows.Forms.PictureBox boxOrange;
        private System.Windows.Forms.PictureBox boxGreen;
        private System.Windows.Forms.PictureBox boxRed;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timerPictionary;
        private System.Windows.Forms.Timer timerPictionaryPuntos;
        private System.Windows.Forms.Label PictionaryAdvisoryLbl;
        private System.Windows.Forms.PictureBox AdvisoryBackground;
        private System.Windows.Forms.PictureBox clearButton;
    }
}