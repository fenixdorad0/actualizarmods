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
using System.Diagnostics;




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
            funcion();

        }

        private bool IsInternetConnectionAvailable()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("¿deses borrar los mods anteriores?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                borrarMods();
                MessageBox.Show("Archivos eliminados con éxito.");
            }
            else
            {
                MessageBox.Show("Se han conservado los antiguos mods y se han descargado los nuevos");
            }

            try
            {
                if (!IsInternetConnectionAvailable())
                {
                    MessageBox.Show("No hay conexión a Internet. Verifica tu conexión e intenta nuevamente.");
                    return;
                }

                string ftpUrl = "ftp://" + direccionIP + "/";
                //string localPath = "@C:\\Users\\Fenixdorad0\\AppData\\Roaming\\.minecraft\\mods";
                string localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods");
                string ftpUsername = "fenix";
                string ftpPassword = ""; // Sin contraseña
                labelState.Text = "Descargando por favor espere";

                FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
                listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                listRequest.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                List<string> lines = new List<string>();

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
                    string fileName = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Last();

                    if (!fileName.ToLower().EndsWith(".jar"))
                        continue;

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
                labelState.Text = "Descargado";
                MessageBox.Show("Todos los archivos .jar han sido descargados con éxito.");
            }
            catch (WebException webEx)
            {
                if (webEx.Response != null)
                {
                    FtpWebResponse ftpResponse = (FtpWebResponse)webEx.Response;
                    MessageBox.Show($"Error de FTP: {ftpResponse.StatusCode} - {ftpResponse.StatusDescription}");
                }
                else
                {
                    MessageBox.Show("Ha ocurrido un error durante la descarga: " + webEx.Message);
                }
                labelState.Text = "Error en la descarga";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error durante la descarga: " + ex.Message);
                labelState.Text = "Error en la descarga";
            }
        }

        void borrarMods()
        {
            string localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods");

            if (Directory.Exists(localPath))
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(localPath);

                    // Elimina cada archivo dentro de la carpeta
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        file.Delete();
                    }

                    MessageBox.Show("Archivos eliminados con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar archivos: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("La carpeta no existe.");
            }
        }

        // Función IsInternetConnectionAvailable()...
        void funcion()
        {
            try
            {
                string GetPublicIpAddress()
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string apiUrl = "https://httpbin.org/ip";

                        HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = response.Content.ReadAsStringAsync().Result;
                            dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
                            string publicIp = jsonObject.origin;
                            return publicIp;
                        }
                        else
                        {
                            throw new Exception("Error al obtener la dirección IP pública. Código de estado: " + response.StatusCode);
                        }
                    }
                }

                Task.Run(() =>
                {
                    try
                    {
                        string publicIp = GetPublicIpAddress();
                        //MessageBox.Show("Tu dirección IP pública es: " + publicIp);

                        // Aquí puedes realizar la lógica adicional según tu necesidad.
                        // Por ejemplo, cambiar la dirección IP como lo has mencionado.
                        if (publicIp == "186.31.27.110")
                        {
                            direccionIP = "192.168.0.200";
                        }
                        else
                        {
                            direccionIP = "186.31.27.110";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener la dirección IP pública: " + ex.Message);
                    }
                }).Wait(); // Espera la finalización de la tarea antes de continuar con la ejecución.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error general: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Ruta al archivo JAR que deseas abrir
                string jarPath = @"E:\aplicaciones c#\actualizarmods\actualizarmods\bin\Debug\net8.0-windows\forge-1.20.1-47.2.19-installer.jar";

                // Configura el proceso de inicio
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", $"/c start \"\" \"{jarPath}\"")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Inicia el proceso
                using (Process process = new Process() { StartInfo = psi })
                {
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Ruta al archivo JAR que deseas abrir
                string jarPath = @"E:\aplicaciones c#\actualizarmods\actualizarmods\bin\Debug\net8.0-windows\jdk-21_windows-x64_bin.exe";

                // Configura el proceso de inicio
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", $"/c start \"\" \"{jarPath}\"")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Inicia el proceso
                using (Process process = new Process() { StartInfo = psi })
                {
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
