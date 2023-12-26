using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Net.Sockets;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Security.Principal;
using System.Text;




namespace actualizarmods
{
    public partial class Form1 : Form
    {
        //private string carpetaMods = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods");

        private string direccionIP = "192.168.0.200";
        private int puerto = 9595;
        private List<string> listaMods = new List<string>();
        public Form1()
        {
            InitializeComponent();

        }
        public static List<string> GetFilesFromIP(string ip, int puerto)
        {
            string url = $"ftp://{ip}:{14147}/";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            List<string> filesList = new List<string>();

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string content = reader.ReadToEnd();
                    filesList = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    reader.Close();
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show($"Error: {ex.Status}\n{ex.Message}");
            }

            return filesList;
        }


        static void GenerarConfiguracion()
        {
            string carpetaMods = @"F:\server youtube mods y plugin test\mods";
            List<ModInfo> mods = new List<ModInfo>();

            foreach (string archivoMod in Directory.GetFiles(carpetaMods, "*.jar"))
            {
                mods.Add(new ModInfo { Name = Path.GetFileName(archivoMod) });
            }

            Configuracion configuracion = new Configuracion { Mods = mods };

            string jsonConfiguracion = Newtonsoft.Json.JsonConvert.SerializeObject(configuracion, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText("configActualizarmods.json", jsonConfiguracion);
        }

        class Configuracion
        {
            public List<ModInfo> Mods { get; set; }
        }

        class ModInfo
        {
            public string Name { get; set; }
        }

        private async void buttonDescargar_Click()
        {
            label1.Text = "Descargando archivo por favor espere...";
            await DescargarArchivosAsync();

            label1.Text = "Archivos descargados";

        }
        private void button2_Click(object sender, EventArgs e)
        {

            string miDireccionIP = ObtenerDireccionIP();

            if (miDireccionIP == "192.168.0.200")
            {
                label2.Text = "Eres el: servidor";

                label1.Text = "iniciando Servidor porfavor espere";
                IniciarServidorHttp(); // Iniciar el servidor HTTP cuando se carga el formulario
                label1.Text = "Servidor iniciado";
            }
            else
            {
                label2.Text = "Eres el: eres el cliente usa el otro boton";

            }


        }

        static string ObtenerDireccionIP()
        {
            string direccionIP = string.Empty;

            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530); // Con�ctate a un servidor externo
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    direccionIP = endPoint.Address.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la direcci�n IP: {ex.Message}");
            }

            return direccionIP;
        }

        private async Task DescargarArchivosAsync()
        {


            string carpetaMods = @"F:\server youtube mods y plugin test\mods";
            string[] archivos = Directory.GetFiles(carpetaMods, "*.jar");

            using (HttpClient cliente = new HttpClient())
            {

                foreach (string archivo in archivos)
                {
                    string nombreArchivo = Path.GetFileName(archivo);
                    string urlDescarga = $"http://{direccionIP}:{puerto}/{nombreArchivo}";
                    string rutaGuardado = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods", nombreArchivo);

                    try
                    {
                        HttpResponseMessage response = await cliente.GetAsync(urlDescarga);
                        if (response.IsSuccessStatusCode)
                        {
                            using (Stream stream = await response.Content.ReadAsStreamAsync())
                            using (FileStream fileStream = new FileStream(rutaGuardado, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                await stream.CopyToAsync(fileStream);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Error al descargar el archivo {nombreArchivo}: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " El archivo no encontrado es: " + urlDescarga);
                    }
                }
            }
        }

        private void IniciarServidorHttp()
        {
            string carpetaMods = @"F:\server youtube mods y plugin test\mods";
            string url = $"http://{direccionIP}:{puerto}/";

            // Inicia el servidor HTTP en un hilo separado
            Task.Run(() =>
            {
                using (HttpListener httpListener = new HttpListener())
                {
                    httpListener.Prefixes.Add(url);
                    httpListener.Start();
                    Console.WriteLine($"Servidor HTTP iniciado en {url}");

                    // Espera solicitudes
                    while (true)
                    {
                        HttpListenerContext context = httpListener.GetContext();
                        HttpListenerResponse response = context.Response;

                        // Obtiene el nombre del archivo de la URL
                        string archivoSolicitado = Path.GetFileName(context.Request.Url.AbsolutePath);

                        // Ruta completa del archivo solicitado
                        string rutaArchivo = Path.Combine(carpetaMods, archivoSolicitado);

                        if (File.Exists(rutaArchivo))
                        {
                            // Configura el tipo de contenido binario
                            response.ContentType = "application/octet-stream";

                            using (Stream output = response.OutputStream)
                            {
                                // Lee el archivo solicitado y env�alo como respuesta
                                byte[] contenido = File.ReadAllBytes(rutaArchivo);
                                output.Write(contenido, 0, contenido.Length);
                            }
                        }
                        else
                        {
                            // Enviar una respuesta de error si el archivo no se encuentra
                            response.StatusCode = 404;
                        }
                        response.Close();
                    }
                }
            });
        }


        // Agregar al inicio de tu clase
        private static readonly HttpClient cliente = new HttpClient(new HttpClientHandler()
        {
            //AllowAutoRedirect = true // Permite redirecciones automáticas
        })
        {
            //Timeout = TimeSpan.FromSeconds(30) // Establece un tiempo de espera de 30 segundos
        };

        private async Task<List<string>> ObtenerListaModsRemota(string direccionIP, int puerto)
        {
            List<string> listaMods = new List<string>();

            string urlLista = $"http://{direccionIP}:{puerto}/";
            Console.WriteLine($"URL: {urlLista}");

            try
            {
                HttpResponseMessage response = await cliente.GetAsync(urlLista);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"JSON: {json}");

                    listaMods = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(json);
                    MostrarMensajeEnUI(json);
                }
                else
                {
                    MostrarMensajeEnUI($"Error en la respuesta HTTP: {response.StatusCode}\nURL: {urlLista}");
                }
            }
            catch (HttpRequestException hrex)
            {
                MostrarMensajeEnUI($"Error de red al obtener lista de mods: {hrex.Message}");
            }
            catch (TaskCanceledException)
            {
                MostrarMensajeEnUI($"La solicitud fue cancelada por un timeout o el usuario.");
            }
            catch (Exception ex)
            {
                MostrarMensajeEnUI($"Error al obtener lista de mods: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }

            return listaMods;
        }

        // Separar la lógica de la interfaz de usuario en un método distinto
        private void MostrarMensajeEnUI(string mensaje)
        {
            this.Invoke(new Action(() =>
            {
                MessageBox.Show(mensaje);
            }));
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                direccionIP = textBox1.Text;
                puerto = int.Parse(textBox2.Text);

                labelIP.Text = direccionIP + ":" + Convert.ToString(puerto);
            }
            catch (Exception ex)
            {

            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(direccionIP + ":" + puerto);
        }

        private void buttonDescargar_Click_1(object sender, EventArgs e)
        {


            buttonDescargar_Click();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            ObtenerListaModsRemota(direccionIP, puerto);

            GetFilesFromIP(direccionIP, puerto);


            List<string> filesList = GetFilesFromIP(direccionIP, puerto);

            if (filesList != null && filesList.Count > 0)
            {
                string filesString = string.Join("\n", filesList);
                MessageBox.Show(filesString, "Lista de Archivos");
            }
            else
            {
                MessageBox.Show("No se encontraron archivos o no se pudo conectar al servidor.", "Error");
            }
            MessageBox.Show("se acabo");

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string ftpUrl = "ftp://192.168.0.200/";
            string localPath = @"C:\Users\fenix3090\Documents\Arduino\";
            string ftpUsername = "fenix";
            string ftpPassword = ""; // Sin contraseña

            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            listRequest.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            List<string> lines = new List<string>();

            try
            {
                using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
                using (Stream listStream = listResponse.GetResponseStream())
                using (StreamReader listReader = new StreamReader(listStream))
                {
                    while (!listReader.EndOfStream)
                    {
                        lines.Add(listReader.ReadLine());
                    }
                }

                foreach (string line in lines)
                {
                    // Asumimos que el servidor FTP devuelve los nombres de archivo en la última columna
                    string fileName = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Last();

                    // Si no es un archivo .jar, continuar con el siguiente archivo
                    if (!fileName.ToLower().EndsWith(".jar"))
                        continue;

                    // Crear solicitud FTP para descargar el archivo .jar
                    FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(ftpUrl + fileName);
                    downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    downloadRequest.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    using (FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse())
                    using (Stream sourceStream = downloadResponse.GetResponseStream())
                    using (Stream targetStream = File.Create(Path.Combine(localPath, fileName)))
                    {
                        sourceStream.CopyTo(targetStream);
                    }
                }

                MessageBox.Show("Todos los archivos .jar han sido descargados con éxito.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error durante la descarga: " + ex.Message);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Carpeta que contiene los archivos que deseas compartir
            string carpetaArchivos = @"C:\server youtube mods y plugin test\mods";

            // URL base del servidor HTTP
            MessageBox.Show("coma monda");
            string urlBase = "http://localhost:8080/";

            // Inicia el servidor HTTP en un hilo separado
            Thread servidorThread = new Thread(() =>
            {
                using (HttpListener listener = new HttpListener())
                {
                    listener.Prefixes.Add(urlBase);

                    listener.Start();

                     Console.WriteLine($"Servidor HTTP iniciado en {urlBase}");

                    while (true)
                    {
                        HttpListenerContext context = listener.GetContext();
                        HttpListenerRequest request = context.Request;

                        // Obtiene el nombre del archivo solicitado
                        string archivoSolicitado = request.Url.LocalPath.TrimStart('/');

                        // Combina la ruta de la carpeta con el nombre del archivo solicitado
                        string rutaCompleta = System.IO.Path.Combine(carpetaArchivos, archivoSolicitado);

                        // Responde con el archivo solicitado o un mensaje de error si no existe
                        if (System.IO.File.Exists(rutaCompleta))
                        {
                            byte[] archivoBytes = System.IO.File.ReadAllBytes(rutaCompleta);
                            context.Response.OutputStream.Write(archivoBytes, 0, archivoBytes.Length);
                        }
                        else
                        {
                            string mensajeError = "Archivo no encontrado";
                            byte[] errorBytes = System.Text.Encoding.UTF8.GetBytes(mensajeError);
                            context.Response.OutputStream.Write(errorBytes, 0, errorBytes.Length);
                        }

                        context.Response.Close();
                    }
                }
            });
            //servidorThread.Start();

            //Console.WriteLine("Presiona cualquier tecla para detener el servidor.");
            //Console.ReadKey();

            // Detiene el servidor cuando se presiona una tecla
            //servidorThread.Abort();
        }
            

    }
}
