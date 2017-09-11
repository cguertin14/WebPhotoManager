using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoManager.Controllers
{
    [Models.ConnectedUserOnly]
    public class PhotosController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SetDateFilter(/* dates filter parameters*/)
        {
            // TODO
            // régler les paramètres de filtrage de dates
            return null;
        }

        public ActionResult AddKeywordFilter(/* keywordId*/)
        {
            // TODO
            // ajouter un mot-clé dans les critères de recherche
            return null;
        }
        public ActionResult RemoveKeywordFilter(/* keywordId*/)
        {
            // TODO
            // retirer un mot-clé dans les critères de recherche
            return null;
        }

        public ActionResult FlushAllKeywordsFilter()
        {
            // TODO
            // retirer tous les mots-clé des critères
            return null;
        }

        public ActionResult GetPhotos()
        {
            // TODO
            // utiliser les critères de sélection pour produire la bonne liste de photos

            int currentUserId = ((Models.User)Session["CurrentUser"]).Id;
            List<Models.Photo> filteredPhotos = DB.Photos.ToList()
                                                .Where(photo => photo.OwnerId == currentUserId)
                                                .OrderBy(photo => photo.ShootDate)
                                                .ToList();
            return new JsonResult { Data = Models.Photos.ToJSON(filteredPhotos), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Edit(int id = 0)
        {
            int currentUserId = ((Models.User)Session["CurrentUser"]).Id;
            Models.Photo photo = null;
            if (id != 0)
            {
                photo = DB.Photos.Get(id);
                if (photo.OwnerId != currentUserId)
                    throw (new Exception("Illegal access!!!"));
            }
            else
            {
                photo = new Models.Photo();
                photo.OwnerId = currentUserId;
            }
            return PartialView("Edit", photo);
        }
        [HttpPost]
        public ActionResult Edit(Models.Photo photo)
        {
            bool error = false;
            try
            {
                DB.DataBase.BeginTransaction();
                photo.UpLoadPhoto(Request);
                if (photo.Id == 0)
                    DB.Photos.Add(photo);
                else
                    DB.Photos.Update(photo);
            }
            catch (Exception)
            {
                error = true;
            }
            finally
            {
                DB.DataBase.EndTransaction(error);
            }
            return new JsonResult { Data = new { status = !error } };
        }

        public ActionResult Delete(int id)
        {
            // TODO
            return null;
        }
    }

}