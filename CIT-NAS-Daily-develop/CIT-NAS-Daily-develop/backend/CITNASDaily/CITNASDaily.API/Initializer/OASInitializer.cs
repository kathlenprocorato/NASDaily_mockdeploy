using CITNASDaily.Entities.Models;
using CITNASDaily.Repositories.Context;
using CITNASDaily.Utils;
using Microsoft.EntityFrameworkCore;

namespace CITNASDaily.API.Initializer
{
    public static class OASInitializer
    {
        public static WebApplication Seed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<NASContext>();
                try
                {
                    context.Database.EnsureCreated();
                    var users = context.Users.FirstOrDefault();
                    if (users is null)
                    {
                        context.Users.Add(new User
                        {
                            Username = "admin",
                            PasswordHash = PasswordManager.HashPassword("admin"),
                            Role = "OAS"
                        });

                        context.Users.Add(new User
                        {
                            Username = "telamomerian",
                            PasswordHash = PasswordManager.HashPassword("telamomerian"),
                            Role = "Superior"
                        });

                        context.SaveChanges();
                    }

                    var oas = context.OAS.FirstOrDefault();
                    if (oas is null)
                    {
                        context.OAS.Add(new OAS
                        {
                            Username = "admin",
                            FirstName = "Ana Marie",
                            MiddleName = "",
                            LastName = "Granaderos",
                            User = context.Users.Where(u => u.Username == "admin").FirstOrDefault()
                        });

                        context.SaveChanges();
                    }

                    var office = context.Offices.FirstOrDefault();
                    List<string> officeList = new()
                    {
                        "TSG",
                        "CCJ OFFICE",
                        "CCS",
                        "CNAHS",
                        "CREATE",
                        "WILDCATS INNOVATION",
                        "ETO",
                        "EXECUTIVE OFFICE",
                        "FAO",
                        "MARKETING OFFICE",
                        "UNIVERSITY REGISTRAR",
                        "COLLEGE GUIDANCE",
                        "ELEMENTARY",
                        "NLO",
                        "OAS",
                        "MASSCOM LABORATORY",
                        "SAO",
                        "SHS",
                        "SSD",
                        "COLLEGE/SHS/ELEM",
                        "LIB.",
                        "MAIN CLINIC",
                        "HRD",
                        "ALUMNI AFFAIRS OFFICE",
                        "MSDO",
                        "IMPO",
                        "JHS PRINCIPAL'S OFFICE",
                        "JHS-COMP. LABORATORY",
                        "JHS-SAO",
                        "CMBA-DHM",
                        "CASE-BIO",
                        "CASE-DHBS_PSYCH",
                        "CASE-PE",
                        "DEMPC",
                        "ECE-ETEEAP-TECH",
                        "CASE",
                        "CMBA OFFICE",
                        "RDCO/ITSO",
                        "CEA-ECE",
                        "CMBA-ETEEAP",
                        "CEA-CPE",
                        "CEA-ME LABORATORY",
                        "CEA-MINING",
                        "CES",
                        "VPAA",
                        "CEA-CHE",
                        "PCO",
                        "CEA-CE",
                        "CEA-IE",
                        "CEA",
                        "NSTP",
                        "QAO"
                    };
                    officeList.Sort();

                    if (office is null)
                    {
                        foreach(string officeName in officeList)
                        {
                            context.Offices.Add(new Office
                            {
                                OfficeName = officeName,
                            });
                        }
                        context.SaveChanges();
                    }

                    var superior = context.Superiors.FirstOrDefault();
                    if(superior is null)
                    {
                        context.Superiors.Add(new Superior
                        {
                            FirstName = "Merian",
                            LastName = "Telamo",
                            Username = "telamomerian",
                            User = context.Users.Where(u => u.Username == "telamomerian").FirstOrDefault()
                        });
                        context.SaveChanges();
                    }

                    var existingOASOffice = context.Offices.FirstOrDefault(o => o.OfficeName == "OAS");
                    var oasSuperior = context.Superiors.FirstOrDefault(s => s.Username == "telamomerian");
                    if (existingOASOffice != null && oasSuperior != null && existingOASOffice.SuperiorFirstName == null && existingOASOffice.SuperiorLastName == null)
                    {
                        existingOASOffice.SuperiorFirstName = oasSuperior.FirstName;
                        existingOASOffice.SuperiorLastName = oasSuperior.LastName;
                        context.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return app;
            }
        }
    }
}