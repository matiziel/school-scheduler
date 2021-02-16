using Xunit;
using System.Threading.Tasks;
using Application;
using System;
using Common;
using Persistence;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Contracts.DataTransferObjects.Dictionaries;
using Model;

namespace UnitTests.DictionariesServiceTests {
    public class CheckCorrectPaths {

        [Fact]
        public async Task GetDictionaryElementAsync() {
            await GetDictionaryElementGenericAsync<Room>("111");
            await GetDictionaryElementGenericAsync<Teacher>("kowalski");
            await GetDictionaryElementGenericAsync<Subject>("eng");
            await GetDictionaryElementGenericAsync<ClassGroup>("1a");
        }
        private async Task GetDictionaryElementGenericAsync<T>(string name) where T : DictionaryElementBase, new() {
            using var context = PrepareData.GetDbContext(); var service = new GenericDictionaryService<T>(context);
            var dictionaryElement = context.Set<T>().FirstOrDefault(e => e.Name == name);
            var dictionaryElementDTO = await service.GetDictionaryElementAsync(dictionaryElement.Id);
            Assert.True(dictionaryElementDTO.IsRight);
            dictionaryElementDTO.IfRight(r => {
                Assert.Equal(dictionaryElement.Id, r.Id);
                Assert.Equal(dictionaryElement.Name, r.Name);
            });
        }

        [Fact]
        public async Task GetDictionaryAsync() {
            await GetDictionaryGenericAsync<Room>();
            await GetDictionaryGenericAsync<Teacher>();
            await GetDictionaryGenericAsync<Subject>();
            await GetDictionaryGenericAsync<ClassGroup>();
        }
        private async Task GetDictionaryGenericAsync<T>() where T : DictionaryElementBase, new() {
            using var context = PrepareData.GetDbContext(); var service = new GenericDictionaryService<T>(context);
            int size = context.Set<T>().Count();
            var dictionaryIndexDTO = await service.GetDictionaryAsync();
            Assert.True(dictionaryIndexDTO.IsRight);
            dictionaryIndexDTO.IfRight(r => {
                Assert.Equal(size, r.Count());
            });
        }
        [Fact]
        public async Task AddKeyTest() {
            await AddKeyGeneric<Room>();
            await AddKeyGeneric<Teacher>();
            await AddKeyGeneric<Subject>();
            await AddKeyGeneric<ClassGroup>();
        }
        private async Task AddKeyGeneric<T>() where T : DictionaryElementBase, new() {
            using var context = PrepareData.GetDbContext(); var service = new GenericDictionaryService<T>(context);
            var dictionaryElement = new DictionaryElementCreateDTO() {
                Name = "elo"
            };
            var result = await service.AddKey(dictionaryElement);
            Assert.True(result.IsRight);
            var value = context.Set<T>().FirstOrDefault(e => e.Name == dictionaryElement.Name);
            Assert.NotNull(value);
        }
        [Fact]
        public async Task UpdateKeyTest() {
            await UpdateKey<Room>("111");
            await UpdateKey<Teacher>("kowalski");
            await UpdateKey<Subject>("eng");
            await UpdateKey<ClassGroup>("1a");
        }
        private async Task UpdateKey<T>(string name) where T : DictionaryElementBase, new() {
            using var context = PrepareData.GetDbContext(); var service = new GenericDictionaryService<T>(context);
            var element = context.Set<T>().AsNoTracking().FirstOrDefault(e => e.Name == name);
            var dictionaryElement = new DictionaryElementEditDTO() {
                Id = element.Id,
                Name = "elo"
            };
            var result = await service.UpdateKey(dictionaryElement);
            Assert.True(result.IsRight);
            var value = context.Set<T>().AsNoTracking().FirstOrDefault(e => e.Id == element.Id);
            Assert.Equal("elo", value.Name);
        }
        [Fact]
        public async Task RemoveKeyTest() {
            await RemoveKey<Room>("111");
            await RemoveKey<Teacher>("kowalski");
            await RemoveKey<Subject>("eng");
            await RemoveKey<ClassGroup>("1a");
        }
        private async Task RemoveKey<T>(string name) where T : DictionaryElementBase, new() {
            using var context = PrepareData.GetDbContext(); var service = new GenericDictionaryService<T>(context);
            var element = context.Set<T>().AsNoTracking().FirstOrDefault(e => e.Name == name);
            var result = await service.RemoveKey(element.Id, element.Timestamp);
            Assert.True(result.IsRight);
            var value = context.Set<T>().AsNoTracking().FirstOrDefault(e => e.Id == element.Id);
            Assert.Null(value);
        }
    }
}