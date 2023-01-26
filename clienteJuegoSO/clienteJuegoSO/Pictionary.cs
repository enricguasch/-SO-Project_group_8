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
    public partial class Pictionary : Form
    {
        // Variables globales del Form
        Socket server;
        string usuario;
        int numJugadores;
        int idPartida;
        string[] jugadoresPartida = new string[4];
        int idJugador;
        int turno = 1;

        ArrayList listOfPoints;
        bool PencilDown;
        int numMensajesPictionary = 0;
        int tiempo = 0;
        string solucionPictionary;
        int[] puntos_Pictionary = { 0, 0, 0, 0 };
        string coordX;
        string coordY;
        Color color;

        // Constructor del Form, importa del formulario principal los valores de las variables globales: usuario, id,
        // idp, numjugadores, jugadoresPartida, server.
        public Pictionary(string usuario, int id, int idp, int numjugadores, string[] jugadoresPartida, Socket server)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.idJugador = id;
            this.idPartida = idp;
            this.numJugadores = numjugadores;
            this.jugadoresPartida = jugadoresPartida;
            this.server = server;
        }

        // Load del Form, define el aspecto del Form, el jugador que le toca dibujar y los timers.
        private void Form3_Load(object sender, EventArgs e)
        {
            this.Text = $"Pictionary (Jugadores: {string.Join(" ", jugadoresPartida)})";

            this.Size = new Size(770, 500);

            turno = 1;
            listOfPoints = new ArrayList();
            PencilDown = false;
            color = Color.Black;
            timerPictionary.Interval = 50;//50 milisegundos

            j1PictionaryLbl.Text = FirstLetterToUpper(jugadoresPartida[0]);
            j2PictionaryLbl.Text = FirstLetterToUpper(jugadoresPartida[1]);
            j3PictionaryLbl.Text = FirstLetterToUpper(jugadoresPartida[2]);
            j4PictionaryLbl.Text = FirstLetterToUpper(jugadoresPartida[3]);

            PictionaryAdvisoryLbl.Text = "";
            PictionaryAdvisoryLbl.Hide();
            AdvisoryBackground.Hide();

            timerPictionaryPuntos.Interval = 1000;//1 segundo
            tiempo = 100;
            numMensajesPictionary = 0;

            if (turno == idJugador)
            {
                prompt_Lbl.Show();
                label9.Show();
                Respuesta_Pictionary.Hide();
                Mensaje_Pictionary_btn.Hide();
                MessageBox.Show("Es tu turno para dibujar!");
                ConsultarServidor("50/8/" + idPartida);
            }
            panel1.Invalidate();
            actualizarPuntos();
        }

        // Función que recibe como parametro un string y lo convierte en bits para enviar este string al servidor.
        private void ConsultarServidor(string mensaje)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        //Funcion que cambia el color con el que se pinta en el panel
        public void CambiarColor(string nombre)
        {
            color = Color.FromName(nombre);
        }

        //Funcion que escribe el nuebo mensaje que se recibe en el chat del Pictionary
        public int ActualizarChat(string respuesta)
        {
            numMensajesPictionary += 1;
            if (numMensajesPictionary > 2000)
            {
                pictionaryChat.Text = pictionaryChat.Text;
                string[] mensajes_separados = pictionaryChat.Text.Split('\n');
                pictionaryChat.Invoke(new Action(() =>
                {
                    pictionaryChat.Text = string.Join(System.Environment.NewLine, mensajes_separados.Skip(1));
                }));
            }
            string[] info = respuesta.Split('/');
            string emisor = FirstLetterToUpper(jugadoresPartida[Convert.ToInt32(info[0]) - 1]);
            pictionaryChat.Invoke(new Action(() =>
            {
                pictionaryChat.Text += System.Environment.NewLine + emisor + ": " + info[1].ToUpper();
                Respuesta_Pictionary.Clear();
                pictionaryChat.SelectionStart = pictionaryChat.Text.Length;
                pictionaryChat.ScrollToCaret();

            }));
            int g = checkWinPictionary(Convert.ToInt32(info[0]) - 1, info[1]);
            return g;
        }

        //Funcion que actualiza el valor de la palabra solución del turno del Pictionary
        public void ActualizarSolucion(string solucion)
        {
            solucionPictionary = solucion;
            this.Invoke(new Action(() =>
            {
                prompt_Lbl.Text = solucionPictionary.ToUpper();
                timerPictionaryPuntos.Start();
            }));
        }

        //Funcion que pinta un nuevo punto en el panel a partir de las coordenadas que recibe del servidor
        public void PintarPunto(string respuesta)
        {
            if (turno != idJugador)
            {
                string[] info = respuesta.Split('/');
                coordX = info[0].Split('+')[0];
                coordY = info[0].Split('+')[1];
                int X = Convert.ToInt32(coordX);
                int Y = Convert.ToInt32(coordY);
                Point points = new Point(X, Y);
                listOfPoints.Add(points);
            }
        }

        //Funcion que muestra un mensaje en pantalla
        private void MostrarAviso(string aviso)
        {
            PictionaryAdvisoryLbl.Text = aviso;
            AdvisoryBackground.Show();
            PictionaryAdvisoryLbl.Show();
        }

        //Funcion que esconde el mensaje mostrado en pantalla por MostrarAviso(string aviso)
        private void EsconderAviso()
        {
            PictionaryAdvisoryLbl.Hide();
            AdvisoryBackground.Hide();
        }

        //Funcion que continua pintando una linea a partir del punto anterior y las coordenadas de un nuevo punto que recibe del servidor
        public void PintarLinea(string respuesta)
        {
            if (turno != idJugador)
            {
                string[] info = respuesta.Split('/');
                coordX = info[0].Split('+')[0];
                coordY = info[0].Split('+')[1];
                int X = Convert.ToInt32(coordX);
                int Y = Convert.ToInt32(coordY);
                Graphics g = panel1.CreateGraphics();
                Point points = new Point(X, Y);
                Pen pencil = new Pen(color, 3);
                if (listOfPoints.Count > 1)
                    g.DrawLine(pencil, (Point)listOfPoints[listOfPoints.Count - 1], points);
                listOfPoints.Add(points);
            }
        }

        //Funcion que limpia el panel, borrando todo lo pintado
        public void ClearPanel()
        {
            this.Invoke(new Action(() =>
            {
                panel1.Invalidate();
            }));
        }

        //Funcion que revisa si la palabra escrita en el chat del Pictionary es la palabra solución,
        //y si lo es cambia el turno, muestra el aviso de cambio de turno y borra lo pintado.
        private int checkWinPictionary(int id, string palabra)
        {
            int g = -1;
            if (palabra.ToLower() == solucionPictionary)
            {
                int puntos_adivinar = Convert.ToInt32(tiempo);
                puntos_Pictionary[id] += puntos_adivinar;
                puntos_Pictionary[turno - 1] += Convert.ToInt32(puntos_adivinar * 0.2);

                timerPictionaryPuntos.Stop();
                timerPictionary.Stop();
                string aviso = jugadoresPartida[id].ToUpper() + " HA ADIVINADO LA PALABRA!\nLA RESPUESTA ERA: " + solucionPictionary.ToUpper() + "\nCAMBIO DE TURNO.";
                this.Invoke(new Action(() =>
                {
                    MostrarAviso(aviso);
                    actualizarPuntos();
                }));
                turno += 1;
                Thread.Sleep(3000);
                this.Invoke(new Action(() =>
                {
                    EsconderAviso();
                }));
                g = changeTurnPictionary();
            }
            return g;
        }

        //Funcion que cambia el turno de pictionary, y si ya han pasado todos los turnos avisa al servidor
        //que se ha acabado la partida y el nombre del ganador
        private int changeTurnPictionary()
        {
            int ganadorp = -1;

            tiempo = 100;
            panel1.Invalidate();
            color = Color.Black;
            if (turno > numJugadores)
            {
                int max = puntos_Pictionary.Max();
                int ganador = puntos_Pictionary.ToList().IndexOf(max);
                if (idJugador == ganador + 1)
                {
                    string mensaje = "50/5/" + idPartida + "/" + usuario;
                    ConsultarServidor(mensaje);
                }
                MessageBox.Show("LA PARTIDA HA TERMINADO!!\n\nEL GANADOR ES:  " + jugadoresPartida[ganador].ToUpper());
                this.Invoke(new Action(() =>
                {
                    this.Close();
                }));
                ganadorp = 0;
            }
            else if (turno == idJugador)
            {
                this.Invoke(new Action(() =>
                {
                    prompt_Lbl.Show();
                    label9.Show();
                    prompt_Lbl.Text = solucionPictionary.ToUpper();
                    Respuesta_Pictionary.Hide();
                    Mensaje_Pictionary_btn.Hide();
                }));
                ConsultarServidor("50/8/" + idPartida);
            }
            else if (turno == idJugador + 1)
            {
                this.Invoke(new Action(() =>
                {
                    prompt_Lbl.Hide();
                    label9.Hide();
                    Respuesta_Pictionary.Show();
                    Mensaje_Pictionary_btn.Show();
                }));
            }
            return ganadorp;

        }

        //Funcion que actualiza los labeols que muestran los puntos de cada jugador
        private void actualizarPuntos()
        {
            puntosJ1PictLbl.Text = Convert.ToString(puntos_Pictionary[0]);
            puntosJ2PictLbl.Text = Convert.ToString(puntos_Pictionary[1]);
            if (numJugadores < 4)
            {
                puntosJ4PictLbl.Text = "";

                if (numJugadores < 3)
                    puntosJ3PictLbl.Text = "";
                else
                    puntosJ3PictLbl.Text = Convert.ToString(puntos_Pictionary[2]);
            }
            else
            {
                puntosJ3PictLbl.Text = Convert.ToString(puntos_Pictionary[2]);
                puntosJ4PictLbl.Text = Convert.ToString(puntos_Pictionary[3]);
            }
        }

        //Funcion que cambia el borderstyle de los pictureboxes que indican los colores que se pueden seleccionar
        //para pintar a None
        private void ResetPictureboxBorderStyle()
        {
            boxRed.BorderStyle = BorderStyle.None;
            boxGreen.BorderStyle = BorderStyle.None;
            boxOrange.BorderStyle = BorderStyle.None;
            boxBlack.BorderStyle = BorderStyle.None;
            boxYellow.BorderStyle = BorderStyle.None;
            boxBlue.BorderStyle = BorderStyle.None;
            boxPurple.BorderStyle = BorderStyle.None;
            clearButton.BorderStyle = BorderStyle.None;
        }

        //Funcion que devuelve un string igual que el que recibe como parametro con la primera letra
        //en mayuscula
        public string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        //Cuando se clica un picturebox de color el pinzel se cambia el color al color seleccionado y se avisa al
        //servidor del color que se ha seleccionado
        private void boxColor_Click(object sender, EventArgs e)
        {
            if (turno == idJugador)
            {
                PictureBox p = (PictureBox)sender;
                color = p.BackColor;
                ResetPictureboxBorderStyle();
                p.BorderStyle = BorderStyle.Fixed3D;
                string mensaje = "50/6/" + idPartida + "/" + color.Name + "/" + idJugador + "\0";
                ConsultarServidor(mensaje);
            }
        }

        //Envia un mensaje al servidor avisando que se debe borrar todo lo pintado en el panel
        private void clearButton_Click(object sender, EventArgs e)
        {
            if (turno == idJugador)
            {
                PictureBox p = (PictureBox)sender;
                ResetPictureboxBorderStyle();
                p.BorderStyle = BorderStyle.Fixed3D;
                string mensaje = "50/9/" + idPartida + "/" + color + "/" + idJugador + "\0";
                ConsultarServidor(mensaje);
            }
        }

        //Cada vez que el timer hace un tick envia al servidor la coordenada donde se encuentra el pinzel que 
        //esta pintando
        private void timerPictionary_Tick(object sender, EventArgs e)
        {
            string mensaje = "50/2/" + idPartida + "/" + coordX + "+" + coordY + "/" + idJugador + "\0";
            ConsultarServidor(mensaje);
        }

        //Cuando se presiona el mouse en el panel envia al servidor el primer punto de la linea y empieza el timer que
        //enviara cada 50ms una nueva coordenada donde esta pintando el pinzel
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (turno == idJugador)
            {
                Point p = new Point(e.X, e.Y);
                listOfPoints.Add(p);
                PencilDown = true;
                string mensaje = "50/3/" + idPartida + "/" + coordX + "+" + coordY + "/" + idJugador + "\0";
                ConsultarServidor(mensaje);
                timerPictionary.Start();
            }
        }

        //Cuando se deja de presionar el boton del mouse se para el timer para parar de enviar coordenadas al servidor
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (turno == idJugador)
            {
                PencilDown = false;
                timerPictionary.Stop();
            }
        }

        //Cuando se mueve el mouse encima del panel mientras se presiona el boton del mouse se pinta en el panel
        //Esta funcion solo se aplica al usuario que está dibujando, y los otros reciben el dibujo a través
        //del servidor con las coordenadas que envia el timer tick
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (turno == idJugador)
            {
                Graphics g = panel1.CreateGraphics();
                Point points = new Point(e.X, e.Y);
                coordX = Convert.ToString(e.X);
                coordY = Convert.ToString(e.Y);
                Pen pencil = new Pen(color, 3);

                if (PencilDown)
                {
                    if (listOfPoints.Count > 1)
                        g.DrawLine(pencil, (Point)listOfPoints[listOfPoints.Count - 1], points);
                    listOfPoints.Add(points);
                }
            }
        }

        //Envia al servidor una palabra para añadir al chat
        private void Mensaje_Pictionary_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Respuesta_Pictionary.Text))
            {
                MessageBox.Show("Se necesita un mensaje para escribirlo en el chat.");
            }
            else
            {
                string mensaje = "50/7/" + idJugador + "/" + Convert.ToString(idPartida) + "/" + Respuesta_Pictionary.Text;
                ConsultarServidor(mensaje);
                //Chat.Text = Chat.Text + "\n" + Mensaje.Text;
            }
        }

        // Envia la palabara escrita al hacer Enter
        private void Respuesta_Pictionary_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Mensaje_Pictionary_btn.PerformClick();
            }
        }

        //Timer que calcula cuanto tiempo queda de turno, si se acaba el tiempo de turno de un jugador se cambia 
        //de turno con la funcion changeTurnPictionary()
        //Si se ha acabado el ultimo tuno pone en pantalla quien ha ganado
        private void timerPictionaryPuntos_Tick(object sender, EventArgs e)
        {
            timePictionaryLbl.Text = tiempo--.ToString();
            if (tiempo == 0)
            {
                tiempo = 100;
                timerPictionaryPuntos.Stop();
                timerPictionary.Stop();
                string aviso = "SE HA ACABADO EL TIEMPO!\n\nCAMBIO DE TURNO.";
                this.Invoke(new Action(() =>
                {
                    MostrarAviso(aviso);
                }));
                turno += 1;
                Thread.Sleep(3000);
                this.Invoke(new Action(() =>
                {
                    EsconderAviso();
                }));
                changeTurnPictionary();
            }
        }
    }
}
