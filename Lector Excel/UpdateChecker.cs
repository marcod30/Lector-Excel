﻿using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows;

namespace Reader_347
{
    public class UpdateChecker
    {
        //Constructor
        public UpdateChecker()
        {
            
        }

        private readonly string ReleasesURI = "https://api.github.com/repos/marcod30/Lector-Excel/releases/latest";
        private readonly Version CurrentApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public bool GetReleases()
        {
            try
            {
                WebClient webClient = new WebClient();
                // Added user agent
                webClient.Headers.Add("User-Agent", "347 Reader Update Checker");
                Uri uri = new Uri(ReleasesURI);
                string releases = webClient.DownloadString(uri);
                Version newVersion;
                Debug.WriteLine("Current version is " + CurrentApplicationVersion.ToString());
                JObject jObject = JObject.Parse(releases);
                if (jObject.ContainsKey("tag_name"))
                {
                    Debug.WriteLine((string)jObject["tag_name"]);
                    newVersion = Version.Parse((string)jObject["tag_name"]);

                    if(newVersion > CurrentApplicationVersion)
                    {
                        MessageBoxResult temp = MessageBox.Show(string.Format("Se ha encontrado una nueva versión ({0}). Actualmente está ejecutando la versión {1}. ¿Desea descargarla ahora?",newVersion.ToString(),CurrentApplicationVersion.ToString()), "Actualización encontrada", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if(temp == MessageBoxResult.Yes)
                        {
                            Process.Start("https://github.com/repos/marcod30/Lector-Excel/releases/latest");
                        }
                    }
                    else if(newVersion == CurrentApplicationVersion)
                    {
                        MessageBoxResult temp = MessageBox.Show("La aplicación ya está actualizada","No hay actualizaciones",MessageBoxButton.OK,MessageBoxImage.Information);
                    }
                    else
                    {
                        throw new BadApplicationVersion(CurrentApplicationVersion, newVersion);
                    }
                }
                else
                {
                    Debug.WriteLine("Aw fuck I can't believe you've done this");
                    throw new Exception("Couldn't fetch data from GitHub");
                }
                
                return true;
            }
            catch (Exception e)
            {
                MessageBoxResult msg = MessageBox.Show("No se pudo obtener la actualización. Código del error: "+e.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }

    [Serializable] class BadApplicationVersion : Exception
    {
        public BadApplicationVersion()
        {

        }

        public BadApplicationVersion(Version current, Version newV) : base (string.Format("La aplicación actual contiene un número de versión ({0}) incompatible con el actual ({1})",current.ToString(),newV.ToString()))
        {

        }
    }
}
