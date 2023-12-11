using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Net;
using System.Net.Sockets;




namespace actualizarmods
{
    public partial class Form1 : Form
    {
        //private string carpetaMods = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods");
        private string carpetaMods = @"F:\server youtube mods y plugin test\mods";
        private string direccionIP = "192.168.0.200";
        private int puerto = 9090;

        public Form1()
        {
            InitializeComponent();

        }

        static void GenerarConfiguracion(string carpetaMods)
        {
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

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Descargando archivo por favor espere...";
            DescargarArchivos();
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
                    socket.Connect("8.8.8.8", 65530); // Conéctate a un servidor externo
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    direccionIP = endPoint.Address.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la dirección IP: {ex.Message}");
            }

            return direccionIP;
        }

        private void DescargarArchivos()
        {
            string[] archivos = Directory.GetFiles(carpetaMods, "*.jar");

            foreach (string archivo in archivos)
            {
                string nombreArchivo = Path.GetFileName(archivo);
                string urlDescarga = $"http://{direccionIP}:{puerto}/{nombreArchivo}";
                string rutaGuardado = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), nombreArchivo);
                //string rutaGuardado = @"C:\Users\fenix3090\Downloads\prueba2\" + nombreArchivo;

                using (WebClient cliente = new WebClient())
                {
                    try
                    {
                        cliente.DownloadFile(urlDescarga, rutaGuardado);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void IniciarServidorHttp()
        {
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
                                // Lee el archivo solicitado y envíalo como respuesta
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                direccionIP = textBox1.Text;
                puerto = int.Parse(textBox2.Text);
                MessageBox.Show(direccionIP);
            }
            catch (Exception ex)
            {

            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(direccionIP + ":" + puerto);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}