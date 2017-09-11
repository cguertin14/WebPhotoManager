using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoManager.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public User()
        {
            Id = 0;
            UserName =
            Password =
            FirstName =
            LastName = "";
        }
        public User Clone()
        {
            User clone = new User();
            clone.Id = this.Id;
            clone.UserName = this.UserName;
            clone.Password = this.Password;
            clone.FirstName = this.FirstName;
            clone.LastName = this.LastName;
            return clone;
        }
        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }

    public class Users : DAL.RecordsDB<User>
    {
        public Users(DAL.DataBase dataBase) : base(dataBase)
        {
            SetCache(true);
        }

        public override int Add(User user)
        {
            user.Password = DAL.Encryption.Encrypt(user.Password);
            return base.Add(user);
        }

        public override User Get(int id)
        {
            User clone = null;
            User user = base.Get(id);
            if (user != null)
            {
                clone = user.Clone();
                clone.Password = DAL.Encryption.Decrypt(clone.Password);
            }
            return clone;
        }

        public override int Update(User user)
        {
            if (user != null)
                user.Password = DAL.Encryption.Encrypt(user.Password);
            return base.Update(user);
        }

        public User GetByName(string name)
        {
            User clone = null;
            User userFound = GetFirstByFieldName("UserName", name);
            if (userFound != null)
            {
                clone = userFound.Clone();
                clone.Password = DAL.Encryption.Decrypt(clone.Password);
            }
            return clone;
        }
    }

    public class UserView
    {
        public int Id { get; set; }

        [Display(Name = "Nom d'usager")]
        [RegularExpression(@"^((?!^Name$)[-a-zA-Z0-9àâäçèêëéìîïòôöùûüÿñÀÂÄÇÈÊËÉÌÎÏÒÔÖÙÛÜ_.'])+$", ErrorMessage = "Caractères illégaux.")]
        [StringLength(50), Required]
        public string UserName { get; set; }

        [Display(Name = "Mot de passe")]
        [Required]
        [StringLength(50, ErrorMessage = "Le mot de passe doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmation")]
        [Compare("Password", ErrorMessage = "Le mot de passe et celui de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Prénom")]
        [RegularExpression(@"^((?!^Name$)[-a-zA-Z0-9àâäçèêëéìîïòôöùûüÿñÀÂÄÇÈÊËÉÌÎÏÒÔÖÙÛÜ_.'])+$", ErrorMessage = "Caractères illégaux.")]
        [StringLength(50), Required]
        public string FirstName { get; set; }

        [Display(Name = "Nom")]
        [RegularExpression(@"^((?!^Name$)[-a-zA-Z0-9àâäçèêëéìîïòôöùûüÿñÀÂÄÇÈÊËÉÌÎÏÒÔÖÙÛÜ_. '])+$", ErrorMessage = "Caractères illégaux.")]
        [StringLength(50), Required]
        public string LastName { get; set; }

        public User ToUser()
        {
            User user = new User();
            user.Id = Id;
            user.UserName = UserName;
            user.Password = Password;
            user.FirstName = FirstName;
            user.LastName = LastName;
            return user;
        }

        public UserView()
        {
            Id = 0;
            UserName = "";
            Password = "";
            FirstName = "";
            LastName = "";
        }

        public UserView(User user)
        {
            Id = user.Id;

            UserName = user.UserName;
            Password = user.Password;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }

    public class LoginView
    {
        [Display(Name = "Nom d'usager")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Mot de passe")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public LoginView()
        {
            UserName = "";
            Password = "";
        }
    }
}