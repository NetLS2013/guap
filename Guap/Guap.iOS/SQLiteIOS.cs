
using Guap.iOS;

using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteIOS))]
namespace Guap.iOS
{
    using System;
    using System.IO;

    using Guap.Contracts;

    public class SQLiteIOS : ISQLite
    {
        public string GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);

            return path;
        }
    }
}