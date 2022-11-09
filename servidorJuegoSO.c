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

//Inicializamos Lista de Conectados
ListaConectados miLista;

//Pasamos la lista por referencia y los par￡metros de cada conectado!!!
int Pon(ListaConectados *lista, char nombre[20], int socket){
	//A￱ade nuevo conectado, Retorna 0 si ok y -1 si no lo ha podido a￱adir.
	if(lista->num==100)
		return -1; //No podemos a￱adir un conectado
	else{
		pthread_mutex_lock(&mutex); // No me interrumpas ahora
		strcpy(lista->conectados[lista->num].nombre,nombre);
		lista->conectados[lista->num].socket = socket;
		lista->num++;
		pthread_mutex_unlock(&mutex); //Ya puedes interrumpirme
		return 0;
		
	}
}

// Funcion que devuelve el nombre de un conectado a partir de su socket si esta en la lista
int DamePosicion(ListaConectados *lista, char nombre[20]){
	//Devuelvela posicion o -1 si no esta en la lista
	int i = 0;
	int encontrado = 0;
	while ((i<lista->num) && !encontrado){
		if(strcmp(lista->conectados[i].nombre,nombre)==0)
			encontrado = 1;
		if(!encontrado)
			i=i+1;
	}
	if(encontrado)
		return i;
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
		int i;
		pthread_mutex_lock(&mutex);
		for(i=pos;i<lista->num-1;i++)	//Recorremos la lista a partir de la posicion que queremos eliminar y asignamos el siguiente elemento al anterior. Recorremos solo hasta num-1!!!
		{
			//lista->conectados[i] = lista->conectados[i+1];
			strcpy(lista->conectados[i].nombre, lista->conectados[i+1].nombre);
			lista->conectados[i].socket = lista->conectados[i+1].socket;	
		}
		lista->num--;	//Actualizamos el numero de elementos de la lista!!
		pthread_mutex_unlock(&mutex);
		return 0;
	}
}

// Funcion que proporciona los nombres de todos los conectados separados por el caracter /
void DameConectados(ListaConectados *lista, char conectados[300]){
	//Escrribe en conectados los nombres de todos los conectados separados por /
	//Primero pone el numero de conectados
	// "3/Juan/Maria/Pedro"
	sprintf(conectados, "%d", lista->num);
	int i;
	for(i=0;i<lista->num;i++)
	{
		sprintf(conectados,"%s/%s",conectados,lista->conectados[i].nombre);
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

void Conectados(int sock_conn){
	char misConectados[512];
	char jugadores[512];
	char sockets[100];
	char respuesta[512];
	char sock_jug[512];
	char jugadores_final[512];
	sock_jug[0]='\0';
	jugadores[0]='\0';
	jugadores_final[0]='\0';
	
	DameConectados(&miLista, misConectados);
	printf("Resultado: %s\n",misConectados);
	
	//Sacamos primer numero, no lo queremos para la peticion
	char *p1;
/*	char *p2;*/
	p1 = strtok(misConectados, "/");
	int n = atoi(p1);
/*	int i;*/
/*	for(i=0;i<n;i++)*/
/*	{*/
/*		p1=strtok(NULL,"/");*/
/*		char nombre[30];*/
/*		strcpy(nombre,p1);*/
/*		printf("NOMBRE: %s\n",nombre);*/
/*		sprintf(jugadores,"%s%s/",jugadores,nombre);*/
/*	}*/
	
/*	printf("Nombres: %s\n", jugadores);*/
	// Pedimos sockets de cada jugador conectado:
/*	DameSockets(&miLista, jugadores, sockets);*/
/*	printf("Sockets: %s\n", sockets);*/
	int i;
	//strcpy(respuesta, "Jugadores Conectados:\n\n");
	for(i=0;i<n;i++)
	{
		char nombre[20];
		p1 = strtok(NULL, "/");
		strcpy(nombre, p1);
		sprintf(jugadores,"%s%s/",jugadores, nombre);
	}
	strcpy(jugadores_final,jugadores);
	printf("RESPUESTA:%s\n",jugadores);
	DameSockets(&miLista,jugadores,sockets);

	printf("SOCKETS: %s\n", sockets);
	
	
	char *p2;
	p2=strtok(sockets,"/");
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
	
	//Enviamos la respuesta
	write(sock_conn,respuesta,strlen(respuesta));
}


void LogIn(char *p, char nombre[30], int sock_conn)
	//Busca en la base de datos el usuario y contrase￱a para comprobar si 
	//el usuario existe.
{

	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta[512];
	char respuesta[512];
	char pword[20];
	char gmail[50];
	
	//Creo una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//inicializar la conexion con la base de datos
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "bd",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	// Ya tenemos el nombre
	// A￱adimos Usuario a la lista:
	Pon(&miLista,nombre,sock_conn);
	
	p = strtok( NULL, "/");
	strcpy(pword, p);
	
	//Tenemos el nombre y la contrase￱a, comprobamos si hay un usuario que coincida.
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
		strcpy(respuesta,"No se han obtenido datos en la consulta\n");
	else
	{
		if (atoi(row[0])==0)
			strcpy(respuesta,"1");
		else
			strcpy(respuesta,"0");
	}
	
	printf("Respuesta: %s\n", respuesta);
	// Enviamos la respuesta
	write (sock_conn,respuesta, strlen(respuesta));
	// cerrar la conexion con el servidor MYSQL
	mysql_close (conn);
}

void Registrar(char *p, char nombre[30], int sock_conn)
	//Inserta en la base de datos un nuevo usuario
{
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta[512];
	char respuesta[512];
	char pword[20];
	char gmail[50];
	int id;
	
	//Creo una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//inicializar la conexion con la base de datos
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "bd",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	// A￱adimos Usuario a la lista:
	Pon(&miLista,nombre,sock_conn);
	
	p = strtok( NULL, "/");
	strcpy(pword, p);
	
	p = strtok( NULL, "/");
	strcpy(gmail, p);
	
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
	
	
	//Tenemos el nombre y la contrase￱a, comprobamos si hay un usuario que coincida.
	sprintf(consulta,"INSERT INTO jugadores (id,username,pword,mail) VALUES (%d,'%s','%s','%s');",id,nombre,pword,gmail);
	
	err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		strcpy(respuesta,"1");
		exit (1);
	}
	else
		strcpy(respuesta,"0");	
	
	printf("Respuesta: %s\n", respuesta);
	// Enviamos la respuesta
	write (sock_conn,respuesta, strlen(respuesta));
	// cerrar la conexion con el servidor MYSQL
	mysql_close (conn);
}

void PartidasGanadas(char *p, int sock_conn)
	//Recibe un usuario y consulta en la base de datos cuantas partidas ha ganado
{
	MYSQL *conn;
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	int num;
	char nombre[20];
	char consulta[400];
	char respuesta[512];
	//Creo una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexion
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "bd",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	p = strtok( NULL, "/");
	strcpy (nombre, p);
	
	// consulta: Cuantas partidas ha ganado el ususario?
	sprintf(consulta,"SELECT COUNT(partidas.id) FROM (partidas,jugadores,registro) WHERE "
			"partidas.resultado='Victoria' AND jugadores.username='%s' AND "
			"jugadores.id=registro.id_j AND registro.id_p=partidas.id;",nombre);
	
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
	write (sock_conn,respuesta, strlen(respuesta));
	
	mysql_close (conn);
}

void MejorJugador(char *p, int sock_conn)
	//Encuentra los usuarios que han ganado mas partidas
{
	//Conector para acceder al servidor de bases de datos 
	MYSQL *conn;
	int err;
	// Estructura especial para almacenar resultados de consultas
	MYSQL_RES *un_jugadores;
	MYSQL_RES *id_partidas_jugadori;
	MYSQL_ROW row_un_j;
	MYSQL_ROW row_id_partidas;
	MYSQL_RES *resultado_partidas_jugadori;
	MYSQL_ROW row_resultado_partidas;
	
	char respuesta[512];
	char nombre[20];

	//Creo una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//inicializar la conexion con la base de datos
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "bd",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	// Agafem el ID de tots els jugadors amb una consulta
	char consulta1[100];
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
	write (sock_conn,respuesta, strlen(respuesta));
	mysql_close (conn);
}

void JugadoresEnPartida(char *p,int sock_conn)
	//Recibe un numero de partida y consulta en la BD que usuarios participaron
{
	MYSQL *conn;
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta [400];
	char respuesta[512];
	char num[3];
	
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
	
	p = strtok( NULL, "/");
	strcpy(num,p);
	
	sprintf(consulta, "SELECT jugadores.username FROM (jugadores,partidas,registro) WHERE partidas.id = %d AND partidas.id = registro.id_p AND jugadores.id = registro.id_j;",atoi(num));
	
	err=mysql_query (conn,consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	resultado = mysql_store_result (conn);
	
	row = mysql_fetch_row (resultado);
	
	
	int i = 1;
	if (row == NULL)
		strcpy(respuesta,"No se han obtenido datos en la consulta\n");
	else
		sprintf(respuesta,"Jugadores de la partida %s:\n", num);
	while (row !=NULL) {
		sprintf (respuesta,"%s%s\n", respuesta, row[0]);
		row = mysql_fetch_row (resultado);
		i++;
	}
	printf("Respuesta: %s\n", respuesta);
	// Enviamos la respuesta
	write (sock_conn,respuesta, strlen(respuesta));
	mysql_close (conn);
}

void *AtenderCliente (void *socket)
{
	int sock_conn;
	int *s;
	s = (int *) socket;
	sock_conn=*s;
	char peticion[512];
	char respuesta[512];
	int ret;
	// Bucle de atencion al cliente
	int terminar = 0;
	while (terminar == 0)
	{
		// Ahora recibimos su peticion
		ret=read(sock_conn,peticion, sizeof(peticion));
		printf ("Recibida una petici￳n\n");
		// Tenemos que a￱adirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		peticion[ret]='\0';
		//Escribimos la peticion en la consola
		printf ("La petici￳n es: %s\n",peticion);
		
		//Interpretamos la petici￳n
		char *p = strtok(peticion, "/");
		int codigo =  atoi (p);
		
		char nombre[30];
		if (codigo==1)
		{
			p=strtok(NULL,"/");
			strcpy(nombre,p);
		}
		else if (codigo==2)
		{
			p=strtok(NULL,"/");
			strcpy(nombre,p);
		}
		
		
		if (codigo == 0)
		{
			Elimina(&miLista,nombre);			
			terminar = 1;
		}
		else if (codigo == 1)
		{
			LogIn(p,nombre,sock_conn);
		}
		else if (codigo == 2)
		{
			Registrar(p,nombre,sock_conn);
		}
		else if (codigo == 3) // consulta: Cuantas partidas ha ganado el jugador elegido?
		{
			PartidasGanadas(p,sock_conn);
		}
		else if (codigo == 4)
		{
			MejorJugador(p,sock_conn);
		}
		else if (codigo==5)//Nueva peticion: Decir si es alto o bajo
		{
			JugadoresEnPartida(p,sock_conn);
		}
		else if (codigo==6) //Dame lista de conectados:
		{
			Conectados(sock_conn);
		}
	}
	// Se acabo el servicio para este cliente
	close(sock_conn); 
}

int main(int argc, char *argv[])
{	
	miLista.num=0;
	//INICIALIZAR CONEXIￓN CON EL CLIENTE
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
	serv_adr.sin_port = htons(9220);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 4) < 0)
		printf("Error en el Listen");
	int sockets[100];
	pthread_t thread;
	int i = 0;
	for(;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		sockets[i] = sock_conn;
		//sock_conn es el socket que usaremos para este cliente
		// Crear thred y decirle que tiene que hacer:
		pthread_create (&thread, NULL, AtenderCliente ,&sockets[i]);	//No podem fer que els vectors siguin infinits i arriba un moment que no podem col￯ﾾﾷlocar els sockets a la posicio que tocaria
		i = i+1;
	}
	exit(0);
}

