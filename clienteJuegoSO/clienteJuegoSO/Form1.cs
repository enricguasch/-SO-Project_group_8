using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace clienteJuegoSO
{
    public partial class Form1 : Form
    {
        Socket server;
        public Form1()
        {
            InitializeComponent();
        }

        private Socket ConectaServidor(Socket server)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9220);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                return server;
            }
            catch(SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return server;
            }
        }

        private string ConsultarServidor(string mensaje)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            return mensaje;
        }

        private void esconderPantalla2()
        {
            Victorias.Hide();
            Mejor.Hide();
            Partida.Hide();
            nombre_txt.Hide();
            partida_txt.Hide();
            label6.Hide();
            label7.Hide();
            request_btn.Hide();
            groupBox1.Hide();
            desconectar_btn.Hide();
            Conectados.Hide();
        }
        private void mostrarPantalla2()
        {
            Victorias.Show();
            Mejor.Show();
            Partida.Show();
            nombre_txt.Show();
            partida_txt.Show();
            label6.Show();
            label7.Show();
            request_btn.Show();
            groupBox1.Show();
            desconectar_btn.Show();
            Conectados.Show();
            ConectadosGrid.Hide();
        }
        private void esconderPantalla1()
        {
            login_btn.Hide();
            register_btn.Hide();
            username_txt.Hide();
            password_txt.Hide();
            gmail_txt.Hide();
            label1.Hide();
            label2.Hide();
            label3.Hide();
            label4.Hide();
            label5.Hide();
        }

        private void mostrarPantalla1()
        {
            login_btn.Show();
            register_btn.Show();
            username_txt.Show();
            password_txt.Show();
            gmail_txt.Show();
            label1.Show();
            label2.Show();
            label3.Show();
            label4.Show();
            label5.Show();
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (server is null)
            {
                MessageBox.Show("Recuerda conectarte al servior!");
            }
            else if (string.IsNullOrEmpty(username_txt.Text) || string.IsNullOrEmpty(password_txt.Text))
            {
                MessageBox.Show("Porfavor rellene los campos de usuario y contraseña");
            }
            else
            {
                string mensaje = "1/" + username_txt.Text.ToLower() + "/" + password_txt.Text;

                // Enviamos al servidor el usuario y contraseña tecleados
                string respuesta = ConsultarServidor(mensaje)
                    ;
                //Si el usuario existe y la contraseña es correcta lo dejamos entrar al juego
                if (Convert.ToInt32(respuesta) == 0)
                {
                    MessageBox.Show("Bienvenido de nuevo " + username_txt.Text);
                    esconderPantalla1();
                    mostrarPantalla2();
                }
                //Si no lo son le pedimos que lo vuelva a intentar
                else if (Convert.ToInt32(respuesta) == 1)
                {
                    MessageBox.Show("Usuario o contraseña equivocados.");
                    username_txt.Clear();
                    password_txt.Clear();
                }
                else if (Convert.ToInt32(respuesta) == 2)
                {
                    MessageBox.Show("Usuario ya conectado.");
                    username_txt.Clear();
                    password_txt.Clear();
                }
                else
                {
                    MessageBox.Show("Error al consultar la base de datos.");
                }
            }
        }

        private void register_btn_Click(object sender, EventArgs e)
        {
            if (server is null)
            {
                MessageBox.Show("Recuerda conectarte al servior!");
            }
            else if (string.IsNullOrEmpty(username_txt.Text) || string.IsNullOrEmpty(password_txt.Text) || 
                string.IsNullOrEmpty(gmail_txt.Text))
            {
                MessageBox.Show("Porfavor rellene los campos de usuario y contraseña");
            }
            else
            {
                string mensaje = "2/" + username_txt.Text.ToLower() + "/" + password_txt.Text + "/" + gmail_txt.Text.ToLower();

                // Enviamos al servidor el usuario y contraseña tecleados
                string respuesta = ConsultarServidor(mensaje);

                //Si el usuario se ha creado correctamente le dejamos entrar al juego
                if (Convert.ToInt32(respuesta) == 0)
                {
                    MessageBox.Show("Gracias por registrarte " + username_txt.Text);
                    esconderPantalla1();
                    mostrarPantalla2();
                }
                else if (Convert.ToInt32(respuesta) == 2)
                {
                    MessageBox.Show("Usuario ya conectado.");
                    username_txt.Clear();
                    password_txt.Clear();
                }
                //Si no lo son le pedimos que lo vuelva a intentar
                else
                {
                    MessageBox.Show("Error en el proceso de registro.");
                    username_txt.Clear();
                    password_txt.Clear();
                    gmail_txt.Clear();
                }
            }
        }

        private void desconectar_btn_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Nos desconectamos
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            esconderPantalla2();
            username_txt.Clear();
            password_txt.Clear();
            mostrarPantalla1();
            server = ConectaServidor(server);
        }

        private void request_btn_Click(object sender, EventArgs e)
        {
            if (Victorias.Checked)
            {
                if(string.IsNullOrEmpty(nombre_txt.Text))
                {
                    MessageBox.Show("Se necesita un nombre para esta consulta.");
                }
                else
                {
                    // Quiere saber cuantas partidas ha ganado el jugador
                    string mensaje = "3/" + nombre_txt.Text.ToLower();

                    // Enviamos al servidor el nombre tecleado
                    string respuesta = ConsultarServidor(mensaje);
                    MessageBox.Show(respuesta);
                }
            }
            if (Mejor.Checked)
            {
                // Quiere saber el mejor jugador
                string mensaje = "4/";
            
                string respuesta = ConsultarServidor(mensaje);

                MessageBox.Show(respuesta);
            }
            if(Partida.Checked)
            {
                if(string.IsNullOrEmpty(partida_txt.Text))
                {
                    MessageBox.Show("Se necesita un numero entero para esta consulta.");
                }
                else
                {
                    // Quiere saber los jugadores de la partida
                    string mensaje = "5/" + partida_txt.Text;

                    // Enviamos al servidor el numero de partida
                    string respuesta = ConsultarServidor(mensaje);

                    MessageBox.Show(respuesta);
                }
            }
            if(Conectados.Checked)
            {

                // Quiere saber los jugadores de la partida
                string mensaje = "6/";

                // Enviamos al servidor el numero de partida
                string respuesta = ConsultarServidor(mensaje);
                string[] respuestaTokens = respuesta.Split(new char[1] { '/' });
                int n = Convert.ToInt32(respuestaTokens[0]);

                ConectadosGrid.Show();
                ConectadosGrid.Rows.Clear();
                ConectadosGrid.Columns.Clear();
                ConectadosGrid.ColumnCount = 2;
                ConectadosGrid.RowCount = n;
                ConectadosGrid.Columns[0].HeaderText = "Jugador";
                ConectadosGrid.Columns[1].HeaderText = "Socket";

                int j = 0;
                int i = 1;
                while(i<=n)
                {
                    ConectadosGrid.Rows[j].Cells[0].Value = respuestaTokens[i];
                    i = i + 1;
                    j = j + 1;
                }
                j = 0;
                while(i<=(2*n))
                {
                    ConectadosGrid.Rows[j].Cells[1].Value = respuestaTokens[i];
                    i = i + 1;
                    j = j + 1;
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            esconderPantalla2();
            server = ConectaServidor(server);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Nos desconectamos
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        private void uiTest_btn_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }
    }
}
