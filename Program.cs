using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AplicacionProyectoAula2
{
    class Program
    {
        static string sesionUsusario;
        static List<string> Vacunas = new List<string>()
        {
            "Vacuna BCG",
            "Vacuna Hepatitis B",
            "Vacunas Pentavalente",
            "Vacuna contra Rotavirus",
            "Vacuna contra neumococo",
            "Vacuna contra influenza",
            "Vacuna DPT",
            "Vacuna triple viral SRP y SR",
            "Vacuna OPV",
            "Vacuna VPH",
            "Vacuna Poliomielitis Sabin",
            "Vacuna Td",
            "Vacuna Tdpa",
            "Vacuna COVID-19"
        };

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;

            int opcSesion;
            Menu:

            Console.Clear();
            Console.WriteLine("----------------------------------------------------------------------------------------------Inicio de sesion--------------------------------------------------------------------------------------\n");

            Console.WriteLine("Para entrar tienes que iniciar sesión, si no tienes una cuenta registrate.\n");

            Console.WriteLine("[1] Iniciar Sesión");
            Console.WriteLine("[2] Registrarse");
            Console.WriteLine("[3] Borrar Cuenta");
            Console.WriteLine("[4] Cerrar aplicación");

            do
            {
                try
                {
                    opcSesion = Byte.Parse(Console.ReadLine());
                }
                catch (OverflowException)
                {
                    Console.WriteLine("¡Has introducido un valor negativo o un valor demasiado grande, por favor ingresa una respuesta correcta!");
                    opcSesion = 0;
                }
                catch (FormatException)
                {
                    Console.WriteLine("¡Has introducido texto, por favor ingresa una respuesta correcta!");
                    opcSesion = 0;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("¡Ha habido un error desconocido, intentalo de nuevo!");
                    opcSesion = 0;
                }

                Console.WriteLine();
            }
            while (opcSesion < 1 || opcSesion > 4);

            switch (opcSesion)
            {
                case 1:
                    if (IniciarSesion() == false) goto Menu;
                    break;

                case 2:
                    Registro();
                    goto Menu;

                case 3:
                    BorrarRegsitro();
                    goto Menu;

                case 4:
                    Console.WriteLine("Gracias por usar nuestra aplicación, hasta pronto.");
                    Console.ReadKey();

                    Environment.Exit(0);
                    break;
            }

            MenuPrincipal();
            goto Menu;
        }

        static bool IniciarSesion()
        {
            string linea, contrasena;
            int intentos = 1;
            Dictionary<string, string> cuentas = new Dictionary<string, string>();
            StreamReader sr = new StreamReader(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\Cuentas.txt");

            linea = sr.ReadLine();

            if(linea == String.Empty)
            {
                Console.WriteLine("Aun no te haz registrado, por favor hazlo (Enter para registrate)");
                Console.ReadKey();
                Console.Clear();
                sr.Close();
                return false;
            }

            while (linea != null)
            {
                var splitCuenta = linea.Split(' ');

                cuentas.Add(splitCuenta[0], splitCuenta[1]);

                linea = sr.ReadLine();
            }
            sr.Close();

            Console.WriteLine("Ingresa tu nombre de usuario: ");
            sesionUsusario = Console.ReadLine();

            if (!cuentas.ContainsKey(sesionUsusario))
            {
                Console.WriteLine("Aun no te haz registrado, por favor hazlo (Enter para registrate)");
                Console.ReadKey();
                return false;
            }
            else
            {
                Contrasena:

                if (intentos < 4)
                {
                    Console.WriteLine("Ingresa tu contraseña: ");
                    contrasena = HideCharacter();
                    Console.WriteLine();

                    if (cuentas[sesionUsusario] != contrasena)
                    {
                        Console.WriteLine("¡Contraseña incorrecta, por favor escriba la contraseña correcta!");
                        intentos++;
                        goto Contrasena;
                    }
                    else
                    {
                        Console.Clear();
                        return true;
                    }
                }
                else
                {
                    Console.Clear();

                    Console.WriteLine("¡Lo sentimos, ya haz intentado iniciar sesion demasiadas veces, se cerrara el programa!");
                    Console.ReadKey();

                    Environment.Exit(0);

                    return false;
                }
            }
        }

        static void Registro()
        {
            string linea, contrasena;
            Dictionary<string, string> cuentas = new Dictionary<string, string>();
            StreamReader sr = new StreamReader(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\Cuentas.txt");

            linea = sr.ReadLine();
            
            if(linea != String.Empty)
            {
                while (linea != null)
                {
                    var splitCuenta = linea.Split(' ');

                    cuentas.Add(splitCuenta[0], splitCuenta[1]);

                    linea = sr.ReadLine();
                }
            }
            sr.Close();

            StreamWriter sw = new StreamWriter(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\Cuentas.txt", true);
            Console.WriteLine("Ingresa tu nombre de usuario: ");
            sesionUsusario = Console.ReadLine();

            if (!cuentas.ContainsKey(sesionUsusario))
            {
                Console.WriteLine("Ingrese una contraseña: ");
                contrasena = HideCharacter();

                linea = sesionUsusario + " " + contrasena;

                sw.WriteLine(linea);

                FileStream fs = new FileStream(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\HistorialPacientes" + "\\" + sesionUsusario + ".txt", FileMode.Create);
                fs.Close();
                sw.Close();

                Console.WriteLine("\nAntes de continuar debe actualizar su informacion...");
                Console.ReadKey();
                ActualizarDatos();
                VacunasAplicadas();

                Console.WriteLine("\n¡Ya puedes iniciar sesion! (Enter para iniciar sesion)");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Ya te has registrado antes, por favor Inicia sesion (Enter para iniciar sesion)");
                Console.ReadKey();
                sw.Close();
            }
            
        }

        static void BorrarRegsitro()
        {
            int intentos = 1;
            string linea, eliminar, contrasena;
            Dictionary<string, string> cuentas = new Dictionary<string, string>();
            StreamReader sr = new StreamReader(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\Cuentas.txt");

            linea = sr.ReadLine();

            if (linea != String.Empty)
            {
                while (linea != null)
                {
                    var splitCuenta = linea.Split(' ');

                    cuentas.Add(splitCuenta[0], splitCuenta[1]);

                    linea = sr.ReadLine();
                }
            }
            sr.Close();

            Console.WriteLine("¿Cual es el nombre de la cuenta que desea eliminar?");
            eliminar = Console.ReadLine();

            if (cuentas.ContainsKey(eliminar))
            {
                Console.WriteLine("Ingresa tu contraseña para confirmar la eliminacion: ");

                PedirContrasena:
                if (intentos < 4)
                {
                    contrasena = HideCharacter();
                    Console.WriteLine();

                    if (cuentas[eliminar] == contrasena)
                    {
                        Console.WriteLine("Cuenta eliminada con exito...");
                        cuentas.Remove(eliminar);

                        File.Delete(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\HistorialPacientes" + "\\" + eliminar + ".txt");

                        StreamWriter sw = new StreamWriter(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\Cuentas.txt");
                        foreach (KeyValuePair<string, string> kvp in cuentas)
                        {
                            sw.WriteLine("{0} {1}", kvp.Key, kvp.Value);
                        }
                        sw.Close();

                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Ingresa la contraseña correcta");
                        intentos++;
                        goto PedirContrasena;
                    }
                }
                else
                {
                    Console.Clear();

                    Console.WriteLine("¡Lo sentimos, ya haz intentado demasiadas veces, puede que la cuenta no sea tuya, se cerrara el programa!");
                    Console.ReadKey();

                    Environment.Exit(0);
                }
            }
            else 
            {
                Console.WriteLine("Esa cuenta no existe (Enter para regresar al menu)");
                Console.ReadKey();
            }
        }

        static void MenuPrincipal()
        {
            Inicio:
            Console.WriteLine("----------------------------------------------------------------------------------------------Bienvenido {0} al Centro de Vacunacion POLIVAC-------------------------------------------------------------------------------", sesionUsusario);

            int opcMenu;
            Console.WriteLine("¿En que le podemos ayudar?");
            Console.WriteLine("[1] Actualizar datos");
            Console.WriteLine("[2] Mostrar informacion Personal");
            Console.WriteLine("[3] Agendar una cita para vacunacion");
            Console.WriteLine("[4] Mostrar citas agendadas citas");
            Console.WriteLine("[5] Abrir el juego");
            Console.WriteLine("[6] Cerrar sesion");

            do
            {
                try
                {
                    opcMenu = Byte.Parse(Console.ReadLine());
                }
                catch (OverflowException)
                {
                    Console.WriteLine("¡Haz introducido un valor negativo o un valor demasiado grande, por favor ingresa una respuesta correcta!");
                    opcMenu = 0;
                }
                catch (FormatException)
                {
                    Console.WriteLine("¡Haz introducido texto, por favor ingresa una respuesta correcta!");
                    opcMenu = 0;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("¡Ha habido un error desconocido, intentalo de nuevo!");
                    opcMenu = 0;
                }

                Console.WriteLine();
            }
            while (opcMenu < 1 || opcMenu > 6);

            switch (opcMenu)
            {
                case 1:
                    ActualizarDatos();
                    break;

                case 2:
                    MostrarInformacion();
                    break;

                case 3:
                    AgendarCita();
                    break;

                case 4:
                    MostarCitasAgendadas();
                    break;

                case 5:
                    Process.Start(@"C:\Users\Kevin\Desktop\BuildJuego\ProyectKOtopic.exe");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
            if(opcMenu != 6) goto Inicio;
        }

        static void ActualizarDatos()
        {
            Console.Clear();
            bool nombreValido = false;
            string nombreCompleto, tipoSangre, alergias;
            char genero;
            DateTime fechaNacimiento;

            FileStream fs = new FileStream(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\HistorialPacientes" + "\\" + sesionUsusario + ".txt", FileMode.Open);
            fs.Seek(0, SeekOrigin.Begin);

            Console.WriteLine("--------------------------------------------------------------------------------------------Información Personal------------------------------------------------------------------------------------");

            fs.Write(ASCIIEncoding.ASCII.GetBytes("--------------------------------------------------------------------------------------------Informacion Personal------------------------------------------------------------------------------------\n"));

            Console.WriteLine("Ingrese su nombre completo: ");
            do
            {
                nombreCompleto = Console.ReadLine();

                for (int j = 0; j < nombreCompleto.Length; j++)
                {
                    if (nombreCompleto[j] != ' ')
                    {
                        if (!Char.IsLetter(nombreCompleto, j))
                        {
                            nombreValido = false;
                            break;
                        }
                        else nombreValido = true;
                    }
                }
            }
            while (nombreValido == false);

            fs.Write(ASCIIEncoding.ASCII.GetBytes($"Nombre: {nombreCompleto}\n"));

            Console.WriteLine("¿Cual es tu Genero? M(masculino) o F(femenino)");
            do
            {
                genero = Char.Parse(Console.ReadLine().ToUpper());
            }
            while (genero != 'F' && genero != 'M');
            fs.Write(ASCIIEncoding.ASCII.GetBytes($"Genero: {genero}\n"));

            Console.WriteLine("Ingresa tu fecha de nacimiento dd/mm/yyyy");
        Fecha:
            try
            {
                fechaNacimiento = DateTime.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Ingresa una fecha valida");
                goto Fecha;
            }
            if (fechaNacimiento > DateTime.Now)
            {
                Console.WriteLine("Ingresa una fecha valida");
                goto Fecha;
            }

            fs.Write(ASCIIEncoding.ASCII.GetBytes($"Fecha Nacimiento: {fechaNacimiento.ToString("dd/MM/yyyy")}\n"));

            Console.WriteLine("Ingresa tu tipo de sangre: ");
            tipoSangre = Console.ReadLine();
            fs.Write(ASCIIEncoding.ASCII.GetBytes($"Sangre: {tipoSangre}\n"));

            Console.WriteLine("¿Tiene alergias a algun medicamento? De ser asi escriba todas seguidas de una (,) o en caso de no solo escriba No");
            alergias = Console.ReadLine();
            fs.Write(ASCIIEncoding.ASCII.GetBytes($"Alergias: {alergias}\n"));
            fs.Write(ASCIIEncoding.ASCII.GetBytes("\n"));

            fs.Write(ASCIIEncoding.ASCII.GetBytes("--------------------------------------------------------------------------------------------Vacunas Aplicadas------------------------------------------------------------------------------------\n"));

            Console.WriteLine("Informaacion actualizada con exito...");
            Console.ReadKey();

            fs.Close();
        }

        static void MostrarInformacion()
        {
            Console.Clear();

            string linea;
            DateTime fNacimiento;
            StreamReader sr = new StreamReader(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\HistorialPacientes" + "\\" + sesionUsusario + ".txt");

            linea = sr.ReadLine();

            while (linea != null && !linea.Contains("Citas"))
            {
                if (!linea.StartsWith("Fecha Nacimiento")) Console.WriteLine(linea);
                else
                {
                    fNacimiento = DateTime.Parse(linea.Split(": ")[1]);

                    int años = DateTime.Now.Year - fNacimiento.Year;

                    if (DateTime.Now.Month >= fNacimiento.Month && DateTime.Now.Day >= fNacimiento.Day) Console.WriteLine($"Edad: {años}");
                    else Console.WriteLine($"Edad: {años - 1}");
                }

                linea = sr.ReadLine();
            }
            sr.Close();

            Console.WriteLine("Enter para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        static void AgendarCita()
        {
            Console.Clear();
            int numVacuna;
            DateTime fechaVacunacion;
            FileStream fs = new FileStream(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\HistorialPacientes" + "\\" + sesionUsusario + ".txt", FileMode.Open);

            byte[] recibeInfo = new byte[fs.Length];
            fs.Read(recibeInfo, 0, (int)fs.Length);

            for (int i = 0; i < recibeInfo.Length; i++)
            {
                if (ASCIIEncoding.ASCII.GetString(recibeInfo) == "--------------------------------------------------------------------------------------------Citas Agendadas------------------------------------------------------------------------------------") fs.Seek(i, SeekOrigin.Begin);
            }

            Console.WriteLine("Ingresa el numero de la vacuna que deseas programar para su aplicacion");
            for (int i = 0; i < Vacunas.Count; i++)
            {
                if (!ASCIIEncoding.ASCII.GetString(recibeInfo).Contains(Vacunas[i])) Console.WriteLine($"[{i + 1}] {Vacunas[i]}");
            }
            Console.WriteLine($"[{Vacunas.Count + 1}] Salir");

            do
            {
                try
                {
                    numVacuna = Int32.Parse(Console.ReadLine()) - 1;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ingresa una opcion Valida");
                    numVacuna = -1;
                }
            }
            while (numVacuna < 0 || numVacuna > Vacunas.Count);

            if (numVacuna != Vacunas.Count)
            {
                Console.WriteLine("\nIngresa la fecha y hora en la cual deseas que se haga la aplicacion de tu vacuna dd/MM/yyyy HH:mm");
                Console.WriteLine("El horario solo esta disponible de 08 a 18hr, no se pueden agendar citas donde la fecha sea antes de la actual y el lapso maximo para agendar una cita es de 1 mes");
                Fecha:
                try
                {
                    fechaVacunacion = DateTime.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ingresa una fecha valida");
                    goto Fecha;
                }
                if (fechaVacunacion.Hour < 8 || fechaVacunacion.Hour > 18 || fechaVacunacion < DateTime.Now || fechaVacunacion.Month > DateTime.Now.Month + 1)
                {
                    goto Fecha;
                }

                if (!ASCIIEncoding.ASCII.GetString(recibeInfo).Contains(Vacunas[numVacuna]))
                {
                    fs.Write(ASCIIEncoding.ASCII.GetBytes(Vacunas[numVacuna] + " " + fechaVacunacion.ToString("dd/MM/yyyy HH:mm")));
                    fs.Write(ASCIIEncoding.ASCII.GetBytes("\n"));
                }
            }

            Console.WriteLine("\nCita agendada con exito... (Enter para continuar)");
            Console.ReadKey();
            Console.Clear();

            fs.Close();
        }

        static void MostarCitasAgendadas()
        {
            Console.Clear();

            String linea;
            bool escribir = false;
            StreamReader sr = new StreamReader(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\HistorialPacientes" + "\\" + sesionUsusario + ".txt");

            linea = sr.ReadLine();
            while (linea != null)
            {
                if (linea == "--------------------------------------------------------------------------------------------Citas Agendadas------------------------------------------------------------------------------------") escribir = true;
                if (escribir == true) Console.WriteLine(linea);

                linea = sr.ReadLine();
            }
            sr.Close();

            Console.WriteLine("\nEnter para continuar...");
            Console.ReadKey();

            Console.Clear();
        }

        static void VacunasAplicadas()
        {
            Console.Clear();
            int numVacuna;

            do
            {
                FileStream fs = new FileStream(@"C:\Users\Kevin\Documents\Proyectos C#\AplicacionProyectoAula2\HistorialPacientes" + "\\" + sesionUsusario + ".txt", FileMode.Open);
                byte[] recibeInfo = new byte[fs.Length];
                fs.Read(recibeInfo, 0, (int)fs.Length);

                for (int i = 0; i < recibeInfo.Length; i++)
                {
                    if (ASCIIEncoding.ASCII.GetString(recibeInfo) == "--------------------------------------------------------------------------------------------Vacunas Aplicadas------------------------------------------------------------------------------------") fs.Seek(i, SeekOrigin.Begin);
                }

                Console.WriteLine("Seleccione la vacuna que ya le ha sido aplicada");
                for (int i = 0; i < Vacunas.Count; i++)
                {
                    if (i == 0) Console.WriteLine("\nVacunas Obligatorias");
                    if (i == Vacunas.IndexOf("Vacuna VPH") + 1) Console.WriteLine("\nVacunas Complementarias");
                    if (!ASCIIEncoding.ASCII.GetString(recibeInfo).Contains(Vacunas[i])) Console.WriteLine($"[{i + 1}] {Vacunas[i]}");
                }
                Console.WriteLine($"[{Vacunas.Count + 1}] Salir");

                do
                {
                    try
                    {
                        numVacuna = Int32.Parse(Console.ReadLine()) - 1;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Ingresa una opcion Valida");
                        numVacuna = -1;
                    }
                }
                while (numVacuna < 0 || numVacuna > Vacunas.Count);

                if (numVacuna != Vacunas.Count)
                {
                    if (!ASCIIEncoding.ASCII.GetString(recibeInfo).Contains(Vacunas[numVacuna]))
                    {
                        fs.Write(ASCIIEncoding.ASCII.GetBytes(Vacunas[numVacuna]));
                        fs.Write(ASCIIEncoding.ASCII.GetBytes("\n"));
                    }
                }
                else
                {
                    fs.Write(ASCIIEncoding.ASCII.GetBytes("\n"));
                    fs.Write(ASCIIEncoding.ASCII.GetBytes("--------------------------------------------------------------------------------------------Citas Agendadas------------------------------------------------------------------------------------\n"));
                }

                fs.Close();

                Console.Clear();
            }
            while (numVacuna != Vacunas.Count);
        }

        static string HideCharacter()
        {
            ConsoleKeyInfo key;
            string cadena = "";
            do
            {
                key = Console.ReadKey(true);

                if (Char.IsNumber(key.KeyChar)) Console.Write("*");

                if (Char.IsLetter(key.KeyChar)) Console.Write("*");

                if (Char.IsSymbol(key.KeyChar)) Console.Write("*");

                cadena += key.KeyChar;

            } while (key.Key != ConsoleKey.Enter);

            return cadena.Trim();
        }
    }
}