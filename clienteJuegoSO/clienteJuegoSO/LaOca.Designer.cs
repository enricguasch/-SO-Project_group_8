
namespace clienteJuegoSO
{
    partial class LaOca
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LaOca));
            this.Finalizar = new System.Windows.Forms.Button();
            this.label_turno = new System.Windows.Forms.Label();
            this.Lanzar_dado = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label_dice = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numero = new System.Windows.Forms.Label();
            this.dice = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numeroDado = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dice)).BeginInit();
            this.SuspendLayout();
            // 
            // Finalizar
            // 
            this.Finalizar.Location = new System.Drawing.Point(21, 141);
            this.Finalizar.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Finalizar.Name = "Finalizar";
            this.Finalizar.Size = new System.Drawing.Size(160, 87);
            this.Finalizar.TabIndex = 6;
            this.Finalizar.Text = "Finalizar Partida";
            this.Finalizar.UseVisualStyleBackColor = true;
            this.Finalizar.Click += new System.EventHandler(this.Finalizar_Click);
            // 
            // label_turno
            // 
            this.label_turno.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.label_turno.Location = new System.Drawing.Point(1408, 63);
            this.label_turno.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_turno.Name = "label_turno";
            this.label_turno.Size = new System.Drawing.Size(349, 119);
            this.label_turno.TabIndex = 5;
            this.label_turno.Text = "Turno del jugador:";
            // 
            // Lanzar_dado
            // 
            this.Lanzar_dado.Location = new System.Drawing.Point(21, 46);
            this.Lanzar_dado.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Lanzar_dado.Name = "Lanzar_dado";
            this.Lanzar_dado.Size = new System.Drawing.Size(160, 69);
            this.Lanzar_dado.TabIndex = 4;
            this.Lanzar_dado.Text = "Lanzar Dado";
            this.Lanzar_dado.UseVisualStyleBackColor = true;
            this.Lanzar_dado.Click += new System.EventHandler(this.Lanzar_dado_Click);
            // 
            // label_dice
            // 
            this.label_dice.AutoSize = true;
            this.label_dice.Location = new System.Drawing.Point(1299, 53);
            this.label_dice.Name = "label_dice";
            this.label_dice.Size = new System.Drawing.Size(0, 25);
            this.label_dice.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.groupBox1.Controls.Add(this.numero);
            this.groupBox1.Controls.Add(this.Finalizar);
            this.groupBox1.Controls.Add(this.Lanzar_dado);
            this.groupBox1.Location = new System.Drawing.Point(1492, 408);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 261);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Acciones";
            // 
            // numero
            // 
            this.numero.AutoSize = true;
            this.numero.Location = new System.Drawing.Point(30, 230);
            this.numero.Name = "numero";
            this.numero.Size = new System.Drawing.Size(0, 25);
            this.numero.TabIndex = 8;
            // 
            // dice
            // 
            this.dice.Location = new System.Drawing.Point(1527, 185);
            this.dice.Name = "dice";
            this.dice.Size = new System.Drawing.Size(136, 138);
            this.dice.TabIndex = 11;
            this.dice.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1501, 348);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 25);
            this.label1.TabIndex = 12;
            // 
            // numeroDado
            // 
            this.numeroDado.AutoSize = true;
            this.numeroDado.Location = new System.Drawing.Point(1528, 337);
            this.numeroDado.Name = "numeroDado";
            this.numeroDado.Size = new System.Drawing.Size(135, 25);
            this.numeroDado.TabIndex = 13;
            this.numeroDado.Text = "numeroDado";
            // 
            // LaOca
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1812, 1038);
            this.ControlBox = false;
            this.Controls.Add(this.numeroDado);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dice);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label_dice);
            this.Controls.Add(this.label_turno);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LaOca";
            this.Text = "La Oca";
            this.Load += new System.EventHandler(this.Form4_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Finalizar;
        private System.Windows.Forms.Label label_turno;
        private System.Windows.Forms.Button Lanzar_dado;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label_dice;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label numero;
        private System.Windows.Forms.PictureBox dice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label numeroDado;
    }
}