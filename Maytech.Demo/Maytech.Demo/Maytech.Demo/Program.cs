using System;
using System.IO;
using Maytech.Quatrix;
using Maytech.Quatrix.Operations;
using System.Net;

namespace Maytech.Demo
{
    class Program
    {
        static QApi api = null;
        static string created_folder_id = "";
        static string folderName = "";

        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Enter number of option:");
                if(api==null)
                Console.WriteLine("1.Login. /session/login");
                Console.WriteLine("2.List home folder content. /file/metadata/{id}");
                Console.WriteLine("3.Make folder with random name. /upload/link/ -> /upload/chunked/ -> /upload/finalize/");
                Console.WriteLine("4.Upload a file into created folder. /upload/link/ -> /upload/chunked/ -> /upload/finalize/");
                if (api!=null)
                Console.WriteLine("5.Logout. /session/logout/");
                Console.WriteLine("Press Q to exit");
                string option = Console.ReadLine();
                switch (option.ToLower().Trim())
                {
                    case "q": { Environment.Exit(0); }; break;
                    default:
                        {
                            int number = 1;
                            if (int.TryParse(option, out number))Operation(number);
                            else Console.WriteLine("Please enter a number or a character ");
                        }; break;
                }
            }
        }

        /// <summary>
        /// Add some https into host if needed
        /// </summary>
        /// <param name="host">input host</param>
        /// <returns>host with https</returns>
        static string InputHost(string host)
        {
            return "https://" + host.Substring(host.IndexOf("://")>=0? host.IndexOf("://")+3:0); 
        }

        /// <summary>
        /// Masking the password
        /// </summary>
        /// <returns>return password, but in Console we will see passwrod as bucnh of star characters</returns>
        static string InputPassword()
        {
            ConsoleKeyInfo cki;
            string pass = string.Empty;
            while ((cki = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (pass.Length > 0)
                    {
                        Console.Write("\b \b");
                        pass = pass.Substring(0, pass.Length - 1);
                    }
                    continue;
                }
                pass += cki.KeyChar;
                Console.Write("*");
            }
            Console.WriteLine();
            return pass;
        }

        /// <summary>
        /// This function has an operations with api like: login, password, show content in home directory, upload folder, upload file in created folder and logout
        /// </summary>
        /// <param name="optionNumber">Number of option you choose</param>
        static void Operation(int optionNumber)
        {
            if (api == null&& optionNumber>1&&optionNumber!=5)
            {
                Console.WriteLine("Please login first");
                    Operation(1);
                if (api == null) return;
            }
            switch(optionNumber)
            {
                case 1:
                    {
                        if (api == null)
                        {
                            try
                            {
                                Console.WriteLine("Please Enter your account or press Q to leave:");
                                var host = Console.ReadLine().Trim();
                                if (host.Trim().ToLower() == "q") { api = null; return; }
                                else api = new QApi(InputHost(host));
                                Console.WriteLine("Please Enter your email or press Q to leave :");
                                var email = Console.ReadLine().Trim();
                                if (email.Trim().ToLower() == "q") { api = null; return; }
                                else api.Email = email;
                                Console.WriteLine("Please Enter your password  or press Q to leave:");
                                var password = InputPassword().Trim();
                                if (password.Trim().ToLower() == "q") { api = null; return; }
                                else api.Password = password;
                                api.Login();
                                if (Directory.Exists(Settings.HOME_DIR)) Directory.Delete(Settings.HOME_DIR, true);
                                Directory.CreateDirectory(Settings.HOME_DIR);
                                created_folder_id = api.Home.id;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                api = null;
                                folderName = "";
                                created_folder_id = "";
                                Operation(1);
                            }
                        }
                    };break;
                case 2:ShowContent(api.GetMetadata(api.Home.id));break;
                case 3:GenerateFolder();break;
                case 4:GenerateFileInFolder();break;
                case 5: { if (api != null) { api.Logout(); api = null; } };break;
            }
        }

        /// <summary>
        /// Function that shows content in current folder
        /// </summary>
        /// <param name="folder"></param>
        static void ShowContent(Metadata folder)
        {
            Console.WriteLine();
            foreach (var content in folder.Content)
                Console.WriteLine(string.Format("{0,-32}{1,-32}{2,5}", content.name, content.size, content.Type));
            Console.WriteLine();
        }

        /// <summary>
        /// Function that generates random name
        /// </summary>
        /// <returns>random name</returns>
        static string GenerateRandomName()
        {
            Random random = new Random();
            int filenameLength = random.Next(6, 12);
            char[] filenameCharacters = new char[filenameLength];
            for (int i = 0; i < filenameLength; i++)
                filenameCharacters[i] = Convert.ToChar(random.Next(97, 112));
            return string.Join("", filenameCharacters);
        }

        /// <summary>
        /// Generates folder in local home directory and upload into the cloud
        /// </summary>
        static void GenerateFolder()
        {
            var dirInfo = Directory.CreateDirectory(Path.Combine(Settings.HOME_DIR, GenerateRandomName()));
            var folderUpload = new FolderUpload(dirInfo.Name,dirInfo.FullName,api.Session.Request,api.Home.id);
            folderUpload.Finished += FolderUpload_Finished;
            folderUpload.Start();
        }

        /// <summary>
        /// Generates file in last created local directory and upload it
        /// </summary>
        static void GenerateFileInFolder()
        {
            var fileInfo = File.Create(Path.Combine(Settings.HOME_DIR, folderName, GenerateRandomName()));
            int nBytes=new Random().Next(50, 250);
                fileInfo.Write(new byte[nBytes],0,nBytes);
            fileInfo.Close();
            var fileUpload = new FileUpload(fileInfo.Name, api.Session.Request, created_folder_id);
            fileUpload.Start();
        }

        /// <summary>
        /// Save last uploaded folder id and name into the cloud 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void FolderUpload_Finished(object sender, EventArgs e)
        {
            created_folder_id = (sender as FolderUpload).Result.id;
            folderName = (sender as FolderUpload).Name;
        }
    }
}