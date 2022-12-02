
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
            this.Seleccionados = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Invitar = new System.Windows.Forms.Button();
            this.ConectadosGrid = new System.Windows.Forms.DataGridView();
            this.partida_txt = new System.Windows.Forms.TextBox();
            this.Partida = new System.Windows.Forms.RadioButton();
            this.Mejor = new System.Windows.Forms.RadioButton();
            this.Victorias = new System.Windows.Forms.RadioButton();
            this.request_btn = new System.Windows.Forms.Button();
            this.nombre_txt = new System.Windows.Forms.TextBox();
            this.uiTest_btn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gmail_txt = new System.Windows.Forms.TextBox();
            this.register_btn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.password_txt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.username_txt = new System.Windows.Forms.TextBox();
            this.login_btn = new System.Windows.Forms.Button();
            this.desconectar_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.conectar_btn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listaConectados_group = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.consultarDatos_group = new System.Windows.Forms.GroupBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.EnviarMensaje = new System.Windows.Forms.Button();
            this.Mensaje = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Chat = new System.Windows.Forms.Label();
            this.pictureBox_levels = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.ConectadosGrid)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.listaConectados_group.SuspendLayout();
            this.consultarDatos_group.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_levels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // Seleccionados
            // 
            this.Seleccionados.AutoSize = true;
            this.Seleccionados.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.Seleccionados.Location = new System.Drawing.Point(337, 170);
            this.Seleccionados.Name = "Seleccionados";
            this.Seleccionados.Size = new System.Drawing.Size(0, 36);
            this.Seleccionados.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label6.Location = new System.Drawing.Point(323, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(329, 31);
            this.label6.TabIndex = 19;
            this.label6.Text = "Jugadores seleccionados:";
            // 
            // Invitar
            // 
            this.Invitar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.Invitar.Location = new System.Drawing.Point(329, 260);
            this.Invitar.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Invitar.Name = "Invitar";
            this.Invitar.Size = new System.Drawing.Size(190, 70);
            this.Invitar.TabIndex = 18;
            this.Invitar.Text = "Invitar";
            this.Invitar.UseVisualStyleBackColor = true;
            this.Invitar.Click += new System.EventHandler(this.Invitar_Click);
            // 
            // ConectadosGrid
            // 
            this.ConectadosGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ConectadosGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConectadosGrid.Location = new System.Drawing.Point(21, 39);
            this.ConectadosGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ConectadosGrid.Name = "ConectadosGrid";
            this.ConectadosGrid.RowHeadersVisible = false;
            this.ConectadosGrid.RowHeadersWidth = 82;
            this.ConectadosGrid.RowTemplate.Height = 33;
            this.ConectadosGrid.Size = new System.Drawing.Size(288, 291);
            this.ConectadosGrid.TabIndex = 15;
            this.ConectadosGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ConectadosGrid_CellClick);
            // 
            // partida_txt
            // 
            this.partida_txt.Location = new System.Drawing.Point(548, 193);
            this.partida_txt.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.partida_txt.Name = "partida_txt";
            this.partida_txt.Size = new System.Drawing.Size(52, 31);
            this.partida_txt.TabIndex = 12;
            this.partida_txt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.partida_txt_KeyPress);
            // 
            // Partida
            // 
            this.Partida.AutoSize = true;
            this.Partida.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Partida.Location = new System.Drawing.Point(21, 188);
            this.Partida.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Partida.Name = "Partida";
            this.Partida.Size = new System.Drawing.Size(490, 35);
            this.Partida.TabIndex = 8;
            this.Partida.TabStop = true;
            this.Partida.Text = "Jugadores que han jugado la partida:";
            this.Partida.UseVisualStyleBackColor = true;
            // 
            // Mejor
            // 
            this.Mejor.AutoSize = true;
            this.Mejor.BackColor = System.Drawing.Color.Silver;
            this.Mejor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mejor.Location = new System.Drawing.Point(21, 114);
            this.Mejor.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Mejor.Name = "Mejor";
            this.Mejor.Size = new System.Drawing.Size(217, 35);
            this.Mejor.TabIndex = 8;
            this.Mejor.TabStop = true;
            this.Mejor.Text = "Mejor Jugador";
            this.Mejor.UseVisualStyleBackColor = false;
            // 
            // Victorias
            // 
            this.Victorias.AutoSize = true;
            this.Victorias.BackColor = System.Drawing.Color.Silver;
            this.Victorias.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Victorias.Location = new System.Drawing.Point(21, 35);
            this.Victorias.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Victorias.Name = "Victorias";
            this.Victorias.Size = new System.Drawing.Size(368, 35);
            this.Victorias.TabIndex = 7;
            this.Victorias.TabStop = true;
            this.Victorias.Text = "Cuantas veces ha ganado:";
            this.Victorias.UseVisualStyleBackColor = false;
            // 
            // request_btn
            // 
            this.request_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.request_btn.Location = new System.Drawing.Point(21, 268);
            this.request_btn.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.request_btn.Name = "request_btn";
            this.request_btn.Size = new System.Drawing.Size(190, 70);
            this.request_btn.TabIndex = 5;
            this.request_btn.Text = "Consultar";
            this.request_btn.UseVisualStyleBackColor = true;
            this.request_btn.Click += new System.EventHandler(this.request_btn_Click);
            // 
            // nombre_txt
            // 
            this.nombre_txt.Location = new System.Drawing.Point(426, 39);
            this.nombre_txt.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.nombre_txt.Name = "nombre_txt";
            this.nombre_txt.Size = new System.Drawing.Size(201, 31);
            this.nombre_txt.TabIndex = 3;
            // 
            // uiTest_btn
            // 
            this.uiTest_btn.Location = new System.Drawing.Point(1391, 36);
            this.uiTest_btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uiTest_btn.Name = "uiTest_btn";
            this.uiTest_btn.Size = new System.Drawing.Size(171, 63);
            this.uiTest_btn.TabIndex = 21;
            this.uiTest_btn.Text = "Set Board";
            this.uiTest_btn.UseVisualStyleBackColor = true;
            this.uiTest_btn.Click += new System.EventHandler(this.uiTest_btn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 30);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1938, 1120);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.BackgroundImage = global::clienteJuegoSO.Properties.Resources.hero;
            this.tabPage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.desconectar_btn);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.conectar_btn);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Location = new System.Drawing.Point(8, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1922, 1074);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Welcome";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.White;
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.gmail_txt);
            this.groupBox3.Controls.Add(this.register_btn);
            this.groupBox3.Location = new System.Drawing.Point(559, 33);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(267, 194);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Register";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 38);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 31);
            this.label5.TabIndex = 16;
            this.label5.Text = "Email:";
            // 
            // gmail_txt
            // 
            this.gmail_txt.Location = new System.Drawing.Point(13, 73);
            this.gmail_txt.Margin = new System.Windows.Forms.Padding(4);
            this.gmail_txt.MaxLength = 100;
            this.gmail_txt.Name = "gmail_txt";
            this.gmail_txt.Size = new System.Drawing.Size(239, 31);
            this.gmail_txt.TabIndex = 17;
            // 
            // register_btn
            // 
            this.register_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.register_btn.Location = new System.Drawing.Point(17, 128);
            this.register_btn.Margin = new System.Windows.Forms.Padding(4);
            this.register_btn.Name = "register_btn";
            this.register_btn.Size = new System.Drawing.Size(235, 56);
            this.register_btn.TabIndex = 14;
            this.register_btn.Text = "REGISTER";
            this.register_btn.UseVisualStyleBackColor = true;
            this.register_btn.Click += new System.EventHandler(this.register_btn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.password_txt);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.username_txt);
            this.groupBox2.Controls.Add(this.login_btn);
            this.groupBox2.Location = new System.Drawing.Point(266, 33);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 313);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log In";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 31);
            this.label2.TabIndex = 10;
            this.label2.Text = "Username:";
            // 
            // password_txt
            // 
            this.password_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_txt.Location = new System.Drawing.Point(18, 176);
            this.password_txt.Margin = new System.Windows.Forms.Padding(4);
            this.password_txt.MaxLength = 30;
            this.password_txt.Name = "password_txt";
            this.password_txt.PasswordChar = '*';
            this.password_txt.Size = new System.Drawing.Size(216, 38);
            this.password_txt.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 141);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 31);
            this.label3.TabIndex = 11;
            this.label3.Text = "Password:";
            // 
            // username_txt
            // 
            this.username_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username_txt.ForeColor = System.Drawing.SystemColors.Desktop;
            this.username_txt.Location = new System.Drawing.Point(18, 76);
            this.username_txt.Margin = new System.Windows.Forms.Padding(4);
            this.username_txt.MaxLength = 30;
            this.username_txt.Name = "username_txt";
            this.username_txt.Size = new System.Drawing.Size(216, 38);
            this.username_txt.TabIndex = 8;
            // 
            // login_btn
            // 
            this.login_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_btn.Location = new System.Drawing.Point(18, 230);
            this.login_btn.Margin = new System.Windows.Forms.Padding(4);
            this.login_btn.Name = "login_btn";
            this.login_btn.Size = new System.Drawing.Size(215, 59);
            this.login_btn.TabIndex = 13;
            this.login_btn.Text = "LOG IN";
            this.login_btn.UseVisualStyleBackColor = true;
            this.login_btn.Click += new System.EventHandler(this.login_btn_Click);
            // 
            // desconectar_btn
            // 
            this.desconectar_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.desconectar_btn.Location = new System.Drawing.Point(26, 146);
            this.desconectar_btn.Margin = new System.Windows.Forms.Padding(4);
            this.desconectar_btn.Name = "desconectar_btn";
            this.desconectar_btn.Size = new System.Drawing.Size(220, 90);
            this.desconectar_btn.TabIndex = 19;
            this.desconectar_btn.Text = "Desconectar";
            this.desconectar_btn.UseVisualStyleBackColor = true;
            this.desconectar_btn.Click += new System.EventHandler(this.desconectar_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1387, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 51);
            this.label1.TabIndex = 9;
            // 
            // conectar_btn
            // 
            this.conectar_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conectar_btn.Location = new System.Drawing.Point(26, 33);
            this.conectar_btn.Margin = new System.Windows.Forms.Padding(4);
            this.conectar_btn.Name = "conectar_btn";
            this.conectar_btn.Size = new System.Drawing.Size(220, 90);
            this.conectar_btn.TabIndex = 22;
            this.conectar_btn.Text = "Conectar";
            this.conectar_btn.UseVisualStyleBackColor = true;
            this.conectar_btn.Click += new System.EventHandler(this.conectar_btn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1350, 84);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 31);
            this.label4.TabIndex = 15;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Silver;
            this.tabPage2.Controls.Add(this.listaConectados_group);
            this.tabPage2.Controls.Add(this.consultarDatos_group);
            this.tabPage2.Location = new System.Drawing.Point(8, 38);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1922, 1074);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Lounge";
            // 
            // listaConectados_group
            // 
            this.listaConectados_group.Controls.Add(this.label10);
            this.listaConectados_group.Controls.Add(this.ConectadosGrid);
            this.listaConectados_group.Controls.Add(this.Invitar);
            this.listaConectados_group.Controls.Add(this.label6);
            this.listaConectados_group.Controls.Add(this.Seleccionados);
            this.listaConectados_group.Location = new System.Drawing.Point(40, 455);
            this.listaConectados_group.Name = "listaConectados_group";
            this.listaConectados_group.Size = new System.Drawing.Size(708, 363);
            this.listaConectados_group.TabIndex = 24;
            this.listaConectados_group.TabStop = false;
            this.listaConectados_group.Text = "Usuarios Conectados";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label10.Location = new System.Drawing.Point(324, 39);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(330, 52);
            this.label10.TabIndex = 21;
            this.label10.Text = "Para invitar a un jugador, \r\nselecciona su nombre en la tabla";
            // 
            // consultarDatos_group
            // 
            this.consultarDatos_group.Controls.Add(this.Victorias);
            this.consultarDatos_group.Controls.Add(this.Partida);
            this.consultarDatos_group.Controls.Add(this.partida_txt);
            this.consultarDatos_group.Controls.Add(this.Mejor);
            this.consultarDatos_group.Controls.Add(this.request_btn);
            this.consultarDatos_group.Controls.Add(this.nombre_txt);
            this.consultarDatos_group.Location = new System.Drawing.Point(40, 54);
            this.consultarDatos_group.Name = "consultarDatos_group";
            this.consultarDatos_group.Size = new System.Drawing.Size(708, 379);
            this.consultarDatos_group.TabIndex = 23;
            this.consultarDatos_group.TabStop = false;
            this.consultarDatos_group.Text = "Consultar Datos";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.EnviarMensaje);
            this.tabPage3.Controls.Add(this.Mensaje);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.Chat);
            this.tabPage3.Controls.Add(this.pictureBox_levels);
            this.tabPage3.Controls.Add(this.uiTest_btn);
            this.tabPage3.Controls.Add(this.tableLayoutPanel6);
            this.tabPage3.Controls.Add(this.tableLayoutPanel7);
            this.tabPage3.Controls.Add(this.tableLayoutPanel5);
            this.tabPage3.Controls.Add(this.tableLayoutPanel4);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.tableLayoutPanel3);
            this.tabPage3.Controls.Add(this.tableLayoutPanel2);
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Controls.Add(this.trackBar1);
            this.tabPage3.Location = new System.Drawing.Point(8, 38);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1922, 1074);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Board";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // EnviarMensaje
            // 
            this.EnviarMensaje.Location = new System.Drawing.Point(1651, 743);
            this.EnviarMensaje.Name = "EnviarMensaje";
            this.EnviarMensaje.Size = new System.Drawing.Size(198, 53);
            this.EnviarMensaje.TabIndex = 26;
            this.EnviarMensaje.Text = "Enviar Mensaje";
            this.EnviarMensaje.UseVisualStyleBackColor = true;
            this.EnviarMensaje.Click += new System.EventHandler(this.EnviarMensaje_Click);
            // 
            // Mensaje
            // 
            this.Mensaje.Location = new System.Drawing.Point(1592, 695);
            this.Mensaje.Name = "Mensaje";
            this.Mensaje.Size = new System.Drawing.Size(301, 31);
            this.Mensaje.TabIndex = 25;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1391, 770);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(171, 66);
            this.button1.TabIndex = 22;
            this.button1.Text = "Add Player";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Chat
            // 
            this.Chat.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Chat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Chat.Location = new System.Drawing.Point(1590, 36);
            this.Chat.Name = "Chat";
            this.Chat.Size = new System.Drawing.Size(303, 615);
            this.Chat.TabIndex = 24;
            // 
            // pictureBox_levels
            // 
            this.pictureBox_levels.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_levels.Image = global::clienteJuegoSO.Properties.Resources.levels;
            this.pictureBox_levels.ImageLocation = "";
            this.pictureBox_levels.InitialImage = null;
            this.pictureBox_levels.Location = new System.Drawing.Point(1391, 162);
            this.pictureBox_levels.Name = "pictureBox_levels";
            this.pictureBox_levels.Size = new System.Drawing.Size(171, 528);
            this.pictureBox_levels.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_levels.TabIndex = 21;
            this.pictureBox_levels.TabStop = false;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Location = new System.Drawing.Point(42, 36);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(150, 150);
            this.tableLayoutPanel6.TabIndex = 19;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Location = new System.Drawing.Point(1134, 36);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(150, 150);
            this.tableLayoutPanel7.TabIndex = 18;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Location = new System.Drawing.Point(42, 686);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(150, 150);
            this.tableLayoutPanel5.TabIndex = 17;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Location = new System.Drawing.Point(1134, 686);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(150, 150);
            this.tableLayoutPanel4.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(1085, 253);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(250, 25);
            this.label8.TabIndex = 15;
            this.label8.Text = "Descartes de Inundación";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(25, 253);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(212, 25);
            this.label9.TabIndex = 14;
            this.label9.Text = "Descartes de Tesoro";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.5F));
            this.tableLayoutPanel3.Location = new System.Drawing.Point(1128, 298);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(156, 268);
            this.tableLayoutPanel3.TabIndex = 13;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.5F));
            this.tableLayoutPanel2.ForeColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(42, 298);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(156, 268);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(262, 36);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 800);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(1341, 181);
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(90, 446);
            this.trackBar1.TabIndex = 20;
            this.trackBar1.Value = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1974, 1129);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ConectadosGrid)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.listaConectados_group.ResumeLayout(false);
            this.listaConectados_group.PerformLayout();
            this.consultarDatos_group.ResumeLayout(false);
            this.consultarDatos_group.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_levels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.RadioButton Partida;
        private System.Windows.Forms.RadioButton Mejor;
        private System.Windows.Forms.RadioButton Victorias;
        private System.Windows.Forms.Button request_btn;
        private System.Windows.Forms.TextBox nombre_txt;
        private System.Windows.Forms.TextBox partida_txt;
        private System.Windows.Forms.DataGridView ConectadosGrid;
        private System.Windows.Forms.Button uiTest_btn;
        private System.Windows.Forms.Button conectar_btn;
        private System.Windows.Forms.Label Seleccionados;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Invitar;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox_levels;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label Chat;
        private System.Windows.Forms.TextBox Mensaje;
        private System.Windows.Forms.Button EnviarMensaje;
        private System.Windows.Forms.GroupBox listaConectados_group;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox consultarDatos_group;
    }
}

