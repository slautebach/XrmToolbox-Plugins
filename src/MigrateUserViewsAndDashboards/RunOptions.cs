using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slautebach.MigrateUserSettings
{
    /// <summary>
    /// Class to define command line options
    /// </summary>
    public class RunOptions
    {
        public string SourceUrl { get; set; }

        public string Username { get; set; }

        public string Domain { get; set; }

        public string Password { get; set; }

        public bool AddDevOpsAppUserToConvPlatform { get; set; }

        public string TargetEnv { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }


        public void SetDefaults()
        {
            foreach (var property in GetType().GetProperties())
            {
                //    var option = property.GetCustomAttributes(typeof(OptionAttribute), true).FirstOrDefault() as OptionAttribute;
                //    if (option == null)
                //    {
                //        // not a run option, skip.
                //        continue;
                //    }
                //    var existingValue = property.GetValue(this, null);
                //    if (!string.IsNullOrEmpty(existingValue?.ToString()))
                //    {
                //        // Property is already set
                //        continue;
                //    }
                //    // initialze the default value
                //    property.SetValue(this, option.Default, null);
            }
        }
       

        /// <summary>
        /// Deterimine any requried parameters are missing.
        /// </summary>
        /// <returns></returns>
        public bool MissingAnyRequiredParameters()
        {
            foreach (var property in GetType().GetProperties())
            {
                //var option = property.GetCustomAttributes(typeof(OptionAttribute), true).FirstOrDefault() as OptionAttribute;
                //if (option == null)
                //{
                //    // not a run option, skip.
                //    continue;
                //}
                //var existingValue = property.GetValue(this, null);
                //if (option.Required && string.IsNullOrEmpty(existingValue?.ToString()))
                //{
                //    return true;
                //}
            }
            return false;
        }

        public void LogDetails()
        {
            Console.WriteLine("Source Connection Details: ");
            Console.WriteLine($"   Dynamics Url: {SourceUrl}");
            Console.WriteLine($"   Domain: {Domain}");
            Console.WriteLine($"   Username: {Username}");


            Console.WriteLine("Target Connection Details: ");
            Console.WriteLine($"   Dynamics Url: {TargetEnv}");
            Console.WriteLine($"   Client Id: {ClientId}");

        }
    }


}
