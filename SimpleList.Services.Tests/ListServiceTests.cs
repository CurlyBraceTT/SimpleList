using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleList.Models;
using Xunit;

namespace SimpleList.Services.Tests
{
    public class ListServiceTests
    {
        [Fact]
        public async Task All()
        {
            var options = new DbContextOptionsBuilder<DataContext.ListDataContext>()
                .UseInMemoryDatabase("All")
                .Options;

            var dbItems = new List<DataContext.ListItem>();
            var length = 3;
                
            for(var i = 0; i < length; i++)
            {
                dbItems.Add(new DataContext.ListItem
                {
                    Description = "Testing...",
                    Done = false
                });
            };

            using (var context = new DataContext.ListDataContext(options))
            {
                context.Items.AddRange(dbItems);
                context.SaveChanges();
            }

            using (var context = new DataContext.ListDataContext(options))
            {
                var service = new ListService(context, CreateMapper());
                var items = await service.All();
                Assert.Equal(length, items.Count);
            }
        }

        [Fact]
        public async Task Update()
        {
            var options = new DbContextOptionsBuilder<DataContext.ListDataContext>()
                .UseInMemoryDatabase("Update")
                .Options;

            var dbItem = new DataContext.ListItem
            {
                Description = "Testing...",
                Done = false
            };

            using (var context = new DataContext.ListDataContext(options))
            {
                context.Items.Add(dbItem);
                context.SaveChanges();
            }

            var item = new ListItem
            {
                Id = dbItem.Id,
                Description = "Updated...",
                Done = false
            };

            using (var context = new DataContext.ListDataContext(options))
            {
                var service = new ListService(context, CreateMapper());
                await service.Update(item);
            }

            using (var context = new DataContext.ListDataContext(options))
            {
                var updated = context.Items.Single();
                AssertAllPropertiesEqual(item, updated);
            }
        }

        [Fact]
        public async Task Get()
        {
            var options = new DbContextOptionsBuilder<DataContext.ListDataContext>()
                .UseInMemoryDatabase("Get")
                .Options;

            using (var context = new DataContext.ListDataContext(options))
            {
                context.Database.EnsureCreated();
            }

            var dbItem = new DataContext.ListItem
            {
                Description = "Testing...",
                Done = false
            };

            using (var context = new DataContext.ListDataContext(options))
            {
                context.Items.Add(dbItem);
                context.SaveChanges();
            }

            using (var context = new DataContext.ListDataContext(options))
            {
                var service = new ListService(context, CreateMapper());
                var item = await service.Get(dbItem.Id);
                AssertAllPropertiesEqual(item, dbItem);
            }
        }

        [Fact]
        public async Task Remove()
        {
            var options = new DbContextOptionsBuilder<DataContext.ListDataContext>()
                .UseInMemoryDatabase("Remove")
                .Options;

            using (var context = new DataContext.ListDataContext(options))
            {
                context.Database.EnsureCreated();
            }

            var item = new ListItem
            {
                Description = "Testing...",
                Done = false
            };

            using (var context = new DataContext.ListDataContext(options))
            {
                var service = new ListService(context, CreateMapper());
                var newId = (await service.Add(item)).Id;
                await service.Remove(newId);
            }

            using (var context = new DataContext.ListDataContext(options))
            {
                Assert.Equal(0, context.Items.Count());
            }
        }

        [Fact]
        public async Task Add()
        {
            var options = new DbContextOptionsBuilder<DataContext.ListDataContext>()
                .UseInMemoryDatabase("Add")
                .Options;

            // Create the schema in the database
            using (var context = new DataContext.ListDataContext(options))
            {
                context.Database.EnsureCreated();
            }

            var item = new ListItem
            {
                Description = "Testing...",
                Done = false
            };

            // Run the test against one instance of the context
            using (var context = new DataContext.ListDataContext(options))
            {
                var service = new ListService(context, CreateMapper());
                await service.Add(item);
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new DataContext.ListDataContext(options))
            {
                Assert.Equal(1, context.Items.Count());
                var inserted = context.Items.Single();
                AssertAllPropertiesEqual(item, inserted);
            }
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ServicesMappingProfile>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        }

        private void AssertAllPropertiesEqual(ListItem item, DataContext.ListItem dbItem)
        {
            Assert.Equal(dbItem.Description, item.Description);
            Assert.Equal(dbItem.Done, item.Done);
        }
    }
}
