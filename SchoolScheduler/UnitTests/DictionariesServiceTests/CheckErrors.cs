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
    public class CheckErrors {
        [Fact]
        public async Task GetDictionaryElementAsyncTest() {
            await GetDictionaryElementAsync<Room>();
            await GetDictionaryElementAsync<Teacher>();
            await GetDictionaryElementAsync<Subject>();
            await GetDictionaryElementAsync<ClassGroup>();

        }
        private async Task GetDictionaryElementAsync<T>() where T : DictionaryElementBase, new() {
            using var context = PrepareData.GetDbContext(); var service = new GenericDictionaryService<T>(context);
            var result = await service.GetDictionaryElementAsync(2137);
            Assert.True(result.IsLeft);
        }
        [Fact]
        public async Task AddKey_CreateElementWhichNameAlreadyExists_ErrorOccured() {
            await AddKey<Room>("111");
            await AddKey<Teacher>("kowalski");
            await AddKey<Subject>("eng");
            await AddKey<ClassGroup>("1a");
        }
        private async Task AddKey<T>(string name) where T : DictionaryElementBase, new() {
            using var context = PrepareData.GetDbContext(); var service = new GenericDictionaryService<T>(context);
            var dictionaryElement = new DictionaryElementCreateDTO() {
                Name = name
            };
            var result = await service.AddKey(dictionaryElement);
            Assert.True(result.IsLeft);
        }
        [Fact]
        public async Task UpdateKey_EditElementWhichNotExists_ErrorOccured() {
            await UpdateKey<Room>();
            await UpdateKey<Teacher>();
            await UpdateKey<Subject>();
            await UpdateKey<ClassGroup>();
        }
        private async Task UpdateKey<T>() where T : DictionaryElementBase, new() {
            using var context = PrepareData.GetDbContext(); var service = new GenericDictionaryService<T>(context);
            var dictionaryElement = new DictionaryElementEditDTO() {
                Id = 1000
            };
            var result = await service.UpdateKey(dictionaryElement);
            Assert.True(result.IsLeft);
        }
        [Fact]
        public async Task UpdateKey_EditElementWhichNameAlreadyExists_ErrorOccured() {
            await UpdateKey<Room>("111", "112");
            await UpdateKey<Teacher>("kowalski", "nowak");
            await UpdateKey<Subject>("eng", "mat");
            await UpdateKey<ClassGroup>("1a", "2a");
        }
        private async Task UpdateKey<T>(string name, string newName) where T : DictionaryElementBase, new() {
            using var context = PrepareData.GetDbContext(); var service = new GenericDictionaryService<T>(context);
            var element = context.Set<T>().FirstOrDefault(e => e.Name == name);
            var dictionaryElement = new DictionaryElementEditDTO() {
                Id = element.Id,
                Name = newName
            };
            var result = await service.UpdateKey(dictionaryElement);
            Assert.True(result.IsLeft);
        }
        [Fact]
        public async Task DeleteElementWhichNotExists_ThrowsArgumentException() {
            await DeleteElement<Room>();
            await DeleteElement<Teacher>();
            await DeleteElement<Subject>();
            await DeleteElement<ClassGroup>();
        }
        private async Task DeleteElement<T>() where T : DictionaryElementBase, new() {
            using var context = PrepareData.GetDbContext(); var service = new GenericDictionaryService<T>(context);
            var result = await service.RemoveKey(10000, null);
            Assert.True(result.IsLeft);
        }
    }
}