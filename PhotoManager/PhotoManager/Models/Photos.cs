using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoManager.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        [Display(Name = "Titre")]
        [StringLength(50), Required]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required]
        public DateTime ShootDate { get; set; }

        public string GUID { get; set; }

        private DAL.ImageGUIDReference FileReference;

        public Photo()
        {
            Id = 0;
            OwnerId = 0;
            Title = "";
            Description = "";
            ShootDate = DateTime.Now;
            GUID = "";
            FileReference = new DAL.ImageGUIDReference(@"/App_Data_Photos/", @"No_image.png");
        }
        public String GetPhotoURL()
        {
            return FileReference.GetURL(GUID).Replace("~/", "") /*web relative url*/;
        }
        public void UpLoadPhoto(HttpRequestBase Request)
        {
            GUID = FileReference.UpLoadImage(Request, GUID);
        }
        public void RemovePhoto()
        {
            FileReference.Remove(GUID);
        }
        public object ToJSON()
        {
            object json = new { Id = Id,
                                Title = Title,
                                Description = Description,
                                ShootDate = ShootDate.ToLongDateString(),
                                GUID = GUID,
                                Source = GetPhotoURL()};

            return json;
        }
    }
    public class Photos:DAL.RecordsDB<Photo>
    {
        public Photos(DAL.DataBase db):base(db)
        {
            SetCache(true);
        }

        public override int Delete(int ID)
        {
            int result = 0;
            Photo photo = Get(ID);
            if (photo != null)
            {
                photo.RemovePhoto();
                result = base.Delete(ID);
            }
            return result;
        }

        public static List<object> ToJSON(List<Photo> photos)
        {
            var json = new List<object>();
            foreach(var photo in photos)
            {
                json.Add(photo.ToJSON());
            }
            return json;
        }
    }
}