using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slautebach.MigrateUserSettings
{
    /// <summary>
    /// Class to specify if an EntityAndAccess entity can be migrated, and if not the reason why it can't.
    /// </summary>
    internal class MigratableReason
    {
        internal MigratableReason()
        {
            Migratable = true;
        }

        /// <summary>
        /// Creates a MigratableReason to signify that the migration will be successful.
        /// </summary>
        internal static MigratableReason CanMigrate
        {
            get
            {
                return new MigratableReason()
                {
                    Migratable = true
                };
            }
        }

        /// <summary>
        /// Creates a MigratableReason, that is fales and with the specified reason 
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        internal static MigratableReason UnableToMigrate(string reason)
        {
            return new MigratableReason()
            {
                Migratable = false,
                Reason = reason
            };
        }

        public bool Migratable { get; set; }
        public string Reason { get; set; }
    }
}
