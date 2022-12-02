
From Client to Server

Services

0 - Desconnexión
	"0/nombre""

1 - LogIn
	"1/username/password"

2 -	Register
	"1/username/password/email"

3 - Get Numero de Partidas jugadas por {jugador}
	"3/jugador"

4 -	Get Mejor Jugador
	"4/"

5 - Get Jugadores de una partida
	 "5/idPartida"

6 -

7 - Get jugadores de la partida
	"7/numInvitados/jugador2/jugador3/jugador4"

8 -

9 - Responder Invitación
	"9/nombreCliente/SI/idPartida"
	"9/nombreCliente/NO/idPartida"

10 -



From Server to Client

Notifications

1 - Respuesta a LogIn

2 - Respuesta a Registro

3 - Respuesta de partidas ganadas

4 - Respuesta de mejor jugador

5 - Respuesta de jugadores en partida

6 - Actualizar DataGridView con ListaConectados

7 -

8 - Recibimos Invitación de partida de otro cliente

9 - Recibimos Respuesra de Invitacion (SI/NO) de un cliente invitado

10 -

11 - 

12 - Orden de empezar la partida







Hide TabControl Tabs
	1. Insert Tabcontrol into a form, the default name being tabcontrol1.
	2. Ensure that tabcontrol1 is selected in the Properties pane in visual studio and change the following properties:
	a. Set Appearance to Buttons
	b. Set ItemSize 0 for Width and 1 for Height
	c. Set Multiline to True
	d. Set SizeMode to Fixed