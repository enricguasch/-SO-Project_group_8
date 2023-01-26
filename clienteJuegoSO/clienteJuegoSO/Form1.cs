using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Globalization;

namespace clienteJuegoSO
{
    public partial class Form1 : Form
    {
        Socket server;
        Thread atender;

        string usuario;
        string jugador2 = "";
        string jugador3 = "";
        string jugador4 = "";
        int numJugadores = 1;
        int numInvitados = 0;
        int idPartida;
        string[] jugadoresPartida = new string[4];
        int[] socketsPartida = new int[4];
        int idJugador;
        int turno = 1;
        int PartidasActivas = 0;

        List<TicTacToe> formulariosTicTacToe = new List<TicTacToe>();
        List<Pictionary> formulariosPictionary = new List<Pictionary>();
        List<LaOca> formulariosOca = new List<LaOca>();

        List<int> idPartidaToidForm = new List<int>();
        int numMensajesLounge = 0;

        Dictionary<int, int> idForms = new Dictionary<int, int>();

        Dictionary<int, string> games = new Dictionary<int, string>(){
            {1, "Tic Tac Toe"},
            {2, "La Oca"},
            {3, "Pictionary"}
        };

        public Form1()
        {
            InitializeComponent();
        }

        //Función que abre un nuevo formulario del juego TicTacToe y lo añade a la lista de formularios.
        private void FormularioTicTacToe()
        {
            TicTacToe f = new TicTacToe(usuario, jugador2, idJugador, idPartida, server);
            formulariosTicTacToe.Add(f);
            idForms.Add(idPartida, formulariosTicTacToe.Count-1);
            f.ShowDialog();
        }

        //Función que empieza un thread del formulario del juego TicTacToe
        private void AbrirFormularioTicTacToe()
        {
            ThreadStart ts = delegate { FormularioTicTacToe(); };
            Thread T = new Thread(ts);
            T.Start();
        }

        //Función que abre un nuevo formulario del juego La Oca y lo añade a la lista de formularios
        private void FormularioOca()
        {
            //idPartidaToidForm.Add(formulariosOca.Count);
            LaOca f = new LaOca(usuario, idJugador, idPartida, numJugadores, jugadoresPartida, server);
            formulariosOca.Add(f);
            idForms.Add(idPartida, formulariosOca.Count - 1);
            f.ShowDialog();
        }

        //Función que empieza un thread del formulario del juego La Oca
        private void AbrirFormularioOca()
        {
            ThreadStart ts = delegate { FormularioOca(); };
            Thread T = new Thread(ts);
            T.Start();
        }

        //Función que abre un nuevo formulario del juego Pictionary y lo añade a la lista de formularios.
        private void FormularioPictionary()
        {

            //idPartidaToidForm.Add(formulariosPictionary.Count);
            Pictionary f = new Pictionary(usuario, idJugador, idPartida, numJugadores, jugadoresPartida, server);
            formulariosPictionary.Add(f);
            idForms.Add(idPartida, formulariosPictionary.Count - 1);
            f.ShowDialog();
        }

        //Función que empieza un thread del formulario del juego Pictionary
        private void AbrirFormularioPictionary()
        {
            ThreadStart ts = delegate { FormularioPictionary(); };
            Thread T = new Thread(ts);
            T.Start();
        }

        //Función que permite canviar de pantalla y parametros relacionados dentro del juego, ya sea para hacer el login, seleccionar jugadores o jugar una partida. 
        public void ChangeTab(int id)
        {
            this.Invoke(new Action(() => { tabControl1.SelectedIndex = id; }));
            switch (id)
            {
                case 0: // WELCOME
                    this.Invoke(new Action(() => { this.Size = new Size(500, 300); }));
                    this.Invoke(new Action(() => { this.ControlBox = true; }));
                    break;
                case 1: // LOUNGE
                    this.Invoke(new Action(() => { this.Size = new Size(850, 600); }));
                    this.Invoke(new Action(() => { this.ControlBox = false; }));
                    break;
            }
        }

        //En el Load del Form1 se configura la aparencia del formulario y se conecta automaticamente
        //con el servidor a través de la función ConectaServidor().
        private void Form1_Load(object sender, EventArgs e)
        {
            //Hide TabControl Tabs
            tabControl1.Appearance = TabAppearance.Buttons;
            tabControl1.ItemSize = new Size(0, 1);

            this.Invoke(new Action(() => { this.ControlBox = true; }));
            this.Invoke(new Action(() => { this.Size = new Size(500, 300); }));
            ConectadosGrid.ColumnCount = 1;

            ConectaServidor();
        }

        //Cuando se cierra el Form1 se envia un mensaje al servidor que avisa de la desconexión y se cierra
        //el socket.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(connexion_status_lbl.ForeColor == Color.Green)
            {
                //Mensaje de desconexión
                string mensaje = "00/" + usuario;
                ConsultarServidor(mensaje);
                //Nos desconectamos
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            if (atender != null)
            {
                atender.Abort();
            }
            
            
        }

        //Si se pulsa este boton se ejecuta ConectaServidor() que conecta el cliente con el servidor.
        private void conectar_btn_Click(object sender, EventArgs e)
        {
            ConectaServidor();
        }

        //Si se pulsa este boton se ejecuta DesconectaServidor() que desconecta el cliente del el servidor
        //y se vuelve a la pestaña del Log In.
        private void desconectar_btn_Click(object sender, EventArgs e)
        {
            DesconectarServidor();
            ResetLounge();
            loungeChat.Invoke(new Action(() => { loungeChat.Clear(); }));
        }

        //Función que es ejecutada mediante un thread y que se mantiene activamente escuchando el servidor
        //para interpretar sus mensajes y ejecutar las ordenes del servidor.
        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string mensaje_limpio = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                string[] trozos = mensaje_limpio.Split('*');
                //Formato del mensaje: codigo/partida*info1/info2/info3.
                string[] codigos = trozos[0].Split('/');
                
                int codigo = Convert.ToInt32(codigos[0]);
                idPartida = Convert.ToInt32(codigos[1]);
                string respuesta = trozos[1].Split('\0')[0];

                string escribirConsulta;
                int idForm;
                switch (codigo)
                {
                    case 01:    // Respuesta a LogIn

                        //Si el usuario existe y la contraseña es correcta lo dejamos entrar al juego
                        if (Convert.ToInt32(respuesta) == 0)
                        {
                            //MessageBox.Show($"Bienvenido de nuevo {FirstLetterToUpper(username_txt.Text)}!"); //Here, "this" makes the MessageBox TopMost
                            usuario = username_txt.Text.ToLower();
                            this.Invoke(new Action(() => { this.Text = $"Juego {FirstLetterToUpper(usuario)}"; }));
                            ChangeTab(1);
                        }

                        //Si no lo son le pedimos que lo vuelva a intentar
                        else if (Convert.ToInt32(respuesta) == -1)
                        {
                            MessageBox.Show("Usuario o contraseña equivocados.");
                            username_txt.Invoke(new Action(() => { username_txt.Clear(); }));
                            password_txt.Invoke(new Action(() => { password_txt.Clear(); }));
                        }

                        //Si el usuario ya está conectado, no se vuelve a conectar
                        else if (Convert.ToInt32(respuesta) == 2)
                        {
                            MessageBox.Show("Usuario ya conectado.");
                            username_txt.Invoke(new Action(() => { username_txt.Clear(); }));
                            password_txt.Invoke(new Action(() => { password_txt.Clear(); }));
                        }

                        //Error al consultar la base de datos
                        else
                        {
                            MessageBox.Show("Error al consultar la base de datos.");
                        }
                        break;

                    case 02:    // Respuesta a Registro

                        //Si el usuario se ha creado correctamente le dejamos entrar al juego
                        if (Convert.ToInt32(respuesta) == 0)
                        {
                            MessageBox.Show("Gracias por registrarte " + username_txt.Text);
                            ChangeTab(1);
                        }

                        //Si el usuario ya está conectado, no se vuelve a conectar
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
                    case 03:        // Respuesta a dar de baja
                        if (Convert.ToInt32(respuesta) == -1)
                            MessageBox.Show("No se ha podido dar de baja");
                        else
                        {
                            MessageBox.Show("Se ha dado de baja correctamente. Hasta pronto " + usuario);
                            desconectar_btn.Invoke(new Action(() => { desconectar_btn.PerformClick(); }));
                        }
                        break;

                    case 10:    // Actualizar DataGridView cuando recibe por notificación que se ha conectado
                                // un nuevo usuario
                        string[] respuestaTokens = respuesta.Split('/');
                        int n = Convert.ToInt32(respuestaTokens[0]);
                        ConectadosGrid.Invoke(new Action(() =>
                        {
                            ConectadosGrid.Rows.Clear();
                            ConectadosGrid.ColumnHeadersVisible = false;
                            ConectadosGrid.Rows[0].Cells[0].Value = respuestaTokens[1];

                            int ii = 2;
                            while (ii <= n)
                            {
                                ConectadosGrid.Rows.Add(respuestaTokens[ii]);
                                ii++;
                            }
                            ConectadosGrid.ClearSelection();
                        }));
                        break;

                    case 11:    // Recibimos la respuesta de la consulta "partidas ganadas"

                        escribirConsulta = $"El jugador {nombre_txt.Text} ha ganado un total de {respuesta} veces.";
                        
                        RespConsulta.Invoke(new Action(() => {RespConsulta.Text = escribirConsulta; }));
                        break;

                    case 12:    // Recibimos la respuesta de la consulta "mejor jugador"

                        string[] res12 = respuesta.Split('/');
                        int j = Convert.ToInt32(res12[1]);
                        if (j == 1)
                            escribirConsulta = $"Los mejores jugadores en el Tic Tac Toe han ganado {res12[0]} partidas y son:\n";
                        else if (j == 2)
                            escribirConsulta = $"Los mejores jugadores en la Oca han ganado {res12[0]} partidas y son:\n";
                        else if (j == 3)
                            escribirConsulta = $"Los mejores jugadores en el Pictionary han ganado {res12[0]} partidas y son:\n";
                        else
                            escribirConsulta = $"Los mejores jugadores de todos los juegos han ganado {res12[0]} partidas y son:\n";
                       
                        int n12;
                        for(n12=2;n12<res12.Length-1;n12++)
                        {
                            escribirConsulta = escribirConsulta + $"\t- {res12[n12]}\n ";
                        }
                        RespConsulta.Invoke(new Action(() => { RespConsulta.Text = escribirConsulta; }));
                        break;

                    case 13:    // Recibimos la respuesta de la consulta "jugadores en partida"
                        string[] res13 = respuesta.Split('/');
                        escribirConsulta = $"En la partida {partida_txt.Text} han jugado {res13[0]} jugadores:\n";
                        int n13;
                        for (n13 = 1; n13 <= Convert.ToInt32(res13[0]); n13++)
                            escribirConsulta = escribirConsulta + $"\t- {res13[n13]}\n";
                        RespConsulta.Invoke(new Action(() => { RespConsulta.Text = escribirConsulta; }));
                        break;

                    case 14:    // Recibimos la respuesta de la consulta "partidas jugadas entre dateFrom y dateTo"
                        //MessageBox.Show(respuesta);
                        string dateFrom_V = DateTime.ParseExact(dateFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        string dateTo_V = DateTime.ParseExact(dateTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        escribirConsulta = $"Se han jugado las partidas:\n";
                        string[] res14 = respuesta.Split('/');
                        int n14;
                        int c14=0;
                        for (n14 = 1; n14 < res14.Length - 1; n14++)
                        {
                            escribirConsulta = escribirConsulta + res14[n14] + "    ";
                            if(c14==6)
                            {
                                escribirConsulta = escribirConsulta + "\n";
                                c14 = 0;
                            }
                            c14++;
                        }
                        escribirConsulta = escribirConsulta + $"\nentre las fechas siguientes:\n{dateFrom_V} - {dateTo_V}";
                        RespConsulta.Invoke(new Action(() => { RespConsulta.Text = escribirConsulta; }));
                        break;
                    case 15:    // Recibimos la respuesta de la consulta "jugadores contra quienes hemos jugado"
                        string[] res15 = respuesta.Split('/');
                        int j15 = Convert.ToInt32(res15[0]);
                        if (j15 == 1)
                            escribirConsulta = $"Has jugado partidas de Tic Tac Toe contra los siguientes jugadores:\n";
                        else if (j15 == 2)
                            escribirConsulta = $"Has jugado partidas de la Oca contra los siguientes jugadores:\n";
                        else if (j15 == 3)
                            escribirConsulta = $"Has jugado partidas del Pictionary contra los siguientes jugadores:\n";
                        else
                            escribirConsulta = $"Has jugado partidas de uno de los tres juegos contra los siguientes jugadores:\n";

                        int n15;
                        int c15=0;
                        for (n15 = 1; n15< res15.Length - 1; n15++)
                        {
                            escribirConsulta = escribirConsulta + $"\t- {res15[n15]} ";
                            if(c15==6)
                            {
                                escribirConsulta = escribirConsulta + "\n";
                                c15 = 0;
                            }
                            c15++;
                        }
                        RespConsulta.Invoke(new Action(() => { RespConsulta.Text = escribirConsulta; }));
                        break;
                    case 16:    // Recibimos la respuesta de la consulta "resultados entre un jugador y yo
                                // en los distintos juegos"
                        string[] res16 = respuesta.Split('/');
                        escribirConsulta = $"Resultados contra {textBox2.Text}:\n";
                        escribirConsulta = escribirConsulta + $"\t- Tic Tac Toe:\t Ganadas: {res16[1]}\t Perdidas: {res16[2]}\n";
                        escribirConsulta = escribirConsulta + $"\t- La Oca:\t Ganadas: {res16[4]}\t Perdidas: {res16[5]}\n";
                        escribirConsulta = escribirConsulta + $"\t- Pictionary:\t Ganadas: {res16[7]}\t Perdidas: {res16[8]}\n";
                        RespConsulta.Invoke(new Action(() => { RespConsulta.Text = escribirConsulta; }));
                        break;
                    case 20:    // Escribir la palabra que ha enviado un usuario en el chat del Lounge.
                        ActualizarChatLounge(respuesta);
                        break;
                    case 30:    // Recibir invitación al juego Tic Tac Toe, se envia la respuesta al servidor
                                //  y si se ha aceptado se abre un nuevo formulario del juego.
                        this.Invoke(new Action(() => { disableLounge(); }));
                        string[]  info = respuesta.Split('/');
                        DialogResult dr = MessageBox.Show($"{FirstLetterToUpper(info[0])} te ha invitado a jugar la partida {info[1]} de Tic Tac Toe\nQuieres aceptar la invitacion?", $"Invitacion para {usuario}", MessageBoxButtons.YesNo);
                        switch (dr)
                        {
                            case DialogResult.Yes:
                                idPartida = Convert.ToInt32(info[1]);
                                idJugador = 2;
                                jugador2 = info[0];
                                string mensaje = "30/1/" + usuario + "/SI/" + info[1];
                                ConsultarServidor(mensaje);
                                AbrirFormularioTicTacToe();
                                PartidasActivas++;
                                EstadoBotonDesconectar();
                                break;
                            case DialogResult.No:
                                string mensaje2 = "30/1/" + usuario + "/NO/" + info[1];
                                ConsultarServidor(mensaje2);
                                this.Invoke(new Action(() => { 
                                    desconectar_btn.Enabled = true;
                                    Baja.Enabled = true;
                                }));
                                break;
                        }
                        ResetLounge();
                        break;
                    case 31:    // Recibe la respuesta a la invitación al Tic Tac Toe, si es positiva se
                                // abre un nuevo formulario con el juego.
                        info = respuesta.Split('/');
                        if (info[1] == "SI")
                        {
                            idPartida = Convert.ToInt32(info[2]);
                            idJugador = 1;
                            //MessageBox.Show($"Empieza la partida {info[2]} de Tic Tac Toe!");
                            AbrirFormularioTicTacToe();
                            PartidasActivas++;
                            EstadoBotonDesconectar();
                        }
                        else
                            MessageBox.Show($"{FirstLetterToUpper(info[0])} no ha aceptado tu invitacion para jugar la partida {info[2]} de Tic Tac Toe\nNo se puede jugar la partida. Vuelve a intentarlo");
                            ResetLounge();
                            this.Invoke(new Action(() => {
                                desconectar_btn.Enabled = true;
                                Baja.Enabled = true;
                            }));
                        break;
                    case 32:    // Recibe un movimiento del contrincante en Tic Tac Toe, lee de que partida
                                // se trata, encuentra en que formulario está dicha partida y ejecuta la 
                                // función que realiza el movimiento en dicho formulario.
                        info = respuesta.Split('/');
                        //MessageBox.Show("IDPartida: " + idPartida);
                        idForm = idForms[idPartida];
                        formulariosTicTacToe[idForm].Movimiento(info[0]);
                        break;
                    case 33:    // Reinicia la partida de Tic Tac Toe en el formulario donde se está jugando
                        info = respuesta.Split('/');
                        //idForm = idPartidaToidForm[idPartida-1];
                        idForm = idForms[idPartida];
                        formulariosTicTacToe[idForm].RestartMensaje();
                        break;
                    case 35:
                        info = respuesta.Split('/');
                        //idForm = idPartidaToidForm[idPartida - 1];
                        idForm = idForms[idPartida];
                        formulariosTicTacToe[idForm].MostrarGanador(info[0]);
                        PartidasActivas--;
                        EstadoBotonDesconectar();
                        break;

                    case 40:    // Recibir invitación al juego La Oca, se envia la respuesta al servidor.
                        this.Invoke(new Action(() => { disableLounge(); }));
                        info = respuesta.Split('/');
                        dr = MessageBox.Show($"{FirstLetterToUpper(info[0])} te ha invitado a jugar la partida {info[1]} de La Oca\nQuieres aceptar la invitacion?", $"Invitacion para {usuario}", MessageBoxButtons.YesNo);
                        numJugadores = 1;
                        switch (dr)
                        {
                            case DialogResult.Yes:
                                string mensaje = "40/1/" + usuario + "/SI/" + info[1];
                                ConsultarServidor(mensaje);
                                break;
                            case DialogResult.No:
                                string mensaje2 = "40/1/" + usuario + "/NO/" + info[1];
                                ConsultarServidor(mensaje2);
                                this.Invoke(new Action(() => {
                                    desconectar_btn.Enabled = true;
                                    Baja.Enabled = true;
                                }));
                                break;
                        }
                        ResetLounge();
                        break;
                    case 41:    // Recibe las respuestas a la invitación a La Oca de los jugadores y si 
                                // todos los invitados han aceptado avisa al servidor que empieza la partida.
                        info = respuesta.Split('/');
                        if (info[1] == "SI")
                        {
                            numJugadores++;
                            if (numJugadores - 1 == numInvitados)
                            {
                                string mensaje3 = "40/2/" + info[2];
                                idPartida = Convert.ToInt32(info[2]);
                                ConsultarServidor(mensaje3);
                                ResetLounge();
                            }
                        }
                        else
                        {
                            MessageBox.Show($"{FirstLetterToUpper(info[0])} no ha aceptado tu invitacion para jugar la partida {info[2]}\nNo se puede jugar la partida. Vuelve a intentarlo");
                            ResetLounge();
                            this.Invoke(new Action(() => {
                                desconectar_btn.Enabled = true;
                                Baja.Enabled = true;
                            }));
                        }
                        break;
                    case 42:    // Empieza la partida de La Oca
                        string[] respuesta_trozos = respuesta.Split('/');
                        //MessageBox.Show($"Empieza la partida {respuesta_trozos[0]} de La Oca!");
                        idPartida = Convert.ToInt32(respuesta_trozos[0]);
                        numJugadores = Convert.ToInt32(respuesta_trozos[1]);
                        int i;
                        for (i = 0; i < numJugadores; i++)
                        {
                            string jugador = respuesta_trozos[2 * i + 2];
                            int socket = Convert.ToInt32(respuesta_trozos[2 * i + 3]);
                            jugadoresPartida[i] = jugador;
                            socketsPartida[i] = socket;
                        }
                        setID();
                        AbrirFormularioOca();
                        PartidasActivas++;
                        EstadoBotonDesconectar();
                        break;
                    case 43:    // Recibe un movimiento de un contrincante en La Oca, lee de que partida
                                // se trata, encuentra en que formulario está dicha partida y ejecuta la 
                                // función que realiza el movimiento en dicho formulario.
                        info = respuesta.Split('/');
                        
                        int num = Convert.ToInt32(info[0]);
                        int idJ = Convert.ToInt32(info[1]);
                        //idForm = idPartidaToidForm[idPartida - 1];
                        idForm = idForms[idPartida];
                        formulariosOca[idForm].Movimiento(num,idJ);

                        break;
                    case 44:    // Recibe el ganador de la partida de La Oca y para la información al form
                                // correspondiente.
                        string ganador = FirstLetterToUpper(jugadoresPartida[Convert.ToInt32(respuesta) - 1]);
                        //idForm = idPartidaToidForm[idPartida - 1];
                        idForm = idForms[idPartida];
                        formulariosOca[idForm].Ganador(ganador);
                        PartidasActivas--;
                        EstadoBotonDesconectar();
                        break;
                    case 45:    // Avisa al form que se ha finalizado la partida de La Oca
                        //idForm = idPartidaToidForm[idPartida - 1];
                        idForm = idForms[idPartida];
                        formulariosOca[idForm].FinalizarPartida();
                        PartidasActivas--;
                        EstadoBotonDesconectar();
                        break;
                    case 50:    // Recibir invitación al juego Pictionary, se envia la respuesta al servidor.
                        this.Invoke(new Action(() => { disableLounge(); }));
                        numJugadores = 1;
                        info = respuesta.Split('/');
                        dr = MessageBox.Show($"{FirstLetterToUpper(info[0])} te ha invitado a jugar la partida {info[1]} del Pictionary\nQuieres aceptar la invitacion?", $"Invitacion para {usuario}", MessageBoxButtons.YesNo);
                        switch (dr)
                        {
                            case DialogResult.Yes:
                                string mensaje = "50/1/" + usuario + "/SI/" + info[1];
                                ConsultarServidor(mensaje);
                                this.Invoke(new Action(() => {
                                    desconectar_btn.Enabled = false;
                                    Baja.Enabled = false;
                                }));
                                break;
                            case DialogResult.No:
                                string mensaje2 = "50/1/" + usuario + "/NO/" + info[1];
                                ConsultarServidor(mensaje2);
                                this.Invoke(new Action(() => {
                                    desconectar_btn.Enabled = true;
                                    Baja.Enabled = true;
                                }));
                                break;
                        }
                        ResetLounge();
                        break;
                    case 51:    // Recibe las respuestas a la invitación al Pictionary de los jugadores y si 
                                // todos los invitados han aceptado avisa al servidor que empieza la partida.
                        info = respuesta.Split('/');
                        if (info[1] == "SI")
                        {
                            numJugadores++;
                            if (numJugadores - 1 == numInvitados)
                            {
                                string mensaje = "50/4/" + info[2];
                                idPartida = Convert.ToInt32(info[2]);
                                ConsultarServidor(mensaje);
                                ResetLounge();
                            }
                        }
                        else
                        {
                            MessageBox.Show($"{FirstLetterToUpper(info[0])} no ha aceptado tu invitacion para jugar la partida {info[2]}\nNo se puede jugar la partida. Vuelve a intentarlo");
                            ResetLounge();
                            this.Invoke(new Action(() => {
                                desconectar_btn.Enabled = true;
                                Baja.Enabled = true;
                            }));
                        }
                        break;
                    case 52:    // Recibe las coordenadas de un punto y las pasa al formulario correspondiente
                                // para que lo pinte en el panel del Pictionary.
                        //idForm = idPartidaToidForm[idPartida - 1];
                        idForm = idForms[idPartida];
                        formulariosPictionary[idForm].PintarLinea(respuesta);
                        break;
                    case 53:    // Le envia al formulario correspondiente un punto a partir del cual se 
                                // dibujará una linea.
                        //idForm = idPartidaToidForm[idPartida - 1];
                        idForm = idForms[idPartida];
                        formulariosPictionary[idForm].PintarPunto(respuesta);
                        break;
                    case 54:    // Empieza la partida del Pictionary
                        info = respuesta.Split('/');
                        idPartida = Convert.ToInt32(info[0]);
                        numJugadores = Convert.ToInt32(info[1]);
                        for (i = 0; i < numJugadores; i++)
                        {
                            string jugador = info[2 * i + 2];
                            int socket = Convert.ToInt32(info[2 * i + 3]);
                            jugadoresPartida[i] = jugador;
                            socketsPartida[i] = socket;
                            puntos_Pictionary[i] = 0;
                        }
                        setID();
                        AbrirFormularioPictionary();
                        PartidasActivas++;
                        EstadoBotonDesconectar();
                        break;
                    case 56:    // Informa al formulario que se debe cambiar el color del lápiz del Pictionary.
                        info = respuesta.Split('/');
                        //idForm = idPartidaToidForm[idPartida - 1];
                        idForm = idForms[idPartida];
                        formulariosPictionary[idForm].CambiarColor(info[0]);
                        break;
                    case 57:    // Escribir en el chat del formulario la palabra que ha enviado un usuario.
                        //idForm = idPartidaToidForm[idPartida - 1];
                        idForm = idForms[idPartida];
                        int ganadorp = formulariosPictionary[idForm].ActualizarChat(respuesta);
                        if (ganadorp == 0)
                        {
                            PartidasActivas--;
                            EstadoBotonDesconectar();
                        }
                        break;
                    case 58:    // Recibir del servidor la palabra solución de la ronda del Pictionary.
                        //idForm = idPartidaToidForm[idPartida - 1];
                        idForm = idForms[idPartida];
                        formulariosPictionary[idForm].ActualizarSolucion(respuesta);
                        break;
                    case 59:    // Orden de borrar todo lo pintado en el panel del Pictionary.
                        info = respuesta.Split('/');
                        //idForm = idPartidaToidForm[idPartida - 1];
                        idForm = idForms[idPartida];
                        formulariosPictionary[idForm].ClearPanel();
                        break;
                }
            }
        }

        // Función que crea una conexión con el servidor a través de un socket y pone en marcha un thread que 
        // ejecutará la función AtenderCliente() para escuchar y procesar los mensajes del servidor.
        private void ConectaServidor()
        {
            //Creamos un IPEndPoint con el ip del servidor y port puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("147.83.117.22"); // IP maquina virtual: 192.168.56.102
            IPEndPoint ipep = new IPEndPoint(direc, 50074);

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                conectar_btn.Enabled = false;
                connexion_status_lbl.Text = "Conectado";
                connexion_status_lbl.ForeColor = Color.Green;
                //Se pone en marcha el thread que atenderá los mensajes del servidor
                ThreadStart ts = delegate { AtenderServidor(); };
                atender = new Thread(ts);
                atender.Start();
            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error 
                MessageBox.Show("No he podido conectar con el servidor");
            }
        }

        // Función que envia un mensaje al servidor notificando la desconexión y para la ejecución del thread
        // AtenderCliente() y cierra el socket que conecta al cliente con el servidor. Vuelve a la pestaña de 
        // Log In.
        private void DesconectarServidor()
        {
            //Mensaje de desconexión
            string mensaje = "00/" + usuario;
            ConsultarServidor(mensaje);
            //Nos desconectamos
            atender.Abort();
            this.BackColor = Color.Gray;
            conectar_btn.Enabled = true;
            connexion_status_lbl.Text = "Desconectado";
            connexion_status_lbl.ForeColor = Color.Red;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

            username_txt.Clear();
            password_txt.Clear();
            gmail_txt.Clear();

            ChangeTab(0);
        }

        // Función que recibe como parametro un string y lo convierte en bits para enviar este string al servidor.
        private void ConsultarServidor(string mensaje)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        // Función que se encarga de deshabilitar/habilitar los botones de Desconectar y Dar de Baja dependiendo del estado del juego.
        private void EstadoBotonDesconectar()
        {
            if(PartidasActivas>0)
            {
                this.Invoke(new Action(() => { 
                    desconectar_btn.Enabled = false;
                    Baja.Enabled = false;
                }));
            }
            else
            {
                this.Invoke(new Action(() => {
                    desconectar_btn.Enabled = true;
                    Baja.Enabled = true;
                }));
            }
        }
        #region Welcome Tab
        //**********************************************************************************
        //*********************************** WELCOME TAB **********************************
        //**********************************************************************************

        // Envia al servidor el usuario y contraseña escritos para iniciar sesión. Si los campos están
        // vacios o no hay conexión avisa del error y no envia nada al servidor.
        private void login_btn_Click(object sender, EventArgs e)
        {   // No hay conexión
            if (!server.Connected)
            {
                MessageBox.Show("Recuerda conectarte al servior!");
            }
            // No se ha rellenado la información de usuario y contraseña
            else if (string.IsNullOrEmpty(username_txt.Text) || string.IsNullOrEmpty(password_txt.Text))
            {
                MessageBox.Show("Porfavor rellene los campos de usuario y contraseña");
            }
            else
            {
                string mensaje = "01/" + username_txt.Text.ToLower() + "/" + password_txt.Text;
                usuario = username_txt.Text.ToLower();
                // Enviamos al servidor el usuario y contraseña tecleados
                ConsultarServidor(mensaje);
            }
        }

        // Envia al servidor el usuario, contraseña y gmail escritos para registrarse. Si los campos están
        // vacios o no hay conexión avisa del error y no envia nada al servidor.
        private void register_btn_Click(object sender, EventArgs e)
        {   // No hay conexión
            if (!server.Connected)
            {
                MessageBox.Show("Recuerda conectarte al servior!");
            }
            // No se ha rellenado la información de usuario, contraseña o gmail
            else if (string.IsNullOrEmpty(username_txt.Text) || string.IsNullOrEmpty(password_txt.Text) ||
                string.IsNullOrEmpty(gmail_txt.Text))
            {
                MessageBox.Show("Porfavor rellene los campos de usuario y contraseña");
            }
            else
            {
                string mensaje = "02/" + username_txt.Text.ToLower() + "/" + password_txt.Text + "/" + gmail_txt.Text.ToLower();
                usuario = username_txt.Text.ToLower();
                // Enviamos al servidor el usuario y contraseña tecleados
                ConsultarServidor(mensaje);
            }
        }

        // Botón que envia al servidor la consulta que se haya seleccionado.
        private void request_btn_Click(object sender, EventArgs e)
        {
            if (Victorias.Checked)
            {
                if (string.IsNullOrEmpty(nombre_txt.Text))
                {
                    MessageBox.Show("Se necesita un nombre para esta consulta.");
                }
                else
                {
                    // Quiere saber cuantas partidas ha ganado el jugador
                    string mensaje = "11/" + nombre_txt.Text.ToLower();
                    // Enviamos al servidor el nombre tecleado
                    ConsultarServidor(mensaje);
                }
            }
            if (Mejor.Checked)
            {
                // Quiere saber el mejor jugador
                string mensaje = $"12/{gamesDropdown.SelectedIndex+1}";
                ConsultarServidor(mensaje);
                
            }
            if (Partida.Checked)
            {
                if (string.IsNullOrEmpty(partida_txt.Text))
                {
                    MessageBox.Show("Se necesita un numero entero para esta consulta.");
                }
                else
                {
                    // Quiere saber los jugadores de la partida
                    string mensaje = "13/" + partida_txt.Text;
                    // Enviamos al servidor el numero de partida
                    ConsultarServidor(mensaje);
                }
            }
            if (FechaPartida.Checked)
            {
                // Quiere saber las partidas jugadas entre dateFrom y dateTo
                string dateFrom_V = DateTime.ParseExact(dateFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                string dateTo_V = DateTime.ParseExact(dateTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                string mensaje = $"14/{dateFrom_V}/{dateTo_V}";
                ConsultarServidor(mensaje);
            }
            if (PartidasConJugadores.Checked)
            {
                // Quiere saber con que jugadores ha jugado alguna partida:
                string consulta = $"15/{usuario}/{gamesDropdown2.SelectedIndex + 1}";
                ConsultarServidor(consulta);
            }
            if(ResultadoPartidas.Checked)
            {
                // Quiere saber los resultados entre el usuario y el jugador especificado
                string consulta = $"16/{usuario}/{textBox2.Text}";
                ConsultarServidor(consulta);
            }
        }
#endregion

        #region Lounge Tab
        //**********************************************************************************
        //*********************************** LOUNGE TAB ***********************************
        //**********************************************************************************

        // Función que resetea los valores del Lounge de Juego y seleccionados.
        private void ResetLounge()
        {
            numInvitados = 0;
            this.Invoke(new Action(() => { 
                Seleccionados.Text = "";
                playTicTackToe_btn.Enabled = true;
                playLaOca_btn.Enabled = true;
                playPictionary_btn.Enabled = true;
                loungeChatSend_btn.Enabled = true;
                ConectadosGrid.ClearSelection();
            }));
            jugador2 = "";
            jugador3 = "";
            jugador4 = "";
            Victorias.Invoke(new Action(() => { Victorias.Checked = false; }));
            nombre_txt.Invoke(new Action(() => { nombre_txt.Text = ""; }));
            Partida.Invoke(new Action(() => { Partida.Checked = false; }));
            gamesDropdown.Invoke(new Action(() => { gamesDropdown.Refresh(); }));
            Mejor.Invoke(new Action(() => { Mejor.Checked = false; }));
            partida_txt.Invoke(new Action(() => { partida_txt.Text=""; }));
            FechaPartida.Invoke(new Action(() => { FechaPartida.Checked = false; }));
            dateFrom.Invoke(new Action(() => { dateFrom.Refresh(); }));
            dateTo.Invoke(new Action(() => { dateTo.Refresh(); }));
            PartidasConJugadores.Invoke(new Action(() => { PartidasConJugadores.Checked = false; }));
            gamesDropdown2.Invoke(new Action(() => { gamesDropdown2.Refresh(); }));
            ResultadoPartidas.Invoke(new Action(() => { ResultadoPartidas.Checked = false; }));
            textBox2.Invoke(new Action(() => { textBox2.Text = "";}));
            RespConsulta.Invoke(new Action(() => { RespConsulta.Text=""; }));
        }

        // Funcion que deshabilita los botones de invitación a una partida.
        private void disableLounge()
        {
            playTicTackToe_btn.Enabled = false;
            playLaOca_btn.Enabled = false;
            playPictionary_btn.Enabled = false;
            desconectar_btn.Enabled = false;
            Baja.Enabled = false;
            loungeChatSend_btn.Enabled = false;
        }

        // Función que permite seleccionar los jugadores a invitar haciendo click en las filas del DataGridView
        private void ConectadosGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string nombre = ConectadosGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (nombre == usuario)
                MessageBox.Show("No puedes invitarte a tu mismo\n");
            else
            {
                //MessageBox.Show("NumInvitados = " + numInvitados + "Nombre = " + nombre);
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
        }

        // Función que escribe en el chat del Lounge el mensaje recibido.
        // Recibe el mensaje a escribir en el chat.
        public void ActualizarChatLounge(string respuesta)
        {
            numMensajesLounge += 1;
            // Evitar que el chat se llene
            if (numMensajesLounge > 2000)
            {
                loungeChat.Text = loungeChat.Text;
                string[] mensajes_separados = loungeChat.Text.Split('\n');
                loungeChat.Invoke(new Action(() =>
                {
                    loungeChat.Text = string.Join(System.Environment.NewLine, mensajes_separados.Skip(1));
                }));
            }
            // Añadir el mensaje en el TextBox que hace de chat
            string[] info = respuesta.Split('/');  // info: nombre/mensaje
            string emisor = FirstLetterToUpper(info[0]);
            loungeChat.Invoke(new Action(() =>
            {
                loungeChat.Text += System.Environment.NewLine + emisor + ": " + info[1];
                loungeChat.SelectionStart = loungeChat.Text.Length;
                loungeChat.ScrollToCaret();

            }));
        }

        // Envia mensaje al servidor que el usuario se quiere dar de baja.
        private void Baja_Click(object sender, EventArgs e)
        {

            ConsultarServidor("03/" + usuario);
        }


        // Permitir solo cierto tipo de caracteres al escribir en el textbox (int numbers) 
        private void partida_txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        // Permitir solo cierto tipo de caracteres al escribir en el textbox (letras y numeros int) 
        private void nombre_txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Permitir solo cierto tipo de caracteres al escribir en el textbox (letras y numeros int)
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Sets the ID of the Player
        private void setID()
        {
            int i;
            for (i = 0; i < numJugadores; i++)
            {
                if (jugadoresPartida[i] == usuario)
                    idJugador = i + 1;
            }
        }

        #endregion


        #region GAME: Tic Tac Toe
        //**********************************************************************************
        //********************************* Tic Tac Toe ************************************
        //**********************************************************************************

        // Función que se encarga de invitar a los jugadores potenciales del TicTacToe al hacer click en el botón
        private void playTicTackToe_btn_Click(object sender, EventArgs e)
        {
            numJugadores = 1;
            string inv;
            if (string.IsNullOrEmpty(Seleccionados.Text))
            {
                MessageBox.Show("Se necesitan jugadores para invitar.");
            }
            else
            {
                // Quiere saber los jugadores de la partida
                if (numInvitados == 0)
                    MessageBox.Show("Debes seleccionar algun jugador!!");
                else if (numInvitados == 1)
                {
                    // Enviamos al servidor la invitación
                    inv = "30/0/1/" + jugador2;
                    ConsultarServidor(inv);
                    disableLounge();
                }
                else
                    MessageBox.Show("En este juego solo pueden participar 2 jugadores.");             
            }
        }

        bool turn;
        int turn_count = 0;
        bool winner_found = false;
        string ttt_me;
        string ttt_oponent;
        #endregion

        #region GAME: La Oca

        //**********************************************************************************
        //************************************ LA OCA **************************************
        //**********************************************************************************

        // Funciones que se encarga de invitar a los jugadores potenciales de La Oca al hacer click en el botón
        private void playLaOca_btn_Click(object sender, EventArgs e)
        {
            desconectar_btn.Enabled = false;
            Baja.Enabled = false;
            numJugadores = 1;
            InvitarJugadores("40/0/");
        }

        // Se le pasa el codigo del juego para Invitar a os diferentes Jugadores.
        private void InvitarJugadores(string codigo)
        {
            if (string.IsNullOrEmpty(Seleccionados.Text))
            {
                desconectar_btn.Enabled = true;
                Baja.Enabled = true;
                MessageBox.Show("Se necesitan jugadores para invitar.");
            }
            else
            {
                // Quiere saber los jugadores de la partida
                string inv;
                if (numInvitados == 0)
                    MessageBox.Show("Debes seleccionar algun jugador!");
                else if (numInvitados == 1)
                {
                    inv = codigo + Convert.ToString(numInvitados) + "/" + jugador2;
                    ConsultarServidor(inv);
                    this.Invoke(new Action(() => { disableLounge(); }));
                }
                else if (numInvitados == 2)
                {
                    inv = codigo + Convert.ToString(numInvitados) + "/" + jugador2 + "/" + jugador3;
                    ConsultarServidor(inv);
                    this.Invoke(new Action(() => { disableLounge(); }));
                }
                else if (numInvitados == 3)
                {
                    inv = codigo + Convert.ToString(numInvitados) + "/" + jugador2 + "/" + jugador3 + "/" + jugador4;
                    ConsultarServidor(inv);
                    this.Invoke(new Action(() => { disableLounge(); }));
                }
                else
                    MessageBox.Show("En este juego pueden participar hasta 4 jugadores.");
            }
        }

        
        #endregion

        #region GAME: Pictionary
        //**********************************************************************************
        //********************************** PICTIONARY ************************************
        //**********************************************************************************

        ArrayList listOfPoints;
        bool PencilDown;
        int numMensajesPictionary = 0;
        int tiempo = 0;
        string[] solucionesPictionary = { "caracol", "moneda", "patata" };
        string solucionPictionary;
        int[] puntos_Pictionary = { 0, 0, 0, 0 };
        string coordX;
        string coordY;
        Color color;

        // Función que se encarga de invitar a los jugadores potenciales del Pictionary al hacer click en el botón
        private void Pictionary_btn_Click(object sender, EventArgs e)
        {
            desconectar_btn.Enabled = false;
            Baja.Enabled = false;
            InvitarJugadores("50/0/");
            numJugadores = 1;
        }

        #endregion


        #region MISCELLANOUS
        //**********************************************************************************
        //********************************* MISCELLANOUS ***********************************
        //**********************************************************************************

        // Función que capitaliza la primera letra de un string
        public string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }



        #endregion

        // Función que envia un mensaje de etxto al servidor para ocuparse del chat.
        private void loungeChatSend_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(loungeChatMessageBox.Text))
            {
                MessageBox.Show("Se necesita un mensaje para escribirlo en el chat.");
            }
            else
            {
                string mensaje = "20/" + usuario + "/" +  loungeChatMessageBox.Text;
                ConsultarServidor(mensaje);
                loungeChatMessageBox.Clear();
                //Chat.Text = Chat.Text + "\n" + Mensaje.Text;
            }
        }

        // Envia la palabara escrita al hacer Enter
        private void loungeChatMessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loungeChatSend_btn.PerformClick();
            }
        }

        
    }


}

