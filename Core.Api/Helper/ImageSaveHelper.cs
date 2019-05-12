using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Helper
{
    public static class ImageSaveHelper
    {
        public static void SaveImageToPath(string Photo, User user, string foldername)
        {
            if (user != null)
            {
                var path = Path.Combine(foldername);
                var directory = new DirectoryInfo(path);
                if (directory.Exists == false)
                    directory.Create();
                else
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        if (file.Name == user.id + ".png")
                            file.Delete();
                    }
                }
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Photo)))
                {
                    using (Bitmap bm2 = new Bitmap(ms))
                    {
                        bm2.Save($"{foldername}/" + user.id + ".png");
                    }
                }

            }
        }
    }
}
