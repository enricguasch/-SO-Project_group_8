#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
// FUNCIONES QUE REALIZAN CONSULTAS:

void LogIn(char *p, int sock_conn)
	//Busca en la base de datos el usuario y contraseña para comprobar si 
	//el usuario existe.
{
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta[512];
	char respuesta[512];
	char nombre[20];
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
	
	p = strtok( NULL, "/");
	strcpy (nombre, p);
	// Ya tenemos el nombre
	printf ("Nombre: %s\n",nombre);
	
	p = strtok( NULL, "/");
	strcpy(pword, p);
	
	//Tenemos el nombre y la contraseña, comprobamos si hay un usuario que coincida.
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

void Registrar(char *p, int sock_conn)
	//Inserta en la base de datos un nuevo usuario
{
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta[512];
	char respuesta[512];
	char nombre[20];
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
	
	p = strtok( NULL, "/");
	strcpy (nombre, p);
	
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
	
	
	//Tenemos el nombre y la contraseña, comprobamos si hay un usuario que coincida.
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
	
	// Recollim el resultat que es una estructura.
	un_jugadores = mysql_store_result (conn);
	
	// Agafa la primera fila de l'estructura anterior
	row_un_j = mysql_fetch_row (un_jugadores);
	
	char resultado[500];	// Declarem aquest char per emmagatzemar el resultat i despres poder llegir-lo.
	
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


int main(int argc, char *argv[])
{	
	//INICIALIZAR CONEXIÓN CON EL CLIENTE
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	
	// Abrimos el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Hacemos el bind al port
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	// Asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el  ismo port que usa el cliente
	serv_adr.sin_port = htons(9010);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 4) < 0)
		printf("Error en el Listen");
	for(;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexi?n\n");
		//sock_conn es el socket que usaremos para este cliente
		
		//Bucle de atención al cliente
		int terminar = 0;
		while (terminar == 0)
		{
			
			// Ahora recibimos su peticion
			ret=read(sock_conn,peticion, sizeof(peticion));
			printf ("Recibida una petición\n");
			// Tenemos que añadirle la marca de fin de string 
			// para que no escriba lo que hay despues en el buffer
			peticion[ret]='\0';
			//Escribimos la peticion en la consola
			printf ("La petición es: %s\n",peticion);

			if (strlen(peticion)!=0)
			{
				//Interpretamos la petición
				char *p = strtok(peticion, "/");
				int codigo =  atoi (p);
				
				if (codigo == 0)
					terminar = 1;
				else if (codigo == 1)
				{
					LogIn(p,sock_conn);
				}
				else if (codigo == 2)
				{
					Registrar(p,sock_conn);
				}
				else if (codigo == 3) // consulta: Cuantas partidas ha ganado el jugador elegido?
				{
					PartidasGanadas(p,sock_conn);
				}
				else if (codigo == 4)
					MejorJugador(p,sock_conn);
				else //Nueva peticion: Decir si es alto o bajo
				{
					JugadoresEnPartida(p,sock_conn);
				}
			}
			else{
				terminar = 1;
			}
		}
		// Se acabo el servicio para este cliente
		close(sock_conn); 
	}
	exit(0);
}

