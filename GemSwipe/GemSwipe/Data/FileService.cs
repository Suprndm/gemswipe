using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GemSwipe.Data
{
    public class FileService<T> where T : class, new()
    {
        protected string _fileName = AppSettings.PlayerPersonalDataFileName + nameof(T);
        protected T _fileData;

        public FileService()
        {
            if (DependencyService.Get<IFileHandler>().CheckExistenceOf(_fileName))
            {
                Load();
            }
            else
            {
                Initialize();
            }
        }

        public T GetData()
        {
            return _fileData;
        }

        public void SaveChanges()
        {
            string playerDataSerialized = JsonConvert.SerializeObject(_fileData);
            var fileWriter = DependencyService.Get<IFileHandler>();
            fileWriter.SaveText(_fileName, playerDataSerialized);
        }

        public void SaveBackup()
        {
            var fileHandler = DependencyService.Get<IFileHandler>();
            string currentSave = fileHandler.LoadText(_fileName);
            fileHandler.SaveText("backup_" + _fileName, currentSave);
        }

        public void Load()
        {
            var fileReader = DependencyService.Get<IFileHandler>();
            var result = fileReader.LoadText(_fileName);
            _fileData = JsonConvert.DeserializeObject<T>(result);
        }

        protected void Initialize()
        {
            _fileData = new T();
            SaveChanges();
        }

    }
}
