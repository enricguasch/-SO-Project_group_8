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
using System.Threading;

namespace clienteJuegoSO
{
    public partial class Form1 : Form
    {
        Socket server;
        Thread atender;
        string nombre;
        string usuario;
        string jugador2 = "";
        string jugador3 = "";
        string jugador4 = "";
        int numJugadores = 1;
        int numInvitados = 0;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; //Necesario para que los elementos de los formularios puedan ser
            //accedidos desde threads diferentes a los que los crearon (ARREGLO TEMPORAL)
        }

        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string mensaje_limpio = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                string[] trozos = mensaje_limpio.Split('*');

                int codigo = Convert.ToInt32(trozos[0]);
                string respuesta = trozos[1].Split('\0')[0];
                //MessageBox.Show(respuesta);
                //MessageBox.Show("OK\n");

                switch (codigo)
                {
                    //case 0:
                    //    if (Convert.ToInt32(respuesta)==0)
                    //    {
                    //        //Nos desconectamos
                    //        this.BackColor = Color.Gray;
                    //        server.Shutdown(SocketShutdown.Both);
                    //        server.Close();
                    //        esconderPantalla2();
                    //        username_txt.Clear();
                    //        password_txt.Clear();
                    //        gmail_txt.Clear();
                    //        mostrarPantalla1();
                    //        atender.Abort();
                    //        server = ConectaServidor(server);
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("No se ha podido realizar la desconexión.");
                    //    }
                    //    break;
                    case 1:  // respuesta a LogIN
                             //Si el usuario existe y la contraseña es correcta lo dejamos entrar al juego
                        if (Convert.ToInt32(respuesta) == 0)
                        {
                            MessageBox.Show("Bienvenido de nuevo " + username_txt.Text);
                            nombre = username_txt.Text.ToLower();
                            esconderPantalla1();
                            mostrarPantalla2();
                        }
                        //Si no lo son le pedimos que lo vuelva a intentar
                        else if (Convert.ToInt32(respuesta) == -1)
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
                        break;
                    case 2:      //respuesta a si mi nombre es bonito
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
                            gmail_txt.Clear();
                        }
                        //Si no lo son le pedimos que lo vuelva a intentar
                        else
                        {
                            MessageBox.Show("Error en el proceso de registro.");
                            username_txt.Clear();
                            password_txt.Clear();
                            gmail_txt.Clear();
                        }
                        break;
                    case 3:       //Recibimos la respuesta de partidas ganadas

                        MessageBox.Show(respuesta);
                        break;
                    case 4:     //Recibimos la respuesta de mejor jugador

                        MessageBox.Show(respuesta);
                        break;
                    case 5:     //Recibimos la respuesta de jugadores en partida
                        MessageBox.Show(respuesta);
                        break;
                    case 6:
                        string[] respuestaTokens = respuesta.Split('/');
                        int n = Convert.ToInt32(respuestaTokens[0]);

                        ConectadosGrid.Rows.Clear();
                        ConectadosGrid.ColumnHeadersVisible = false;
                        ConectadosGrid.Rows[0].Cells[0].Value = respuestaTokens[1];

                        int i = 2;
                        while (i <= n)
                        {
                            ConectadosGrid.Rows.Add(respuestaTokens[i]);
                            i++;
                        }
                        //j = 0;
                        //while (i <= (2 * n))
                        //{
                        //    ConectadosGrid.Rows.Add(respuestaTokens[i]);
                        //    i++;
                        //    j++;
                        //}
                        ConectadosGrid.ClearSelection();
                        break;
                    case 8:
                        string[] info = respuesta.Split('/');
                        DialogResult dr = MessageBox.Show("El Jugador: " + info[0] + " te ha invitado a jugar la partida: " + info[1] + "\n" + "Quieres aceptar la invitacion?", "Invitacion", MessageBoxButtons.YesNo);
                        switch (dr)
                        {
                            case DialogResult.Yes:
                                string mensaje = "9/" + usuario + "/SI/" + info[1];
                                ConsultarServidor(mensaje);
                                break;
                            case DialogResult.No:
                                string mensaje2 = "9/" + usuario + "/NO/" + info[1];
                                ConsultarServidor(mensaje2);
                                break;
                        }
                        break;
                    case 10:
                        string[] info2 = respuesta.Split('/');
                        numJugadores++;
                        if (info2[1] == "SI")
                        {
                            MessageBox.Show("El jugador: " + info2[0] + " ha aceptado tu invitacion para jugar la partida " + info2[2]);
                            MessageBox.Show("NumJugadores = " + numJugadores + "     NumInvitados = " + numInvitados);
                            if (numJugadores - 1 == numInvitados)
                            {
                                string mensaje3 = "11/1/" + info2[2];
                                ConsultarServidor(mensaje3);
                                uiTest_btn.PerformClick();
                            }
                        }
                        else
                            MessageBox.Show("El jugador: " + info2[0] + " no ha aceptado tu invitacion para jugar la partida " + info2[2] + "\nNo se puede jugar la partida. Vuelve a intentarlo");
                        break;
                    case 12:
                        MessageBox.Show("Empieza la partida " + respuesta + "!!!!!");
                        uiTest_btn.PerformClick();
                        break;
                }
            }
        }

        private void ConectaServidor()
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9060);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                //pongo en marcha el thread que atenderá los mensajes del servidor
                ThreadStart ts = delegate { AtenderServidor(); };
                atender = new Thread(ts);
                atender.Start();
            }
            catch(SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
            }
        }

        private void ConsultarServidor(string mensaje)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private string[] RespuestaServidor()
        {
            //Recibimos mensaje del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            string[] trozos = Encoding.ASCII.GetString(msg2).Split('*');
            return trozos;
        }

        private void esconderPantalla2()
        {
            Victorias.Hide();
            Mejor.Hide();
            Partida.Hide();
            nombre_txt.Hide();
            partida_txt.Hide();
            label7.Hide();
            request_btn.Hide();
            groupBox1.Hide();
            desconectar_btn.Hide();
            ConectadosGrid.Hide();
        }
        private void mostrarPantalla2()
        {
            Victorias.Show();
            Mejor.Show();
            Partida.Show();
            nombre_txt.Show();
            partida_txt.Show();
            label7.Show();
            request_btn.Show();
            groupBox1.Show();
            desconectar_btn.Show();
            ConectadosGrid.Show();
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
            conectar_btn.Hide();
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
            conectar_btn.Show();
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (!server.Connected)
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
                usuario = username_txt.Text.ToLower();
                // Enviamos al servidor el usuario y contraseña tecleados
    
                ConsultarServidor(mensaje);
            }
        }

        private void register_btn_Click(object sender, EventArgs e)
        {
            if (!server.Connected)
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
                usuario = username_txt.Text.ToLower();
                // Enviamos al servidor el usuario y contraseña tecleados

                ConsultarServidor(mensaje);

                
            }
        }

        private void desconectar_btn_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/" + nombre;
            ConsultarServidor(mensaje);
            //Nos desconectamos
            atender.Abort();
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            esconderPantalla2();
            username_txt.Clear();
            password_txt.Clear();
            gmail_txt.Clear();
            mostrarPantalla1();
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
                    ConsultarServidor(mensaje);
                }
            }
            if (Mejor.Checked)
            {
                // Quiere saber el mejor jugador
                string mensaje = "4/";
                ConsultarServidor(mensaje);
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
                    ConsultarServidor(mensaje);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            esconderPantalla2();
            ConectaServidor();
            ConectadosGrid.ColumnCount = 1;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/" + nombre;
            ConsultarServidor(mensaje);
            //Nos desconectamos
            atender.Abort();
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        private void uiTest_btn_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        private void conectar_btn_Click(object sender, EventArgs e)
        {
            ConectaServidor();
        }

        private void ConectadosGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string nombre = ConectadosGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (nombre == usuario)
                MessageBox.Show("No puedes invitarte a tu mismo\n");
            else
            {
                MessageBox.Show("NumInvitados = " + numInvitados + "Nombre = " + nombre);
                if (nombre == jugador2)
                {
                    jugador2 = "";
                    Seleccionados.Text = jugador3 + "  &  " + jugador4;
                    numInvitados--;
                }
                else if (nombre == jugador3)
                {
                    jugador3 = "";
                    Seleccionados.Text = jugador2 + "  &  " + jugador4;
                    numInvitados--;
                }
                else if (nombre == jugador4)
                {
                    jugador4 = "";
                    Seleccionados.Text = jugador2 + "  &  " + jugador3;
                    numInvitados--;
                }
                else
                {
                    if (numInvitados == 0)
                    {
                        jugador2 = nombre;
                        Seleccionados.Text = jugador2;
                        numInvitados++;
                    }
                    else if (numInvitados == 1)
                    {
                        jugador3 = nombre;
                        Seleccionados.Text = jugador2 + "  &  " + jugador3;
                        numInvitados++;
                    }
                    else if (numInvitados == 2)
                    {
                        jugador4 = nombre;
                        Seleccionados.Text = jugador2 + "  &  " + jugador3 + "  &  " + jugador4;
                        numInvitados++;
                    }
                }
            }
            //else
            //{
            //    numInvitados++;
            //    invitacion = invitacion + "/" + nombre;
            //    peticion_in = Convert.ToString(numInvitados) + invitacion;

            //    Seleccionados.Text = invitacion;
            //}

        }

        string inv;
        private void Invitar_Click(object sender, EventArgs e)
        {
            numJugadores = 1;
            if (string.IsNullOrEmpty(Seleccionados.Text))
            {
                MessageBox.Show("Se necesitan jugadores para invitar.");
            }
            else
            {
                //numJugadores = numInvitados + numJugadores;
                // Quiere saber los jugadores de la partida
                if (numInvitados == 0)
                    MessageBox.Show("Debes seleccionar algun jugador!!");
                else if (numInvitados == 1)
                    inv = "7/" + Convert.ToString(numInvitados) + "/" + jugador2;
                else if (numInvitados == 2)
                    inv = "7/" + Convert.ToString(numInvitados) + "/" + jugador2 + "/" + jugador3;
                else
                    inv = "7/" + Convert.ToString(numInvitados) + "/" + jugador2 + "/" + jugador3 + "/" + jugador4;
                // Enviamos al servidor el numero de partida
                MessageBox.Show(inv);
                ConsultarServidor(inv);
            }
        }
    }
}
