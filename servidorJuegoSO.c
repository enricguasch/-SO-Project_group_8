//##############################   CODIGO DEL SERVIDOR DEL JUEGO DE SISTEMAS OPERATIVOS ##########################################################
//############################## Enric Guasch Mesia, Gerard Lopez Lopez, Pau Gimenez Bosch #######################################################

#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
#include <pthread.h>
#include <time.h>

// --------------------------- VARIABLES GLOBALES ---------------------------

//Estructura necesaria para garantizar acceso excluyente
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

int puerto=50073;
int idSiguientePartida = 1;
int sockets[100];
char *p;

//Variables globales mysql
MYSQL *conn;
MYSQL_RES *resultado;
MYSQL_ROW row;


// --------------------------- ESTRUCTURAS ---------------------------
// Creeacion de la estructura de un conectado
typedef struct {
	char nombre[20];
	int socket;
}Conectado;

// Creacion de la estructura de datos de una lista de conectados
typedef struct{
	Conectado conectados[100];
	int num;
}ListaConectados;

//Inicializamos lista de conectados
ListaConectados miLista;

// Creacion de la estructura de datos de una tabla de partidas
typedef struct{
	int ocupacion;
	int idPartida;
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

// Inicializamos tabla de partidas
typedef Tpartidas TablaPartidas[100];
TablaPartidas miTabla;
void InicializarTabla (TablaPartidas tabla){
	int j=0;
	while (j<100)
		tabla[j].ocupacion = 0;
}



// ################################# DEFINICION DE FUNCIONES #####################################################

// --------------------------- FUNCIONES DE LISTA DE CONECTADOS --------------------------------------------------

// Funcion que devuelve la posicion de un conectado a partir de su socket. 
// Devuelve la posicion si se encuentra en la lista o -1 si el usuario no esta en la lista.
int DameSocket(ListaConectados *lista, int socket){
	int j = 0;
	int encontrado = 0;
	while ((j<lista->num) && !encontrado){
		if(lista->conectados[j].socket==socket)
			encontrado = 1;
		if(!encontrado)
			j=j+1;
	}
	if(encontrado)
		return j;
	else
		return -1;
}

// Funcion que pone un conectado a la lista si le pasamos el nombre y el socket.
// Devuelve 0 si se a￯﾿ﾱade con exito o -1 si no se puede a￯﾿ﾱadir.
int Pon(ListaConectados *lista, char nombre[20], int socket){
	if(lista->num==100)
		return -1; //No podemos a￯﾿ﾃ￯ﾾﾱadir un conectado
	else{
		pthread_mutex_lock(&mutex); // Impedimos la interrupci￯﾿ﾃ￯ﾾﾳn del thread quando se accede a una variable global
		int pos=DameSocket(lista, socket);
		printf("Socket test: %d en %d", socket,pos);
		strcpy(lista->conectados[pos].nombre,nombre);
		pthread_mutex_unlock(&mutex); //Desbloqueamos la interrupci￯﾿ﾃ￯ﾾﾳn
		return 0;
		
	}
}

// Funcion que devuelve la posicion de un conectado a partir de su nombre.
// Devuelve la posicion si se encuentra en la lista o -1 si el usuario no esta en la lista.
int DamePosicion(ListaConectados *lista, char nombre[20]){
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

// Funcion que devuelve el socket de un conectado a partir de su nombre.
// Devuelve el socket si se encuentra en la lista o -1 si el usuario no esta en la lista.
int BuscaSocket(ListaConectados *lista, char nombre[20]){
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

// Funcion que elimina un conectado a partir de su nombre.
// Devuelve 0 si encuentra el conectado o -1 si el usuario no esta en la lista.
int Elimina(ListaConectados *lista, char nombre[20]){
	int pos = DamePosicion(lista, nombre); //No le ponemos el & delante. La tendriamos que pasar por referencia pero ya la estamos recibiendo por referencia y es un puntero.
	if (pos==-1){
		return -1;
	}
	else{
		int j;
		pthread_mutex_lock(&mutex);
		for(j=pos;j<lista->num;j++)	//Recorremos la lista a partir de la posicion que queremos eliminar y asignamos el siguiente elemento al anterior. Recorremos solo hasta num-1!
		{
			strcpy(lista->conectados[j].nombre, lista->conectados[j+1].nombre);
			lista->conectados[j].socket = lista->conectados[j+1].socket;
		}
		lista->num = lista->num -1;	//Actualizamos el numero de elementos de la lista!
		pthread_mutex_unlock(&mutex);
		return 0;
	}
}

// Funcion que proporciona los nombres de todos los conectados separados por el caracter / y con el numero de conectados en el inicio.
void DameConectados(ListaConectados *lista, char conectados[300]){
	int cont=0;
	char nombres[300];
	nombres[0]='\0';
	int j;
	for(j=0;j<lista->num;j++)
	{
		if(strcmp(lista->conectados[j].nombre, "")!=0)
		{
			sprintf(nombres,"%s%s/",nombres,lista->conectados[j].nombre);
			cont++;
		}
	}
	printf("Nombres: %s, cont:%d \n",nombres,cont);
	sprintf(conectados,"%d/%s",cont,nombres);
}

// Funcion que recibe vector de caracteres con nombres separados por comas
// Devuelve una cadena de caracteres con los sockets de cada uno separados por comas.
void DameSockets(ListaConectados *lista, char nombres[300], char sockets[100]){
	char *p;
	p = strtok(nombres, "/");
	sockets[0]='\0';
	while (p!=NULL)
	{
		char nombre[20];
		strcpy(nombre, p);
		int pos = DamePosicion(lista,nombre);
		char socket[3];
		sprintf(socket, "%d",lista->conectados[pos].socket);
		sprintf(sockets,"%s%s/",sockets,socket);
		p = strtok(NULL,"/");
	}
}




// --------------------------- FUNCIONES DE LA TABLA DE PARTIDAS -------------------------------------------

// Funcion que anade a la tabla de partidas la partida que le pasamos. 
// Si se puede anadir retorna la posicion donde se ha anadido la partida. Si no retorna -1.
int AnadirPartida(TablaPartidas tabla, int numJugadores,int idp, char jugador1[20], int socket1, char jugador2[20], int socket2, char jugador3[20], int socket3 ,char jugador4[20], int socket4){
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
		tabla[j].idPartida = idp;
		return j;
	}
	else
		return -1;
}

//Funcion que busca en la tabla la partida que tiene el id que se le da como input
//y devuelve la posicin de la tabla donde se encuentra esta partida.
int DamePartida(TablaPartidas tabla, int idPartida)
{
	int j = 0;
	int encontrado = 0;
	while ((j<100) && (!encontrado))
	{
		if(tabla[j].idPartida == idPartida)
		{
			encontrado = 1;
			return j;
		}
		j = j+1;
	}
}



// --------------------------- FUNCIONES QUE ACCEDEN A LA BASE DE DATOS -------------------------------------

// Funcion que le pasamos un nombre y contrasena y busca en la base de datos si existen las credenciales.
// Devuelve -1 si no existe el usuario y 0 si ha podido hacer el login correctamente.
int LogIn (char pword[30], char nombre[30]){
	int err;
	char consulta[512];
	printf("Nombre:%s, pwrd:%s\n", nombre, pword);
	//Tenemos el nombre y la contrasena, comprobamos si hay un usuario que coincida.
	sprintf(consulta,"SELECT COUNT(jugadores.id) FROM (jugadores) WHERE "
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

// Funcion que accede a la base de datos y registra el usuario si no lo est￯﾿ﾃ￯ﾾﾡ. Le asigna un id que corresponde al numero de jugadores en la bbdd + 1.
// Devuelve 0 si lo puede a￯﾿ﾃ￯ﾾﾱadir y -1 si no.
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

// Funcion que recibe el nombre de un usuario y introduce en respuesta el numero de partidas ganadas o indica que no se han obtenido datos en 
// la consulta si el usuario no existe o no ha jugado ninguna partida.
void PartidasGanadas(char dato[20], char respuesta[512]){
	int err;
	
	int num;
	char consulta[512];
	
	// consulta: Cuantas partidas ha ganado el ususario?
	sprintf(consulta,"SELECT COUNT(partidas.id) FROM (partidas,jugadores,registro) WHERE "
			"partidas.ganador='%s' AND jugadores.username='%s' AND "
			"jugadores.id=registro.id_j AND registro.id_p=partidas.id;",dato,dato);
	
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

// Funcion que escribe en respuesta los usuarios que han ganado m￯﾿ﾃ￯ﾾﾡs partidas separados por barras y con el numero de partidas ganadas en el inicio.
// Por ejemplo -> 3/pau/gerard	(Partidas ganadas/jugadores)
void MejorJugador(char info[512], char respuesta[512]){
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
	int juego;
	
	p = strtok(info,"/");
	juego = atoi(p);
	
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
			if (juego!=4)
				sprintf(consulta2, "SELECT partidas.id FROM (partidas,registro,jugadores) WHERE jugadores.username ='%s' AND jugadores.id = registro.id_j AND registro.id_p = partidas.id AND partidas.juego =%d;",row_un_j[0],juego);
			else
				sprintf(consulta2, "SELECT partidas.id FROM (partidas,registro,jugadores) WHERE jugadores.username ='%s' AND jugadores.id = registro.id_j AND registro.id_p = partidas.id;",row_un_j[0]);
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
					strcpy(consulta3, "SELECT partidas.ganador FROM partidas WHERE partidas.id =");
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
						if (strcmp(row_resultado_partidas[0],row_un_j[0]) == 0)
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
		
		p2 = strtok(resultado,"/");
		char respuesta_inicial[512];
		respuesta_inicial[0]='\0';
		while (p2!=NULL)
		{
			strcpy(nombre,p2);
			p2 = strtok(NULL, "*");
			int partidas_ganadas = atoi(p2);
			if (partidas_ganadas == record)
			{
				sprintf(respuesta_inicial, "%s%s/", respuesta_inicial, nombre);
			}
			p2 = strtok(NULL, "/");	
		}
		printf("RECORD: %d\n",record);
		sprintf(respuesta,"%d/%d/%s",record,juego,respuesta_inicial);
	}
	printf("Respuesta: %s\n", respuesta);
	// Enviamos la respuesta
}

// Funcion que recibe dos fechasy ecribe en respuesta los id de las partidas jugadas entre ambas fechas.
void PartidasJugadasEntreFechas(char dateFrom[20], char dateTo[20], char respuesta[512]){
	int err;
	char consulta[512];
	
	printf("TEST 1 | From: %s, To: %s\n",dateFrom,dateTo);
	sprintf(consulta, "SELECT partidas.id FROM partidas WHERE partidas.fecha BETWEEN '%s' AND '%s';",dateFrom,dateTo);
	printf("TEST 2 | %s\n", consulta);
	
	err=mysql_query (conn,consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	printf("TEST 3 | %d\n", err);
	
	resultado = mysql_store_result (conn);
	
	row = mysql_fetch_row (resultado);
	
	int j = 0;
	if (row == NULL)
		strcpy(respuesta,"No se han obtenido datos en la consulta\n");
	
	char respuesta_inicial[512];
	respuesta_inicial[0]='\0';
	while (row !=NULL) {
		printf("TEST 5 | %s\n", row[0]);
		sprintf (respuesta_inicial,"%s%s/", respuesta_inicial, row[0]);
		row = mysql_fetch_row (resultado);
		j++;
	}
	sprintf(respuesta,"%d/%s",j,respuesta_inicial);
	printf("Respuesta: %s\n", respuesta);
	// Enviamos la respuesta
	
}

// Funcion que recibe el id de una partida y escribe en respuesta los jugadores que han jugado esa partida y el numero de jugadores en el inicio.
// Por ejemplo -> 3/pau/gerard/enric (numero jugadores/jugadores)
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
	
	
	int j = 0;
	if (row == NULL)
		strcpy(respuesta,"No se han obtenido datos en la consulta\n");
	
	char respuesta_inicial[512];
	respuesta_inicial[0]='\0';
	while (row !=NULL) {
		sprintf (respuesta_inicial,"%s%s/", respuesta_inicial, row[0]);
		row = mysql_fetch_row (resultado);
		j++;
	}
	sprintf(respuesta,"%d/%s",j,respuesta_inicial);
	printf("Respuesta: %s\n", respuesta);
	// Enviamos la respuesta
	
}

// Funcion que recibe el nombre de un usuario y retorna su id. Si no lo puede hacer correctamente retorna -1.
int DameID (char nombre[20]){
	char consulta[100];
	int err;
	sprintf(consulta,"SELECT jugadores.id FROM (jugadores) WHERE jugadores.username='%s';",nombre);
	err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	else
	{
		//recojo el resultado de la consulta.
		resultado = mysql_store_result (conn);
		//El resultado es un numero en la primera fila
		row = mysql_fetch_row (resultado);
		if(row == NULL)
			return -1;
		else
		{
			int id = atoi(row[0]);
			return id;
		}
	}
}

// Funcion que recibe el nombre de un jugador y lo elimina de la base de datos. Retorna -1 si no se ha podido hacer bien.
int Baja(char nombre[20]){
	
	int correcto = 0;
	int err;
	char consulta[512];
	int idj = DameID(nombre);
	
	sprintf(consulta, "SELECT partidas.id FROM (jugadores,partidas,registro) WHERE jugadores.id = %d AND jugadores.id = registro.id_j AND registro.id_p=partidas.id;",idj);
	err=mysql_query (conn,consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	
	
	sprintf(consulta,"DELETE FROM registro WHERE registro.id_j=%d;",idj);
	err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		correcto = -1;
	}
	
	int j = 0;
	int id_anterior = 0;
	while (row!=NULL)
	{
		if(atoi(row[j]) != id_anterior)
		{
			sprintf(consulta,"DELETE FROM registro WHERE registro.id_p=%d;",atoi(row[0]));
			err=mysql_query (conn, consulta);
			if (err!=0) 
			{
				printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
				correcto = -1;
			}
			sprintf(consulta,"DELETE FROM partidas WHERE partidas.id=%d;",atoi(row[0]));
			err=mysql_query (conn, consulta);
			if (err!=0) 
			{
				printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
				correcto = -1;
			}
		}
		id_anterior = row[0];
		row = mysql_fetch_row(resultado);
		
	}
	
	sprintf(consulta, "DELETE FROM jugadores WHERE jugadores.username ='%s';",nombre);
	err=mysql_query (conn,consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		correcto = -1;
	}
	else
		correcto = 0;
	return correcto;
}

// Funcion que recibe el nombre de usuario y lo da de baja. Comunica a este si el proceso se ha llevado a cabo correctamente. 
void TramitarBaja(char nombre[512]){
	int socket = BuscaSocket(&miLista, nombre);
	int r = Baja(nombre);
	Elimina(&miLista, nombre);
	if (r==-1)
	{
		write(socket, "3/0*-1",strlen("3/0*-1"));
	}
	else if (r==0)
	{
		write(socket,"3/0*0",strlen("3/0*0"));
	}
}

// Funcion que recibe informacion del nombre y el juego y escribe en respuesta los nombres de los jugadores con quienes el usuario ha jugado una partida
void PartidasConJugadores(char datos[512], char respuesta[512]){
	int err;
	char consulta[350];
	char nombre[20];
	int juego;
	
	p=strtok(datos,"/");
	strcpy(nombre,p);
	p=strtok(NULL,"/");
	juego=atoi(p);
	
	respuesta[0]='\0';
	sprintf(respuesta,"%d/",juego);
	
	int idj = DameID(nombre);
	if(juego==4)
		sprintf(consulta, "SELECT DISTINCT jugadores.username FROM (jugadores,partidas,registro) WHERE jugadores.id = registro.id_j AND jugadores.username!='%s' AND registro.id_p IN (SELECT partidas.id FROM (partidas,registro,jugadores) WHERE jugadores.id =%d AND jugadores.id = registro.id_j AND registro.id_p=partidas.id);",nombre,idj);
	else
		sprintf(consulta, "SELECT DISTINCT jugadores.username FROM (jugadores,partidas,registro) WHERE jugadores.id = registro.id_j AND jugadores.username!='%s' AND registro.id_p IN (SELECT partidas.id FROM (partidas,registro,jugadores) WHERE jugadores.id =%d AND jugadores.id = registro.id_j AND registro.id_p=partidas.id AND partidas.juego=%d);",nombre,idj,juego);
	
	
	err=mysql_query (conn,consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	
	while(row!=NULL)
	{
		sprintf(respuesta,"%s%s/",respuesta,row[0]);
		row = mysql_fetch_row(resultado);
	}
}

// Funcion que recibe informacion del nombre y el juego y escribe en respuesta los nombres de los jugadores con quienes el usuario ha jugado una partida
void ResultadoPartidas(char datos[512], char respuesta[512]){
	int err;
	char consulta[350];
	char nombre[20];
	char rival[20];
	
	p=strtok(datos,"/");
	strcpy(nombre,p);
	p=strtok(NULL,"/");
	strcpy(rival,p);
	
	int idj = DameID(nombre);
	int idr = DameID(rival);
	
	int juego;
	
	for(juego=1;juego<4;juego++)
	{
		sprintf(consulta, "SELECT partidas.ganador FROM (jugadores,partidas,registro) WHERE jugadores.id =%d AND registro.id_j = jugadores.id AND registro.id_p = partidas.id AND partidas.id IN (SELECT partidas.id FROM (partidas,registro,jugadores) WHERE jugadores.id =%d AND registro.id_j = jugadores.id AND registro.id_p=partidas.id AND partidas.juego =%d);",idj,idr,juego);
		
		err=mysql_query (conn,consulta);
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit (1);
		}
		
		resultado = mysql_store_result (conn);
		row = mysql_fetch_row (resultado);
		
		int ganadas=0;
		int perdidas=0;
		while(row!=NULL)
		{
			if(strcmp(row[0],nombre)==0)
				ganadas++;
			else
				perdidas++;
			row = mysql_fetch_row(resultado);
		}
		sprintf(respuesta,"%s%d/%d/%d/",respuesta,juego,ganadas,perdidas);
	}
}

// Funcion que recibe el ganador y el id de una partida y actualiza la base de datos, tanto la tabla registro como la partidas.
// Tambien elimina de la tabla la partida que se ha terminado.
void RegistrarPartida(char info[512], int juego){
	int err;
	int id;
	char ganador[20];
	p=strtok(info,"/");
	int idp = atoi(p);
	id = DamePartida(miTabla, idp);
	p=strtok(NULL,"/");
	strcpy(ganador,p);	
	char ids[20];
	char jugadores[100];
	char datos_consulta[150];
	int numJugadores = miTabla[id].numJugadores;
	if (numJugadores==2)
	{
		int socket1 = miTabla[id].socket1;
		int socket2 = miTabla[id].socket2;
		char jugador1[20];
		strcpy(jugador1,miTabla[id].jugador1);
		char jugador2[20];
		strcpy(jugador2,miTabla[id].jugador2);
		char jugador3[2];
		char jugador4[20];
		strcpy(jugador3,"");
		strcpy(jugador4,"");
		int socket3 = -1;
		int socket4 = -1;
		
		
		sprintf(jugadores, "%s/%s/",jugador1,jugador2);
		
		
	}
	else if (numJugadores==3)
	{
		int socket1 = miTabla[id].socket1;
		int socket2 = miTabla[id].socket2;
		int socket3 = miTabla[id].socket3;
		char jugador1[20];
		strcpy(jugador1,miTabla[id].jugador1);
		char jugador2[20];
		strcpy(jugador2,miTabla[id].jugador2);
		char jugador3[20];
		strcpy(jugador3,miTabla[id].jugador3);
		char jugador4[20];
		strcpy(jugador4, "");
		int socket4 = -1;
		sprintf(jugadores, "%s/%s/%s/",jugador1,jugador2,jugador3);
		
	}
	else if (numJugadores==4)
	{
		int socket1 = miTabla[id].socket1;
		int socket2 = miTabla[id].socket2;
		int socket3 = miTabla[id].socket3;
		int socket4 = miTabla[id].socket4;
		char jugador1[20];
		strcpy(jugador1,miTabla[id].jugador1);
		char jugador2[20];
		strcpy(jugador2,miTabla[id].jugador2);
		char jugador3[20];
		strcpy(jugador3,miTabla[id].jugador3);
		char jugador4[20];
		strcpy(jugador4,miTabla[id].jugador4);
		sprintf(jugadores, "%s/%s/%s/%s/",jugador1,jugador2,jugador3,jugador4);
		
	}
	
	printf("Jugadoresss: %s\n",jugadores);
	p=strtok(jugadores,"/");
	char jugador[20];
	strcpy(jugador,p);
	printf("Jugador: %s\n",jugador);
	int id_j = DameID(jugador);
	printf("IDDD: %d\n",id_j);
	sprintf(ids,"%d/",id_j);
	while(p!=NULL)
	{
		strcpy(jugador,p);
		printf("Jugador: %s\n",jugador);
		id_j = DameID(jugador);
		printf("IDDD: %d\n",id_j);
		sprintf(ids,"%s%d/",ids,id_j);
		p=strtok(NULL,"/");
	}
	printf("IDS1: %s\n",ids);
	char asignarID[100];
	sprintf(asignarID,"SELECT COUNT(partidas.id) FROM (partidas);");
	
	err=mysql_query (conn, asignarID);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//recojo el resultado de la consulta.
	resultado = mysql_store_result (conn);
	// El resultado es un numero en la primera fila
	row = mysql_fetch_row (resultado);
	
	int idPartida = atoi(row[0]) + 1;
	char insertar_partidas[512];
	
	//Get current date and time in the format DD/MM/YYYY and HH:MM and store them in date and time as string
	//We do it on the server-side as clients could be located in multiple timezones. The reference time is UTC+0
	time_t t = time(NULL);
	struct tm tm = *localtime(&t);
	char date[11];
	strftime(date, sizeof(date), "%Y-%m-%d", &tm);
	printf("Date: %s\n",date);
	
	char time[6];
	strftime(time, sizeof(time), "%H:%M", &tm);
	printf("Time: %s\n",time);
	
	printf ("Test D | date: %s, time: %s\n", date, time);
	
	sprintf(insertar_partidas,"INSERT INTO partidas (id,jugadores,juego,fecha,hora,ganador) VALUES (%d,%d,%d,'%s','%s','%s');",idPartida,numJugadores,juego,date,time,ganador);
	
	err=mysql_query (conn, insertar_partidas);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
	}
	else
	{
		miTabla[id].ocupacion = 0;	
	}
	printf("IDS: %d\n",ids);
	p=strtok(ids,"/");
	while(p!=NULL)
	{
		int idj = atoi(p);
		char insertar_registro[512];
		sprintf(insertar_registro,"INSERT INTO registro (id_j,id_p) VALUES (%d,%d);",idj,idPartida);		
		printf("UUUUUU %S\n", insertar_registro);
		err=mysql_query (conn, insertar_registro);
		printf("222222\n");		
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit(1);
		}	
	
		else
			p=strtok(NULL,"/");
	}
	printf("Ha acabat Reg partida\n");
}

// Funcion que coje una palabra aleatoria de la base de datos y la copia en la variable respuesta.
void PalabraAleatoria(char respuesta[50])
{
	int j = 0;
	char consulta[100];
	int err;
	strcpy(consulta,"SELECT * FROM (palabras)");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	else
	{
		//recojo el resultado de la consulta.
		resultado = mysql_store_result (conn);
		//El resultado es un numero en la primera fila
		row = mysql_fetch_row (resultado);
		srand(time(NULL));
		int idPalabra = rand() % 23;
		while(j < idPalabra)
		{
			row = mysql_fetch_row(resultado);
			j=j+1;
		}
		if(row == NULL)
			  return -1;
		else
		{
			printf("%d\n",idPalabra);
			strcpy(respuesta,row[0]);
			printf("%s\n",respuesta);
		}
	}
}



// --------------------------- FUNCIONES QUE DAN RESPUESTAS AL CLIENTE ------------------------------------------------------------------------------------------------------

// Funcion que escribe en respuesta "0" si puede desconectar el usuario que le pasamos y devuelve 1 si lo puede hacer.
// En caso contrario escribe "-1" i devuelve 0 cuando no puede eliminar el usuario pedido ya que no se encuentra en la lista.
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

// Funcion que escribe en respuesta el numero de conectados seguido de los nombres y los sockets separado por barras.
// Por ejemplo -> 3/Pau/Gerard/Enric/5/6/7	
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

// Funcion que utiliza la funcion previa Login para comprobar si se puede hacer el login correctamente. 
// Si se puede a￯﾿ﾱade el usuario a la lista de conectados. Devuelve 0 si se ha hecho el login y a￯﾿ﾱadido correctamente.
// Devuelve -1 si no encuentra el usuario. Devuelve 2 si el usuario ya esta conectado. 
// Copia la respuesta "0" / "-1" / "2" en respuesta.
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

// Funcion que utiliza la funcion previa Registrar para comprobar si se puede hacer el registro correctamente. 
// Si se puede a￯﾿ﾱade el usuario a la lista de conectados. Devuelve 0 si se ha hecho el registro y a￯﾿ﾱadido correctamente.
// Devuelve 2 si el usuario ya esta conectado. Copia la respuesta "0" / "2" en respuesta.
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
		// A￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾﾾ￯ﾾﾱadimos Usuario a la lista:
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

// Funcion que recibe una lista de jugadores y sockets a los cuales se quiere invitar, el jugador que los invita y el juego al que se quiere jugar.
// Se a￯﾿ﾱade la partida a la tabla de partidas y se asigna un id que sera utilizado a partir de este momento para identificar la partida.
// La funcion envia automaticamente las invitaciones a los jugadores correspondientes.
void InvitarJugadores(char datos[512], char nombre[20], int juego){
	int num;
	char invitacion[512];
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
	printf("OKKKKK1\n");
	if (p!=NULL)
	{
		printf("OKKKK2\n");
		strcpy(jugador3, p);
		p=strtok(NULL,"/");
		if (p!=NULL)
			strcpy(jugador4, p);
		else
			strcpy(jugador4, "");
	}
	else
	{
		printf("OKKKK3\n");
		strcpy(jugador3, "");
		strcpy(jugador4,"");
	}	
	
	// Si no hay jugador, devuelve -1 y ya nos sirve
	socket1 = BuscaSocket(&miLista,jugador1);
	socket2 = BuscaSocket(&miLista,jugador2);
	socket3 = BuscaSocket(&miLista,jugador3);
	socket4 = BuscaSocket(&miLista,jugador4);
	
	// De momento anyadimos la partida a la tabla, si no aceptan, la quitaremos, devuelve la posicion
	int idp = idSiguientePartida;
	int id =  AnadirPartida(miTabla,num, idp, jugador1, socket1, jugador2, socket2, jugador3, socket3, jugador4, socket4);
	idSiguientePartida = idSiguientePartida + 1;
	// Debemos enviar a los jugadores que nos han pedido la consulta de si quieren jugar o no
	sprintf(invitacion, "%d/0*%s/%d",juego, jugador1, idp);
	printf("Invitacion: %s\n", invitacion);
	if (socket2>0)
		write(socket2, invitacion, strlen(invitacion));
	if (socket3>0)
		write(socket3, invitacion, strlen(invitacion));
	if (socket4>0)
		write(socket4, invitacion, strlen(invitacion));
}

// Funcion que recibe la informacion de id partida, jugador y respuesta a la invitacion de todos los jugadores invitados. 
// Si todos los jugadores aceptan la invitacion, se envia la confirmacion al jugador 1 que es el creador de la partida.
// En caso que un jugador no acepte la invitacion la partida se quita de la tabla de partidas y se informa al jugador 1.
void RespuestaInvitacion(char info[512], int juego){
	char respuesta[512];
	char nombre[20];
	char res[5];
	int id;
	p=strtok(info, "/");
	strcpy(nombre, p);
	p=strtok(NULL,"/");
	strcpy(res, p);
	p=strtok(NULL, "/");
	int idp = atoi(p);
	
	id = DamePartida(miTabla, idp);
	printf("IDPARTIDAAAA: %d\n",id);
	int socket = miTabla[id].socket1;
	
	if (strcmp(res, "SI") == 0)
		sprintf(respuesta, "%d/0*%s/%s/%d",juego,nombre,res,idp);
	else
	{
		miTabla[id].ocupacion=0;
		sprintf(respuesta, "%d/0*%s/%s/%d",juego,nombre,res,idp);
	}
	printf("Respuesta: %s\n", respuesta);
	write(socket, respuesta, strlen(respuesta));
	printf("Respuesta: %s\n", respuesta);
	printf("SOCKET: %d\n", socket);
}

// Funcion que recibe el id de la partida que se puede empezar a jugar y de que juego se trata.
// Se envia a todos los jugadores de la partida que se puede empezar a jugar y pueden abrir el tablero.
// Se envia idJuego/idPartida/numJugadores/jugador1/socket1/jugador2/socket2...
void ComunicarCliente (char info[512], int juego){
	int id;
	char abrirTablero[50];
	int idp = atoi(info);
	id = DamePartida(miTabla, idp);
	int numJugadores = miTabla[id].numJugadores;
	printf("NumJugadores:%d, idPartida: %d\n",numJugadores,idp);
	if (numJugadores==2)
	{
		int socket1 = miTabla[id].socket1;
		int socket2 = miTabla[id].socket2;
		char jugador1[20];
		strcpy(jugador1,miTabla[id].jugador1);
		char jugador2[20];
		strcpy(jugador2,miTabla[id].jugador2);
		sprintf(abrirTablero,"%d/0*%d/%d/%s/%d/%s/%d",juego,idp,numJugadores,jugador1,socket1,jugador2,socket2);
		write(miTabla[id].socket1, abrirTablero, strlen(abrirTablero));
		write(miTabla[id].socket2, abrirTablero, strlen(abrirTablero));
		printf("Abrir tablero: %s\n", abrirTablero);
	}
	else if (numJugadores==3)
	{
		int socket1 = miTabla[id].socket1;
		int socket2 = miTabla[id].socket2;
		int socket3 = miTabla[id].socket3;
		char jugador1[20];
		strcpy(jugador1,miTabla[id].jugador1);
		char jugador2[20];
		strcpy(jugador2,miTabla[id].jugador2);
		char jugador3[20];
		strcpy(jugador3,miTabla[id].jugador3);
		sprintf(abrirTablero,"%d/0*%d/%d/%s/%d/%s/%d/%s/%d",juego,idp,numJugadores,jugador1,socket1,jugador2,socket2,jugador3,socket3);
		write(miTabla[id].socket1, abrirTablero, strlen(abrirTablero));
		write(miTabla[id].socket2, abrirTablero,strlen(abrirTablero));
		write(miTabla[id].socket3, abrirTablero,strlen(abrirTablero));
	}
	else if (numJugadores==4)
	{
		int socket1 = miTabla[id].socket1;
		int socket2 = miTabla[id].socket2;
		int socket3 = miTabla[id].socket3;
		int socket4 = miTabla[id].socket4;
		char jugador1[20];
		strcpy(jugador1,miTabla[id].jugador1);
		char jugador2[20];
		strcpy(jugador2,miTabla[id].jugador2);
		char jugador3[20];
		strcpy(jugador3,miTabla[id].jugador3);
		char jugador4[20];
		strcpy(jugador4,miTabla[id].jugador4);
		sprintf(abrirTablero,"%d/0*%d/%d/%s/%d/%s/%d/%s/%d/%s/%d",juego,idp,numJugadores,jugador1,socket1,jugador2,socket2,jugador3,socket3,jugador4,socket4);
		write(miTabla[id].socket1, abrirTablero, strlen(abrirTablero));
		write(miTabla[id].socket2, abrirTablero,strlen(abrirTablero));
		write(miTabla[id].socket3, abrirTablero,strlen(abrirTablero));
		write(miTabla[id].socket4, abrirTablero,strlen(abrirTablero));
	}
}

// Funcion que recibe el id de una partida, el id de un jugador y el numero de movimientos que se deben hacer.
// Envia a todos los jugadores de la partida el movimiento que ha hecho el jugador correspondiente.
void MovimientoJuego(char info[512], int codigo){
	int idJugador;
	int id;
	char movimiento[20];
	char EnviarMovimiento[512];
	p = strtok(info, "/");
	int idp = atoi(p);
	id = DamePartida(miTabla, idp);
	p = strtok(NULL,"/");
	strcpy(movimiento,p);
	p=strtok(NULL,"/");
	idJugador = atoi(p);
	printf("Partida:%d, Movimiento:%s, Jugador%d\n",idp,movimiento,idJugador);
	sprintf(EnviarMovimiento, "%d/%d*%s/%d",codigo, idp, movimiento, idJugador);
	int numJugadores = miTabla[id].numJugadores;
	printf("EnviarMovimiento: %s\n", EnviarMovimiento);
	if (numJugadores==2)
	{
		write(miTabla[id].socket1, EnviarMovimiento, strlen(EnviarMovimiento));
		write(miTabla[id].socket2, EnviarMovimiento, strlen(EnviarMovimiento));
	}
	else if (numJugadores==3)
	{
		write(miTabla[id].socket1, EnviarMovimiento, strlen(EnviarMovimiento));
		write(miTabla[id].socket2, EnviarMovimiento, strlen(EnviarMovimiento));
		write(miTabla[id].socket3, EnviarMovimiento, strlen(EnviarMovimiento));
	}
	else if (numJugadores==4)
	{
		write(miTabla[id].socket1, EnviarMovimiento, strlen(EnviarMovimiento));
		write(miTabla[id].socket2, EnviarMovimiento, strlen(EnviarMovimiento));
		write(miTabla[id].socket3, EnviarMovimiento, strlen(EnviarMovimiento));
		write(miTabla[id].socket4, EnviarMovimiento, strlen(EnviarMovimiento));
	}
}

// Funcion que recibe el id del jugador que ha ganado una partida de la oca de la cual tambien se recibe el id.
// Envia a todos los jugadores de la partida el id del jugador que ha ganado.
void LaOca_Ganador(char info[512], int codigo){
	int idJugador;
	int id;
	char EnviarGanador[512];
	p = strtok(info, "/");
	int idp = atoi(p);
	id = DamePartida(miTabla, idp);
	p = strtok(NULL,"/");
	idJugador = atoi(p);
	sprintf(EnviarGanador, "%d/%d*%d",codigo, idp, idJugador);
	int numJugadores = miTabla[id].numJugadores;
	printf("EnviarGanador: %s\n", EnviarGanador);
	if (numJugadores==2)
	{
		write(miTabla[id].socket1, EnviarGanador, strlen(EnviarGanador));
		write(miTabla[id].socket2, EnviarGanador, strlen(EnviarGanador));
	}
	else if (numJugadores==3)
	{
		write(miTabla[id].socket1, EnviarGanador, strlen(EnviarGanador));
		write(miTabla[id].socket2, EnviarGanador, strlen(EnviarGanador));
		write(miTabla[id].socket3, EnviarGanador, strlen(EnviarGanador));
	}
	else if (numJugadores==4)
	{
		write(miTabla[id].socket1, EnviarGanador, strlen(EnviarGanador));
		write(miTabla[id].socket2, EnviarGanador, strlen(EnviarGanador));
		write(miTabla[id].socket3, EnviarGanador, strlen(EnviarGanador));
		write(miTabla[id].socket4, EnviarGanador, strlen(EnviarGanador));
	}
}

// Funcion que recibe el id de una partida que el creador ha decidido finalizar antes de que termine.
// Envia a los jugadores la indicacion de partida finalizada.
void FinalizarPartida(char info[512], int codigo){
	int id;
	p = strtok(info, "/");
	int idp = atoi(p);
	id = DamePartida(miTabla, idp);
	int numJugadores = miTabla[id].numJugadores;
	printf("Finalizar Partida\n");
	char mensaje[300];
	sprintf(mensaje,"%d/%d*",codigo,idp);
	if (numJugadores==2)
	{
		write(miTabla[id].socket1, mensaje, strlen(mensaje));
		write(miTabla[id].socket2, mensaje, strlen(mensaje));
	}
	else if (numJugadores==3)
	{
		write(miTabla[id].socket1, mensaje, strlen(mensaje));
		write(miTabla[id].socket2, mensaje, strlen(mensaje));
		write(miTabla[id].socket3, mensaje, strlen(mensaje));
	}
	else if (numJugadores==4)
	{
		write(miTabla[id].socket1, mensaje, strlen(mensaje));
		write(miTabla[id].socket2, mensaje, strlen(mensaje));
		write(miTabla[id].socket3, mensaje, strlen(mensaje));
		write(miTabla[id].socket4, mensaje, strlen(mensaje));
	}
}

// Funcion que recibe el id de la partida de tictactoe y el movimiento que se debe hacer.
// Envia a todos los jugadores el movimiento a realizar.
void TicTacToe_Movimiento (char info[300], int codigo, int sock_conn){
	char mensaje[300];
	char movimiento[300];
	int id;
	int idp;
	p=strtok(info,"/");
	idp = atoi(p);
	id = DamePartida(miTabla, idp);
	p=strtok(NULL,"/");
	strcpy(movimiento, p);
	sprintf(mensaje,"%d/%d*%s",codigo,idp,movimiento);
	if(miTabla[id].socket1 == sock_conn)
		write(miTabla[id].socket2, mensaje, strlen(mensaje));
	else
		write(miTabla[id].socket1, mensaje, strlen(mensaje));
	printf("Mensaje enviado: %s\n",mensaje);
}

// Funcion que recibe el id de la partida de tictactoe y el socket. 
// Comunica a los que juegan la partida que ha finalizado.
void Reiniciar_TicTacToe(char info[300], int sock_conn){
	char mensaje[300];
	int id;
	int idp;
	p=strtok(info,"/");
	idp = atoi(p);
	id = DamePartida(miTabla, idp);
	sprintf(mensaje,"%d/%d*",33,idp);
	if(miTabla[id].socket1 == sock_conn)
		write(miTabla[id].socket2, mensaje, strlen(mensaje));
	else
		write(miTabla[id].socket1, mensaje, strlen(mensaje));
	printf("Mensaje enviado: %s\n",mensaje);
}

// Funcion que envia la palabra random de la base de datos para jugar al pictionary a todos los clientes de la partida que recibe como parametro.
void EnviarPalabra(char info[200]){
	int id;
	char palabra[50];
	char mensaje[200];
	int idp = atoi(info);
	id = DamePartida(miTabla, idp);
	int numJugadores = miTabla[id].numJugadores;
	PalabraAleatoria(palabra);
	printf("Palabra random: %s\n",palabra);
	sprintf(mensaje,"58/%d*%s",idp,palabra);
	if (numJugadores==2)
	{
		write(miTabla[id].socket1, mensaje, strlen(mensaje));
		write(miTabla[id].socket2, mensaje, strlen(mensaje));
	}
	else if (numJugadores==3)
	{
		write(miTabla[id].socket1, mensaje, strlen(mensaje));
		write(miTabla[id].socket2, mensaje, strlen(mensaje));
		write(miTabla[id].socket3, mensaje, strlen(mensaje));
	}
	else if (numJugadores==4)
	{
		write(miTabla[id].socket1, mensaje, strlen(mensaje));
		write(miTabla[id].socket2, mensaje, strlen(mensaje));
		write(miTabla[id].socket3, mensaje, strlen(mensaje));
		write(miTabla[id].socket4, mensaje, strlen(mensaje));
	}
	printf("Mensaje enviado: %s\n",mensaje);
}

// Funcion que recibe el id de una partida, un mensaje y el emisor de este. Envia el mensaje y quien es el emisor a todos los jugadores de la partida.
void EscribirChat(char info[512], int codigo){
	int idemisor;
	int id;
	char mensaje[300];
	char EnviarMensaje[512];
	p = strtok(info, "/");
	idemisor = atoi(p);
	p=strtok(NULL,"/");
	int idp = atoi(p);
	id = DamePartida(miTabla, idp);
	p=strtok(NULL,"/");
	strcpy(mensaje, p);
	sprintf(EnviarMensaje,"%d/%d*%d/%s",codigo, idp, idemisor, mensaje);
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

// Funcion que recibe los mensajes que se escriben en el chat y los envia a todos los conectados.
void EscribirChatLounge(char info[512], int code){
	char nombre[20];
	char mensaje[300];
	char notificacionChat[512];
	printf("Test 1: %s\n",info);
	
	p = strtok(info, "/");
	strcpy(nombre, p);
	printf("Test 2: %s\n",nombre);
	p=strtok(NULL,"/");
	strcpy(mensaje, p);
	printf("Test 3: %s\n",mensaje);
	
	sprintf(notificacionChat,"%d/0*%s/%s",code, nombre, mensaje); //20/gerard/Hola, alguien quiere jugar a La oca?
	printf("Mensaje Lounge: %s\n", notificacionChat);
	printf("Test 4\n");
	
	int j;
	for (j=0; j<miLista.num; j++)
	{
		printf("Test 5\n");
		write (miLista.conectados[j].socket,notificacionChat, strlen(notificacionChat));
	}
}

// Funcion que comunica el ganador a todos los jugadores de la partida.
void ComunicarGanador(char info[200], int codigo, int sock_conn){
	printf("OKKKKK %s\n", info);
	char mensaje[300];
	char ganador[20];
	int id;
	int idp;
	p=strtok(info,"/");
	idp = atoi(p);
	id = DamePartida(miTabla, idp);
	p=strtok(NULL,"/");
	strcpy(ganador, p);
	printf("GANADORRR: %s\n",ganador);
	sprintf(mensaje,"%d/%d*%s",codigo,idp,ganador);
	write(miTabla[id].socket2, mensaje, strlen(mensaje));
	write(miTabla[id].socket1, mensaje, strlen(mensaje));
	printf("Mensaje enviado: %s\n",mensaje);
}


// ######################################################################################################## BUCLE PARA ATENDER CLIENTE ##############################################################################3
void *AtenderCliente (void *socket)
{
	int sock_conn;
	int *s;
	s = (int *) socket;
	sock_conn=*s;
	char peticion[512];
	char notificacion[512];
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
	char info5[512];
	int ret;
	// Bucle de atencion al cliente
	int terminar = 0;
	while (terminar == 0)
	{
		// Ahora recibimos su peticion
		ret=read(sock_conn,peticion, sizeof(peticion));
		printf ("Recibida una peticion\n");
		// Tenemos que a￯﾿ﾃ￯ﾾﾱadirle la marca de fin de string para que no escriba lo que hay despues en el buffer
		peticion[ret]='\0';
		//Escribimos la peticion en la consola
		printf ("La peticion es: %s\n",peticion);
		
		//Interpretamos la peticion
		p = strtok(peticion, "/");
		int codigo =  atoi (p);
		printf("Codigo: %d\n",codigo);
		
		if (codigo==01 || codigo== 02|| codigo == 03)	// Si el codigo es 01, 02 o 03 arrancamos el nombre (Login/Registro/Baja)
		{			
			p=strtok(NULL,"/");
			strcpy(nombre,p);
			if (codigo != 03)	//Login y Registro
			{
				p=strtok(NULL,"/");
				strcpy(pwrd,p);
			}
			if (codigo == 02)	//Registro
			{
				p = strtok(NULL,"/");
				strcpy(email,p);
			}
			
		}
		else if (codigo==11 || codigo==13)	//Consultas: necesitamos arrancar algun dato
		{
			p=strtok(NULL,"/");
			strcpy(dato,p);
		}
		
		else if (codigo==12 || codigo==15||codigo==16)	//Consultas: no necesitamos arrancar ningun dato
		{
			p=strtok(NULL, "\0");
			strcpy(info, p);
			printf("Peticion: %s\n", info);
		}
		
		if (codigo == 00)
		{		
			if(strcmp(nombre,"")==0)
			{
				terminar=1;
			}
			else
			{
				p=strtok(NULL,"/");
				strcpy(nombre, p);
				//terminar = Respuesta_Desconectar(nombre,respuesta);
				Elimina(&miLista,nombre);
				terminar = 1;
			}	
			
		}
		else if (codigo == 01)
		{
			Respuesta_LogIn(pwrd,nombre,sock_conn,respuesta);
		}
		else if (codigo == 02)
		{
			Respuesta_Registrar(pwrd,nombre,email,sock_conn,respuesta);
		}
		else if (codigo == 03)
		{
			TramitarBaja(nombre);
		}
		
		//Codigo/Case 10 -> Actualizar DataGridView
		else if (codigo == 11) // Consulta: Cuantas partidas ha ganado el jugador elegido
		{
			PartidasGanadas(dato,respuesta);
		}
		else if (codigo == 12) // Consulta: Decir mejor jugador
		{
			MejorJugador(info,respuesta);
		}
		else if (codigo == 13) // Consulta: Saber los jugadores en partida
		{
			JugadoresEnPartida(dato,respuesta);
		}
		else if (codigo == 14) // Consulta: Saber las partidas jugadas entre dos fechas
		{
			char dateFrom[20], dateTo[20]; 
			p=strtok(NULL,"/");
			strcpy(dateFrom, p);
			p=strtok(NULL,"/");
			strcpy(dateTo, p);
			printf("From: %s, To: %s\n", dateFrom, dateTo);
			PartidasJugadasEntreFechas(dateFrom,dateTo,respuesta);
		}
		else if (codigo == 15)
		{
			PartidasConJugadores(info,respuesta);
		}
		else if (codigo == 16)
		{
			ResultadoPartidas(info,respuesta);
		}
		
		else if (codigo == 20) //Envia el mensaje recibido a todos los conectados (case 20)
		{
			p=strtok(NULL, "\0");
			strcpy(info, p);
			EscribirChatLounge(info,20);
		}
		
		else if (codigo==30)   //JUEGO: Tic Tac Toe
		{
			p = strtok(NULL,"/");
			int operacion = atoi(p);
			p=strtok(NULL, "\0");
			strcpy(info, p);
			printf("Operacion: %d, Info: %s\n",operacion,info);
			if (operacion == 0)			//Invitar Tic Tac Toe
			{
				InvitarJugadores(info,nombre,30);
			}
			else if (operacion == 1)	//Respuesta invitacion Tic Tac Toe
			{
				RespuestaInvitacion(info,31);
			}
			else if (operacion == 2)	//Moviemiento Tic Tac Toe
			{
				TicTacToe_Movimiento(info,32,sock_conn);
			}
			else if (operacion == 3)	//Registrar Partida Tic Tac Toe
			{
				RegistrarPartida(info, 1);
			}
			else if (operacion == 4)	//Reiniciar Partida Tic Tac Toe
			{
				Reiniciar_TicTacToe(info,sock_conn);
			}
			else if(operacion == 5)
			{
				ComunicarGanador(info,35,sock_conn);
			}
		}
		
		
		else if (codigo==40)   //JUEGO: La Oca
		{
			p = strtok(NULL,"/");
			int operacion = atoi(p);
			p=strtok(NULL, "\0");
			strcpy(info, p);
			printf("Operacion: %d, Info: %s\n",operacion,info);
			
			if (operacion == 0)			//Invitar La Oca (case 40)
			{
				InvitarJugadores(info,nombre,40);
			}
			else if (operacion == 1) 	//Respuesta Invitacion La Oca (case 41)
			{
				RespuestaInvitacion(info,41);
			}
			else if (operacion == 2)  	// Empezar La Oca (case 42)
			{
				ComunicarCliente(info, 42);
			}
			else if (operacion == 3) 	//Moviemiento La Oca (case 43)
			{
				MovimientoJuego(info, 43);
			}
			else if (operacion == 4)	//Ganador La Oca (case 44)
			{
				LaOca_Ganador(info, 44);
			}
			else if (operacion == 5) 	//Finalizar Partida La Oca (case 45)
			{
				FinalizarPartida(info, 45);
			}
			else if (operacion == 6) 	//Registrar Partida La Oca (no case)
			{
				RegistrarPartida(info,2);
			}
		}
		
		else if (codigo==50)   //JUEGO: Pictionary
		{
			p = strtok(NULL,"/");
			int operacion = atoi(p);
			p=strtok(NULL, "\0");
			strcpy(info, p);
			printf("Operacion: %d, Info: %s\n",operacion,info);
			
			if (operacion == 0)			// Invitacion al Pictionary (case 50)
			{
				InvitarJugadores(info,nombre,50);
			}
			else if (operacion == 1)	// Respuesta Invitacion (case 51)
			{
				RespuestaInvitacion(info,51);
			}
			else if (operacion == 2)	// Pintar el panel (case 52)
			{
				MovimientoJuego(info,52);
			}
			else if (operacion == 3)	// (case 53)
			{
				MovimientoJuego(info,53);
			}
			else if (operacion == 4)	// Empieza Pictionary (case 54)
			{
				ComunicarCliente(info,54);
			}
			else if (operacion == 5)	// Registrar Partida Pictionary (no case)
			{
				RegistrarPartida(info,3);
			}
			else if (operacion == 6)	// Cambiar Color (case 56)
			{
				MovimientoJuego(info,56);
			}
			else if (operacion == 7)	// Chat Pictionary (case 57)
			{
				EscribirChat(info,57);
			}
			else if (operacion == 8)	// Cambio de turno, envia una palabra aleatoria
			{
				EnviarPalabra(info);
			}
			else if (operacion == 9)	// Borrar dibujo
			{
				MovimientoJuego(info,59);
			}
		}
		if(codigo!=3 && codigo!=20 && codigo!=30 && codigo!=40 && codigo!=50) //Envia consulta al cliente adecuado
		{
			sprintf(mensaje,"%d/0*%s",codigo,respuesta);
			printf("Mensaje enviado: %s\n",mensaje);
			write (sock_conn,mensaje,strlen(mensaje));
		}
		
		
		if ((codigo==00)||(codigo==01)||(codigo==02)) //Actualiza el dataGridView al hacer Desconexion, LogIn o Registrar
		{
			Conectados(respuesta);
			sprintf(notificacion,"10/0*%s",respuesta);
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

// ################################################# PROGRAMA PRINCIPAL ######################################################
int main(int argc, char *argv[]){	
	//Creamos una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexi??n: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexion
	conn = mysql_real_connect (conn, "shiva2.upc.es","root", "mysql", "T8_BBDD", 0, NULL, 0);
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
	serv_adr.sin_port = htons(puerto);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 4) < 0)
		printf("Error en el Listen");
	
	pthread_t thread;
	for(;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		miLista.conectados[miLista.num].socket = sock_conn;
		//sock_conn es el socket que usaremos para este cliente
		// Crear thred y decirle que tiene que hacer:
		pthread_create (&thread, NULL, AtenderCliente ,&miLista.conectados[miLista.num].socket);	//No podem fer que els vectors siguin infinits i arriba un moment que no podem col￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾ﾿￯﾿ﾯ￯ﾾﾾ￯ﾾﾯ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾ﾿￯ﾾﾯ￯﾿ﾯ￯ﾾﾾ￯ﾾﾾ￯﾿ﾯ￯ﾾﾾ￯ﾾﾷlocar els sockets a la posicio que tocaria
		miLista.num++;
	}
	mysql_close (conn);
	exit(0);
}