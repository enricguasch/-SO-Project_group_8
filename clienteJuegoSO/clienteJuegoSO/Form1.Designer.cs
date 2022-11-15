
namespace clienteJuegoSO
{
    partial class Form1
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
            this.label4 = new System.Windows.Forms.Label();
            this.register_btn = new System.Windows.Forms.Button();
            this.login_btn = new System.Windows.Forms.Button();
            this.password_txt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.username_txt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gmail_txt = new System.Windows.Forms.TextBox();
            this.desconectar_btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ConectadosGrid = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.partida_txt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Partida = new System.Windows.Forms.RadioButton();
            this.Mejor = new System.Windows.Forms.RadioButton();
            this.Victorias = new System.Windows.Forms.RadioButton();
            this.request_btn = new System.Windows.Forms.Button();
            this.nombre_txt = new System.Windows.Forms.TextBox();
            this.uiTest_btn = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConectadosGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(125, 358);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(563, 31);
            this.label4.TabIndex = 15;
            this.label4.Text = "- No tienes cuenta? Rellena los 3 campos.";
            // 
            // register_btn
            // 
            this.register_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.register_btn.Location = new System.Drawing.Point(847, 358);
            this.register_btn.Margin = new System.Windows.Forms.Padding(4);
            this.register_btn.Name = "register_btn";
            this.register_btn.Size = new System.Drawing.Size(267, 109);
            this.register_btn.TabIndex = 14;
            this.register_btn.Text = "REGISTER";
            this.register_btn.UseVisualStyleBackColor = true;
            this.register_btn.Click += new System.EventHandler(this.register_btn_Click);
            // 
            // login_btn
            // 
            this.login_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_btn.Location = new System.Drawing.Point(847, 202);
            this.login_btn.Margin = new System.Windows.Forms.Padding(4);
            this.login_btn.Name = "login_btn";
            this.login_btn.Size = new System.Drawing.Size(267, 105);
            this.login_btn.TabIndex = 13;
            this.login_btn.Text = "LOG IN";
            this.login_btn.UseVisualStyleBackColor = true;
            this.login_btn.Click += new System.EventHandler(this.login_btn_Click);
            // 
            // password_txt
            // 
            this.password_txt.Location = new System.Drawing.Point(416, 286);
            this.password_txt.Margin = new System.Windows.Forms.Padding(4);
            this.password_txt.Name = "password_txt";
            this.password_txt.Size = new System.Drawing.Size(216, 31);
            this.password_txt.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(233, 288);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 31);
            this.label3.TabIndex = 11;
            this.label3.Text = "Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(233, 202);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 31);
            this.label2.TabIndex = 10;
            this.label2.Text = "Username:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(179, 105);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 51);
            this.label1.TabIndex = 9;
            this.label1.Text = "LOG IN:";
            // 
            // username_txt
            // 
            this.username_txt.Location = new System.Drawing.Point(416, 202);
            this.username_txt.Margin = new System.Windows.Forms.Padding(4);
            this.username_txt.Name = "username_txt";
            this.username_txt.Size = new System.Drawing.Size(216, 31);
            this.username_txt.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(233, 435);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 31);
            this.label5.TabIndex = 16;
            this.label5.Text = "Gmail:";
            // 
            // gmail_txt
            // 
            this.gmail_txt.Location = new System.Drawing.Point(416, 434);
            this.gmail_txt.Margin = new System.Windows.Forms.Padding(4);
            this.gmail_txt.Name = "gmail_txt";
            this.gmail_txt.Size = new System.Drawing.Size(216, 31);
            this.gmail_txt.TabIndex = 17;
            // 
            // desconectar_btn
            // 
            this.desconectar_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.desconectar_btn.Location = new System.Drawing.Point(16, 612);
            this.desconectar_btn.Margin = new System.Windows.Forms.Padding(4);
            this.desconectar_btn.Name = "desconectar_btn";
            this.desconectar_btn.Size = new System.Drawing.Size(213, 71);
            this.desconectar_btn.TabIndex = 19;
            this.desconectar_btn.Text = "Desconectar";
            this.desconectar_btn.UseVisualStyleBackColor = true;
            this.desconectar_btn.Click += new System.EventHandler(this.desconectar_btn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.ConectadosGrid);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.partida_txt);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.Partida);
            this.groupBox1.Controls.Add(this.Mejor);
            this.groupBox1.Controls.Add(this.Victorias);
            this.groupBox1.Controls.Add(this.request_btn);
            this.groupBox1.Controls.Add(this.nombre_txt);
            this.groupBox1.Location = new System.Drawing.Point(240, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox1.Size = new System.Drawing.Size(1039, 664);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Peticion";
            // 
            // ConectadosGrid
            // 
            this.ConectadosGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConectadosGrid.Location = new System.Drawing.Point(511, 318);
            this.ConectadosGrid.Name = "ConectadosGrid";
            this.ConectadosGrid.RowHeadersVisible = false;
            this.ConectadosGrid.RowHeadersWidth = 82;
            this.ConectadosGrid.RowTemplate.Height = 33;
            this.ConectadosGrid.Size = new System.Drawing.Size(451, 227);
            this.ConectadosGrid.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(765, 222);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(167, 26);
            this.label7.TabIndex = 13;
            this.label7.Text = "(numero entero)";
            // 
            // partida_txt
            // 
            this.partida_txt.Location = new System.Drawing.Point(743, 254);
            this.partida_txt.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.partida_txt.Name = "partida_txt";
            this.partida_txt.Size = new System.Drawing.Size(201, 31);
            this.partida_txt.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(584, 70);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 26);
            this.label6.TabIndex = 11;
            this.label6.Text = "(minúsculas)";
            // 
            // Partida
            // 
            this.Partida.AutoSize = true;
            this.Partida.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Partida.Location = new System.Drawing.Point(73, 254);
            this.Partida.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Partida.Name = "Partida";
            this.Partida.Size = new System.Drawing.Size(594, 35);
            this.Partida.TabIndex = 8;
            this.Partida.TabStop = true;
            this.Partida.Text = "Dime los jugadores que han jugado la partida:";
            this.Partida.UseVisualStyleBackColor = true;
            // 
            // Mejor
            // 
            this.Mejor.AutoSize = true;
            this.Mejor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mejor.Location = new System.Drawing.Point(73, 180);
            this.Mejor.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Mejor.Name = "Mejor";
            this.Mejor.Size = new System.Drawing.Size(316, 35);
            this.Mejor.TabIndex = 8;
            this.Mejor.TabStop = true;
            this.Mejor.Text = "Dame el mejor jugador";
            this.Mejor.UseVisualStyleBackColor = true;
            // 
            // Victorias
            // 
            this.Victorias.AutoSize = true;
            this.Victorias.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Victorias.Location = new System.Drawing.Point(73, 101);
            this.Victorias.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Victorias.Name = "Victorias";
            this.Victorias.Size = new System.Drawing.Size(432, 35);
            this.Victorias.TabIndex = 7;
            this.Victorias.TabStop = true;
            this.Victorias.Text = "Dime cuantas veces ha ganado:";
            this.Victorias.UseVisualStyleBackColor = true;
            // 
            // request_btn
            // 
            this.request_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.request_btn.Location = new System.Drawing.Point(391, 554);
            this.request_btn.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.request_btn.Name = "request_btn";
            this.request_btn.Size = new System.Drawing.Size(220, 98);
            this.request_btn.TabIndex = 5;
            this.request_btn.Text = "ENVIAR";
            this.request_btn.UseVisualStyleBackColor = true;
            this.request_btn.Click += new System.EventHandler(this.request_btn_Click);
            // 
            // nombre_txt
            // 
            this.nombre_txt.Location = new System.Drawing.Point(545, 101);
            this.nombre_txt.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.nombre_txt.Name = "nombre_txt";
            this.nombre_txt.Size = new System.Drawing.Size(201, 31);
            this.nombre_txt.TabIndex = 3;
            // 
            // uiTest_btn
            // 
            this.uiTest_btn.Location = new System.Drawing.Point(16, 554);
            this.uiTest_btn.Name = "uiTest_btn";
            this.uiTest_btn.Size = new System.Drawing.Size(213, 51);
            this.uiTest_btn.TabIndex = 21;
            this.uiTest_btn.Text = "UI Test";
            this.uiTest_btn.UseVisualStyleBackColor = true;
            this.uiTest_btn.Click += new System.EventHandler(this.uiTest_btn_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(103, 318);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(377, 37);
            this.label8.TabIndex = 16;
            this.label8.Text = "Jugadores Conectados:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1296, 699);
            this.Controls.Add(this.uiTest_btn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.desconectar_btn);
            this.Controls.Add(this.gmail_txt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.register_btn);
            this.Controls.Add(this.login_btn);
            this.Controls.Add(this.password_txt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.username_txt);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConectadosGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button register_btn;
        private System.Windows.Forms.Button login_btn;
        private System.Windows.Forms.TextBox password_txt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox username_txt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox gmail_txt;
        private System.Windows.Forms.Button desconectar_btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton Partida;
        private System.Windows.Forms.RadioButton Mejor;
        private System.Windows.Forms.RadioButton Victorias;
        private System.Windows.Forms.Button request_btn;
        private System.Windows.Forms.TextBox nombre_txt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox partida_txt;
        private System.Windows.Forms.DataGridView ConectadosGrid;
        private System.Windows.Forms.Button uiTest_btn;
        private System.Windows.Forms.Label label8;
    }
}

