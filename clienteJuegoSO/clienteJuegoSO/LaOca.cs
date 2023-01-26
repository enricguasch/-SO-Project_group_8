// #############################################################################################################################################################
// #################################################### FORMULARIO Y CÓDIGO DE LA OCA ##########################################################################
// #############################################################################################################################################################
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
    public partial class LaOca : Form
    {
        public LaOca()
        {
            InitializeComponent();
        }

        // Variables globales
        Socket server;
        string usuario;
        int numJugadores;
        int idPartida;
        string[] jugadoresPartida = new string[4];
        int idJugador;
        int turno = 1;
        PictureBox[] pictures = new PictureBox[63];
        Panel[] panels = new Panel[63];
        PictureBox[] players = new PictureBox[4];
        Image[] diceImages = new Image[6];
        Random random = new Random();
        int diceIndex = 0;
        int timeElapsed = 0;
        int num;
        int[] posiciones = new int[4] { 1, 1, 1, 1 };
        int[] contadorCarcel = new int[4] { 0, 0, 0, 0 };
        int[] jugadorCarcel = new int[4] { 0, 0, 0, 0 };

        // Constructor para el formulario de la oca
        public LaOca(string usuario, int id, int idp, int numjugadores, string[] jugadoresPartida, Socket server)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.idJugador = id;
            this.idPartida = idp;
            this.numJugadores = numjugadores;
            this.jugadoresPartida = jugadoresPartida;
            this.server = server;
        }

        // Load del formulario de la oca
        private void Form4_Load(object sender, EventArgs e)
        {
            this.Text = $"La Oca (Jugadores: {string.Join(" ", jugadoresPartida)})";


            this.Size = new Size(800, 700);

            ResetOca();
            int i;
            for (i = 0; i < 4; i++)
            {
                PictureBox player = new PictureBox();
                player.Image = Image.FromFile($"app/pawns/{i + 1}.gif");
                player.BackColor = Color.Transparent;
                player.Size = new Size(25, 40);
                player.Location = new Point(0 + 20 * i, 10);
                player.SizeMode = PictureBoxSizeMode.StretchImage;
                players[i] = player;
            }
            for (i = 0; i < 6; i++)
                diceImages[i] = Image.FromFile($"app/dice/{i + 1}.png");

            dice.BackColor = Color.Transparent;
            dice.SizeMode = PictureBoxSizeMode.Zoom;
            dice.Image = diceImages[0];
            this.Invoke(new Action(() => { this.Controls.Add(dice); }));
            timer1.Interval = 50;
            timer1.Tick += Timer_Tick;
            if (usuario != jugadoresPartida[0])
                Finalizar.Invoke(new Action(() =>
                {
                    Finalizar.Hide();
                }));
            Tablero();
            MuestraFichas(numJugadores);
            label_turno.Invoke(new Action(() => { label_turno.Text = "Turno de:  \n" + FirstLetterToUpper(jugadoresPartida[turno - 1]); }));
        }

        // Funcion que recibe un mensaje para consultar al servidor y lo envía. No retorna nada.
        private void ConsultarServidor(string mensaje)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        // Funcion que pone todos las variables correspondientes al juego de la oca a su valor inicial. No retorna nada.
        private void ResetOca()
        {
            diceIndex = 1;
            turno = 1;

            int i;
            for (i = 0; i < posiciones.Length; i++)
            {
                posiciones[i] = 1;
            }
            for (i = 0; i < contadorCarcel.Length; i++)
            {
                contadorCarcel[i] = 0;
                jugadorCarcel[i] = 0;
            }
            for (i = 0; i < panels.Length; i++)
            {
                this.Invoke(new Action(() => { this.Controls.Remove(panels[i]); }));
            }

            for (i = 0; i < players.Length; i++)
            {
                this.Invoke(new Action(() => { this.Controls.Remove(players[i]); }));
            }
            if(dice.IsHandleCreated == true)
            {
                dice.Invoke(new Action(() => { dice.Image = diceImages[0]; }));
                numeroDado.Invoke(new Action(() => { numeroDado.Text = "Numero: " + Convert.ToString(diceIndex); }));
            }


        }

        // Timer para poder generar el efecto del dado moviendose. No retorna nada.
        private void Timer_Tick(object sender, EventArgs e)
        {
            timeElapsed += 50;
            if (timeElapsed > 500)
            {
                int i = random.Next(6);
                dice.Image = diceImages[i];
                timeElapsed = 0;
                timer1.Stop();
                num = 1 + i;
                numeroDado.Invoke(new Action(() => { numeroDado.Text = "Numero: " + Convert.ToString(num); }));
                string movimiento = "40/3/" + idPartida + "/" + num + "/" + idJugador;
                ConsultarServidor(movimiento);

                num = 0;
            }
            else
            {
                diceIndex++;
                if (diceIndex == 6)
                    diceIndex = 0;
                dice.Image = diceImages[diceIndex];
            }
        }

        // Funcion que genera el tablero de La Oca. No retorna nada.
        private void Tablero()
        {
            int pos = 1;
            int n = 0;
            int y = 0;
            int x = 0;
            int l = 1;
            bool acabado = false;
            while (acabado == false)
            {
                Panel panel = new Panel();
                panels[n] = panel;
                PictureBox picture = new PictureBox();
                pictures[n] = picture;
                if (pos == 1 || pos == 63)
                {
                    panels[n].Size = new Size(125, 60);
                    panels[n].Location = new Point(25 * l + 65 * x, 550 - 65 * y);
                    l = 3;
                }
                else
                {
                    panels[n].Size = new Size(60, 60);
                    panels[n].Location = new Point(30 * l + 65 * x, 550 - 65 * y);
                }
                pictures[n].Image = Image.FromFile($"app/tiles/{pos}.jpg");
                pictures[n].Size = MaximumSize; //Maximum size
                pictures[n].Dock = DockStyle.Fill; //fill max space
                pictures[n].SizeMode = PictureBoxSizeMode.StretchImage;

                //Definimos la posicion de cada tile de La Oca
                if (pos <= 7)
                    x++;
                else if ((pos > 7) && (pos <= 15))
                    y++;
                else if ((pos > 15) && (pos <= 23))
                    x--;
                else if ((pos > 23) && (pos <= 30))
                    y--;
                else if ((pos > 30) && (pos <= 37))
                    x++;
                else if ((pos > 37) && (pos <= 43))
                    y++;
                else if ((pos > 43) && (pos <= 49))
                    x--;
                else if ((pos > 49) && (pos <= 54))
                    y--;
                else if ((pos > 54) && (pos <= 59))
                    x++;
                else
                    y++;
                if (pos == 62)
                    acabado = true;
                n++;
                pos++;
            }

            // Hcemos el panel central más grande
            Panel panel_central = new Panel();
            panel_central.Size = new Size(254, 255);
            panel_central.Location = new Point(156, 160);
            panels[panels.Length - 1] = panel_central;
            PictureBox picture_central = new PictureBox();
            picture_central.Image = Image.FromFile($"app/tiles/63.jpg");
            picture_central.Size = MaximumSize; //Maximum size
            picture_central.Dock = DockStyle.Fill; //fill max space
            picture_central.SizeMode = PictureBoxSizeMode.StretchImage;
            pictures[pictures.Length - 1] = picture_central;
            int p;
            for (p = 0; p < panels.Length; p++)
            {
                this.Invoke(new Action(() => { this.Controls.Add(panels[p]); }));
                panels[p].Invoke(new Action(() => { panels[p].Controls.Add(pictures[p]); panels[p].Show(); }));
            }
        }

        // Mostramos las fichas de los jugadores donde toca. Recibe un numero correspondiente al numero de jugadores que van a jugar la partida (Entre 0 y 4).
        private void MuestraFichas(int num)
        {
            int i;
            for (i = 0; i < num; i++)
            {
                panels[0].Invoke(new Action(() => { panels[0].Controls.Add(players[i]); }));
                players[i].Invoke(new Action(() => { players[i].Parent = pictures[0]; }));
            }

        }

        // Función que mueve un jugador de una casilla a otra teniendo en cuenta si cae en una oca o si está en la prision.
        // Recibe el numero de casillas a mover y el id del jugador a mover.
        // Retorna 1 si el jugador ha caido en una oca y 0 si no.
        private int Mover(int num, int IDPlayer)
        {
            int oca = 0;
            int i = 0;
            if(IDPlayer>4)
            {
                IDPlayer = Convert.ToInt32(IDPlayer.ToString().ToCharArray()[0]);
            }
            if ((posiciones[IDPlayer - 1] + num) > 63 && (posiciones[IDPlayer - 1]) != 63)
            {
                int posicion_final = 63 - (posiciones[IDPlayer - 1] + num - 63);
                panels[posicion_final - 1].Invoke(new Action(() => { panels[posicion_final - 1].Controls.Add(players[IDPlayer - 1]); }));
                players[IDPlayer - 1].Invoke(new Action(() => { players[IDPlayer - 1].Parent = pictures[posicion_final - 1]; }));
                posiciones[IDPlayer - 1] = posicion_final;
            }
            else
            {
                while (i < num)
                {
                    int posicion = posiciones[IDPlayer - 1];
                    if ((posiciones[IDPlayer - 1] == 62) && (num == 1))
                    {
                        panels[posicion].Invoke(new Action(() => { panels[posicion].Controls.Add(players[IDPlayer - 1]); }));
                        players[IDPlayer - 1].Invoke(new Action(() => { players[IDPlayer - 1].Parent = pictures[posicion]; }));
                        posiciones[IDPlayer - 1] = 63;
                    }
                    else
                    {
                        panels[posicion].Invoke(new Action(() => { panels[posicion].Controls.Add(players[IDPlayer - 1]); }));
                        players[IDPlayer - 1].Invoke(new Action(() => { players[IDPlayer - 1].Parent = pictures[posicion]; }));
                        posiciones[IDPlayer - 1] = posicion + 1;
                    }
                    i++;
                }
            }

            int pos = posiciones[IDPlayer - 1];
            //Posicions de les oques
            if ((pos == 5) || (pos == 10) || (pos == 15) || (pos == 20) || (pos == 25) || (pos == 30) || (pos == 35) || (pos == 40) || (pos == 45) || (pos == 50) || (pos == 55))
            {
                if (idJugador == IDPlayer)
                    MessageBox.Show("De oca a oca y tiras porque te toca!");
                panels[pos + 5 - 1].Invoke(new Action(() => { panels[pos + 5 - 1].Controls.Add(players[IDPlayer - 1]); }));
                players[IDPlayer - 1].Invoke(new Action(() => { players[IDPlayer - 1].Parent = pictures[pos + 5 - 1]; }));
                posiciones[IDPlayer - 1] = pos + 5;
                turno = IDPlayer;
                oca = 1;
            }
            if (pos == 60)
            {
                panels[62].Invoke(new Action(() => { panels[62].Controls.Add(players[IDPlayer - 1]); }));
                players[IDPlayer - 1].Invoke(new Action(() => { players[IDPlayer - 1].Parent = pictures[62]; }));
                posiciones[IDPlayer - 1] = 63;
            }

            if (pos == 6) //Puente
            {
                if (idJugador == IDPlayer)
                    MessageBox.Show("De puente a puente porque te lleva la corriente!");
                panels[12 - 1].Invoke(new Action(() => { panels[12 - 1].Controls.Add(players[IDPlayer - 1]); }));
                players[IDPlayer - 1].Invoke(new Action(() => { players[IDPlayer - 1].Parent = pictures[12 - 1]; }));
                posiciones[IDPlayer - 1] = 12;
            }
            if (pos == 12) //Puente
            {
                if (idJugador == IDPlayer)
                    MessageBox.Show("De puente a puente porque te lleva la corriente!");
                panels[6 - 1].Invoke(new Action(() => { panels[6 - 1].Controls.Add(players[IDPlayer - 1]); }));
                players[IDPlayer - 1].Invoke(new Action(() => { players[IDPlayer - 1].Parent = pictures[6 - 1]; }));
                posiciones[IDPlayer - 1] = 6;
            }
            if (pos == 26) //Dados
            {
                if (idJugador == IDPlayer)
                    MessageBox.Show("Has caído en los dados de la suerte! Avanzas hasta los siguientes dados.");
                panels[53 - 1].Invoke(new Action(() => { panels[53 - 1].Controls.Add(players[IDPlayer - 1]); }));
                players[IDPlayer - 1].Invoke(new Action(() => { players[IDPlayer - 1].Parent = pictures[53 - 1]; }));
                posiciones[IDPlayer - 1] = 53;
            }
            if (pos == 53) //Dados
            {
                if (idJugador == IDPlayer)
                    MessageBox.Show("Que mala suerte! Has caído en los dados y debes retroceder hasta los primeros dados!");
                panels[26 - 1].Invoke(new Action(() => { panels[26 - 1].Controls.Add(players[IDPlayer - 1]); }));
                players[IDPlayer - 1].Invoke(new Action(() => { players[IDPlayer - 1].Parent = pictures[26 - 1]; }));
                posiciones[IDPlayer - 1] = 26;
            }
            if (pos == 58) //Muerte
            {
                if (idJugador == IDPlayer)
                    MessageBox.Show("La muerte te lleva al inicio de la partida!");
                panels[0].Invoke(new Action(() => { panels[0].Controls.Add(players[IDPlayer - 1]); }));
                players[IDPlayer - 1].Invoke(new Action(() => { players[IDPlayer - 1].Parent = pictures[0]; }));
                posiciones[IDPlayer - 1] = 1;
            }

            if (jugadorCarcel.Sum() == 0)
            {
                if (pos == 19) // POSADA: Pierde 1 turno
                {
                    int c = PonJugadorCarcel(IDPlayer, 1);
                    if (c == -1)
                    {
                        if (idJugador == IDPlayer)
                            MessageBox.Show("Ya hay otro jugador en la posada.");
                    }
                    else
                    {
                        if (idJugador == IDPlayer)
                        {
                            MessageBox.Show("Has caido a la posada!");
                        }
                        contadorCarcel[IDPlayer - 1] += 1*(numJugadores-1)/2;
                    }
                }

                if (pos == 31) // POZO: Pierde 3 turnos
                {
                    int c = PonJugadorCarcel(IDPlayer, 1);
                    if (c == -1)
                    {
                        if (idJugador == IDPlayer)
                        {
                            MessageBox.Show("Ya hay otro jugador en el pozo.");
                        }
                    }
                    else
                    {
                        if (idJugador == IDPlayer)
                        {
                            MessageBox.Show("Has caido en el pozo!");
                        }
                        contadorCarcel[IDPlayer - 1] += 1*(numJugadores-1)/2;
                    }
                }

                if (pos == 52) // CARCEL: Pierde 2 turnos
                {
                    int c = PonJugadorCarcel(IDPlayer, 1);

                    if (c == -1)
                    {
                        if (idJugador == IDPlayer)
                        {
                            MessageBox.Show("Ya hay otro jugador en la carcel.");
                        }
                    }
                    else
                    {
                        if (idJugador == IDPlayer)
                        {
                            MessageBox.Show("Has caido en la carcel!");
                        }
                        contadorCarcel[IDPlayer - 1] += 1 * (numJugadores - 1)/2;
                    }
                }
            }
            else if (jugadorCarcel[IDPlayer - 1] > 0)
            {
                if (contadorCarcel[IDPlayer - 1] > 1)
                {
                    if (idJugador == IDPlayer)
                        MessageBox.Show($"Aun no puedes tirar, te quedan {contadorCarcel[IDPlayer - 1]} turnos para poder moverte.");
                    contadorCarcel[IDPlayer - 1] -= 1;
                }
                else
                {
                    jugadorCarcel[IDPlayer - 1] = 0;
                    turno = IDPlayer;
                }
            }

            if (posiciones[IDPlayer - 1] == 63)
            {
                MessageBox.Show("TENEMOS GANADOR!!!");
                if (jugadoresPartida[IDPlayer - 1] == usuario)
                {
                    string mensaje_ganador = "40/4/" + idPartida + "/" + IDPlayer;
                    ConsultarServidor(mensaje_ganador);
                }
            }

            return oca;
        }

        // Funcion que recibe el id de un jugador y retorna el id del jugador que va a jugar el proximo turno.
        private int DameID_Siguiente(int idJ)
        {
            int ids = 0;
            if (numJugadores == 2 && idJ == 2)
                ids = 1;
            else if (numJugadores == 3 && idJ == 3)
                ids = 1;
            else if (numJugadores == 4 && idJ == 4)
                ids = 1;
            else
                ids = idJ + 1;
            return ids;
        }

        // Función que pone en el vector jugadorCarcel el jugador que le pasamos como parametro. Tambien recibe el id de la carcel (1,2,3). 
        // Solo puede haber un jugador por carcel, por lo tanto, si esto ocurre no coloca el jugador en el vector y retorna 0.
        // Si puede colocarlo correctamente retorna 1.
        private int PonJugadorCarcel(int idJ, int idCarcel)
        {
            int colocado = 0;
            int i;
            if(jugadorCarcel.Sum() == 0)
            {
                for (i = 0; i < jugadorCarcel.Length; i++)
                {
                    if (jugadorCarcel[i] == idCarcel)
                        colocado = -1;
                }
                if (colocado == 0)
                    jugadorCarcel[idJ - 1] = idCarcel;
            }
            return colocado;
        }

        // Función que incrementa el turno de juego en todos los clientes en funcion del numero de jugadores participando en el juego.
        private void IncrementarTurno()
        {
            if (numJugadores == 2)
            {
                if (turno == 2)
                    turno = 1;
                else
                    turno++;
            }
            else if (numJugadores == 3)
            {
                if (turno == 3)
                {
                    turno = 1;
                }
                else
                    turno++;
            }
            else
            {
                if (turno == 4)
                    turno = 1;
                else
                    turno++;
            }
        }

        // Funcion que recibe el numero de posiciones a mover y el jugador al que hay que mover. No retorna nada.
        public void Movimiento(int num, int idJ)
        {
            int oca = Mover(num, idJ);
            int pos = posiciones[idJ - 1];
            if (oca == 0)
                IncrementarTurno();
            int id_siguiente = DameID_Siguiente(idJ);
            if ((jugadorCarcel[id_siguiente - 1] > 0 && oca == 0) && (idJugador==id_siguiente))// Si tenemos el siguiente jugador en la carcel debemos incrementar el turno automaticamente
            {
                string movimiento = "40/3/" + idPartida + "/" + 0 + "/" + idJugador;
                ConsultarServidor(movimiento);
            }
            label_turno.Invoke(new Action(() => { label_turno.Text = "Turno de:  \n" + FirstLetterToUpper(jugadoresPartida[turno - 1]); }));

        }

        // Funcion que recibe el nombre del ganador y lo comunical servidor. No retorna nada.
        public void Ganador(string ganador)
        {
            label_turno.Invoke(new Action(() => { label_turno.Text = "EL GANADOR ES " + ganador; }));
            MessageBox.Show("El ganador de la partida es: " + ganador);

            if (FirstLetterToUpper(usuario) == ganador)
            {
                string ganador_msg = "40/6/" + idPartida + "/" + ganador;
                ConsultarServidor(ganador_msg);
            }
            this.Invoke(new Action(()=> { this.Close(); }));
        }

        // Funcion que envia al servidor que el creador ha finalizado la partida cuando este pulsa el boton de finalizar partida. No retorna nada.
        private void Finalizar_Click(object sender, EventArgs e)
        {
            string mensaje_fin = "40/5/" + idPartida;
            ConsultarServidor(mensaje_fin);
        }

        // Funcion que cierra el formulario cuando el creador finaliza la partida. No retorna nada.
        public void FinalizarPartida()
        {
            MessageBox.Show("El creador ha finalizado la partida");
            this.Invoke(new Action(() => { this.Close(); }));
        }

        // Función que recibe un string y retorna el mismo string con la primera letra en mayusculas.
        public string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        // Funcion que inicializa el timer cuando pulsamos el boton de lanzar dado. No retorna nada.
        private void Lanzar_dado_Click(object sender, EventArgs e)
        {
            if (idJugador == turno)
                timer1.Start();
            else
                MessageBox.Show("No es tu turno!");
        }
    }
}
