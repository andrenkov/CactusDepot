using CactusDepot.Shared;
using CactusDepot.Shared.Models.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace CactusDepot.Seeds.Controllers
{
    [Authorize(Roles = "Administrator,Manager,SuperAdmin")]
    public partial class SeedsController : Controller
    {
        #region Seed photo
        [HttpGet]
        public async Task<IActionResult> EditPhoto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SharedUtil.WriteLogToConsole("SeedsController", "GET.EditPhoto()");
            CactusSeed? oneseed = await _context.CactusSeeds.FindAsync(id);

            if (oneseed is not null)
            {

                SeedPicSaveModel pic = new()
                {
                    SeedId = oneseed.SeedId,
                    SeedName = oneseed.SeedName,
                    CatalogNum = oneseed.Parent1CatalogNum,
                    SeedPhotoFile = null
                };
                return View(pic);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePhoto(SeedPicSaveModel _model)
        {
            if (ModelState.IsValid)
            {
                SharedUtil.WriteLogToConsole("SeedsController", "POST.SavePhoto()");
                string? uniqueFileName;
                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if ((_model is not null) && (_model.SeedPhotoFile is not null))
                {
                    #region Resize Image
                    Image img = ResizeImage(_model.SeedPhotoFile, 600, 400);
                    #endregion

                    #region Upload Folder path
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    //string uploadsFolder = Path.Combine(_hostingWebEnvironment.WebRootPath, "Db", "Images");//path "/app/wwwroot/Db/Images"
                    if (!Directory.Exists(_uploadsFolder))
                    {
                        Directory.CreateDirectory(_uploadsFolder);
                    }
                    #endregion

                    #region Copy file

                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + _model.SeedPhotoFile.FileName;
                    string filePath = Path.Combine(_uploadsFolder, uniqueFileName);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    try
                    {
                        //_model.SeedPhotoFile.CopyTo(new FileStream(filePath, FileMode.Create));
                        img.SaveAsJpeg(filePath);
                    }
                    catch (Exception ex)
                    {
                        SharedUtil.WriteLogToConsole("Exception", $"POST.SavePhoto(): {ex.Message}");
                    }
                    #endregion

                    CactusSeed? oneseed = await _context.CactusSeeds.FindAsync(_model.SeedId);
                    if (oneseed is not null)
                    {
                        #region clean existing file first
                        if (!string.IsNullOrEmpty(oneseed.SeedSource))
                        {
                            string oldFilePath = Path.Combine(_uploadsFolder, oneseed.SeedSource);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                try
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                                catch (IOException ioex)
                                {
                                    SharedUtil.WriteLogToConsole("Exception", $"POST.SavePhoto().File.Delete.IOException: {ioex.Message}");
                                }
                                catch (Exception ex)
                                {
                                    SharedUtil.WriteLogToConsole("Exception", $"POST.SavePhoto().File.Delete.Exception: {ex.Message}");
                                }
                                SharedUtil.WriteLogToConsole("SeedsController", "POST.SavePhoto().OldFileDeleted");
                            }
                        }
                        #endregion

                        #region Save Db Context
                        oneseed.SeedSource = uniqueFileName;
                        try
                        {
                            _context.Update(oneseed);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!SeedExists(oneseed.SeedId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        SharedUtil.WriteLogToConsole("SeedsController", "POST.SavePhoto().NewFileSaved");
                        #endregion

                        return RedirectToAction(nameof(Edit), new { id = oneseed.SeedId });
                    }
                }

            }
            ViewBag.Endpoint = "EditPhoto";
            return View("NotFound");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ViewPhoto(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }
            SharedUtil.WriteLogToConsole("SeedsController", "GET.ViewPhoto()");

            CactusSeed? oneseed = await _context.CactusSeeds.FindAsync(id);

            if ((oneseed is not null) && (oneseed.SeedSource is not null))
            {
                //return View(oneseed);
                string filePath = Path.Combine(_uploadsFolder, oneseed.SeedSource);
                SeedPicViewModel picData = new()
                {
                    SeedId = oneseed.SeedId,
                    SeedName = oneseed.SeedName,
                    SeedsPhotoData = GetImage(filePath, "image/jpeg")
                };
                return View(picData);
            }
            SharedUtil.WriteLogToConsole("SeedsController", "GET.ViewPhoto(): Seed has no photo");
            return View("NotFound");
        }

        [HttpPost, ActionName("DeletePhoto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoto(int? id)
        {
            if (id is not null)
            {
                SharedUtil.WriteLogToConsole("SeedsController", "POST.DeletePhoto()");
                CactusSeed? oneseed = await _context.CactusSeeds.FindAsync(id);

                if (oneseed is not null)
                {
                    #region clean existing file first
                    if (!string.IsNullOrEmpty(oneseed.SeedSource))
                    {
                        string fileDelPath = Path.Combine(_uploadsFolder, oneseed.SeedSource);
                        if (System.IO.File.Exists(fileDelPath))
                        {
                            try
                            {
                                System.IO.File.Delete(fileDelPath);
                            }
                            catch (IOException ioex)
                            {
                                SharedUtil.WriteLogToConsole("Exception", $"POST.SavePhoto().File.Delete.IOException: {ioex.Message}");
                            }
                            catch (Exception ex)
                            {
                                SharedUtil.WriteLogToConsole("Exception", $"POST.SavePhoto().File.Delete.Exception: {ex.Message}");
                            }
                            SharedUtil.WriteLogToConsole("SeedsController", "POST.SavePhoto().OldFileDeleted");
                        }
                    }
                    #endregion

                    #region Save Db Context
                    oneseed.SeedSource = null;
                    try
                    {
                        _context.Update(oneseed);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SeedExists(oneseed.SeedId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    SharedUtil.WriteLogToConsole("SeedsController", "POST.DeletePhoto().Deleted");
                    #endregion

                    return RedirectToAction(nameof(Edit), new { id = oneseed.SeedId });
                }
            }

            ViewBag.Endpoint = "DeletePhoto";
            return View("NotFound");
        }

        private static string GetImage(string fileName, string fileType)// ImageDataUrl = GetImage("utah2017.jpg", "image/jpeg"),
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Db/Images", fileName);
            byte[] imageBytes = System.IO.File.ReadAllBytes(path);
            return $"data:{fileType}; base64,{Convert.ToBase64String(imageBytes)}";
        }

        private static Image ResizeImage(IFormFile sourceImage, int width = 600, int height = 400)
        {
            Image res = Image.Load(sourceImage.OpenReadStream());

            if (res.Height > res.Width)
            {
                res.Mutate(x => x.Resize(height, width));//portrate
            }
            else
            {
                res.Mutate(x => x.Resize(width, height));//landscape
            }

            return res;
        }



        #endregion

    }
}
