using Microsoft.EntityFrameworkCore;
using System;
using WebApp.DAL;
using Xunit;

namespace Webapp.Tests
{
    public class UnitTestAppDbContext
    {

        public static ApplicationDbContext GenerateDbContext()
        {

            var uniqueId = Guid.NewGuid().ToString();


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: uniqueId)
              .Options;
            var context = new ApplicationDbContext(options);

            context.Products.Add(
                    new Product
                    {
                        Id = 1,
                        Name = "First",
                        Rating = 3
                    });
            context.Products.Add(new Product
            {
                Id = 2,
                Name = "Second",
                Rating = 5
            });

            context.Products.Add(new Product
            {
                Id = 3,
                Name = "Second",
                Rating = 5
            }
                    );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public void Test1()
        {
            Assert.Equal(1, 1);
        }

        [Fact]
        public async void Test2()
        {
            var _dbContext = GenerateDbContext();

            Assert.Equal(3, await _dbContext.Products.AsNoTracking().CountAsync());

        }
    }
    }
