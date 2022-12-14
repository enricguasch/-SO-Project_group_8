#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
#include <pthread.h>
//Estructura nexesaria para acceso excluyente
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

int i;
int sockets[100];


//Creamos estructura de datos de una lista de conectados
// Creamos un conectado:
typedef struct {
	char nombre[20];
	int socket;
}Conectado;

typedef struct{
	Conectado conectados[100];
	int num;
}ListaConectados;

typedef struct{
	int ocupacion;
	int numJugadores;
	char jugador1[20];
	char jugador2[20];
	char jugador3[20];
	char jugador4[20];
	int socket1;
	int socket2;
	int socket3;
	int socket4;
}Tpartidas;

typedef Tpartidas TablaPartidas[100];
TablaPartidas miTabla;

//Variables globales mysql
MYSQL *conn;

// Estructura especial para almacenar resultados de consultas 
MYSQL_RES *resultado;
MYSQL_ROW row;


//Inicializamos Lista de Conectados
ListaConectados miLista;

char *p;
//Pasamos la lista por referencia y los parametros de cada conectado!!!
int Pon(ListaConectados *lista, char nombre[20], int socket){
	//A￱ade nuevo conectado, Retorna 0 si ok y -1 si no lo ha podido a￱adir.
	if(lista->num==100)
		return -1; //No podemos a￱adir un conectado
	else{
		pthread_mutex_lock(&mutex); // No me interrumpas ahora
		int pos=DameSocket(lista, socket);
		strcpy(lista->conectados[pos].nombre,nombre);
		pthread_mutex_unlock(&mutex); //Ya puedes interrumpirme
		return 0;
		
	}
}

// Funcion que devuelve el nombre de un conectado a partir de su socket si esta en la lista
int DamePosicion(ListaConectados *lista, char nombre[20]){
	//Devuelvela posicion o -1 si no esta en la lista
	int j = 0;
	int encontrado = 0;
	while ((j<lista->num) && !encontrado){
		if(strcmp(lista->conectados[j].nombre,nombre)==0)
			encontrado = 1;
		if(!encontrado)
			j=j+1;
	}
	if(encontrado)
		return j;
	else
		return -1;
}

int BuscaSocket(ListaConectados *lista, char nombre[20]){
	//Devuelvela socket o -1 si no esta en la lista
	int j = 0;
	int encontrado = 0;
	while ((j<lista->num) && !encontrado){
		if(strcmp(lista->conectados[j].nombre,nombre)==0)
			encontrado = 1;
		if(!encontrado)
			j=j+1;
	}
	if(encontrado)
		return miLista.conectados[j].socket;
	else
		return -1;
}

int BuscaSocket(ListaConectados *lista, char nombre[20]){
	//Devuelvela socket o -1 si no esta en la lista
	int j = 0;
	int encontrado = 0;
	while ((j<lista->num) && !encontrado){
		if(strcmp(lista->conectados[j].nombre,nombre)==0)
			encontrado = 1;
		if(!encontrado)
			j=j+1;
	}
	if(encontrado)
		return miLista.conectados[j].socket;
	else
		return -1;
}

// Funcion que elimina un conectado a partir de su nombre
int Elimina(ListaConectados *lista, char nombre[20]){
	//Retorna 0 si elimina y -1 si el usuario no esta en la lista
	int pos = DamePosicion(lista, nombre); //No le ponemos el & delante. La tendriamos que pasar por referencia pero ya la estamos recibiendo por referencia y es un puntero.
	if (pos==-1)
		return -1;
	else{
		int j;
		pthread_mutex_lock(&mutex);
		for(j=pos;j<lista->num-1;j++)	//Recorremos la lista a partir de la posicion que queremos eliminar y asignamos el siguiente elemento al anterior. Recorremos solo hasta num-1!!!
		{
			//lista->conectados[i] = lista->conectados[i+1];
			strcpy(lista->conectados[j].nombre, lista->conectados[j+1].nombre);
			lista->conectados[j].socket = lista->conectados[j+1].socket;
		}
		i = i-1;
		lista->num = lista->num -1;	//Actualizamos el numero de elementos de la lista!!
		pthread_mutex_unlock(&mutex);
		return 0;
	}
}

int Respuesta_Desconectar(char nombre[20],char respuesta[20]){
	int res = Elimina(&miLista,nombre);
	if(res==0)
	{
		strcpy(respuesta,"0");
		return 1;
	}
	else
	{
		strcpy(respuesta,"-1");
		return 0;
	}
}

// Funcion que proporciona los nombres de todos los conectados separados por el caracter /
void DameConectados(ListaConectados *lista, char conectados[300]){
	//Escrribe en conectados los nombres de todos los conectados separados por /
	//Primero pone el numero de conectados
	// "3/Juan/Maria/Pedro"
	sprintf(conectados, "%d", lista->num);
	int j;
	for(j=0;j<lista->num;j++)
	{
		sprintf(conectados,"%s/%s",conectados,lista->conectados[j].nombre);
	}
}


//Funcion que recibe vector de caracteres con nombres separados por comas y devuelve cadena de caracteres con los sockets de cada uno separados por comas.
void DameSockets(ListaConectados *lista, char nombres[300], char sockets[100]){
	char *p;
	p = strtok(nombres, "/");
	//sprintf(sockets, "");
	sockets[0]='\0';
	while (p!=NULL)
	{
		char nombre[20];
		strcpy(nombre, p);
		int pos = DamePosicion(lista,nombre);
		char socket[3];
		sprintf(socket, "%d",lista->conectados[pos].socket);
		//printf("***********%d\n",lista->conectados[pos].socket);
		sprintf(sockets,"%s%s/",sockets,socket);
		p = strtok(NULL,"/");
	}
}


// FUNCIONES QUE REALIZAN CONSULTAS:

void Conectados(char respuesta[512]){
	char misConectados[512];
	char jugadores[512];
	char listaSockets[100];
	char sock_jug[512];
	char jugadores_final[512];
	sock_jug[0]='\0';
	jugadores[0]='\0';
	jugadores_final[0]='\0';
	
	DameConectados(&miLista, misConectados);
	printf("Resultado: %s\n",misConectados);
	
	//Sacamos primer numero, no lo queremos para la peticion
	char *p1;
	p1 = strtok(misConectados, "/");
	int n = atoi(p1);
	int j;
	
	for(j=0;j<n;j++)
	{
		char nombre[20];
		p1 = strtok(NULL, "/");
		strcpy(nombre, p1);
		sprintf(jugadores,"%s%s/",jugadores, nombre);
	}
	strcpy(jugadores_final,jugadores);
	printf("RESPUESTA:%s\n",jugadores);
	DameSockets(&miLista,jugadores,listaSockets);
	
	printf("SOCKETS: %s\n", listaSockets);
	
	char *p2;
	p2=strtok(listaSockets,"/");
	while(p2!=NULL)
	{
		int socket=atoi(p2);
		printf("SOCKET JUG: %d\n", socket);
		sprintf(sock_jug,"%s%d/",sock_jug, socket);
		p2 = strtok(NULL, "/");
	}
	printf("SOCKETS JUGADORES: %s\n", sock_jug);
	printf("JUGADORES FINAL: %s\n",jugadores_final);
	sprintf(respuesta,"%d/%s%s",n,jugadores_final,sock_jug);
	printf("Respuesta: %s\n",respuesta);
	
}

int LogIn (char pword[30], char nombre[30]){
	int err;
	char consulta[512];
	
	//Tenemos el nombre y la contrase￯﾿ﾱa, comprobamos si hay un usuario que coincida.
	sprintf(consulta,"SELECT COUNT(jugadores.id) FROM (partidas,jugadores,registro) WHERE "
			"jugadores.username='%s' AND jugadores.pword='%s';",nombre,pword);
	
	err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//recojo el resultado de la consulta.
	resultado = mysql_store_result (conn);
	// El resultado es un numero en la primera fila
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)
		return -2;
	else
	{
		if (atoi(row[0])==0)
			return -1;
		else
			return 0;
	}
	
}
//Busca en la base de datos el usuario y contrase￱a para comprobar si 
//el usuario existe.
void Respuesta_LogIn(char pword[30], char nombre[30], int sock_conn,char respuesta[512]){	
	int err;
	int resultado;	
	int usuario_conectado = 0;
	int j = 0;
	while (j<miLista.num){
		if (strcmp(miLista.conectados[j].nombre, nombre)==0)
			usuario_conectado=1;
		j++;
	}
	if (usuario_conectado==0)
	{
		resultado = LogIn(pword, nombre);
		
		if (resultado==0)
			Pon(&miLista,nombre,sock_conn);
		
	}
	else{
		resultado = 2;
	}
	sprintf(respuesta,"%d",resultado);
	printf("Respuesta: %s\n", respuesta);
}

int Registrar (char pword[30], char nombre[30], char gmail[50]){
	int err;
	char consulta[512];
	int id;
	
	sprintf(consulta,"SELECT COUNT(jugadores.id) FROM (jugadores);");
	
	err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//recojo el resultado de la consulta.
	resultado = mysql_store_result (conn);
	// El resultado es un numero en la primera fila
	row = mysql_fetch_row (resultado);
	
	id = atoi(row[0]) + 1;
	
	
	//Tenemos el nombre y la contrase￯﾿ﾱa, comprobamos si hay un usuario que coincida.
	sprintf(consulta,"INSERT INTO jugadores (id,username,pword,mail) VALUES (%d,'%s','%s','%s');",id,nombre,pword,gmail);
	
	err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	else
		return 0;	
}

//Inserta en la base de datos un nuevo usuario
void Respuesta_Registrar(char pword[30], char nombre[30], char gmail[50], int sock_conn,char respuesta[512]){
	char consulta[512];
	int resultado;
	
	int usuario_conectado = 0;
	int j = 0;
	while (j<miLista.num){
		if (strcmp(miLista.conectados[j].nombre, nombre)==0)
			usuario_conectado=1;
		j++;
	}
	if (usuario_conectado==0)
	{
		// A￱adimos Usuario a la lista:
		Pon(&miLista,nombre,sock_conn);
		
		resultado = Registrar(pword, nombre, gmail);
		
	}
	else
	{
		strcpy(respuesta, "2");
	}
	sprintf(respuesta,"%d",resultado);
	printf("Respuesta: %s\n", respuesta);
	// Enviamos la respuesta
}

//Recibe un usuario y consulta en la base de datos cuantas partidas ha ganado
void PartidasGanadas(char dato[20], char respuesta[512]){
	int err;
	
	int num;
	char consulta[512];
	
	// consulta: Cuantas partidas ha ganado el ususario?
	sprintf(consulta,"SELECT COUNT(partidas.id) FROM (partidas,jugadores,registro) WHERE "
			"partidas.resultado='Victoria' AND jugadores.username='%s' AND "
			"jugadores.id=registro.id_j AND registro.id_p=partidas.id;",dato);
	
	err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//recojo el resultado de la consulta.
	resultado = mysql_store_result (conn);
	// El resultado es un numero en la primera fila
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)
		strcpy(respuesta,"No se han obtenido datos en la consulta\n");
	else
		strcpy(respuesta,row[0]);
	printf("Respuesta: %s\n", respuesta);
	// Enviamos la respuesta
}
//Encuentra los usuarios que han ganado mas partidas
void MejorJugador(char respuesta[512]){
	int err;
	// Estructura especial para almacenar resultados de consultas
	MYSQL_RES *un_jugadores;
	MYSQL_RES *id_partidas_jugadori;
	MYSQL_ROW row_un_j;
	MYSQL_ROW row_id_partidas;
	MYSQL_RES *resultado_partidas_jugadori;
	MYSQL_ROW row_resultado_partidas;
	
	char nombre[20];
	char consulta1[512];
	
	
	// Agafem el ID de tots els jugadors amb una consulta
	strcpy(consulta1, "SELECT jugadores.username FROM jugadores");
	
	err = mysql_query(conn, consulta1);
	if (err!=0){
		printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	
	// Recollim el resultat que es una estructura
	un_jugadores = mysql_store_result (conn);
	
	// Agafa la primera fila de l'estructura anterior
	row_un_j = mysql_fetch_row (un_jugadores);
	
	char resultado[500];	// Declarem aquest char per emmagatzemar el resultat i despres poder llegir-lo.
	strcpy(resultado,"");
	
	if (row_un_j == NULL)
		printf ("No se han obtenido datos en la consulta\n");	// Seguretat per si la consulta no es fa be
	else{
		while (row_un_j !=NULL) 	// Recorrerem la taula de id de jugadors que ens han passat, cada cop mirarem una fila, un id.
		{
			// A partir de l'id del jugador que anem mirant cada cop que fem el bucle, consultem a la bbdd que ens doni el id de les partides que ha jugat aquest jugador
			char consulta2[100];
			strcpy(consulta2, "SELECT partidas.id FROM (partidas,registro,jugadores) WHERE jugadores.username ='");
			strcat(consulta2, row_un_j[0]); // Aqui es on afegim a la consulta el id del jugador que estem analitzant 
			strcat(consulta2, "' AND jugadores.id = registro.id_j AND registro.id_p = partidas.id");
			
			err = mysql_query(conn, consulta2);
			if (err!=0){
				printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
				exit(1);
			}
			// recollim els resultats i fem el mateix que abans, ara tindrem una estructura de la qual anirem extraient el id de cada partida.
			id_partidas_jugadori = mysql_store_result (conn);
			row_id_partidas = mysql_fetch_row (id_partidas_jugadori);
			if (row_id_partidas != NULL){
				sprintf(resultado, "%s%s/",resultado,row_un_j[0]);
				int ganadas = 0;
				while (row_id_partidas !=NULL){
					char consulta3[100];
					strcpy(consulta3, "SELECT partidas.resultado FROM partidas WHERE partidas.id =");
					strcat(consulta3, row_id_partidas[0]);
					
					err = mysql_query(conn, consulta3);
					if (err!=0){
						printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
						exit(1);
					}
					
					resultado_partidas_jugadori = mysql_store_result (conn);
					row_resultado_partidas = mysql_fetch_row (resultado_partidas_jugadori);
					while (row_resultado_partidas != NULL)
					{
						if (strcmp(row_resultado_partidas[0],"Victoria") == 0)
						{
							ganadas = ganadas + 1;
						}
						row_resultado_partidas = mysql_fetch_row (resultado_partidas_jugadori);
					}
					row_id_partidas = mysql_fetch_row (id_partidas_jugadori);
				}
				sprintf(resultado,"%s%d*",resultado,ganadas);
				
			}
			// obtenemos la siguiente fila
			row_un_j = mysql_fetch_row (un_jugadores);
		}
		
		char res[200];
		char *p1;
		char *p2;
		int record = 0;
		
		strcpy(res, resultado);
		p1 = strtok(res,"/");
		while (p1!=NULL)
		{
			int id_jugador = atoi(p1);
			p1 = strtok(NULL, "*");
			int partidas_ganadas = atoi(p1);
			if (partidas_ganadas >= record)
			{
				record = partidas_ganadas;
			}
			p1 = strtok(NULL, "/");	
		}
		
		strcpy(respuesta, "Mejores jugadores:\n\n");
		
		p2 = strtok(resultado,"/");
		while (p2!=NULL)
		{
			strcpy(nombre,p2);
			p2 = strtok(NULL, "*");
			int partidas_ganadas = atoi(p2);
			if (partidas_ganadas == record)
			{
				sprintf(respuesta, "%s\t- %s\n", respuesta, nombre);
			}
			p2 = strtok(NULL, "/");	
		}
		
	}
	printf("Respuesta: %s\n", respuesta);
	// Enviamos la respuesta
}

//Recibe un numero de partida y consulta en la BD que usuarios participaron
void JugadoresEnPartida(char dato[20], char respuesta[512]){
	int err;
	char consulta[512];
	
	sprintf(consulta, "SELECT jugadores.username FROM (jugadores,partidas,registro) WHERE partidas.id = %s AND partidas.id = registro.id_p AND jugadores.id = registro.id_j;",dato);
	
	err=mysql_query (conn,consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	resultado = mysql_store_result (conn);
	
	row = mysql_fetch_row (resultado);
	
	
	int j = 1;
	if (row == NULL)
		strcpy(respuesta,"No se han obtenido datos en la consulta\n");
	else
		sprintf(respuesta,"Jugadores de la partida %s:\n", dato);
	while (row !=NULL) {
		sprintf (respuesta,"%s%s\n", respuesta, row[0]);
		row = mysql_fetch_row (resultado);
		j++;
	}
	printf("Respuesta: %s\n", respuesta);
	// Enviamos la respuesta
	
}


void InicializarTabla (TablaPartidas tabla){
	int j=0;
	while (j<100)
		tabla[j].ocupacion = 0;
}

// Pasamos parametros y retorna posicion donde ha a￱adido partida o -1 si no lo ha podido hacer
int AnadirPartida(TablaPartidas tabla, int numJugadores, char jugador1[20], int socket1, char jugador2[20], int socket2, char jugador3[20], int socket3 ,char jugador4[20], int socket4){
	int j = 0;
	int colocado = 0;
	while ((j<100) && (!colocado))
	{
		if (tabla[j].ocupacion == 0)
			colocado=1;
		else
			j=j+1;
	}
	if (colocado==1)
	{
		tabla[j].ocupacion = 1;
		strcpy(tabla[j].jugador1, jugador1);
		strcpy(tabla[j].jugador2, jugador2);
		strcpy(tabla[j].jugador3, jugador3);
		strcpy(tabla[j].jugador4, jugador4);
		tabla[j].socket1 = socket1;
		tabla[j].socket2 = socket2;
		tabla[j].socket3 = socket3;
		tabla[j].socket4 = socket4;
		tabla[j].numJugadores = numJugadores;
		return j;
	}
	else
		return -1;
}




int InvitarJugadores(char datos[512], char nombre[20], char invitacion[512]){
	int num;
	char jugador1[20];
	int socket1;
	char jugador2[20];
	int socket2;
	char jugador3[20];
	int socket3;
	char jugador4[20];
	int socket4;
	char datos_cpy[512];
	
	strcpy(jugador1, nombre);
	strcpy(datos_cpy, datos);
	
	p=strtok(datos_cpy, "/");
	num = atoi(p)+1;
	
	p = strtok(NULL, "/");
	strcpy(jugador2, p);
	
	p=strtok(NULL, "/");
	if (p!=NULL)
	{
		strcpy(jugador3, p);
		p=strtok(NULL,"/");
		if (p!=NULL)
			strcpy(jugador4, p);
		else
			strcpy(jugador4, "");
	}
	else
	{
		strcpy(jugador3, "");
		strcpy(jugador4,"");
	}	
	
	// Si no hay jugador, devuelve -1 y ya nos sirve
	socket1 = BuscaSocket(&miLista,jugador1);
	socket2 = BuscaSocket(&miLista,jugador2);
	socket3 = BuscaSocket(&miLista,jugador3);
	socket4 = BuscaSocket(&miLista,jugador4);
	
	// De momento a￱adimos la partida a la tabla, si no aceptan, la quitaremos, devuelve la posicion
	int id =  AnadirPartida(miTabla,num, jugador1, socket1, jugador2, socket2, jugador3, socket3, jugador4, socket4);
	
	// Debemos enviar a los jugadores que nos han pedido la consulta de si quieren jugar o no
	sprintf(invitacion, "8*%s/%d", jugador1, id);
	printf("Invitacion: %s\n", invitacion);
	if (socket2>0)
		write(socket2, invitacion, strlen(invitacion));
	if (socket3>0)
		write(socket3, invitacion, strlen(invitacion));
	if (socket4>0)
		write(socket4, invitacion, strlen(invitacion));
}

void RespuestaInvitacion(char info[512]){
	char respuesta[512];
	char nombre[20];
	char res[5];
	int id;
	p=strtok(info, "/");
	strcpy(nombre, p);
	p=strtok(NULL,"/");
	strcpy(res, p);
	p=strtok(NULL, "/");
	id = atoi(p);
	
	int socket = miTabla[id].socket1;
	
	if (strcmp(res, "SI") == 0)
		sprintf(respuesta, "10*%s/%s/%d",nombre,res,id);
	else
	{
		miTabla[id].ocupacion=0;
		sprintf(respuesta, "10*%s/%s/%d",nombre,res,id);
	}
	printf("Respuesta: %s\n", respuesta);
	write(socket, respuesta, strlen(respuesta));
}

void ComunicarCliente (char info[512]){
	int id;
	int codigo;
	p=strtok(info, "/");
	codigo = atoi(p);
	if (codigo==1) // Queremos decir a todos los jugadores de la partida que se puede jugar y se abre el tablero
	{
		char abrirTablero[50];
		p=strtok(NULL,"/");
		id = atoi(p);
		sprintf(abrirTablero,"12*%d",id);
		int numJugadores = miTabla[id].numJugadores;
		printf("abrirTablero: %s\n", abrirTablero);
		if (numJugadores==2)
		{
			write(miTabla[id].socket2, abrirTablero, strlen(abrirTablero));
		}
		else if (numJugadores==3)
		{
			write(miTabla[id].socket2, abrirTablero,strlen(abrirTablero));
			write(miTabla[id].socket3, abrirTablero,strlen(abrirTablero));
		}
		else if (numJugadores==4)
		{
			write(miTabla[id].socket2, abrirTablero,strlen(abrirTablero));
			write(miTabla[id].socket3, abrirTablero,strlen(abrirTablero));
			write(miTabla[id].socket4, abrirTablero,strlen(abrirTablero));
		}
	}
}

void EscribirChat(char info[512]){
	char emisor[20];
	int id;
	char mensaje[300];
	char EnviarMensaje[512];
	p = strtok(info, "/");
	strcpy(emisor, p);
	p=strtok(NULL,"/");
	id = atoi(p);
	p=strtok(NULL,"/");
	strcpy(mensaje, p);
	sprintf(EnviarMensaje,"14*%s: %s",emisor, mensaje);
	int numJugadores = miTabla[id].numJugadores;
	printf("EnviarMensaje: %s\n", EnviarMensaje);
	if (numJugadores==2)
	{
		write(miTabla[id].socket1, EnviarMensaje, strlen(EnviarMensaje));
		write(miTabla[id].socket2, EnviarMensaje, strlen(EnviarMensaje));
	}
	else if (numJugadores==3)
	{
		write(miTabla[id].socket1, EnviarMensaje, strlen(EnviarMensaje));
		write(miTabla[id].socket2, EnviarMensaje, strlen(EnviarMensaje));
		write(miTabla[id].socket3, EnviarMensaje, strlen(EnviarMensaje));
	}
	else if (numJugadores==4)
	{
		write(miTabla[id].socket1, EnviarMensaje, strlen(EnviarMensaje));
		write(miTabla[id].socket2, EnviarMensaje, strlen(EnviarMensaje));
		write(miTabla[id].socket3, EnviarMensaje, strlen(EnviarMensaje));
		write(miTabla[id].socket4, EnviarMensaje, strlen(EnviarMensaje));
	}
	
}
void *AtenderCliente (void *socket)
{
	int sock_conn;
	int *s;
	s = (int *) socket;
	sock_conn=*s;
	char peticion[512];
	char notificacion[512];
	char invitacion[512];
	char respuesta[512];
	char mensaje[512];
	char nombre[30];
	char pwrd[30];
	char email[50];
	char dato[20];
	char info[512];
	char info2[512];
	char info3[512];
	char info4[512];
	int ret;
	// Bucle de atencion al cliente
	int terminar = 0;
	while (terminar == 0)
	{
		// Ahora recibimos su peticion
		ret=read(sock_conn,peticion, sizeof(peticion));
		printf ("Recibida una peticion\n");
		// Tenemos que a￱adirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		peticion[ret]='\0';
		//Escribimos la peticion en la consola
		printf ("La peticion es: %s\n",peticion);
		
		//Interpretamos la peticion
		p = strtok(peticion, "/");
		int codigo =  atoi (p);
		printf("Codigo: %d\n",codigo);
		
		if (codigo==1 || codigo==2)
		{			
			p=strtok(NULL,"/");
			strcpy(nombre,p);
			p=strtok(NULL,"/");
			strcpy(pwrd,p);
			if (codigo==2)
			{
				p = strtok(NULL,"/");
				strcpy(email,p);
			}
			
		}
		else if (codigo==3 || codigo==5)
		{
			p=strtok(NULL,"/");
			strcpy(dato,p);
		}
		
		else if (codigo==7)
		{
			p=strtok(NULL, "\0");
			strcpy(info, p);
			printf("Peticion: %s\n", info);
		}
		else if (codigo==9)
		{
			p=strtok(NULL, "\0");
			strcpy(info2, p);
			printf("Peticion: %s\n", info2);
		}
		else if (codigo==11)
		{
			p=strtok(NULL, "\0");
			strcpy(info3, p);
			printf("Peticion: %s\n", info3);
		}
		else if (codigo==13)
		{
			p=strtok(NULL, "\0");
			strcpy(info4, p);
			printf("Peticion: %s\n", info4);
		}
		
		if (codigo == 0)
		{		
			p = strtok(NULL,"/");
			strcpy(nombre,p);
			//terminar = Respuesta_Desconectar(nombre,respuesta);
			Elimina(&miLista,nombre);
			terminar = 1;
			
		}
		else if (codigo == 1)
		{
			Respuesta_LogIn(pwrd,nombre,sock_conn,respuesta);
		}
		else if (codigo == 2)
		{
			Respuesta_Registrar(pwrd,nombre,email,sock_conn,respuesta);
		}
		else if (codigo == 3) // consulta: Cuantas partidas ha ganado el jugador elegido?
		{
			PartidasGanadas(dato,respuesta);
		}
		else if (codigo == 4)
		{
			MejorJugador(respuesta);
		}
		else if (codigo==5)//Nueva peticion: Decir si es alto o bajo
		{
			JugadoresEnPartida(dato,respuesta);
		}
		else if (codigo==7)
		{
			InvitarJugadores(info,nombre,invitacion);
		}
		else if (codigo == 9)
		{
			RespuestaInvitacion(info2);
		}
		else if (codigo == 11)
		{
			ComunicarCliente(info3);
		}
		else if (codigo == 13)
		{
			EscribirChat(info4);
		}
		if(codigo!=7 && codigo !=9 && codigo!=11 && codigo !=13)
		{
			sprintf(mensaje,"%d*%s",codigo,respuesta);
			printf("Mensaje enviado: %s\n",mensaje);
			write (sock_conn,mensaje,strlen(mensaje));
		}
		
		if ((codigo==0)||(codigo==1)||(codigo==2))
		{
			Conectados(respuesta);
			sprintf(notificacion,"6*%s",respuesta);
			int j;
			for (j=0; j<miLista.num; j++)
			{
				printf("Noti enviado: %s\n",notificacion);
				write (miLista.conectados[j].socket,notificacion, strlen(notificacion));
			}
		}
		strcpy(respuesta,"");
		strcpy(mensaje,"");
	}
	// Se acabo el servicio para este cliente
	close(sock_conn); 
}

int main(int argc, char *argv[])
{	
	//Creamos una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexi??n: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexion
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "bd", 0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexi??n: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	miLista.num=0;
	//INICIALIZAR CONEXION CON EL CLIENTE
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	
	// Abrimos el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Hacemos el bind al port
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el  ismo port que usa el cliente
	serv_adr.sin_port = htons(9020);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 4) < 0)
		printf("Error en el Listen");
	
	pthread_t thread;
	i = 0;
	for(;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		miLista.conectados[miLista.num].socket = sock_conn;
		//sock_conn es el socket que usaremos para este cliente
		// Crear thred y decirle que tiene que hacer:
		pthread_create (&thread, NULL, AtenderCliente ,&miLista.conectados[miLista.num].socket);	//No podem fer que els vectors siguin infinits i arriba un moment que no podem col￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾷlocar els sockets a la posicio que tocaria
		i = i+1;
	}
	mysql_close (conn);
	exit(0);
}
