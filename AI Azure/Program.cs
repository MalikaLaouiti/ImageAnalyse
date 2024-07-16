using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ComputerVisionQuickstart
{
    class Program
    {
        // Add your Computer Vision key and endpoint
        static string? key = Environment.GetEnvironmentVariable("VISION_KEY");
        static string? endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");

        // Chemin de l'image locale à analyser
        private const string LOCAL_IMAGE_PATH = @"C:\malika\back.jpg";

        static void Main(string[] args)
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision - Exemple de démarrage rapide en .NET");
            Console.WriteLine();

            try
            {
                // Vérifiez les valeurs de la clé et du point de terminaison
                if (!string.IsNullOrEmpty(endpoint) && !string.IsNullOrEmpty(key))
                {
                    // Créer un client
                    ComputerVisionClient client = Authenticate(endpoint, key);
                    // Analyser une image locale pour obtenir des fonctionnalités et d'autres propriétés.
                    AnalyzeImageLocal(client, LOCAL_IMAGE_PATH).Wait();
                }
                else
                {
                    Console.WriteLine("Clé ou point de terminaison non définis. Veuillez vérifier vos valeurs.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
        }

        /*
         * AUTHENTICATION
         * Crée un client Computer Vision utilisé par chaque exemple.
         */
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public static async Task AnalyzeImageLocal(ComputerVisionClient client, string imagePath)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYSE D'IMAGE - LOCALE");
            Console.WriteLine();

            // Créer une liste qui définit les fonctionnalités à extraire de l'image.
            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Tags
            };

            Console.WriteLine($"Analyse de l'image {Path.GetFileName(imagePath)}...");
            Console.WriteLine();

            // Ouvrir le fichier image local en tant que flux
            using (Stream imageStream = File.OpenRead(imagePath))
            {
                // Analyser l'image locale
                ImageAnalysis results = await client.AnalyzeImageInStreamAsync(imageStream, visualFeatures: features);
                Console.Write("Donner un element à cherché: ");
                string? input = Console.ReadLine();
                // Tags de l'image et leur score de confiance
                Console.WriteLine("Tags:");
                foreach (var tag in results.Tags)
                {
                    if (tag.Name == input)
                    {
                        Console.WriteLine($"{tag.Name} {tag.Confidence}");
                    }
                    
                    
                }
                Console.WriteLine();
            }
        }
    }
}
