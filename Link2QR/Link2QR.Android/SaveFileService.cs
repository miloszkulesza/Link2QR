using System;
using System.IO;
using System.Threading.Tasks;
using Java.IO;
using Link2QR.Services;
using Xamarin.Essentials;

namespace Link2QR.Droid
{
    public class SaveFileService : ISaveFile
    {
        public async Task<bool> SaveFile(byte[] bytes, string filename)
        {
            var writeStorageStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if(writeStorageStatus == PermissionStatus.Denied)
            {
                var requestStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
            var dir = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryPictures);
            bool dirExists = Directory.Exists(dir);
            if(!dirExists)
                Directory.CreateDirectory(dir);
            try
            {
                var filepath = Path.Combine(dir, $"{filename}.png");
                Java.IO.File file = new Java.IO.File(dir, $"{filename}.png");
                FileOutputStream input = new FileOutputStream(file);
                input.Write(bytes);
                input.Close();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}