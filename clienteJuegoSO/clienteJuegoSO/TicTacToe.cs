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

namespace clienteJuegoSO
{
    public partial class TicTacToe : Form
    {
        //Variables globales
        Socket server;
        string usuario;
        string contrincante = "";
        int idPartida;
        int idJugador;

        bool turn;
        int turn_count = 0;
        bool winner_found = false;
        string ttt_me;
        string ttt_oponent;

        // Constructor del Form, importa del formulario principal los valores de las variables globales: usuario,
        // contrincante, id, idp, server.
        public TicTacToe(string usuario, string contrincante, int id, int idp, Socket server)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.contrincante = contrincante;
            this.idJugador = id;
            this.idPartida = idp;
            this.server = server;
        }

        //Load del Form, define la apariencia del form y que ficha usa cada jugador
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = $"TicTacToe ({FirstLetterToUpper(usuario)} VS {FirstLetterToUpper(contrincante)})";

            this.Size = new Size(500, 400);

            if (idJugador == 1)
            {
                ttt_me = "X";
                ttt_oponent = "O";
                turn = true;
                displayturn.Text = ttt_me;
            }
            else
            {
                ttt_oponent = "X";
                ttt_me = "O";
                turn = false;
                displayturn.Text = ttt_oponent;
            }
            displaysymbol.Text = ttt_me;
            restart_tictactoe.Hide();
            ttt_to_lounge_btn.Hide();
            label_ganador.Hide();
            enableButtons();
        }

        private void ConsultarServidor(string mensaje)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        // Se encarga de enviar al servidor la posicion del tablero clicada y se marca.
        // La usan los 9 botones del tablero.
        private void button_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (turn)
            {
                b.Text = ttt_me;
                turn_count++;
                b.Enabled = false;
                string mensaje = "30/2/" + idPartida + "/" + b.Name;
                turn = false;
                displayturn.Text = ttt_oponent;
                ConsultarServidor(mensaje);
                checkWinTicTacToe();
            }
            else
                MessageBox.Show("No es tu turno!");
        }

        // Se prepara la tirada del jugador para poseriormente comprobar si es el ganador de la tirada.
        public int Movimiento(string mensaje)
        {
            int ganador = 0;
            Button button = (Button)this.Controls.Find(mensaje, true).FirstOrDefault();
            if (button != null)
            {
                this.Invoke(new Action(() =>
                {
                    button.Text = ttt_oponent;
                    button.Enabled = false;
                    displayturn.Text = ttt_me;
                }));

                turn_count++;
                turn = true;
                this.Invoke(new Action(() =>
                {
                    ganador = checkWinTicTacToe();
                }));
            }
            return ganador;
        }

        // Se comprueba si el jugador que ha jugado es el ganador.
        private int checkWinTicTacToe()
        {
            winner_found = false;
            //Buscamos ganador en horizontales
            if ((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A1.Enabled))
                winner_found = true;
            else if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B1.Enabled))
                winner_found = true;
            else if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C1.Enabled))
                winner_found = true;
            //Buscamos ganador en verticales
            else if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!A1.Enabled))
                winner_found = true;
            else if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!A2.Enabled))
                winner_found = true;
            else if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!A3.Enabled))
                winner_found = true;
            //Buscamos ganador en diagonales
            else if ((A3.Text == B2.Text) && (B2.Text == C1.Text) && (!A3.Enabled))
                winner_found = true;
            else if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!A1.Enabled))
                winner_found = true;

            if (winner_found)
            {
                disableButtons();
                turn_count = 0;
                if (!turn)
                {
                    string mensaje = "30/3/" + idPartida + "/" + usuario;
                    ConsultarServidor(mensaje);
                    Thread.Sleep(10);
                    mensaje = "30/5/" + idPartida + "/" + usuario;
                    ConsultarServidor(mensaje);
                }
                ttt_to_lounge_btn.Show();
                return 0;
            }
            else
            {
                if (turn_count == 9)
                {
                    restart_tictactoe.Show();
                    MessageBox.Show("Empate!");
                }
                return -1;
            }

        }

        // Se deshabilitan los botones del TicTacToe
        private void disableButtons()
        {
            A1.Enabled = false;
            A2.Enabled = false;
            A3.Enabled = false;
            B1.Enabled = false;
            B2.Enabled = false;
            B3.Enabled = false;
            C1.Enabled = false;
            C2.Enabled = false;
            C3.Enabled = false;
        }


        // Se habilitan los botones del TicTacToe
        private void enableButtons()
        {
            A1.Enabled = true;
            A2.Enabled = true;
            A3.Enabled = true;
            B1.Enabled = true;
            B2.Enabled = true;
            B3.Enabled = true;
            C1.Enabled = true;
            C2.Enabled = true;
            C3.Enabled = true;

            A1.Text = "";
            A2.Text = "";
            A3.Text = "";
            B1.Text = "";
            B2.Text = "";
            B3.Text = "";
            C1.Text = "";
            C2.Text = "";
            C3.Text = "";

            restart_tictactoe.Hide();
            ttt_to_lounge_btn.Hide();
        }

        public string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        // Se resetean los botones del TicTacToe cuando es el turno del jugador
        private void restart_tictactoe_Click(object sender, EventArgs e)
        {
            turn = true;
            turn_count = 0;
            enableButtons();
            string mensaje = "30/4/" + idPartida;
            displayturn.Text = ttt_me;
            ConsultarServidor(mensaje);
        }

        // Se resetean los botones del TicTacToe cuando no es el turno del jugador
        public void RestartMensaje()
        {
            turn = false;
            turn_count = 0;
            this.Invoke(new Action(() =>
            {
                displayturn.Text = ttt_oponent;
                enableButtons();
            }));
        }

        // Función que muestra el ganador de la partida en el label de ganador
        public void MostrarGanador(string ganador)
        {
            //MessageBox.Show("Ganador: " + ganador);
            label_ganador.Invoke(new Action(() => { label_ganador.Show(); }));
            label_ganador.Invoke(new Action(() => { label_ganador.Text = "Ganador: " + ganador; }));
        }

        // Función que cierra el juego de TicTacToe volviendo al Lounge
        private void ttt_to_lounge_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
