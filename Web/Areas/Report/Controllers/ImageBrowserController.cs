using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
   
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    public class ImageBrowserController : Controller
// ReSharper restore ClassWithVirtualMembersNeverInherited.Global
    {
        private const string FileKey = "f";
        private const string DirectoryKey = "d";
        private const string ImageContentType = "image/png";
        private const string TextContentType = "text/plain";
        private const string DefaultFilter = "*.png,*.gif,*.jpg,*.jpeg";
        private const int ThumbnailHeight = 50;
        private const int ThumbnailWidth = 50;

        private readonly DirectoryBrowser _directoryBrowser;
        private readonly ThumbnailCreator _thumbnailCreator;

        public ImageBrowserController()
        {
            _directoryBrowser = new DirectoryBrowser();
            _thumbnailCreator = new ThumbnailCreator();
        }
        
        public virtual JsonResult Read(string path)
        {
            //Create root folder if its not exist
            if (!Directory.Exists(GlobalConfigVariable.LetterTemplateImagePath))
            {
                Directory.CreateDirectory(GlobalConfigVariable.LetterTemplateImagePath);
            }

            path = NormalizePath(path);

            if (CanAccess(path))
            {
                try
                {
                    var result = _directoryBrowser
                        .GetContent(path, DefaultFilter)
                        .Select(f => new
                        {
                            name = f.Name,
                            type = f.Type == EntryType.File ? FileKey : DirectoryKey,
                            size = f.Size
                        });

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (DirectoryNotFoundException)
                {
                    throw new HttpException(404, "File Not Found");
                }
            }
            throw new HttpException(403, "Forbidden");
        }


        public virtual bool AuthorizeThumbnail(string path)
        {
            return CanAccess(path);
        }

        [OutputCache(Duration = 3600, VaryByParam = "path")]
        public virtual ActionResult Thumbnail(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeThumbnail(path))
            {
                if (System.IO.File.Exists(path))
                {
                    Response.AddFileDependency(path);

                    return CreateThumbnail(path);
                }
                throw new HttpException(404, "File Not Found");
            }
            throw new HttpException(403, "Forbidden");
        }

        private FileContentResult CreateThumbnail(string physicalPath)
        {
            using (var fileStream = System.IO.File.OpenRead(physicalPath))
            {
                var desiredSize = new ImageSize
                {
                    Width = ThumbnailWidth,
                    Height = ThumbnailHeight
                };

                return File(_thumbnailCreator.Create(fileStream, desiredSize, ImageContentType), ImageContentType);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Destroy(string path, string name, string type)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
            {
                path = Path.Combine(GlobalConfigVariable.LetterTemplateImagePath, path, name);
                if (type.ToLowerInvariant() == FileKey)
                {
                    DeleteFile(path);
                }
                else
                {
                    DeleteDirectory(path);
                }

                return Json(null);
            }
            throw new HttpException(404, "File Not Found");
        }

        protected virtual void DeleteFile(string path)
        {
            if (!CanAccess(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        protected virtual void DeleteDirectory(string path)
        {
            if (!CanAccess(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Create(string path, FileBrowserEntry entry)
        {
            path = NormalizePath(path);
            var name = entry.Name;

            if (!string.IsNullOrEmpty(name) && CanAccess(path))
            {
                path = Path.Combine(path, name);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return Json(null);
            }

            throw new HttpException(403, "Forbidden");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Upload(string path, HttpPostedFileBase file)
        {
            path = NormalizePath(path);

            var fileName = Path.GetFileName(file.FileName);

            if (fileName != null && AuthorizeUpload(path, file))
            {
                file.SaveAs(Path.Combine(path, fileName));

                return Json(new
                {
                    size = file.ContentLength,
                    name = fileName,
                    type = FileKey
                }, TextContentType);
            }

            throw new HttpException(403, "Forbidden");
        }

        [OutputCache(Duration = 360, VaryByParam = "path")]
        public ActionResult Image(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeImage(path) && System.IO.File.Exists(path))
            {
                return File(System.IO.File.OpenRead(path), ImageContentType);
            }

            throw new HttpException(403, "Forbidden");
        }


        private string NormalizePath(string path)
        {
            return GlobalConfigVariable.LetterTemplateImagePath + "\\" + path;
        }

        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = DefaultFilter.Split(',');
            return allowedExtensions.Any(e => extension != null && e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual bool AuthorizeImage(string path)
        {
            return CanAccess(path) && IsValidFile(Path.GetExtension(path));
        }
        
        public virtual bool AuthorizeUpload(string path, HttpPostedFileBase file)
        {
            return CanAccess(path) && IsValidFile(file.FileName);
        }

        protected virtual bool CanAccess(string path)
        {
            return path.StartsWith(path, StringComparison.OrdinalIgnoreCase);
        }

    }
}
