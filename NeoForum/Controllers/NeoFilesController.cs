using Microsoft.AspNetCore.Mvc;
using NeoForum.Models;

namespace NeoForum.Controllers
{
    public class NeoFilesController : Controller
    {
		public IActionResult Index()
		{
            var model = new NeoFileViewModel();
            foreach (var item in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "upload")))
            {
                model.Files.Add(
                    new NeoFile { Name = System.IO.Path.GetFileName(item), Path = item});
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(IFormFile[] files)
        {
            foreach (var file in files)
            {
                // Получить имя файла из браузера
                var fileName = System.IO.Path.GetFileName(file.FileName);

                // Получить путь к загружаемому файлу
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "upload", fileName);

                // Проверка, существует ли файл с таким же именем, если да то удаляет его
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Создание нового локального файла и копирование содержимого загруженного файла
                using (var localFile = System.IO.File.OpenWrite(filePath))
                using (var uploadedFile = file.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
            }
            ViewBag.Message = "Файлы успешно загружены";

            // Получение файлов с сервера
            var model = new NeoFileViewModel();
            foreach (var item in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "upload")))
            {
                model.Files.Add(
                    new NeoFile { Name = System.IO.Path.GetFileName(item), Path = item});
            }
            return View(model);
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("Неверное имя файла");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "upload", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".zip", "archive/zip"},
                {".rar", "archive/rar"},
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
