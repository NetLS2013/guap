using Guap.Droid;

using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDroid))]
namespace Guap.Droid
{
    using System;
    using System.IO;

    using Guap.Contracts;

    public class SQLiteDroid : ISQLite
    {
        public string GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            return path;
        }
    }
}