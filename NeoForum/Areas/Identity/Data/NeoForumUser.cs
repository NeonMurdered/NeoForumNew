using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NeoForum.Models;
using NeoForum.Models.Enums;

namespace NeoForum.Areas.Identity.Data;

// Добавление данных профиля для пользователей приложения
public class NeoForumUser : IdentityUser
{
    public NeoForumUser()
    {
        Messages = new HashSet<Message>();
        AdminMessages = new HashSet<AdminMessage>();
    }
    public virtual ICollection<Message> Messages { get; set; }

    public virtual ICollection<AdminMessage> AdminMessages { get; set; }

    [PersonalData]
    [Column(TypeName ="nvarchar(100)")]
    public string? FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? LastName { get; set; } 

    public byte[]? ProfilePicture { get; set; }

    public Country Country { get; set; }

    [PersonalData]
    public int Age { get; set; }
}

