using Xunit;
using System.Threading.Tasks;
using Application;
using System;
using Common;
using Persistence;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Contracts.ViewModels.Dictionaries;

namespace UnitTests.DictionariesServiceTests {
    public class CheckErrors {
        [Theory]
        [InlineData(DataType.ClassGroup)]
        [InlineData(DataType.Room)]
        [InlineData(DataType.Subject)]
        [InlineData(DataType.Teacher)]
        public async Task GetDictionaryElementAsync(DataType type) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await service.GetDictionaryElementAsync(2137, type));
            }
        }
        private ValueTuple<int, string> GetDictionaryElement(ApplicationDbContext context, DataType type, string name) {
            switch (type) {
                case DataType.ClassGroup:
                    var group = context.ClassGroups.FirstOrDefault(c => c.Name == name);
                    if (group is null)
                        return (-1, "");
                    return (group.Id, group.Name);
                case DataType.Room:
                    var room = context.Rooms.FirstOrDefault(c => c.Name == name);
                    if (room is null)
                        return (-1, "");
                    return (room.Id, room.Name);
                case DataType.Teacher:
                    var teacher = context.Teachers.FirstOrDefault(c => c.Name == name);
                    if (teacher is null)
                        return (-1, "");
                    return (teacher.Id, teacher.Name);
                case DataType.Subject:
                    var subject = context.Subjects.FirstOrDefault(c => c.Name == name);
                    if (subject is null)
                        return (-1, "");
                    return (subject.Id, subject.Name);
                default:
                    return (-1, "");
            }
        }
        [Theory]
        [InlineData(DataType.ClassGroup, "1a")]
        [InlineData(DataType.Room, "111")]
        [InlineData(DataType.Subject, "eng")]
        [InlineData(DataType.Teacher, "kowalski")]
        public async Task AddKey_CreateElementWhichNameAlreadyExists_ThrowsInvalidOperationException(
            DataType type, string name) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                var dictionaryElement = new DictionaryElementEditViewModel() {
                    Name = name
                };
                await Assert.ThrowsAsync<InvalidOperationException>(
                    async () => await service.AddKey(dictionaryElement, type));
            }
        }
        [Theory]
        [InlineData(DataType.ClassGroup)]
        [InlineData(DataType.Room)]
        [InlineData(DataType.Subject)]
        [InlineData(DataType.Teacher)]
        public async Task UpdateKey_EditElementWhichNotExists_ThrowsArgumentException(DataType type) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                var dictionaryElement = new DictionaryElementEditViewModel() {
                    Id = 1000
                };
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await service.UpdateKey(dictionaryElement, type));
            }
        }
        [Theory]
        [InlineData(DataType.ClassGroup, "1a", "2a")]
        [InlineData(DataType.Room, "111", "112")]
        [InlineData(DataType.Subject, "eng", "mat")]
        [InlineData(DataType.Teacher, "kowalski", "nowak")]
        public async Task UpdateKey_EditElementWhichNameAlreadyExists_ThrowsInvalidOperationException(
            DataType type, string name, string newName) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                var element = GetDictionaryElement(context, type, name);
                var dictionaryElement = new DictionaryElementEditViewModel() {
                    Id = element.Item1,
                    Name = newName
                };
                await Assert.ThrowsAsync<InvalidOperationException>(
                    async () => await service.UpdateKey(dictionaryElement, type));
            }
        }
        [Theory]
        [InlineData(DataType.ClassGroup)]
        [InlineData(DataType.Room)]
        [InlineData(DataType.Subject)]
        [InlineData(DataType.Teacher)]
        public async Task UpdateKey_DeleteElementWhichNotExists_ThrowsArgumentException(DataType type) {
            using (var context = PrepareData.GetDbContext()) {
                var service = new DictionariesService(context);
                await Assert.ThrowsAsync<InvalidOperationException>(
                    async () => await service.RemoveKey(10000, null, type));
            }
        }
    }
}