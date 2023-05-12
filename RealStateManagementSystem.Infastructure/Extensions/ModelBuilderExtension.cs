using Microsoft.EntityFrameworkCore;
using RealStateManagementSystem.Domain.Entities;
using RealStateManagementSystem.Domain.HashingMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Infastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SeedData(this ModelBuilder builder)
        {

            builder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SistemYoneticisi", CreateDate = DateTime.Now, IsActive = true },
                new Role { Id = 2, Name = "Kullanici", CreateDate = DateTime.Now, IsActive = true }
            );

            // İller
            builder.Entity<Province>().HasData(
                new Province { Id = 1, Name = "Ankara", CreateDate = DateTime.Now, IsActive = true },
                new Province { Id = 2, Name = "İstanbul", CreateDate = DateTime.Now, IsActive = true },
                new Province { Id = 3, Name = "İzmir", CreateDate = DateTime.Now, IsActive = true }
            );

            // İlçeler
            builder.Entity<District>().HasData(
                // Ankara
                new District { Id = 1, Name = "Çankaya", ProvinceId = 1, CreateDate = DateTime.Now, IsActive = true },
                new District { Id = 2, Name = "Keçiören", ProvinceId = 1, CreateDate = DateTime.Now, IsActive = true },
                new District { Id = 3, Name = "Yenimahalle", ProvinceId = 1, CreateDate = DateTime.Now, IsActive = true },
                // İstanbul
                new District { Id = 4, Name = "Levent", ProvinceId = 2, CreateDate = DateTime.Now, IsActive = true },
                new District { Id = 5, Name = "Kadıköy", ProvinceId = 2, CreateDate = DateTime.Now, IsActive = true },
                new District { Id = 6, Name = "Beşiktaş", ProvinceId = 2, CreateDate = DateTime.Now, IsActive = true },
                // İzmir
                new District { Id = 7, Name = "Bornova", ProvinceId = 3, CreateDate = DateTime.Now, IsActive = true },
                new District { Id = 8, Name = "Karşıyaka", ProvinceId = 3, CreateDate = DateTime.Now, IsActive = true },
                new District { Id = 9, Name = "Konak", ProvinceId = 3, CreateDate = DateTime.Now, IsActive = true }
            );

            // Mahalleler
            builder.Entity<Neighbourhood>().HasData(
                // Çankaya
                new Neighbourhood { Id = 1, Name = "Dikmen", DistrictId = 1, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 2, Name = "Bahçelievler", DistrictId = 1, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 3, Name = "Kızılay", DistrictId = 1, CreateDate = DateTime.Now, IsActive = true },
                // Keçiören
                new Neighbourhood { Id = 4, Name = "Etlik", DistrictId = 2, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 5, Name = "Karşıyaka", DistrictId = 2, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 6, Name = "Güzelkent", DistrictId = 2, CreateDate = DateTime.Now, IsActive = true },
                // Yenimahalle
                new Neighbourhood { Id = 7, Name = "Batıkent", DistrictId = 3, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 8, Name = "Demetevler", DistrictId = 3, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 9, Name = "Ümitköy", DistrictId = 3, CreateDate = DateTime.Now, IsActive = true },
                // Levent
                new Neighbourhood { Id = 10, Name = "Levent Mahallesi", DistrictId = 4, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 11, Name = "Levent 1. Bölge", DistrictId = 4, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 12, Name = "Levent 2. Bölge", DistrictId = 4, CreateDate = DateTime.Now, IsActive = true },
                // Kadıköy
                new Neighbourhood { Id = 13, Name = "Caddebostan", DistrictId = 5, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 14, Name = "Fenerbahçe", DistrictId = 5, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 15, Name = "Göztepe", DistrictId = 5, CreateDate = DateTime.Now, IsActive = true },
                // Beşiktaş
                new Neighbourhood { Id = 16, Name = "Abbasağa", DistrictId = 6, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 17, Name = "Akaretler", DistrictId = 6, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 18, Name = "Arnavutköy", DistrictId = 6, CreateDate = DateTime.Now, IsActive = true },
                // Bornova
                new Neighbourhood { Id = 19, Name = "Kazımdirik", DistrictId = 7, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 20, Name = "Çamdibi", DistrictId = 7, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 21, Name = "Gülbahçe", DistrictId = 7, CreateDate = DateTime.Now, IsActive = true },
                // Karşıyaka
                new Neighbourhood { Id = 22, Name = "Bahçelievler", DistrictId = 8, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 23, Name = "Bostanlı", DistrictId = 8, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 24, Name = "Çiğli", DistrictId = 8, CreateDate = DateTime.Now, IsActive = true },
                // Konak
                new Neighbourhood { Id = 25, Name = "Alsancak", DistrictId = 9, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 26, Name = "Bahribaba", DistrictId = 9, CreateDate = DateTime.Now, IsActive = true },
                new Neighbourhood { Id = 27, Name = "Basmane", DistrictId = 9, CreateDate = DateTime.Now, IsActive = true }
             );

            var appUser = new AppUser
            {
                Id = 1,
                Name = "Erhan",
                Surname = "Gökçeoğlu",
                Address = "Etimesgut",
                Email = "erhangokceoglu@hotmail.com",
                CreateDate = DateTime.Now,
                Password = "Ankara1.",
                IsActive = true,
                RoleId = 1,
            };

            var appUser2 = new AppUser
            {
                Id = 2,
                Name = "Erhan",
                Surname = "Gökçeoğlu",
                Address = "Etimesgut",
                Email = "erhangokceoglu91@gmail.com",
                CreateDate = DateTime.Now,
                Password = "Ankara1.",
                IsActive = true,
                RoleId = 2,
            };

            byte[] salt, hash, salt2, hash2;
            Hashing.CreatePasswordHash(appUser.Password, out hash, out salt);
            Hashing.CreatePasswordHash(appUser2.Password, out hash2, out salt2);
            appUser.PasswordHash = hash;
            appUser.PasswordSalt = salt;
            appUser2.PasswordHash = hash2;
            appUser2.PasswordSalt = salt2;
            builder.Entity<AppUser>().HasData(appUser);
            builder.Entity<AppUser>().HasData(appUser2);
        }
    }
}
